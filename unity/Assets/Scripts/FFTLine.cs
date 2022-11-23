using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FFTLine : MonoBehaviour
{
    public enum Shape
    {
        Line,
        Circle,
    }

    public Shape _Shape = Shape.Line;

    public FrequencyBandAnalyser _FFT;
    public FrequencyBandAnalyser.Bands _FreqBands = FrequencyBandAnalyser.Bands.SixtyFour;

    LineRenderer _Line;
    public float _LineLengthRadius = 2;
    float _Spacing = .2f;
    public float _Strength = 1;
    public int yPos = 3;
    private float timer = 0.0f;
    public float ChangeingSpeed = 1.0f;
    public float SpeedStrength = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        _Line = GetComponent<LineRenderer>();

        if(_FreqBands == FrequencyBandAnalyser.Bands.Eight)
        {
            _Line.positionCount = 8;
        }
        else
        {
            _Line.positionCount = 64;
        }

        _Spacing = _LineLengthRadius / _Line.positionCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (_Shape == Shape.Line)
        {
            for (int i = 0; i < _Line.positionCount; i++)
            {
                float normPosition = i / (float)_Line.positionCount;
                float xPos = i * _Spacing;
                float zPos = _FFT.GetBandValue(i, _FreqBands) * _Strength;

                Vector3 pos = new Vector3(xPos, yPos, zPos);

                _Line.SetPosition(i, pos);
            }
        }
        else if (_Shape == Shape.Circle)
        {
            float angleSpacing = (2f * Mathf.PI) / (int)_FreqBands;
            //---   CIRCULAR
            for (int i = 0; i < _Line.positionCount; i++)
            {
            float speed = _FFT.GetBandValue(3, _FreqBands)*SpeedStrength+10;
            timer += Time.deltaTime;
                float frequencyStrength = _FFT.GetBandValue(i, _FreqBands) * _Strength;
                float y = yPos + frequencyStrength;

                float angle = i * angleSpacing-timer/(ChangeingSpeed*speed);
                float x = Mathf.Sin(angle) * _LineLengthRadius;
                float z = Mathf.Cos(angle) * _LineLengthRadius;

                Vector3 pos = new Vector3(x, y, z);

                _Line.SetPosition(i, pos);
            }
        }
    }
}
