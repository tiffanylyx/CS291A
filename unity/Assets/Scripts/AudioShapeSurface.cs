// Each sentence is a shape
// Position: Sentence Vector
// FrameNumber: Noun and Verb Numbers
// Each Object's color: Sentiment Value
// Change of color: Overall sentiment changing
// Strength: Pitch
// Shapes Mapping: Sentence Size, content

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioShapeSurface : MonoBehaviour
{
    LineRenderer _Line;
    LineRenderer[] LineList;
    public LineRenderer lineRenderer;
    
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    GameObject[] _FFTGameObjects;
    public GameObject _ObjectToSpawn;
    [Header("FFT")]
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.SixtyFour;
    public int column = 5;

    [Header("Sphere")]
    public float x_pos_start = 0;
    public float y_pos_start = 0;
    public float z_pos_start = 0;
    public float max_strength = 40f;
    public float min_strength = 20f;
    public float space = 0.5f;
    public float radius = 2f;
    public float max_radius = 4f;
    public float p1 = 4f;
    public float angle_offset = 1f;
    public int shape = 0;
    public float k = 0.5f;
    

    [Header("Spin")]
    public float speed = 2f;

    [Header("Emission col")]
    public Color _EmissionCol;
    public float _EmissionStrength = 1;

    int Size = 50;
    private float timer = 0.0f;
    

    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        _FFT = GetComponent<FrequencyBandAnalyser>();
        CreateShape();
        

}
     void Update(){
        UpdateMesh();
    }
    void CreateShape(){  
        //_Line = GetComponent<LineRenderer>();
        LineList = new LineRenderer[(Size+1)];  
        
        _FFTGameObjects = new GameObject[(Size+1)*(Size+1)];  
        vertices = new Vector3[(Size+1)*(Size+1)];
        int t = 0;
        float r = 0;
        for(int z = 0; z<=Size; z++){
            r = max_radius * z/Size;
            
            
            _Line = Instantiate(lineRenderer);
            _Line.useWorldSpace = false;
            _Line.positionCount = Size+1; 
            _Line.transform.SetParent(transform);
            _Line.transform.position = transform.position;
            _Line.transform.rotation = transform.rotation;
            //_Line.transform.localScale = transform.localScale;
            

            LineList[z] = _Line;
            if(shape<4){
            for(int x = 0; x<=Size; x++){
            float theta =angle_offset+ 2*Mathf.PI * x/Size;
            if(shape==0){
                vertices[t] = new Vector3(
                r*Mathf.Cos(theta)+x_pos_start, 
                r*r*Mathf.Cos(3*theta)/p1+y_pos_start,
                r*Mathf.Sin(theta)+z_pos_start);
                }
            else if (shape==1){
                if(r<=1){
                vertices[t] = new Vector3(
                r*Mathf.Cos(theta)+x_pos_start, 
                -(Mathf.Pow(r,2)*Mathf.Cos(5*theta)*Mathf.Sin(theta)/p1+Mathf.Pow(r,5)+y_pos_start),
                r*Mathf.Sin(theta)+z_pos_start);
                }
                else{
                vertices[t] = new Vector3(
                r*Mathf.Cos(theta)+x_pos_start, 
                -(Mathf.Pow(r,2)*Mathf.Cos(5*theta)*Mathf.Sin(theta)/p1+Mathf.Pow(r,2)+y_pos_start),
                r*Mathf.Sin(theta)+z_pos_start);

                }
            }
            else if (shape==2){
                if(r<=0.5f){
                vertices[t] = new Vector3(
                r*Mathf.Cos(theta)+x_pos_start, 
                3*r*r*Mathf.Cos(0.5f*theta)+Mathf.Pow(0.5f,2f)*Mathf.Cos(3*theta)*Mathf.Sin(theta)+Mathf.Pow(0.5f,8f)-3*0.5f*0.5f*Mathf.Cos(0.5f*theta)+y_pos_start,
                r*Mathf.Sin(theta)+z_pos_start);
                }
                else{
                vertices[t] = new Vector3(
                r*Mathf.Cos(theta)+x_pos_start, 
                Mathf.Pow(r,2)*Mathf.Cos(3*theta)*Mathf.Sin(theta)+Mathf.Pow(r,8)+y_pos_start,
                r*Mathf.Sin(theta)+z_pos_start);
                }
            }
            else if (shape ==3){
                float u = 2*Mathf.PI * x/Size;
                float v = 2*Mathf.PI * z/Size;
               
                float K = Mathf.Cos(u)/(Mathf.Sqrt(2)-k*Mathf.Sin(2*u)*Mathf.Sin(3*v));
 vertices[t] = new Vector3(
                K*(Mathf.Cos(u)*Mathf.Cos(2*u)+Mathf.Sqrt(2)*Mathf.Sin(u)*Mathf.Cos(v))+x_pos_start, 
                K*(Mathf.Cos(u)*Mathf.Sin(2*v)-Mathf.Sqrt(2)*Mathf.Sin(u)*Mathf.Sin(v))+y_pos_start,
                3*K*Mathf.Cos(u));

            }

            
            t++;}}
            else{
                for(int x = 0; x<=Size; x++){
                vertices[t] = new Vector3(0,0,0);
                t++;}


            }
            }

            //GameObject newFFTObject = Instantiate(_ObjectToSpawn);
            //newFFTObject.transform.SetParent(transform);
            //newFFTObject.transform.localPosition = vertices[t];
            //_FFTGameObjects[t] = newFFTObject;
                
            
        
        



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
    if(shape==1){
        gameObject.transform.Rotate(180.0f, 0.0f, 0.0f);
    }
       // mesh.vertices = vertices;
        //mesh.triangles = triangles;
        //mesh.uv = uvs;
        //mesh.RecalculateNormals(); 

            // APPLY SET MAT COL COMPONENT
        /*
        for (int i = 0; i < _FFTGameObjects.Length; i++)
        {
            FFTSetMaterialColour setMaterialCol = _FFTGameObjects[i].AddComponent<FFTSetMaterialColour>();
            setMaterialCol._Col = _EmissionCol;
            setMaterialCol._StrengthScalar = _EmissionStrength;

            setMaterialCol._FFT = _FFT;
            setMaterialCol._FreqBands = _FreqBands;
            setMaterialCol._FrequencyBandIndex = i%63;
        }
        */
        
        
        
    }
    void UpdateMesh()
    {
        mesh.Clear();
        int t = 0;
        int offset = 32;
        float strength = _FFT.GetBandValue(0, _FreqBands);
        timer += Time.deltaTime;
        float r = 0;
        
        
        for(int z = 0; z<=Size; z++){
            //float z = (float)(rz+angle);
            r = max_radius*Mathf.Sin(z*Mathf.PI/(2*Size));
            if(shape<4){
        for(int x = 0; x<=Size; x++){
           
            float theta =angle_offset+ 2*Mathf.PI * x/Size;

            if(shape==0){
                p1 = 1+5*strength;
            vertices[t] = new Vector3(
            r*Mathf.Cos(theta)+x_pos_start, 
            r*r*Mathf.Cos(p1*theta)/4+y_pos_start,
            r*Mathf.Sin(theta)+z_pos_start);
            }
            else if (shape==1){

                p1 = 60*Mathf.Max(0.04f,strength)+5*strength;

                if(r<=1){
                vertices[t] = new Vector3(
                r*Mathf.Cos(theta)+x_pos_start, 
                -(0.2f*(Mathf.Pow(r,2)*Mathf.Cos(5*theta)*Mathf.Sin(theta)/p1+y_pos_start+Mathf.Pow(r,5))),
                r*Mathf.Sin(theta)+z_pos_start);
                }
                else{
              vertices[t] = new Vector3(
                r*Mathf.Cos(theta)+x_pos_start, 
                -(0.2f*(Mathf.Pow(r,2)*Mathf.Cos(5*theta)*Mathf.Sin(theta)/p1+y_pos_start+Mathf.Pow(r,2))),
                r*Mathf.Sin(theta)+z_pos_start);

                }

            }
                else if (shape==2){
                    r = z/Size;
                if(r<=0.5f){
                vertices[t] = new Vector3(
                r*Mathf.Cos(theta)+x_pos_start, 
                1f*(3*r*r*Mathf.Cos(0.5f*theta)+Mathf.Pow(0.5f,2f)*Mathf.Cos(3*theta)*Mathf.Sin(theta)+Mathf.Pow(0.5f,8f)-3*0.5f*0.5f*Mathf.Cos(0.5f*theta)+y_pos_start),
                r*Mathf.Sin(theta)+z_pos_start);
                }
                else{
                vertices[t] = new Vector3(
                r*Mathf.Cos(theta)+x_pos_start, 
                1f*(Mathf.Pow(r,2)*Mathf.Cos(3*theta)*Mathf.Sin(theta)+Mathf.Pow(r,8)+y_pos_start),
                r*Mathf.Sin(theta)+z_pos_start);
                }
            }
            else if (shape ==3){
                k = Mathf.Max(0,Mathf.Min(1,4*strength));
                float u = 2*Mathf.PI * x/Size;
                float v = 2*Mathf.PI * z/Size;
                float K = Mathf.Cos(u)/(Mathf.Sqrt(2)-k*Mathf.Sin(2*u)*Mathf.Sin(3*v));
 vertices[t] = new Vector3(
                max_radius*K*(Mathf.Cos(u)*Mathf.Cos(2*u)+Mathf.Sqrt(2)*Mathf.Sin(u)*Mathf.Cos(v))+x_pos_start, 
                max_radius*K*(Mathf.Cos(u)*Mathf.Sin(2*v)-Mathf.Sqrt(2)*Mathf.Sin(u)*Mathf.Sin(v))+y_pos_start,
                max_radius*3*K*Mathf.Cos(u)+z_pos_start);

            }


            //_FFTGameObjects[t].transform.localPosition = vertices[t];
            

            LineList[z].SetPosition(x,transform.localScale[0]*vertices[t]);

                
            t++;
            }}
        else{
            if(shape==4){
            r = max_radius * z/Size;
            
            int count = 0;
            
            for(int x = z; x<=z+Size; x++){
                int x1;
                if(x>Size){
                    x1 = x-Size;
                }
                else{
                    x1 = x;
                }
                
                strength = _FFT.GetBandValue(Mathf.Min(63,x1), _FreqBands);
                 float theta =2*Mathf.PI * count/Size;
                 vertices[t] = new Vector3(
                    Mathf.Cos(theta)*r+x_pos_start,
                    5*strength,
                    Mathf.Sin(theta)*r+z_pos_start);
                    LineList[count].SetPosition(z,transform.localScale[0]* vertices[t]);
                    count++;
                    t++;
                    }
            }
            else if (shape==5){
                int count = 0;
                for(int x = z; x<=z+Size; x++){
                int x1;
                if(x>Size){
                    x1 = x-Size;
                }
                else{
                    x1 = x;
                }
                    strength = 0.5f+Mathf.Min(1,3*_FFT.GetBandValue(Mathf.Min(63,x1), _FreqBands));
                    float theta =2*Mathf.PI * count/Size;
                  vertices[t] = new Vector3(
                    Mathf.Pow(Mathf.Cos(theta),3)*((float)0.01f*(float)z+strength)+x_pos_start,
                    (float)0.05f*(float)z,
                    Mathf.Pow(Mathf.Sin(theta),3)*((float)0.01f*(float)z+strength)+z_pos_start);
                    

                    LineList[z].SetPosition(count, transform.localScale[0]*vertices[t]);    
                    count++;
                     t++;           
                }   

            }
            else if (shape==6){
                int count = 0;
                for(int x = z; x<=z+Size; x++){
                int x1;
                if(x>Size){
                    x1 = x-Size;
                }
                else{
                    x1 = x;
                }
                    strength = 0.5f+Mathf.Min(1,3*_FFT.GetBandValue(Mathf.Min(63,x1), _FreqBands));
                    float theta =2*Mathf.PI * count/Size;
                    vertices[t] = new Vector3(
                    Mathf.Cos(theta)*((2*Mathf.Sin(0.3f*(float)z)-1)*Mathf.Min(0.2f,(float)0.1f*(float)z)+strength)+x_pos_start,
                    (float)0.05f*(float)z,
                    Mathf.Sin(theta)*strength+z_pos_start);
                    

                    LineList[z].SetPosition(count, transform.localScale[0]*vertices[t]);    
                    count++;
                     t++;           
                }   

            }           
            else if (shape==7){
                for(int x = 0; x<=Size; x++){
                strength = 0.5f+Mathf.Min(1,3*_FFT.GetBandValue(Mathf.Min(63,x), _FreqBands));
                float rr = (1+0.1f*Mathf.Cos(10.0f*(float)x));
                float x1 = Mathf.Cos(x)*Mathf.Cos(z)*rr*strength;
                float z1 = Mathf.Sin(x)*Mathf.Cos(z)*rr*strength;  
                float y1 = (Mathf.Cos(Mathf.PI*x)+Mathf.Sin(Mathf.PI*z));
                vertices[t] = new Vector3(x1, y1, z1);
                LineList[z].SetPosition(x, transform.localScale[0]*vertices[t]);    

                t++;           

                }   

            }   
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

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        //Material material = GetComponent<MeshRenderer>().sharedMaterial;
        //Debug.Log(material.GetColor("_TintColor"));
        //material.SetColor("_TintColor",Color.Lerp(Color.red, Color.blue, strength*20));
        //cubeRenderer.material.TintColor("_Color", Color.red);
    }

}

