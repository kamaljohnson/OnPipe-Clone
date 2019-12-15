using System;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum GameStatus
{
    Playing,
    AtMenu,
    GameOver,
    Loading
}

public class Game : MonoBehaviour
{
    public float pipeSpeed;
    
    public Transform pipeHolder;
    
    public int currentLevel;
    public static int BucketFill;

    public TMP_Text currentLevelText;
    public GameObject tapToStart;
    public GameObject tapToRestart;
    public Slider currentLevelProgressSlider;

    public static GameStatus GameState;

    public GameObject loadScreen;
    private float _loadScreenCounter;
    public float loadScreenDelay;
    public static bool LoadedPipes;

    public static bool RingLocked;
    private bool _pressed;

    public GameObject ring;
    
    public void Start()
    {
        RingLocked = true;
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = 1;
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        
        currentLevelText.text = currentLevel.ToString();
        GameState = GameStatus.Loading;
    }

    void Update()
    {
        if (GameState == GameStatus.Playing)
        {
            currentLevelProgressSlider.value = (float) BucketFill / 100;
            if (BucketFill == 100)
            {
                BucketFill = 0;
                currentLevel++;
                currentLevelText.text = currentLevel.ToString();
                PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            }
        }
        
        pipeHolder.transform.localPosition += new Vector3(0, -Time.deltaTime * pipeSpeed, 0);
        if (GameState == GameStatus.Loading)
        {
            HandleLoadScreen();
        }

        if (GameState != GameStatus.Playing)
        {
            CheckStateChange();
        }
    }
    
    public void CheckStateChange()
    {
        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount >= 1 && !_pressed))
        {
            Debug.Log("pressed");
            switch (GameState)
            {
                case GameStatus.AtMenu:
                    StartGame();
                    break;
                case GameStatus.GameOver:
                    ResetGame();
                    break;
            }
            _pressed = true;
        }

        if (Input.touchCount < 1)
        {
            _pressed = false;
        }
    }

    public void ResetGame()
    {
        GameState = GameStatus.AtMenu;
        tapToStart.SetActive(true);
        tapToRestart.SetActive(false);
        Time.timeScale = 1;
        RingLocked = false;
        ring.SetActive(true);
    }

    public void StartGame()
    {
        tapToStart.SetActive(false);
        tapToRestart.SetActive(false);
        RingLocked = false;
        GameState = GameStatus.Playing;
    }

    public void GameOver()
    {
        BucketFill = 0;
        tapToRestart.SetActive(true);
        GameState = GameStatus.GameOver;
        Time.timeScale = 0.3f;
        RingLocked = true;
        ring.SetActive(false);
    }

    public void HandleLoadScreen()
    {
        GameState = GameStatus.Loading;
        _loadScreenCounter+=Time.deltaTime;
        if (_loadScreenCounter >= loadScreenDelay)
        {
            loadScreen.SetActive(false);
            LoadedPipes = true;
            _loadScreenCounter = 0;
            ResetGame();
        }
    }
}
