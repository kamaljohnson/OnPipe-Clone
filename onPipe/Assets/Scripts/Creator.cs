using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Creator : MonoBehaviour
{
    private float _pipeSpeed;

    public GameObject generalPipe;
    public List<GameObject> obstreclePipe;
    public GameObject fillerFrame;
    public GameObject gameEndRing;
    
    private Transform _pipeHolder;
    public Transform pipeCreationSensor;

    public Transform fillerCreatioinSensor;
    public Transform fillerPipeSizeChangeSensor;
    public Transform fillerCreationLocation;
    private bool _createFiller;
    private bool _fillerCreationStarted;
    public float fillerOffsetDelay;
    public float fillerGapDelay;
    private float _fillerCreationCounter;             
    private float _currentPipeSize;
    
    public Transform obstrecleCreationLocation;
    public Transform obstrecleCreationSensor;
    public float maxObstrecleCreationDelay;
    private float _obstrecleCreationTimer;
    private bool _newObstrecleCreated;
    
    public Transform createLocation;

    private int _currentGeneralPipeIndex;

    private bool _createPipe;

    public List<float> pipeSizes = new List<float>();
    public List<float> pipeWidths = new List<float>();

    public GameObject ring;

    public static bool gameEndShown = false; 

    void Start()
    {
        _pipeSpeed = Game.pipeSpeed;
        _pipeHolder = gameObject.GetComponent<Game>().pipeHolder;
        _newObstrecleCreated = true;
        _currentGeneralPipeIndex = 0;
        _createPipe = true;

        _fillerCreationStarted = false;
        _createFiller = false;
        _fillerCreationCounter = 0;
    }

    void Update()
    {

        switch (Game.gameState)
        {
            case GameStatus.Playing:
                break;
            case GameStatus.AtMenu:
                Game.pipeSpeed = 3f;
                break;
            case GameStatus.GameOver:
                Game.pipeSpeed -= Game.pipeSpeed * Time.deltaTime * 2f;
                if (Game.pipeSpeed < 1f)
                    Game.pipeSpeed = 0f;
                ChangeAllObstrecleType2ToType1();
                return;
            case GameStatus.Loading:
                break;
            case GameStatus.GameWon:
                break;
            case GameStatus.GameWonUi:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        if (CheckPipeCreationSensor())
        {
            CreatePipe();
            _newObstrecleCreated = false;
            _obstrecleCreationTimer = 0;
        }

        CheckFillerCreationSensor();
        
        if (!_newObstrecleCreated)
        {
            _obstrecleCreationTimer += Time.deltaTime;
        }                              
        
        if (_obstrecleCreationTimer > Random.Range(1, maxObstrecleCreationDelay))
        {
            CreateObstrecle();
            _obstrecleCreationTimer = 0;
        }
        
        if (_createFiller)
        {
            if(Game.gameState == GameStatus.GameWonUi || Game.gameState == GameStatus.GameWon)
                return;
            
            _fillerCreationCounter += Time.deltaTime;
            if (!_fillerCreationStarted)
            {
                if (_fillerCreationCounter >= fillerOffsetDelay)
                {
                    _fillerCreationStarted = true;
                    CreateFillers();
                    _fillerCreationCounter = 0;
                }
            }
            else
            {
                if (_fillerCreationCounter >= fillerGapDelay)
                {
                    CreateFillers();
                    _fillerCreationCounter = 0;
                }
            }
        }
        else
        {
            _fillerCreationCounter = 0;
            _fillerCreationStarted = false;
        }
    }

    public bool CheckPipeCreationSensor()
    {
        RaycastHit hit;
//        Debug.DrawRay(pipeCreationSensor.position, pipeCreationSensor.forward * 10, Color.red);
        if (!Physics.Raycast(pipeCreationSensor.position, pipeCreationSensor.forward, out hit, 10))
        {
//            Debug.DrawRay(pipeCreationSensor.position, pipeCreationSensor.forward * 10, Color.green);
            return true;
        }

        return false;
    }

    private void CheckFillerCreationSensor()
    {
        
        if (Physics.Raycast(fillerPipeSizeChangeSensor.position, fillerPipeSizeChangeSensor.forward, out var hit, 10))
        {

            if (hit.collider.CompareTag("Obstrecle"))
            {
                _createFiller = false;
                return;
            }
            _createFiller = true;

            var tempCurrentPipeSize = hit.transform.parent.localScale.z;
            if (Math.Abs(_currentPipeSize - tempCurrentPipeSize) > 0.0001f)
            {
                _currentPipeSize = tempCurrentPipeSize;
                _createFiller = false;
            }
        }
    }
    
    public void CreatePipe()
    {
        var rand = 0;
        while (rand == _currentGeneralPipeIndex)
        {
            rand = Random.Range(0, pipeWidths.Count);
        }

        _currentGeneralPipeIndex = rand;
        if (Game.gameState == GameStatus.GameWon || Game.gameState == GameStatus.GameWonUi)
            _currentGeneralPipeIndex = 0;

        var tempPipe = Instantiate(generalPipe, createLocation.position, createLocation.rotation, _pipeHolder);
        tempPipe.transform.localScale += new Vector3(
                pipeWidths[_currentGeneralPipeIndex],
                pipeSizes[Random.Range(0, pipeSizes.Count)],
                pipeWidths[_currentGeneralPipeIndex]
                );
    }

    public void CreateObstrecle()
    {
        if (Game.gameState == GameStatus.GameWon && !gameEndShown)
        {
            gameEndShown = true;
            var gameEndRing = Instantiate(this.gameEndRing, obstrecleCreationLocation.position, Quaternion.identity, _pipeHolder);
            return;
        }
        
        if (Game.gameState == GameStatus.GameWon || Game.gameState == GameStatus.GameWonUi)
            return;

        var rand = Random.RandomRange(0, 2);
        if (Game.gameState != GameStatus.Playing)
        {
            rand = 0;
        }
        
        var tempPipe = Instantiate(obstreclePipe[rand], obstrecleCreationLocation.position, Quaternion.identity, _pipeHolder);
        
        Physics.Raycast(obstrecleCreationSensor.position, obstrecleCreationSensor.forward, out var hit, 10);
        if (rand == 0)
        {
            tempPipe.transform.localScale = new Vector3(hit.transform.parent.localScale.z + .5f, tempPipe.transform.localScale.y, hit.transform.parent.localScale.z + .5f);    
        }
        _newObstrecleCreated = true;
    }

    public void CreateFillers()
    {
        Physics.Raycast(fillerCreatioinSensor.position, fillerCreatioinSensor.forward, out var hit, 10);
        if (!hit.collider.CompareTag("Obstrecle"))
        {
            var tempPipe = Instantiate(fillerFrame, fillerCreationLocation.position, Quaternion.identity, _pipeHolder);
            tempPipe.transform.GetChild(0).localScale = new Vector3( hit.transform.parent.localScale.z + 0.1f, tempPipe.transform.localScale.y,  hit.transform.parent.localScale.z + 0.1f);
        }
    }

    public void ChangeAllObstrecleType2ToType1()
    {
        
    }
    
}