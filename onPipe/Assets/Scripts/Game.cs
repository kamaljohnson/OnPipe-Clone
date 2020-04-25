using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatus
{
    Playing,
    AtMenu,
    GameOver,
    Loading,
    BucketFull,
    GameWon
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
    public GameObject tapToContinue;
    public Slider currentLevelProgressSlider;

    public GameObject gameOverUi;
    public TMP_Text gameOverCurrentLevelText;
    public TMP_Text gameOverCompletedPercentText;
    public TMP_Text gameOverScoreText;
    public TMP_Text gameOverBestScoreText;

    public GameObject gameWonUi;
    public TMP_Text gameWonCurrentLevelText;
    public TMP_Text gameWonScoreText;
    public TMP_Text gameWonBestScoreText;
    
    public static GameStatus gameState;

    public GameObject loadScreen;
    private float _loadScreenCounter;
    public float loadScreenDelay;
    private static bool _loadedPipes;

    public static bool ringLocked;
    private bool _pressed;

    public Ring ring;

    private static int _currentScore;
    private static int _bestScore;

    public UnityVideoAds videoAds;
    public UnityRewardedVideoAds rewardedVideoAds;

    private bool _firstStart;
    
    public void Start()
    {
        ringLocked = true;
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = 1;
            _currentScore = 0;
            _bestScore = 0;
            PlayerPrefs.SetInt("CurrentScore", _currentScore);
            PlayerPrefs.SetInt("BestScore", _bestScore);
            
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            _currentScore = PlayerPrefs.GetInt("CurrentScore");
            _bestScore = PlayerPrefs.GetInt("BestScore");
        }

        currentLevelText.text = currentLevel.ToString();
        gameState = GameStatus.Loading;
        _firstStart = true;
    }

    void Update()
    {
        if (gameState == GameStatus.Playing)
        {
            float multiplier = Mathf.Min(60, currentLevel);
            currentLevelProgressSlider.value = bucketFill * 3f / (100f + multiplier * 10f) ;
            if (currentLevelProgressSlider.value >= 1f)
            {
                BucketFull();
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

    private void CheckStateChange()
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
                case GameStatus.GameWon:
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

    private void ResetGame()
    {
        ring.ResetLocation();
        Creator.gameEndShown = false;
        gameOverUi.SetActive(false);
        gameWonUi.SetActive(false);
        tapToStart.SetActive(true);
        tapToRestart.SetActive(false);
        tapToContinue.SetActive(false);
        Time.timeScale = 1;
        ring.Activate(false);
        AtMenu();
    }

    private void AtMenu()
    {
        if (!_firstStart)
        {
            videoAds.ShowAd();
        }
        _firstStart = false;
        gameState = GameStatus.AtMenu;
    }

    private void StartGame()
    {
        _currentScore = PlayerPrefs.GetInt("CurrentScore");
        gameOverUi.SetActive(false);
        tapToStart.SetActive(false);
        tapToRestart.SetActive(false);
        tapToContinue.SetActive(false);
        gameState = GameStatus.Playing;
        ring.Activate(true);
    }

    private void BucketFull()
    {
        gameState = GameStatus.BucketFull;
    }

    public void GameWon()
    {
        currentLevel++;
        _currentScore += bucketFill;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.SetInt("CurrentScore", _currentScore);
        if (_currentScore > _bestScore)
        {
            _bestScore = _currentScore;
            PlayerPrefs.SetInt("BestScore", _bestScore);
        }
        SetGameWonUi();
        currentLevelText.text = currentLevel.ToString();
        bucketFill = 0;
        gameState = GameStatus.GameWon;
        ring.Deactivate();
    }
    
    public void GameOver()
    {
        _currentScore += bucketFill;

        if (_currentScore > _bestScore)
        {
            _bestScore = _currentScore;
            PlayerPrefs.SetInt("BestScore", _bestScore);
            PlayerPrefs.SetInt("CurrentScore", 0);
        }
        
        SetGameOverUi();
        bucketFill = 0;
        gameState = GameStatus.GameOver;
        Time.timeScale = 0.3f;
        ring.Destruct();
    }

    private void SetGameWonUi()
    {
        gameWonUi.SetActive(true);
        gameWonCurrentLevelText.text = "LEVEL " + currentLevel;
        gameWonScoreText.text = _currentScore.ToString();
        gameWonBestScoreText.text = "BEST " + _bestScore;
        tapToContinue.SetActive(true);
    }

    private void SetGameOverUi()
    {
        gameOverUi.SetActive(true);
        gameOverCurrentLevelText.text = "LEVEL " + currentLevel;
        gameOverCompletedPercentText.text = "COMPLETED " + int.Parse(bucketFill.ToString()) + "%";
        gameOverScoreText.text = _currentScore.ToString();
        gameOverBestScoreText.text = "BEST " + _bestScore;
        tapToRestart.SetActive(true);
    }

    private void HandleLoadScreen()
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
