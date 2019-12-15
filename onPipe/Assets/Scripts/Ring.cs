using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Ring : MonoBehaviour
{

    public GameObject ring;

    public float maxRingSize;
    public float ringExpandSpeed;
    public float ringContractSpeed;

    public Transform ringContractionSensor1;
    public float fitOffsetLimit;
    
    private bool _pressedScreen;
    private float _contractLimit;
    private float _preContractLimit = 10;

    public static bool RingClosed = false;
    
    public void Start()
    {
        _pressedScreen = false;
    }

    public void Update()
    {
        if (Game.RingLocked) return;
        
        if (Input.GetKey(KeyCode.Space) || Input.touchCount >= 1)
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
        if (CheckRingContractable())
        {
            ring.transform.localScale = Vector3.Lerp(ring.transform.localScale, new Vector3(_contractLimit, _contractLimit, ring.transform.localScale.z), ringExpandSpeed * Time.deltaTime);
        }

        RingClosed = true;
    }

    public void Expand()
    {
        if (ring.transform.localScale.x < maxRingSize)
        {
            ring.transform.localScale = Vector3.Lerp(ring.transform.localScale, maxRingSize * new Vector3(1, 1, ring.transform.localScale.z/maxRingSize), ringExpandSpeed * Time.deltaTime);
        }

        _preContractLimit = 10;
        
        RingClosed = false;
    }

    public bool CheckRingContractable()
    {
        RaycastHit hit;

        if (Physics.Raycast(ringContractionSensor1.transform.position, ringContractionSensor1.transform.forward, out hit, 20))
        {
            _contractLimit = hit.point.x + fitOffsetLimit;
            Debug.DrawRay(ringContractionSensor1.transform.position, ringContractionSensor1.transform.forward * Vector3.Distance(hit.point, ringContractionSensor1.position), Color.red, 1);
        }
        else
        {
            _contractLimit = 0;
            Debug.DrawRay(ringContractionSensor1.transform.position, ringContractionSensor1.transform.forward * 20, Color.blue, 1);
        }
        
        if (_preContractLimit < _contractLimit)
        {
            FindObjectOfType<Game>().GameOver();
        }
        
        _preContractLimit = _contractLimit;
        
        return true;
    }
}
    