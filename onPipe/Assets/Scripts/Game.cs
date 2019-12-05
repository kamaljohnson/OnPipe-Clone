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
    public float pipeSpeed;
    
    public Transform pipeHolder;
    
    public int currentLevel;
    public static int BucketFill;

    public TMP_Text currentLevelText;
    public TMP_Text tapToStartText;
    public Slider currentLevelProgressSlider;

    public static GameStatus GameState;

    public GameObject loadScreen;
    private float _loadScreenCounter;
    public float loadScreenDelay;
    public static bool LoadedPipes;
    
    public void Start()
    {        
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
        ResetGame();
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
    }

    void ResetGame()
    {
        tapToStartText.gameObject.SetActive(true);
    }

    public void OnGameStart()
    {
        tapToStartText.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        GameState = GameStatus.GameOver;
        Time.timeScale = 0.3f;
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
            GameState = GameStatus.AtMenu;
        }
    }
}
