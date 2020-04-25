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
    
    private float _contractLimit;
    private float _preContractLimit = 10;

    public static bool RingClosed = false;

    public static bool IsActive;

    public Game game;

    public Animator ringAnimator;
    
    public void Update()
    {
        if (Game.ringLocked)
        {
            Expand();

            return;
        }
        
        if ((Input.GetKey(KeyCode.Space) || Input.touchCount >= 1) && !(Game.gameState == GameStatus.AtMenu || Game.gameState == GameStatus.GameWon))
        {
            Contract();
        }
        else
        {
            Expand();
        }

        if (Game.gameState == GameStatus.BucketFull)
        {
            FinishingMove();
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
        }
        else
        {
            _contractLimit = 0;
        }
        
        
        if (_preContractLimit < _contractLimit)
        {
            game.GameOver();
        }
        
        _preContractLimit = _contractLimit;
        
        return true;
    }

    public void FinishingMove()
    {
        ringAnimator.Play("FinishingMoveAnimation", -1, 0);
    }

    public void ResetLocation()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
    }
    
    public void Activate(bool flag)
    {
        gameObject.SetActive(true);
        if(!flag)
            return;
        
        IsActive = true;
        Game.ringLocked = false;
        ring.transform.localScale = new Vector3(maxRingSize, maxRingSize, ring.transform.localScale.z);
    }

    public void Deactivate()
    {
        IsActive = false;
    }
    
    public void Destruct()
    {
        Deactivate();
        Game.ringLocked = true;
        gameObject.SetActive(false);
    }
}
    