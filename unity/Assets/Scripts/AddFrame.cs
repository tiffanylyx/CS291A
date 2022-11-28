using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFrame : MonoBehaviour
{
    public Shader shader;
    public GameObject _FrameToSpawn1;
    public GameObject _FrameToSpawn2;
    public GameObject outsideCube;

    List<float> senti_all;
    Vector3 positions;
    public int size;
    public float frame_size;
    public float senti_average;
    int number;

    public float spawn_prob1;
    public float spawn_prob2;

    int wordCount;
    int nounCount;
    int verbCount;

    private loadJSON parser;
    private readonly List<GameObject> graphics = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        parser = gameObject.GetComponent<loadJSON>();
        parser.OnChange.AddListener(GenerateGraphics);
    }

    private void ClearGraphics()
    {
        foreach (var item in graphics)
            Destroy(item);
        graphics.Clear();
    }

    private void GenerateGraphics()
    {
        ClearGraphics();

        // Accumulate features
        wordCount += parser.wordCount;
        nounCount += parser.nounCount;
        verbCount += parser.verbCount;
        senti_all.AddRange(parser.senti_all);

        spawn_prob1 = (float)nounCount / (float)wordCount;
        spawn_prob2 = (float)verbCount / (float)wordCount;
        number = (int)(2 * (float)size / frame_size);
        float p1, p2, p3, p4;
        
        for (int i = 0; i < senti_all.Count; i++)
        {
            senti_average += senti_all[i];
        }
        senti_average = senti_average / senti_all.Count;

        GameObject cube = Instantiate(outsideCube);
        cube.transform.SetParent(transform);
        Material material = new Material(shader);
        Color customColor = Color.HSVToRGB(senti_average, senti_average / 3, senti_average / 2);

        material.SetColor("_BaseColor", customColor);
        cube.GetComponent<MeshRenderer>().material = material;

        for (int r = 0; r < number; r++)
        {
            for (int c = 0; c < number; c++)
            {
                p1 = Random.Range(0.0f, 1.0f);
                if (p1 < spawn_prob1)
                {
                    GameObject newFFTObject = Instantiate(_FrameToSpawn1);
                    newFFTObject.transform.SetParent(transform);
                    newFFTObject.transform.localPosition = new Vector3(-size, r - size + frame_size / 2, c - size + frame_size / 2);
                    graphics.Add(newFFTObject);
                }
                p2 = Random.Range(0.0f, 1.0f);
                if (p2 < spawn_prob1)
                {
                    GameObject newFFTObject = Instantiate(_FrameToSpawn1);
                    newFFTObject.transform.SetParent(transform);
                    newFFTObject.transform.localPosition = new Vector3(size, r - size + 0.5f, c - size + 0.5f);
                    graphics.Add(newFFTObject);
                }
                p3 = Random.Range(0.0f, 1.0f);
                if (p3 < spawn_prob2)
                {
                    GameObject newFFTObject = Instantiate(_FrameToSpawn2);
                    newFFTObject.transform.SetParent(transform);
                    newFFTObject.transform.localPosition = new Vector3(r - size + 0.5f, c - size + 0.5f, size);
                    graphics.Add(newFFTObject);
                }
                p4 = Random.Range(0.0f, 1.0f);
                if (p4 < spawn_prob2)
                {
                    GameObject newFFTObject = Instantiate(_FrameToSpawn2);
                    newFFTObject.transform.SetParent(transform);
                    newFFTObject.transform.localPosition = new Vector3(r - size + 0.5f, c - size + 0.5f, -size);
                    graphics.Add(newFFTObject);
                }
            }
        }
    }
}
