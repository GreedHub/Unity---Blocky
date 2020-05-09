using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public class ChunkRenderer : ComponentSystem{

    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    protected override void OnUpdate(){

        Entities.WithAll<ChunkComponent>().ForEach((entity)=>{

/*             EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            DynamicBuffer<Block> dynamicBuffer = entityManager.GetBuffer<Block>(entity); 
            DynamicBuffer<Block> blockBuffer = dynamicBuffer.Reinterpret<Block>(); 

            DynamicBuffer<VertexBuffer> dynamicVertexBuffer = entityManager.GetBuffer<VertexBuffer>(entity); 
            var vertexBuffer = dynamicVertexBuffer.Reinterpret<VertexBuffer>();

            List<Vector3> vertex = new List<Vector3>();
            for (int i = 0; i < vertexBuffer.Length; i++)
            {
                Debug.Log(vertexBuffer[i].vertex);
                vertex.Add(Vector3.zero);
            } 

            ComponentDataFromEntity<ChunkComponent> chunkType = GetComponentDataFromEntity<ChunkComponent>(true);

            ChunkComponent chunk = chunkType[entity];

            List<int> triangles = GenerateCubesByTriangles(chunk.xSize, chunk.ySize, chunk.zSize, blockBuffer);

            UpdateMesh(vertex.ToArray(),triangles.ToArray());

            entityManager.SetSharedComponentData(entity, new RenderMesh{
                mesh = mesh,
                material = material
            }); */

        });
        

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
                    
                    if(blockBuffer[x*_ySize+y*_zSize+z].material == BlockMaterials.air) {vert++; continue;}; 
  
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
                    if((x==0 &&blockBuffer[x*_ySize+y*_zSize+z].material != BlockMaterials.air) || (x>0 && blockBuffer[(x-1)*_ySize+y*_zSize+z].material == BlockMaterials.air)){
                        
                        _triangles.Add(formulas[2]);
                        _triangles.Add(formulas[0]);
                        _triangles.Add(formulas[1]);
  
                        _triangles.Add(formulas[3]);
                        _triangles.Add(formulas[2]);
                        _triangles.Add(formulas[1]);
                        
                    }
  
                    /* FONDO */
                    if((x==_xSize-1 &&blockBuffer[x*_ySize+y*_zSize+z].material != BlockMaterials.air) || (x < _xSize-1 && blockBuffer[(x+1)*_ySize+y*_zSize+z].material == BlockMaterials.air)){
                        _triangles.Add(formulas[4]);
                        _triangles.Add(formulas[6]);
                        _triangles.Add(formulas[5]);
  
                        _triangles.Add(formulas[5]);
                        _triangles.Add(formulas[6]);
                        _triangles.Add(formulas[7]);
  
                    }
  
    
                    /* PISO */
                    if((y==0 &&blockBuffer[x*_ySize+y*_zSize+z].material != BlockMaterials.air)|| (y>0 && blockBuffer[x*_ySize+(y-1)*_zSize+z].material == BlockMaterials.air)){
  
                        _triangles.Add(formulas[0]);
                        _triangles.Add(formulas[4]);
                        _triangles.Add(formulas[5]);
  
                        _triangles.Add(formulas[0]);
                        _triangles.Add(formulas[5]);
                        _triangles.Add(formulas[1]);
  
                    }
  
  
                    /* TECHO */
                    if(y<_ySize-1 && blockBuffer[x*_ySize+(y+1)*_zSize+z].material == BlockMaterials.air){
                        _triangles.Add(formulas[7]);
                        _triangles.Add(formulas[6]);
                        _triangles.Add(formulas[2]);
  
                        _triangles.Add(formulas[7]);
                        _triangles.Add(formulas[2]);
                        _triangles.Add(formulas[3]);
                    }
  
                    
                    /* LATERAL */
                    if((z==0 &&blockBuffer[x*_ySize+y*_zSize+z].material != BlockMaterials.air) || (z>0 && blockBuffer[x*_ySize+y*_zSize+z-1].material == BlockMaterials.air)){
                        _triangles.Add(formulas[6]);
                        _triangles.Add(formulas[4]);
                        _triangles.Add(formulas[0]);
  
                        _triangles.Add(formulas[2]);
                        _triangles.Add(formulas[6]);
                        _triangles.Add(formulas[0]);
                    }
  
  
                    /* LATERAL2 */
                    if((z==_zSize-1 &&blockBuffer[x*_ySize+y*_zSize+z].material != BlockMaterials.air) || (z<_zSize-1 && blockBuffer[x*_ySize+y*_zSize+z+1].material == BlockMaterials.air)){
  
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