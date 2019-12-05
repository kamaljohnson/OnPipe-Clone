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

    public static bool RingClosed = false;
    
    public void Start()
    {
        _pressedScreen = false;
    }

    public void Update()
    {
        if (Game.GameState == GameStatus.Loading) return;
        if (Application.isEditor)
        {
            _pressedScreen = Input.GetKey(KeyCode.Space);
        }
        else
        {
            _pressedScreen = Input.touchCount >= 1;
        }

        if (_pressedScreen)
        {
            if (Game.GameState == GameStatus.AtMenu)
            {
                Game.GameState = GameStatus.Playing;
                FindObjectOfType<Game>().OnGameStart();
            }
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
        return true;
    }

    public void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<Game>().GameOver();
    }
}
    