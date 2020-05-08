using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CubeRender : MonoBehaviour
{

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 1;
    public int ySize = 1;
    public int zSize = 1;


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        StartCoroutine(CreateShape());
    }

    // Update is called once per frame
     void Update()
    {
        UpdateMesh();
    }

    IEnumerator CreateShape(){
        vertices = GenerateVertexArray(xSize, ySize, zSize);
        Debug.Log(vertices);

        triangles = new int[xSize * ySize * zSize * 36];
    
        int vert = 0;
        int tris = 0;

        Debug.Log(zSize + 1 * ySize + 1);
        
        for(int x=0; x< xSize; x++){
            
            for(int y=0; y< ySize; y++){
                
                for(int z=0; z< zSize; z++){

                    int[] formulas = new int[]{
                        vert + 0,
                        vert + 1 ,
                        vert + (zSize + 1),
                        vert + (zSize + 1) + 1,
                        vert + (zSize + 1) * (ySize + 1),
                        vert + (zSize + 1) * (ySize + 1) + 1,
                        vert + (zSize + 1) * (ySize + 1) + (zSize + 1)  ,
                        vert + (zSize + 1) * (ySize + 1) + (zSize + 1) + 1
                    };

                    /* FRENTE */
                    triangles[tris + 0] = formulas[2];
                    triangles[tris + 1] = formulas[0];
                    triangles[tris + 2] = formulas[1];

                    triangles[tris + 3] = formulas[3];
                    triangles[tris + 4] = formulas[2];
                    triangles[tris + 5] = formulas[1];

                    /* FONDO */
                    triangles[tris + 6] =  formulas[4];
                    triangles[tris + 7] =  formulas[6];
                    triangles[tris + 8] =  formulas[5];

                    triangles[tris + 9] =  formulas[5];
                    triangles[tris + 10] = formulas[6];
                    triangles[tris + 11] = formulas[7];
                    
                    /* PISO */
                    triangles[tris + 12] = formulas[0];
                    triangles[tris + 13] = formulas[4];
                    triangles[tris + 14] = formulas[5];

                    triangles[tris + 15] = formulas[0];
                    triangles[tris + 16] = formulas[5];
                    triangles[tris + 17] = formulas[1];

                    /* TECHO */
                    triangles[tris + 18] = formulas[7];
                    triangles[tris + 19] = formulas[6];
                    triangles[tris + 20] = formulas[2];

                    triangles[tris + 21] = formulas[7];
                    triangles[tris + 22] = formulas[2];
                    triangles[tris + 23] = formulas[3];
                    
                    /* LATERAL */
                    triangles[tris + 24] = formulas[6];
                    triangles[tris + 25] = formulas[4];
                    triangles[tris + 26] = formulas[0];

                    triangles[tris + 27] = formulas[2];
                    triangles[tris + 28] = formulas[6];
                    triangles[tris + 29] = formulas[0];

                     /* LATERAL2 */
                    triangles[tris + 30] = formulas[7];
                    triangles[tris + 31] = formulas[1];
                    triangles[tris + 32] = formulas[5];

                    triangles[tris + 33] = formulas[7];
                    triangles[tris + 34] = formulas[3];
                    triangles[tris + 35] = formulas[1];                
    
                    vert++;
                    tris+= 36;

                }

                vert++;
            
            }
        
             vert+=zSize+1;
        
        }
        yield return new WaitForSeconds(.1f);
    
    }

    Vector3[] GenerateVertexArray(int _xSize, int _ySize, int _zSize){
        Vector3[] array = new Vector3[(_xSize +1) * (_ySize +1) * (_zSize +1)];

        for(int x=0,i=0; x<= _xSize; x++){
            for(int y=0; y<= _ySize; y++){
                for(int z=0; z<= _zSize; z++){
                    array[i] = new Vector3(x,y,z);
                    i++;
                }
            }  
        }

        return array;
    }

    void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

/*     private void OnDrawGizmos(){

        if(vertices==null) return;

        for(int i=0; i< vertices.Length; i++){
            if(i%9 == 0 || i==0){
                Gizmos.color = Color.red;
            }

            Gizmos.DrawSphere(vertices[i],.1f);
                Gizmos.color = Color.blue;
        }

        


    } */
    
}
