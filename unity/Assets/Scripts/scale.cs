using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scale : MonoBehaviour
{
    private float mSize = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Scale", 0.0f, 0.01f);
    }

    // Update is called once per frame
    void Scale()
    {
        if(mSize >= 100.0f)
        {
            CancelInvoke("Scale");
        }
        GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize++);
    }
}
