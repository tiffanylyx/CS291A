using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter ))]
public class AudioReactMesh : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    GameObject[] _FFTGameObjects;
    public GameObject _ObjectToSpawn;
    [Header("FFT")]
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;
    public float x_pos_start = 0;
    public float y_pos_start = 0;
    public float z_pos_start = 0;
    public float max_strength = 40f;
    public float min_strength = 20f;
    public float space = 0.5f;
    public int zSize = 40;
    public int xSize = 60;

    [Header("Emission col")]
    public Color _EmissionCol;
    public float _EmissionStrength = 1;

   
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();

}
     void Update(){
        UpdateMesh();
    }
    void CreateShape(){   
        _FFTGameObjects = new GameObject[(xSize+1)*(zSize+1)];  
        vertices = new Vector3[(xSize+1)*(zSize+1)];
        int t = 0;
        for(int z = 0; z<=zSize; z++){
        for(int x = 0; x<=xSize; x++){
                vertices[t] = new Vector3(x_pos_start+x*space, y_pos_start+0, z_pos_start+z*space);
                GameObject newFFTObject = Instantiate(_ObjectToSpawn);
                newFFTObject.transform.SetParent(transform);
                newFFTObject.transform.localPosition = new Vector3(x_pos_start+x*space, y_pos_start+0, z_pos_start+z*space);
                _FFTGameObjects[t] = newFFTObject;
                t++;
                
            }
        }
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for(int z = 0; z<zSize; z++){
        for(int x = 0; x<xSize; x++){
        
        triangles[tris+0] = vert+0;
        triangles[tris+1] = vert+xSize+1;
        triangles[tris+2] = vert+1; 
        triangles[tris+3] = vert+1;
        triangles[tris+4] = vert+xSize+1;
        triangles[tris+5] = vert+xSize+2;
        vert++;
        tris += 6;          
        }
        vert++;

    }  
    t = 0;
    uvs = new Vector2[vertices.Length];
        for(int z = 0; z<=zSize; z++){
        for(int x = 0; x<=xSize; x++){
            uvs[t] = new Vector2((float)(x*space)/(space*xSize), (float)(z*space)/(space*zSize));
                t++;
                
            }
        }
            // APPLY SET MAT COL COMPONENT
        for (int i = 0; i < _FFTGameObjects.Length; i++)
        {
            FFTSetMaterialColour setMaterialCol = _FFTGameObjects[i].AddComponent<FFTSetMaterialColour>();
            setMaterialCol._Col = _EmissionCol;
            setMaterialCol._StrengthScalar = _EmissionStrength;

            setMaterialCol._FFT = _FFT;
            setMaterialCol._FreqBands = _FreqBands;
            setMaterialCol._FrequencyBandIndex = i%(int)_FreqBands;
        }
        
        
    }
    void UpdateMesh()
    {
        mesh.Clear();
        int t = 0;
        int offset = (int)_FreqBands/2;
        
        float y_pos;
        for(int z = 0; z<=zSize; z++){
        for(int x = 0; x<=xSize; x++){
            y_pos = Random.Range(min_strength,max_strength) * _FFT.GetBandValue((offset+x)%(int)_FreqBands, _FreqBands);
            
            vertices[t] = new Vector3(x_pos_start+x*space, y_pos_start+y_pos, z_pos_start+z*space);
            _FFTGameObjects[t].transform.localPosition = new Vector3(x_pos_start+x*space, y_pos_start+y_pos, z_pos_start+z*space);
            t++;
            }
            offset++;
            offset = offset%(int)_FreqBands;
            
        }
        int vert = 0;
        int tris = 0;
        for(int z = 0; z<zSize; z++){
        for(int x = 0; x<xSize; x++){
        
        triangles[tris+0] = vert+0;
        triangles[tris+1] = vert+xSize+1;
        triangles[tris+2] = vert+1; 
        triangles[tris+3] = vert+1;
        triangles[tris+4] = vert+xSize+1;
        triangles[tris+5] = vert+xSize+2;
        vert++;
        tris += 6;          
        }
        vert++;

    }  
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
 
    }
    private void OnDrawGizmos(){
        if (vertices==null ){
            return;
        }
         for(int i = 0; i<vertices.Length; i++){
             Gizmos.DrawSphere(vertices[i], .1f);
         }
    }
}
