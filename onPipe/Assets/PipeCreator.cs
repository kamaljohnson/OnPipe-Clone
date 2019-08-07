using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PipeType
{
    Genearl,
    Obstrecle
}

public class PipeCreator : MonoBehaviour
{   
    public List<GameObject> generalPipe;
    public List<GameObject> obsteclePipe;

    public Transform createLocation;

    private int _currentGeneralPipeIndex;

    private List<GameObject> _bufferListOfPipes;

    private bool _createPipe;
    
    void Start()
    {
        _bufferListOfPipes = new List<GameObject>();
        _currentGeneralPipeIndex = 0;
        _createPipe = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_createPipe)
        {
            CreatePipe(PipeType.Genearl);
            _createPipe = false;
        }
    }

    public void CreatePipe(PipeType type)
    {
        int rand = 0;
        GameObject tempPipe;
        switch (type)
        {
            case PipeType.Genearl:
                while (rand != _currentGeneralPipeIndex)
                {
                    rand = Random.Range(0, generalPipe.Count);
                }

                tempPipe = Instantiate(generalPipe[rand], createLocation);
                break;
            case PipeType.Obstrecle:
                rand = Random.Range(0, obsteclePipe.Count);
                tempPipe = Instantiate(obsteclePipe[rand], createLocation);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        _bufferListOfPipes.Add(tempPipe);
    }
}