using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObject : MonoBehaviour
{
    public GameObject _ObjectToSpawn;

    Vector3 positions;
    Vector3 quaternions;
    Vector3 positions_org = new Vector3(0, 0, 0);
    public float r;

    List<int> syllables_all;
    List<float> senti_all;
    List<string> sentence_all;

    List<Vector3> sent_vec_all;
    List<int> sent_length_all;
    int numberToSpawn;
    int sent_length;

    int syllables;
    float senti;
    string sentence;
    Vector3 sent_vec;

    private loadJSON parser;


    void Start()
    {
        parser = gameObject.GetComponent<loadJSON>();
        parser.OnChange.AddListener(GenerateGraphics);
    }

    private void GenerateGraphics()
    {
        sent_vec_all = parser.sent_vec_all;

        senti_all = parser.senti_all;

        numberToSpawn = parser.number;

        sent_length_all = parser.sent_length_all;


        for (int i = 0; i < numberToSpawn; i++)
        {
            positions_org = sent_vec_all[i];
            senti = senti_all[i];

            sent_length = sent_length_all[i];
            Color customColor = Color.HSVToRGB(senti, senti, senti);
            Debug.Log(customColor);

            int i1 = Random.Range(1, 4);
            int i2 = 2 * Random.Range(0, 2) - 1;
            if (i1 == 1)
            {
                positions = new Vector3(r * i2, r * positions_org[1], r * positions_org[2]);
                if (i2 == 1)
                {
                    quaternions = new Vector3(0, 0, 90);
                }
                else
                {
                    quaternions = new Vector3(0, 0, -90);
                }
            }
            else if (i1 == 2)
            {
                positions = new Vector3(r * positions_org[0], r * i2, r * positions_org[2]);
                if (i2 == 1)
                {
                    quaternions = new Vector3(0, 0, 180);
                }
                else
                {
                    quaternions = new Vector3(0, 0, 0);
                }
            }
            else if (i1 == 3)
            {
                positions = new Vector3(r * positions_org[0], r * positions_org[1], r * i2);
                if (i2 == 1)
                {
                    quaternions = new Vector3(-90, 0, 0);
                }
                else
                {
                    quaternions = new Vector3(90, 0, 0);
                }
            }
            GameObject newFFTObject = Instantiate(_ObjectToSpawn);
            newFFTObject.transform.SetParent(transform);
            newFFTObject.transform.localPosition = positions;
            newFFTObject.transform.rotation = Quaternion.Euler(quaternions);
            newFFTObject.transform.localScale = new Vector3((float)sent_length / 5.0f, (float)sent_length / 5.0f, (float)sent_length / 5.0f);

            if (i <= 1)
            {
                newFFTObject.GetComponent<AudioShapeSurface>().shape = i;
            }
            else
            {
                newFFTObject.GetComponent<AudioShapeSurface>().shape = i + 2;
            }
            Material material = new Material(Shader.Find("Shader Graphs/GlassShader"));
            material.SetColor("_TintColor", customColor);
            //Material material = newFFTObject.GetComponent<MeshRenderer>().sharedMaterial;
            newFFTObject.GetComponent<MeshRenderer>().material = material;

        }

    }
}
