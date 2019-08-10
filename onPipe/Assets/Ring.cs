using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{

    public GameObject ring;

    public float maxRingSize;
    public float ringExpandSpeed;
    public float ringContractSpeed;

    public Transform ringContractionSensor1;
    public Transform ringContractionSensor2;
    public float fitOffsetLimit;
    
    private bool _pressedScreen;
    
    public void Start()
    {
        _pressedScreen = false;
    }

    public void Update()
    {

        _pressedScreen = Input.GetKey(KeyCode.Space);
        
        if (_pressedScreen)
        {
            Contract();
        }
        else
        {
            Expand();
        }
        
    }

    public void Contract()
    {
        if (checkRingContractable())
        {
            Debug.Log("contracting");
            ring.transform.localScale = Vector3.Lerp(ring.transform.localScale, new Vector3(0, 0, ring.transform.localScale.z), ringExpandSpeed * Time.deltaTime);
        }
    }

    public void Expand()
    {
        if (ring.transform.localScale.x < maxRingSize)
        {
            ring.transform.localScale = Vector3.Lerp(ring.transform.localScale, maxRingSize * new Vector3(1, 1, ring.transform.localScale.z/maxRingSize), ringExpandSpeed * Time.deltaTime);
        }
    }

    public bool checkRingContractable()
    {
        if (Physics.Raycast(ringContractionSensor1.transform.position, ringContractionSensor1.transform.forward,
            fitOffsetLimit))
        {
            return false;
        }
        return true;
    }
    
}
