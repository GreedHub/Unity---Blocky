using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class ChunkRendererOLD : MonoBehaviour
{

//    Mesh mesh;
//    MeshCollider meshCollider;    
//    Vector3[] vertices;
//    int[] triangles;
//    Block[,,] blocks;
//    float offset;
//    int xSize = 16;
//    int ySize = 225;
//    int zSize = 16;
//    FastNoise noise = new FastNoise();
//    float[,] heightMap = new float[xSize,zSize];
//    float[,] heightMap2 = new float[xSize,zSize];
//
//    // Start is called before the first frame update
//    void Start()
//    {
//        offset = Random.Range(0f,10f);
//
//        mesh = new Mesh();
//
//        meshCollider = GetComponent<MeshCollider>();
//
//        GetComponent<MeshFilter>().mesh = mesh;
//
//        StartCoroutine(CreateShape());
//
//        UpdateMesh();
//        
//    }
//    
//    IEnumerator CreateShape(){        
//
//        blocks = new Block[xSize,ySize,zSize];
//
//        GenerateBlocks(xSize,ySize,zSize);
//
//        vertices = GenerateVertexArray(xSize, ySize, zSize).ToArray();
//
//        triangles = GenerateCubesByTriangles(xSize, ySize , zSize).ToArray();
//
//        yield return new WaitForSeconds(.1f);
//    
//    }
//
//    void GenerateBlocks(int _xSize,int _ySize,int _zSize){
//
//        
//
//        for(int x=0; x< _xSize; x++){
//            
//            for(int y=0; y< _ySize; y++){
//                
//                for(int z=0; z< _zSize; z++){
//                    
//                    blocks[x,y,z] = new Block();   
//
//                    float simplex1 = noise.GetSimplex(x*.8f, z*.8f)*10;
//                    float simplex2 = noise.GetSimplex(x * 3f, z * 3f) * 10*(noise.GetSimplex(x*.3f, z*.3f)+.5f);
//                    float heightMap = simplex1 + simplex2;
//                    float dirtHeight = ySize * .5f + heightMap;                        
//
//                   // Debug.Log($"Heightmap: {heightMap} | dirtHeight: {dirtHeight}");
//
//                    blocks[x,y,z].material = (y > dirtHeight) ?  "air" : "dirt";
//
//                }
//
//            }
//
//        }
//
//    }
//
//List<int> GenerateCubesByTriangles(int _xSize, int _ySize, int _zSize){
//        List<int> _triangles = new List<int>();
//    
//        int vert = 0;
//        
//        for(int x=0; x< _xSize ; x++){
//
//            for(int y=0; y< _ySize; y++){
//                
//                for(int z=0; z< _zSize; z++){        
//
//                    if(blocks[x,y,z].material == "air") {vert++; continue;}; 
//
//                    int[] formulas = new int[]{
//                        vert + 0,
//                        vert + 1 ,
//                        vert + (_zSize + 1),
//                        vert + (_zSize + 1) + 1,
//                        vert + (_zSize + 1) * (_ySize + 1),
//                        vert + (_zSize + 1) * (_ySize + 1) + 1,
//                        vert + (_zSize + 1) * (_ySize + 1) + (_zSize + 1)  ,
//                        vert + (_zSize + 1) * (_ySize + 1) + (_zSize + 1) + 1
//                    };
//
//                    /* FRENTE */
//                    if((x==0 &&blocks[x,y,z].material != "air") || (x>0 && blocks[x-1,y,z].material == "air")){
//
//                        _triangles.Add(formulas[2]);
//                        _triangles.Add(formulas[0]);
//                        _triangles.Add(formulas[1]);
//
//                        _triangles.Add(formulas[3]);
//                        _triangles.Add(formulas[2]);
//                        _triangles.Add(formulas[1]);
//                        
//                    }
//
//                    /* FONDO */
//                    if((x==xSize-1 &&blocks[x,y,z].material != "air") || (x < xSize-1 && blocks[x+1,y,z].material == "air")){
//                        _triangles.Add(formulas[4]);
//                        _triangles.Add(formulas[6]);
//                        _triangles.Add(formulas[5]);
//
//                        _triangles.Add(formulas[5]);
//                        _triangles.Add(formulas[6]);
//                        _triangles.Add(formulas[7]);
//
//                    }
//
//    
//                    /* PISO */
//                    if((y==0 &&blocks[x,y,z].material != "air")|| (y>0 && blocks[x,y-1,z].material == "air")){
//
//                        _triangles.Add(formulas[0]);
//                        _triangles.Add(formulas[4]);
//                        _triangles.Add(formulas[5]);
//
//                        _triangles.Add(formulas[0]);
//                        _triangles.Add(formulas[5]);
//                        _triangles.Add(formulas[1]);
//
//                    }
//
//
//                    /* TECHO */
//                    if(y<ySize-1 && blocks[x,y+1,z].material == "air"){
//                        _triangles.Add(formulas[7]);
//                        _triangles.Add(formulas[6]);
//                        _triangles.Add(formulas[2]);
//
//                        _triangles.Add(formulas[7]);
//                        _triangles.Add(formulas[2]);
//                        _triangles.Add(formulas[3]);
//                    }
//
//                    
//                    /* LATERAL */
//                    if((z==0 &&blocks[x,y,z].material != "air") || (z>0 && blocks[x,y,z-1].material == "air")){
//                        _triangles.Add(formulas[6]);
//                        _triangles.Add(formulas[4]);
//                        _triangles.Add(formulas[0]);
//
//                        _triangles.Add(formulas[2]);
//                        _triangles.Add(formulas[6]);
//                        _triangles.Add(formulas[0]);
//                    }
//
//
//                    /* LATERAL2 */
//                    if((z==zSize-1 &&blocks[x,y,z].material != "air") || (z<zSize-1 && blocks[x,y,z+1].material == "air")){
//
//                        _triangles.Add(formulas[7]);
//                        _triangles.Add(formulas[1]);
//                        _triangles.Add(formulas[5]);
//
//                        _triangles.Add(formulas[7]);
//                        _triangles.Add(formulas[3]);
//                        _triangles.Add(formulas[1]);     
//                    }
//    
//    
//                    vert++;
//                 
//
//                }
//
//                vert++;
//            
//            }
//        
//             vert += _zSize + 1;
//           
//        }
//
//        return _triangles;
//    }
//
//    List<Vector3> GenerateVertexArray(int _xSize, int _ySize, int _zSize){
//        
//        List<Vector3> list = new List<Vector3>();
//
//        for(int x=0,i=0; x<= _xSize; x++){
//            for(int y=0; y<= _ySize; y++){
//                for(int z=0; z<= _zSize; z++){
//                    list.Add(new Vector3(x,y,z));
//                    i++;
//                }
//            }  
//        }
//
//        return list;
//    }
//
//    void UpdateMesh(){
//        mesh.Clear();
//        mesh.vertices = vertices;
//        mesh.triangles = triangles;
//        meshCollider.sharedMesh = mesh;
//        mesh.RecalculateNormals();
//    }

}
