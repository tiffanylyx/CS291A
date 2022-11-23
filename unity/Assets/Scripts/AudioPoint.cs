using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter ))]
public class AudioPoint : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    [Header("FFT")]
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;
    GameObject[] _FFTGameObjects;
    public GameObject _ObjectToSpawn;


    [Header("Object Array")]
    public float _Spacing = 1;
    public Vector3 _ScaleStrength = Vector3.up;
    Vector3 _BaseScale;


    [Header("Emission col")]
    public Color _EmissionCol;
    public float _EmissionStrength = 1;
    int xSize = 7;  
    int zSize = 7;  
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();}
    void CreateShape(){     
        vertices = new Vector3[(xSize+1)*(zSize+1)];
        _FFTGameObjects = new GameObject[(int)_FreqBands];
        _BaseScale = _ObjectToSpawn.transform.localScale;
        int t = 0;
        for(int z = 0; z<=zSize; z++){
        for(int x = 0; x<=xSize; x++){
                GameObject newFFTObject = Instantiate(_ObjectToSpawn);
                newFFTObject.transform.SetParent(transform);
                newFFTObject.transform.localPosition = new Vector3(x, 0, z);
                _FFTGameObjects[t] = newFFTObject;
                vertices[t] = new Vector3(x, 0, z);
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
            setMaterialCol._FrequencyBandIndex = i;
        }
    }

    private void Update()
    {
        int t = 0;
        for(int z = 0; z<=zSize; z++){
        for(int x = 0; x<=xSize; x++){
            float y_pos = 40 * _FFT.GetBandValue(t, _FreqBands);
            _FFTGameObjects[t].transform.localPosition = new Vector3(x, y_pos, z);
            t++;
            }
        }
    }
}
