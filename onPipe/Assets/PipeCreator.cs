using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GameStatus
{
    Playing,
    AtMenu,
    GameOver
}

public class PipeCreator : MonoBehaviour
{
    private float _pipeSpeed;
    public float obstrecleCreationDelay;
    private float _obstrecleCreationTimer;
    
    public GameObject generalPipe;
    public List<GameObject> obstreclePipe;

    private Transform pipeHolder;
    public Transform pipeCreationSensor;
    public Transform pipeDeletionSensor;

    public Transform createLocation;

    private int _currentGeneralPipeIndex;

    private bool _createPipe;

    public List<float> pipeSizes = new List<float>();
    public List<float> pipeWidths = new List<float>();

    public GameObject ring;

    void Start()
    {
        _pipeSpeed = gameObject.GetComponent<Game>().pipeSpeed;
        pipeHolder = gameObject.GetComponent<Game>().pipeHolder;
        
        _currentGeneralPipeIndex = 0;
        _createPipe = true;
    }

    void Update()
    {
        
        if (checkPipeCreationSensor())
        {
            _createPipe = true;
        }
        else
        {
            _obstrecleCreationTimer += Time.deltaTime;
            if (_obstrecleCreationTimer > obstrecleCreationDelay)
            {
                CreateObstrecle();
            }
        }
        
        if (_createPipe)
        {
            CreatePipe();
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
    
    public void CreatePipe()
    {
        var rand = 0;
        while (rand == _currentGeneralPipeIndex)
        {
            rand = Random.Range(0, pipeWidths.Count);
        }

        _currentGeneralPipeIndex = rand;
        
        var tempPipe = Instantiate(generalPipe, createLocation.position, createLocation.rotation, pipeHolder);
        tempPipe.transform.localScale += new Vector3(
                pipeWidths[rand],
                pipeSizes[Random.Range(0, pipeSizes.Count)], 
                pipeWidths[rand]
                );
    }

    public void CreateObstrecle()
    {
        var tempPipe = Instantiate(generalPipe, createLocation.position, createLocation.rotation, pipeHolder);
        tempPipe.transform.localScale = new Vector3(2, 2, tempPipe.transform.localScale.z);
    }
}