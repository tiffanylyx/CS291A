using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter ))]

public class AudioShape : MonoBehaviour
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
    public int column = 5;

    [Header("Sphere")]
    public float x_pos_start = 0;
    public float y_pos_start = 0;
    public float z_pos_start = 0;
    public float max_strength = 40f;
    public float min_strength = 20f;
    public float space = 0.5f;
    public float radius = 2f;
    

    [Header("Spin")]
    public float speed = 2f;

    [Header("Emission col")]
    public Color _EmissionCol;
    public float _EmissionStrength = 1;

    int Size = 63;
    private float timer = 0.0f;
    

    
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
        _FFTGameObjects = new GameObject[(Size+1)*(Size+1)];  
        vertices = new Vector3[(Size+1)*(Size+1)];
        int t = 0;
        for(int z = 0; z<=Size; z++){
        for(int x = 0; x<=Size; x++){
                vertices[t] = new Vector3(
                Mathf.Cos(Mathf.PI * x/Size)*Mathf.Cos(2*Mathf.PI * z/Size)+x_pos_start, 
                Mathf.Sin(Mathf.PI * x/Size)*Mathf.Cos(2*Mathf.PI * z/Size)+y_pos_start, 
                Mathf.Cos(Mathf.PI * x/Size)+z_pos_start);

                GameObject newFFTObject = Instantiate(_ObjectToSpawn);
                newFFTObject.transform.SetParent(transform);
                newFFTObject.transform.localPosition = new Vector3(
                Mathf.Cos(Mathf.PI * x/Size)*Mathf.Cos(2*Mathf.PI * z/Size)+x_pos_start, 
                Mathf.Sin(Mathf.PI * x/Size)*Mathf.Cos(2*Mathf.PI * z/Size)+y_pos_start, 
                Mathf.Cos(Mathf.PI * x/Size)+z_pos_start);
                _FFTGameObjects[t] = newFFTObject;
                t++;
                
            }
        }
        triangles = new int[Size * Size * 6];
        int vert = 0;
        int tris = 0;
        for(int z = 0; z<Size; z++){
        for(int x = 0; x<Size; x++){
        
        triangles[tris+0] = vert+0;
        triangles[tris+1] = vert+Size+1;
        triangles[tris+2] = vert+1; 
        triangles[tris+3] = vert+1;
        triangles[tris+4] = vert+Size+1;
        triangles[tris+5] = vert+Size+2;
        vert++;
        tris += 6;          
        }
        vert++;

    }  

            // APPLY SET MAT COL COMPONENT
        for (int i = 0; i < _FFTGameObjects.Length; i++)
        {
            FFTSetMaterialColour setMaterialCol = _FFTGameObjects[i].AddComponent<FFTSetMaterialColour>();
            setMaterialCol._Col = _EmissionCol;
            setMaterialCol._StrengthScalar = _EmissionStrength;

            setMaterialCol._FFT = _FFT;
            setMaterialCol._FreqBands = _FreqBands;
            setMaterialCol._FrequencyBandIndex = i%63;
        }
        
        
    }
    void UpdateMesh()
    {
        mesh.Clear();
        int t = 0;
        int offset = 32;
        float angle;
        float strength;
        timer += Time.deltaTime;
        
        for(int rz = 0; rz<=Size; rz++){
            //float z = (float)(rz+angle);
        for(int rx = 0; rx<=Size; rx++){
            
            
            
            strength = (float)(radius +Random.Range(min_strength,max_strength) * _FFT.GetBandValue((offset+rx)%63, _FreqBands));
            angle = timer*strength;
            float z = (float)(rz+angle);
            float x = (float)(rx+angle);

            
            vertices[t] = new Vector3(
                strength*Mathf.Cos(Mathf.PI * x/Size)*Mathf.Cos(2*Mathf.PI * z/Size)+x_pos_start, 
                strength*Mathf.Sin(Mathf.PI * x/Size)*Mathf.Cos(2*Mathf.PI * z/Size)+y_pos_start , 
                strength*Mathf.Cos(Mathf.PI * x/Size)+z_pos_start);
            _FFTGameObjects[t].transform.localPosition = new Vector3(
                strength*Mathf.Cos(Mathf.PI * x/Size)*Mathf.Cos(2*Mathf.PI * z/Size)+x_pos_start, 
                strength*Mathf.Sin(Mathf.PI * x/Size)*Mathf.Cos(2*Mathf.PI * z/Size)+y_pos_start, 
                strength*Mathf.Cos(Mathf.PI * x/Size)+z_pos_start);
            t++;
            }
            offset+=column;
            if(offset>63){
                offset = offset-64;
            }
        }
        int vert = 0;
        int tris = 0;
        for(int z = 0; z<Size; z++){
        for(int x = 0; x<Size; x++){
        
        triangles[tris+0] = vert+0;
        triangles[tris+1] = vert+Size+1;
        triangles[tris+2] = vert+1; 
        triangles[tris+3] = vert+1;
        triangles[tris+4] = vert+Size+1;
        triangles[tris+5] = vert+Size+2;
        vert++;
        tris += 6;          
        }
        vert++;

    }
        t = 0;
        /*
    uvs = new Vector2[vertices.Length];
        for(int z = 0; z<=Size; z++){
        for(int x = 0; x<=Size; x++){
            uvs[t] = new Vector2((float)(0.5f + Mathf.Atan2(vertices[t][0], vertices[t][2])) / (2 * Mathf.PI);, (float)(0.5f - asin(vertices[t][0] / radius) / PI));
                t++;
                
            }
        }*/

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

