using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using UnityEngine.Events;
using System.Linq;

public class loadJSON : MonoBehaviour
{

    public UnityEvent OnChange = new UnityEvent();

    public List<int> syllables_all = new List<int>();
    public List<float> senti_all = new List<float>();
    public List<string> sentence_all = new List<string>();
    public List<Vector3> sent_vec_all = new List<Vector3>();
    public List<int> sent_length_all = new List<int>();
    public int number;
    public int wordCount;
    public int nounCount;
    public int verbCount;

    int syllables;
    int sent_length;
    float senti;
    string sentence;
    Vector3 sent_vec;

    private const string ElizaResponseKey = "eliza";
    public string ElizaResponse = String.Empty;

    float[] sent_vec1;


    private void Start()
    {
        ReqRep.EventManager.Instance.onResponse.AddListener(Parse);
    }

    private void Parse(string json)
    {
        Debug.Log("Parse");
        try
        {
            ElizaResponse = String.Empty;
            sent_vec1 = new float[3];
            var uni = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);
            foreach (var item in uni)
            {
                Debug.Log($"parsing {item}");
                syllables = Convert.ToInt32(item.Value["syllables"]);
                syllables_all.Add(syllables);
                senti = Convert.ToSingle(item.Value["senti"]);
                senti_all.Add(senti);
                sentence = Convert.ToString(item.Value["sentence"]);
                sentence_all.Add(sentence);
                wordCount += Convert.ToInt32(item.Value["sentence_length"]);

                sent_length = Convert.ToInt32(item.Value["sentence_length"]);
                sent_length_all.Add(sent_length);

                nounCount += Convert.ToInt32(item.Value["noun"]);
                verbCount += Convert.ToInt32(item.Value["verb"]);
                IEnumerable enumerable = item.Value["sent_vec"] as IEnumerable;
                if (enumerable != null)
                {
                    int i = 0;
                    foreach (object element in enumerable)
                    {
                        sent_vec1[i] = Convert.ToSingle(element);
                        i++;
                    }
                }
                sent_vec = new Vector3(sent_vec1[0], sent_vec1[1], sent_vec1[2]);
                sent_vec_all.Add(sent_vec);


                Debug.Log($"parsing {item} eliza");
                ElizaResponse += item.Value[ElizaResponseKey].ToString() +  " ";
                Debug.Log($"parsing {item} eliza ok");

            }
            number = sent_vec_all.Count;
            Debug.Log(number);

            OnChange.Invoke();
        }
        catch (Exception e)
        {
            ElizaResponse = e.ToString();
            OnChange.Invoke();
            Debug.LogError(e.ToString());
        }
    }
}
