using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameStates;

namespace GameStates
{
    public enum GameState {  StartMenu, GamePlay, Paused  };
}

public class PlaySessionManager : MonoBehaviour
{
    public float timeScaleReduction = 0.1f;
    public int score = 0;
    public float totalTimePlayed = 0.0f;
    public float difficultyOffsetIncrement = 0.5f;
    public float timeScale;
    public Animator animator;
    public int targetScene;
    public int currentGameState = 0;
    public bool playerDead = false;
    public bool resetStats = false;
    public bool levelStart = false;
    public int killCount = 0;

    [Header("Player Pickups")]
    public GameObject shieldPickup;
    public GameObject weaponPickup;
    [Range(0.0f, 1.0f)]
    public float shieldPickupSpawnrate = 0.5f;
    [Range(0.0f, 1.0f)]
    public float weaponPickupSpawnrate = 0.5f;

    [HideInInspector]
    public float gameStartTime = 0.0f;
    [HideInInspector]
    public float difficultyOffset = 0.0f;
    [HideInInspector]
    public Vector3 worldSize;

    private List<int> highScores = new List<int>();
    private float gameTimeElapsed = 0.0f;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void Start ()
    {
        LoadHighScores();
        Time.timeScale = timeScale;
        worldSize = GameManager.Instance.cameraControl.myCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));

        GameManager.Instance.poolManager.CreatePool(shieldPickup, 5);
        GameManager.Instance.poolManager.CreatePool(weaponPickup, 5);
        GameManager.Instance.cameraControl.gameObject.SetActive(false);

    }

    private void Update()
    {
        if(currentGameState == (int)GameState.Paused)
        {
            Time.timeScale = 0.0f;
        }
        else if(!playerDead)
        {
            Time.timeScale = 1.0f;
        }

        if(levelStart)
        {
            gameTimeElapsed = Time.time - gameStartTime;
        }
        
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        FadeIn();
        GameManager.Instance.uiManager.ResetUI();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Time.timeScale = 1;
            currentGameState = (int)GameState.StartMenu;
            GameManager.Instance.audioManager.StopAllMusic();
            GameManager.Instance.audioManager.Play("Music Menu");
            GameManager.Instance.audioManager.FadeStandardLevelsIn(-1.0f);
            Invoke("FadeToNextScene", 2.0f);
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            playerDead = false;
            GameManager.Instance.audioManager.StopAllMusic();
            GameManager.Instance.audioManager.Play("Music Game");
            GameManager.Instance.audioManager.FadeStandardLevelsIn(-1.0f);
            GameManager.Instance.cameraControl.gameObject.SetActive(true);
            GameManager.Instance.uiManager.gamePlayUI.SetActive(true);
            
        }
        else
        {
            GameManager.Instance.uiManager.gamePlayUI.SetActive(false);
            GameManager.Instance.enemyManager.waveSpawnStart = false;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            currentGameState = (int)GameState.StartMenu;
            
            GameManager.Instance.uiManager.EnableMenuPanel();
            GameManager.Instance.uiManager.OpenStartMenu();

        }

    }

    public void CallSpawnPlayer()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            GameManager.Instance.playerManager.SpawnPlayer();
        }
    }

    public void StartTheLevel()
    {
        levelStart = true;
        gameStartTime = Time.time;
    }

    public void UpdateScore(int points)
    {
        if (!playerDead) score += points;
    }

    public void FadeToTargetScene(int scene)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            GameManager.Instance.audioManager.FadeMusicOut(1.0f);

        }
        targetScene = scene;
        if (animator != null) animator.SetBool("FadeOut", true);
    }

    void FadeIn()
    {
        if(animator != null) animator.SetBool("FadeOut", false);
    }

    void FadeToNextScene()
    {
        FadeToTargetScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void LoadTargetScene()
    {
        if (resetStats) ResetGame();
        SceneManager.LoadScene(targetScene);
    }

    public void QueueGameScene()
    {
        FadeToTargetScene(2);
    }

    public void TogglePause()
    {
        if(currentGameState != (int)GameState.Paused && !playerDead)
        {
            currentGameState = (int)GameState.Paused;
            GameManager.Instance.uiManager.OpenOptionsMenu();
        }
    }

    public void PlayerDead()
    {
        playerDead = true;
        levelStart = false;
        Time.timeScale = 0.1f;
        GameManager.Instance.uiManager.UpdateGameOverPanel(score, killCount, gameTimeElapsed);
        UpdateStats();
        UpdateHighScores();
        StartCoroutine(GameOverScreen());
        
    }


    IEnumerator GameOverScreen()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        Time.timeScale = 0;
        GameManager.Instance.uiManager.DisplayGameOver();
    }

    public void ResetGame()
    {
        score = 0;
        gameTimeElapsed = 0.0f;
        killCount = 0;
        GameManager.Instance.enemyManager.StopAllCoroutines();
        GameManager.Instance.enemyManager.waveSpawnStart = false;
        GameManager.Instance.poolManager.DisableAllPoolObjects();
        resetStats = false;
    }
    
    private void LoadHighScores()
    {
        highScores.Add(PlayerPrefsManager.GetFirstPlace());
        highScores.Add(PlayerPrefsManager.GetSecondPlace());
        highScores.Add(PlayerPrefsManager.GetThirdPlace());
        highScores.Add(PlayerPrefsManager.GetFourthPlace());
        highScores.Add(PlayerPrefsManager.GetFifthPlace());
        highScores.Sort();
    }

    void UpdateHighScores()
    {
        if (score < highScores[0]) return;
        highScores.Add(score);
        highScores.Sort();
        highScores.RemoveAt(0);
        SaveHighScores();
        GameManager.Instance.uiManager.UpdateHighScoreDisplay();
    }

    void UpdateStats()
    {
        if(gameTimeElapsed > PlayerPrefsManager.GetBestTime())
        {
            PlayerPrefsManager.SetBestTime(gameTimeElapsed);
        }

        PlayerPrefsManager.SetEnemyKillTotal(PlayerPrefsManager.GetEnemyKillTotal() + killCount);
    }

    void SaveHighScores()
    {
        PlayerPrefsManager.SetFirstPlace(highScores[4]);
        PlayerPrefsManager.SetSecondPlace(highScores[3]);
        PlayerPrefsManager.SetThirdPlace(highScores[2]);
        PlayerPrefsManager.SetFourthPlace(highScores[1]);
        PlayerPrefsManager.SetFifthPlace(highScores[0]);
    }
}
