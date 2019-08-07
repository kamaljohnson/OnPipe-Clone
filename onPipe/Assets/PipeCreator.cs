using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PipeType
{
    General,
    Obstecle
}

public class PipeCreator : MonoBehaviour
{
    private float _pipeSpeed;
    
    public List<GameObject> generalPipe;
    public List<GameObject> obstreclePipe;

    private Transform pipeHolder;
    public Transform pipeCreationSensor;
    public Transform pipeDeletionSensor;

    public Transform createLocation;

    private int _currentGeneralPipeIndex;

    private List<GameObject> _bufferListOfPipes;

    private bool _createPipe;

    public List<float> pipeSizes = new List<float>();

    void Start()
    {
        _pipeSpeed = gameObject.GetComponent<Game>().pipeSpeed;
        pipeHolder = gameObject.GetComponent<Game>().pipeHolder;
        
        _bufferListOfPipes = new List<GameObject>();
        _currentGeneralPipeIndex = 0;
        _createPipe = true;
    }

    void Update()
    {

        if (checkPipeCreationSensor())
        {
            _createPipe = true;
        }
        
        if (_createPipe)
        {
            CreatePipe(PipeType.General);
            _createPipe = false;
        }
        
        checkPipeDeletionSensor();
    }

    public void checkPipeDeletionSensor()
    {
        RaycastHit hit;
        Debug.DrawRay(pipeDeletionSensor.position, pipeDeletionSensor.forward * 10, Color.green);
        if (Physics.Raycast(pipeDeletionSensor.position, pipeDeletionSensor.forward, out hit, 10))
        {
            Debug.DrawRay(pipeDeletionSensor.position, pipeDeletionSensor.forward * 10, Color.red);
            Destroy(hit.collider.gameObject.transform.parent.gameObject);
        }
    }
    
    public bool checkPipeCreationSensor()
    {
        RaycastHit hit;
        Debug.DrawRay(pipeCreationSensor.position, pipeCreationSensor.forward * 10, Color.red);
        if (!Physics.Raycast(pipeCreationSensor.position, pipeCreationSensor.forward, out hit, 10))
        {
            Debug.DrawRay(pipeCreationSensor.position, pipeCreationSensor.forward * 10, Color.green);
            return true;
        }

        return false;
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
                tempPipe.transform.localScale += new Vector3(0, pipeSizes[Random.Range(0, pipeSizes.Count)], 0);
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