using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ssss : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    int num;
    // Start is called before the first frame update
    void Start () {
        num =0;
    //InvokeRepeating("SetRandomPos",0,1);
    }

    void SetRandomPos() {
    Vector3 temp = new Vector3(Random.Range(0.0f,200.0f),Random.Range(-0.3f,2f),Random.Range(0.0f,200.0f));
    Vector3 pos = transform.position;
   // transform.position = temp;
    transform.position = Vector3.SmoothDamp(temp,pos,ref velocity, 1000f);
    }
    Vector3 pos;

   


    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        if (num == 15)
        {
            if (pos.x < 0)
        {
            transform.position += new Vector3(Random.Range(0.0f,1.0f), 0, 0);
        }
        if (pos.x > 200)
        {
            transform.position += new Vector3(Random.Range(-1.0f,0.0f), 0, 0);
        }
        if (pos.x < 200 && pos.x > 0)
        {
            transform.position += new Vector3(Random.Range(-1.0f,1.0f), 0, 0);
        }


        if (pos.y < 0)
        {
            transform.position += new Vector3(0, Random.Range(0.0f,0.5f), 0);
        }
        if (pos.y > 2)
        {
            transform.position += new Vector3(0, Random.Range(-0.2f,0.0f), 0);
        }
        if(pos.y > 0 && pos.y < 2)
        {
            transform.position += new Vector3(0, Random.Range(-0.2f,1f), 0);
        }


        if (pos.z < 0)
        {
            transform.position += new Vector3(0, 0, Random.Range(0.0f,0.2f));
        }
        if (pos.z > 200)
        {
            transform.position += new Vector3(0, 0, Random.Range(-1.0f,0.0f));
        }
        if (pos.z < 200 && pos.z > 0)
        {
            transform.position += new Vector3(0, 0, Random.Range(-1.0f,1.0f));
        }
        num = 0;
        }
        else
        {
            num++;
        }

        
        
        Debug.Log(pos);
        
    }
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(transform.position);
        transform.position = pos;
    }
}
