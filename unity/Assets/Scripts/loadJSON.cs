using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
public class loadJSON : MonoBehaviour
{
    public List<int> syllables_all = new List<int>();
    public List<float>  senti_all = new List<float>();
    public List<string>  sentence_all = new List<string>();
    public List<Vector3>  sent_vec_all = new List<Vector3>();
    public List<int>  sent_length_all = new List<int>();
    public int number;
    public int wordCount;
    public int nounCount;
    public int verbCount;

    int syllables;
    int sent_length;
    float senti;
    string sentence;
    Vector3 sent_vec;

    float[] sent_vec1;
    // Start is called before the first frame update
    void Awake()
    {
        sent_vec1 = new float[3];  
        var json = @"
        {""0"": 
        {""syllables"": 4, 
        ""senti"": 0.5, 
        ""sent_vec"": [0.9959597923198767, -0.08773115355083885, 0.019166031899883623], 
        ""sentence"": ""this is a domo"",
        ""sentence_length"": 4, 
        ""noun"": 1, 
        ""verb"": 1},
        ""1"": 
        {""syllables"": 7, 
        ""senti"": 0.9, 
        ""sent_vec"": [0.13886386766965236, 0.9746676040134301, 0.17532794398653306], 
        ""sentence"": ""i am happy to meet you today"", 
        ""sentence_length"": 7, 
        ""noun"": 2, 
        ""verb"": 2}, 
        ""2"": 
        {""syllables"": 9, 
        ""senti"": 1.0, 
        ""sent_vec"": [-0.8916486764117253, -0.4452784632911229, 0.08179075731593577], 
        ""sentence"": ""i went to the museum with my best friend"", 
        ""sentence_length"": 9, 
        ""noun"": 2, 
        ""verb"": 1}, 
        ""3"":
        {""syllables"": 7,
        ""senti"": 0.925, 
        ""sent_vec"": [-0.1601836035738476, -0.6181071559273247, 0.7696003878231419], 
        ""sentence"": ""it is a beautiful day for me"", 
        ""sentence_length"": 7, 
        ""noun"": 1, 
        ""verb"": 1}}";
        var uni = JsonConvert.DeserializeObject<Dictionary<string,Dictionary<string, object>>>(json);
        foreach (var item in uni)
        {
            
            syllables = Convert.ToInt32(item.Value["syllables"]);
            syllables_all.Add(syllables);
            senti = Convert.ToSingle(item.Value["senti"]);
            senti_all.Add(senti);
            sentence = Convert.ToString(item.Value["sentence"]);
            sentence_all.Add(sentence);
            wordCount+= Convert.ToInt32(item.Value["sentence_length"]);

            sent_length = Convert.ToInt32(item.Value["sentence_length"]);
            sent_length_all.Add(sent_length);

            nounCount+=Convert.ToInt32(item.Value["noun"]);
            verbCount+=Convert.ToInt32(item.Value["verb"]);
            IEnumerable enumerable = item.Value["sent_vec"] as IEnumerable;
            if (enumerable != null)
            {
                    int i = 0;
                    foreach(object element in enumerable)
                    {
                        sent_vec1[i] = Convert.ToSingle(element);
                        i++;
                    }
            } 
            sent_vec = new Vector3(sent_vec1[0],sent_vec1[1],sent_vec1[2]);
            sent_vec_all.Add(sent_vec);
            }
            /*
            if(item.Key=="syllables"){
                
                syllables = Convert.ToInt32(item.Value);
            }
            else if(item.Key=="senti"){
                senti = Convert.ToSingle(item.Value);
            }   
            else if(item.Key=="sentence"){
                sentence = Convert.ToString(item.Value);
            }       
            else if(item.Key=="sent_vec"){
                sent_vec1 = new float[3];  
                IEnumerable enumerable = item.Value as IEnumerable;
                if (enumerable != null)
                {
                    int i = 0;
                    foreach(object element in enumerable)
                    {
                        sent_vec1[i] = Convert.ToSingle(element);
                        i++;
                    }
            } 
            sent_vec = new Vector3(sent_vec1[0],sent_vec1[1],sent_vec1[2]);
            }
            */
            number = sent_vec_all.Count;
            Debug.Log(number);
        }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
