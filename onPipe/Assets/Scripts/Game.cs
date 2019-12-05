using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatus
{
    Playing,
    AtMenu,
    GameOver
}

public class Game : MonoBehaviour
{
    public float pipeSpeed;
    
    public Transform pipeHolder;
    
    public int currentLevel;
    public static int bucketFill;

    public TMP_Text currentLevelText;
    public TMP_Text tapToStartText;
    public Slider currentLevelProgressSlider;

    public static GameStatus gameState;

    public GameObject LoadScreen;
    
    public void Start()
    {
        Destroy(LoadScreen, 7);
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = 1;
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }
        
        currentLevelText.text = currentLevel.ToString();
        gameState = GameStatus.AtMenu;
        ResetGame();
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
        }
        pipeHolder.transform.localPosition += new Vector3(0, -Time.deltaTime * pipeSpeed, 0);
    }

    void ResetGame()
    {
        tapToStartText.gameObject.SetActive(true);
    }

    public void onGameStart()
    {
        tapToStartText.gameObject.SetActive(false);
    }
}
