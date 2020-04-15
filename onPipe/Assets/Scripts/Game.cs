using TMPro;
using UnityEngine;
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
    
    public void Start()
    {
        ringLocked = true;
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = 1;
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        
        currentLevelText.text = currentLevel.ToString();
        gameState = GameStatus.Loading;
    }

    void Update()
    {
        if (gameState == GameStatus.Playing)
        {
            currentLevelProgressSlider.value = (float) bucketFill / 100;
            if (bucketFill == 100)
            {
                bucketFill = 0;
                currentLevel++;
                currentLevelText.text = currentLevel.ToString();
                PlayerPrefs.SetInt("CurrentLevel", currentLevel);
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
        gameOverUi.SetActive(false);
        gameState = GameStatus.AtMenu;
        tapToStart.SetActive(true);
        tapToRestart.SetActive(false);
        Time.timeScale = 1;
        ring.Activate(false);
    }

    public void StartGame()
    {
        gameOverUi.SetActive(false);
        tapToStart.SetActive(false);
        tapToRestart.SetActive(false);
        gameState = GameStatus.Playing;
        ring.Activate(true);
    }

    public void GameOver()
    {
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
        gameOverScoreText.text = "0";
        gameOverBestScoreText.text = "BEST 0";
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
