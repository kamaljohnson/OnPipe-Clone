using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum GameStatus
{
    Playing,
    AtMenu,
    GameOver,
    Loading,
    GameWon,
    GameWonUi
}

public class Game : MonoBehaviour
{
    public static float pipeSpeed = 3f;
    
    public Transform pipeHolder;
    
    public int currentLevel;
    public static int bucketFill;

    public TMP_Text currentLevelText;
    public GameObject tapToStart;
    public GameObject tapToRestart;
    public Slider currentLevelProgressSlider;

    public GameObject gameOverUi;
    public TMP_Text gameOverCurrentLevelText;
    public TMP_Text gameOverCompletedPercentText;
    public TMP_Text gameOverScoreText;
    public TMP_Text gameOverBestScoreText;

    public static GameStatus gameState;

    public GameObject loadScreen;
    private float _loadScreenCounter;
    public float loadScreenDelay;
    private static bool _loadedPipes;

    public static bool ringLocked;
    private bool _pressed;

    public Ring ring;

    public static int currentScore;
    public static int bestScore;

    public void Start()
    {
        ringLocked = true;
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = 1;
            currentScore = 0;
            bestScore = 0;
            PlayerPrefs.SetInt("CurrentScore", currentScore);
            PlayerPrefs.SetInt("BestScore", bestScore);
            
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            currentScore = PlayerPrefs.GetInt("CurrentScore");
            bestScore = PlayerPrefs.GetInt("BestScore");
        }
        
        currentLevelText.text = currentLevel.ToString();
        gameState = GameStatus.Loading;
    }

    void Update()
    {
        if (gameState == GameStatus.Playing)
        {
            currentLevelProgressSlider.value = (float) bucketFill / (50 + 5 * currentLevel);
            if (bucketFill >= 50 + 5 * currentLevel)
            {
                GameWon();
            }
        } else if (gameState == GameStatus.AtMenu)
        {
            currentLevelProgressSlider.value = 0;
        }

        pipeHolder.transform.localPosition += new Vector3(0, -Time.deltaTime * pipeSpeed, 0);
        if (gameState == GameStatus.Loading)
        {
            HandleLoadScreen();
        }

        if (gameState != GameStatus.Playing)
        {
            CheckStateChange();
        }

        if (gameState == GameStatus.GameWonUi)
        {
            GameWonUi();
        }
    }

    public void GameWon()
    {
        gameState = GameStatus.GameWon;
        currentLevelText.text = currentLevel.ToString();
        PlayerPrefs.SetInt("CurrentScore", currentScore);
    }

    public void GameWonUi()
    {
        Debug.Log("GameWonUi");
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        gameState = GameStatus.GameWonUi;
        bucketFill = 0;
    }
    
    public void CheckStateChange()
    {
        if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount >= 1 && !_pressed))
        {
            switch (gameState)
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
        Creator.gameEndShown = false;
        gameOverUi.SetActive(false);
        gameState = GameStatus.AtMenu;
        tapToStart.SetActive(true);
        tapToRestart.SetActive(false);
        Time.timeScale = 1;
        ring.Activate(false);
    }

    public void StartGame()
    {
        currentScore = PlayerPrefs.GetInt("CurrentScore");
        gameOverUi.SetActive(false);
        tapToStart.SetActive(false);
        tapToRestart.SetActive(false);
        gameState = GameStatus.Playing;
        ring.Activate(true);
    }

    public void GameOver()
    {
        currentScore += bucketFill;

        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.SetInt("CurrentScore", 0);
        }
        
        SetGameOverUi();
        bucketFill = 0;
        tapToRestart.SetActive(true);
        gameState = GameStatus.GameOver;
        Time.timeScale = 0.3f;
        ring.Destruct();
        
    }

    public void SetGameOverUi()
    {
        gameOverUi.SetActive(true);
        gameOverCurrentLevelText.text = "LEVEL " + currentLevel;
        gameOverCompletedPercentText.text = "COMPLETED " + int.Parse(bucketFill.ToString()) + "%";
        gameOverScoreText.text = currentScore.ToString();
        gameOverBestScoreText.text = "BEST " + bestScore;
    }
    
    public void HandleLoadScreen()
    {
        gameState = GameStatus.Loading;
        _loadScreenCounter+=Time.deltaTime;
        if (_loadScreenCounter >= loadScreenDelay)
        {
            loadScreen.SetActive(false);
            _loadedPipes = true;
            _loadScreenCounter = 0;
            ResetGame();
        }
    }
    
}
