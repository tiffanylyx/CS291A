using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTObjectV2 : MonoBehaviour
{


    [Header("FFT")]
    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.Eight;
    GameObject[] _FFTGameObjects;
    public GameObject _ObjectToSpawn;


    [Header("Object Array")]
    public Vector3 StartPosition = new Vector3(0, 0,0);
    public float _Spacing = 1;
    public Vector3 _BaseScale;
    public int Max_Height=10;
    public float Speed = 1;
    float step;
    public float Strength = 10f;
    public int Size = 10;
    public int EdgeSize = 2;

    float[] scale;
    bool[] add_sign;
    



    // Start is called before the first frame update
    void Start()
    {
        _FFTGameObjects = new GameObject[Size*Size];
        scale = new float[_FFTGameObjects.Length];
        add_sign = new bool[_FFTGameObjects.Length];
        
        int t = 0;
        step = (float)Speed*Max_Height/_FFTGameObjects.Length;
        for (int i = 0; i < Size; i++){
            for (int j = 0; j < Size; j++)
            {
                GameObject newFFTObject = Instantiate(_ObjectToSpawn);
                newFFTObject.transform.SetParent(transform);
                newFFTObject.transform.localPosition = new Vector3(_Spacing * i+StartPosition[0],0+StartPosition[1], _Spacing * j+StartPosition[2]);
                _FFTGameObjects[t] = newFFTObject;
                scale[t] = (float)step*t;
                add_sign[t] = (scale[t]<Max_Height);
                t+=1;
            }
        }

    }

    private void Update()
    {
        int t = 0;
        for (int i = 0; i < Size; i++){
            for (int j = 0; j < Size; j++){
                if((i>EdgeSize-1)&&(i<(Size-EdgeSize))&&(j>EdgeSize-1)&&(j<(Size-EdgeSize))){
                    _FFTGameObjects[t].transform.localScale = new Vector3(0,0,0);
                }
                else{
                    float s_now;
                    if(add_sign[t]){
                        s_now = scale[t]+(step+_FFT.GetBandValue((i+j)%(int)_FreqBands, _FreqBands)*Strength);
                        add_sign[t] = (s_now<Max_Height);
                        }
                    else{
                        s_now = scale[t]-(step+_FFT.GetBandValue((i+j)%(int)_FreqBands, _FreqBands)*Strength);
                        add_sign[t] = (s_now<0);
                        }
                    scale[t] = s_now;
                    Vector3 change_scale = new Vector3(_BaseScale[0], s_now, _BaseScale[2]);
                    _FFTGameObjects[t].transform.localScale = change_scale;}
                    t++;
                }
        }
    }
}

