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

    public Transform ringContractionSensor;
    public float fitOffsetLimit;
    
    private bool _pressedScreen;
    private float _contractLimit;
    private float _preContractLimit = 10;

    public static bool RingClosed = false;

    public static bool IsActive;

    public Game game;
    
    public void Start()
    {
        _pressedScreen = false;
    }

    public void Update()
    {
        if (Game.ringLocked) return;
        
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
        if (CheckRingContractible())
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

    private bool CheckRingContractible()
    {
        if (Physics.Raycast(ringContractionSensor.transform.position, ringContractionSensor.transform.forward, out var hit, 20))
        {
            _contractLimit = -hit.point.z + fitOffsetLimit;
            Debug.DrawRay(ringContractionSensor.transform.position, ringContractionSensor.transform.forward * Vector3.Distance(hit.point, ringContractionSensor.position), Color.red, 0);
        }
        else
        {
            _contractLimit = 0;
            Debug.DrawRay(ringContractionSensor.transform.position, ringContractionSensor.transform.forward * 20, Color.blue, 0);
        }
        
        
        if (_preContractLimit < _contractLimit)
        {
            Debug.Log("-> " + _preContractLimit + " : " + _contractLimit);
            game.GameOver();
        }
        
        _preContractLimit = _contractLimit;
        
        return true;
    }

    public void Activate()
    {
        IsActive = true;
        Game.ringLocked = false;
        ring.transform.localScale = new Vector3(maxRingSize, maxRingSize, ring.transform.localScale.z);
        gameObject.SetActive(true);
    }
    
    public void Destruct()
    {
        IsActive = false;
        Game.ringLocked = true;
        gameObject.SetActive(false);
    }
}
    