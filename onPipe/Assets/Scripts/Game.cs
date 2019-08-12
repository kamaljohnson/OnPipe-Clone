using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public float pipeSpeed;
    
    public Transform pipeHolder;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pipeHolder.transform.localPosition += new Vector3(0, -Time.deltaTime * pipeSpeed, 0);
    }
}
