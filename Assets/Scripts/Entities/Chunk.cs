using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public class Chunk : MonoBehaviour {
    
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    [SerializeField] private int xSize = 16;
    [SerializeField] private int ySize = 225;
    [SerializeField] private int zSize = 16;

    FastNoise noise = new FastNoise();
    float[,] heightMap ;
    float[,] heightMap2;

    private void Start(){
        heightMap = new float[xSize,zSize];
        heightMap2 = new float[xSize,zSize];

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(Chunk),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(RenderBounds),
            typeof(NativeArray<Vector3>)
        );

        Entity entity = entityManager.CreateEntity(entityArchetype);

        //NativeArray<Entity> entityArray = new NativeArray<Entity>(1,Allocator.Temp);

        //entityManager.CreateEntity(entityArchetype, entityArray);

        DynamicBuffer<Block> dynamicBuffer =  entityManager.AddBuffer<Block>(entity);        

        NativeArray<Vector3> vertex = new NativeArray<Vector3>();        

        for(int x=0; x< xSize ; x++) 
            for(int y=0; y< ySize; y++)
                for(int z=0; z< zSize; z++){                    

                    float simplex1 = noise.GetSimplex(x*.8f, z*.8f)*10;
                    float simplex2 = noise.GetSimplex(x * 3f, z * 3f) * 10*(noise.GetSimplex(x*.3f, z*.3f)+.5f);
                    float heightMap = simplex1 + simplex2;
                    float dirtHeight = ySize * .5f + heightMap;

                    dynamicBuffer.Add(new Block{
                        xCoord = x,
                        yCoord = y,
                        zCoord = z,
                        material = y > dirtHeight ?  BlockMaterials.air : BlockMaterials.dirt
                    });

                }

        for(int x=0; x<= xSize ; x++) 
            for(int y=0; y<= ySize; y++)
                for(int z=0; z<= zSize; z++){
                    vertex[x*ySize + y*zSize + z] = new Vector3(x,y,z);
                }

        entityManager.SetComponentData(entity, new ChunkComponent{
            xSize = xSize,
            ySize = ySize,
            zSize = zSize
        });

        entityManager.SetComponentData(entity, new NativeArray<Vector3>.CopyFrom(vertex));
    
    }

    void UpdateMesh(Vector3[] vertices, int[] triangles){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    List<int> GenerateCubesByTriangles(int _xSize, int _ySize, int _zSize, DynamicBuffer<Block> blockBuffer){
        List<int> _triangles = new List<int>();

        int vert = 0;
        
        for(int x=0; x< _xSize ; x++){
  
            for(int y=0; y< _ySize; y++){
                
                for(int z=0; z< _zSize; z++){        
                    
                    if(blockBuffer[x*ySize+y*zSize+z].material == BlockMaterials.air) {vert++; continue;}; 
  
                    int[] formulas = new int[]{
                        vert + 0,
                        vert + 1 ,
                        vert + (_zSize + 1),
                        vert + (_zSize + 1) + 1,
                        vert + (_zSize + 1) * (_ySize + 1),
                        vert + (_zSize + 1) * (_ySize + 1) + 1,
                        vert + (_zSize + 1) * (_ySize + 1) + (_zSize + 1)  ,
                        vert + (_zSize + 1) * (_ySize + 1) + (_zSize + 1) + 1
                    };

                    /* FRENTE */
                    if((x==0 &&blockBuffer[x*ySize+y*zSize+z].material != BlockMaterials.air) || (x>0 && blockBuffer[(x-1)*ySize+y*zSize+z].material == BlockMaterials.air)){
                        
                        _triangles.Add(formulas[2]);
                        _triangles.Add(formulas[0]);
                        _triangles.Add(formulas[1]);
  
                        _triangles.Add(formulas[3]);
                        _triangles.Add(formulas[2]);
                        _triangles.Add(formulas[1]);
                        
                    }
  
                    /* FONDO */
                    if((x==xSize-1 &&blockBuffer[x*ySize+y*zSize+z].material != BlockMaterials.air) || (x < xSize-1 && blockBuffer[(x+1)*ySize+y*zSize+z].material == BlockMaterials.air)){
                        _triangles.Add(formulas[4]);
                        _triangles.Add(formulas[6]);
                        _triangles.Add(formulas[5]);
  
                        _triangles.Add(formulas[5]);
                        _triangles.Add(formulas[6]);
                        _triangles.Add(formulas[7]);
  
                    }
  
    
                    /* PISO */
                    if((y==0 &&blockBuffer[x*ySize+y*zSize+z].material != BlockMaterials.air)|| (y>0 && blockBuffer[x*ySize+(y-1)*zSize+z].material == BlockMaterials.air)){
  
                        _triangles.Add(formulas[0]);
                        _triangles.Add(formulas[4]);
                        _triangles.Add(formulas[5]);
  
                        _triangles.Add(formulas[0]);
                        _triangles.Add(formulas[5]);
                        _triangles.Add(formulas[1]);
  
                    }
  
  
                    /* TECHO */
                    if(y<ySize-1 && blockBuffer[x*ySize+(y+1)*zSize+z].material == BlockMaterials.air){
                        _triangles.Add(formulas[7]);
                        _triangles.Add(formulas[6]);
                        _triangles.Add(formulas[2]);
  
                        _triangles.Add(formulas[7]);
                        _triangles.Add(formulas[2]);
                        _triangles.Add(formulas[3]);
                    }
  
                    
                    /* LATERAL */
                    if((z==0 &&blockBuffer[x*ySize+y*zSize+z].material != BlockMaterials.air) || (z>0 && blockBuffer[x*ySize+y*zSize+z-1].material == BlockMaterials.air)){
                        _triangles.Add(formulas[6]);
                        _triangles.Add(formulas[4]);
                        _triangles.Add(formulas[0]);
  
                        _triangles.Add(formulas[2]);
                        _triangles.Add(formulas[6]);
                        _triangles.Add(formulas[0]);
                    }
  
  
                    /* LATERAL2 */
                    if((z==zSize-1 &&blockBuffer[x*ySize+y*zSize+z].material != BlockMaterials.air) || (z<zSize-1 && blockBuffer[x*ySize+y*zSize+z+1].material == BlockMaterials.air)){
  
                        _triangles.Add(formulas[7]);
                        _triangles.Add(formulas[1]);
                        _triangles.Add(formulas[5]);
  
                        _triangles.Add(formulas[7]);
                        _triangles.Add(formulas[3]);
                        _triangles.Add(formulas[1]);     
                    }    
    
                    vert++;

                }

                vert++;

            }

            vert += _zSize + 1;

        }
  
        return _triangles;
    }

}
