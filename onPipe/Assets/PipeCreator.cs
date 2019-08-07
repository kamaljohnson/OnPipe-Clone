using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PipeType
{
    General,
    Obstecle
}

public class PipeCreator : MonoBehaviour
{
    public float pipeSpeed;
    
    public List<GameObject> generalPipe;
    public List<GameObject> obstreclePipe;

    public Transform pipeHolder;

    public Transform createLocation;

    private int _currentGeneralPipeIndex;

    private List<GameObject> _bufferListOfPipes;

    private bool _createPipe;
    private float _pipeCreationTimer;
    
    void Start()
    {
        _bufferListOfPipes = new List<GameObject>();
        _currentGeneralPipeIndex = 0;
        _createPipe = true;
        _pipeCreationTimer = 1 / pipeSpeed;
    }

    void Update()
    {
        if (_pipeCreationTimer >= 1 / pipeSpeed)
        {
            CreatePipe(PipeType.General);
            _createPipe = false;
            _pipeCreationTimer = 0;
        }

        _pipeCreationTimer += Time.deltaTime;
    }

    public void CreatePipe(PipeType type)
    {
        int rand = 0;
        GameObject tempPipe;
        switch (type)
        {
            case PipeType.General:
                while (rand == _currentGeneralPipeIndex)
                {
                    rand = Random.Range(0, generalPipe.Count);
                }

                _currentGeneralPipeIndex = rand;
                
                tempPipe = Instantiate(generalPipe[rand], createLocation.position, createLocation.rotation, pipeHolder);
                break;
            case PipeType.Obstecle:
                rand = Random.Range(0, obstreclePipe.Count);
                tempPipe = Instantiate(obstreclePipe[rand], createLocation.position, createLocation.rotation, pipeHolder);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        _bufferListOfPipes.Add(tempPipe);
    }
}