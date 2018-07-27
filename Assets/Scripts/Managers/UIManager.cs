using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GameStates;

public class UIManager : MonoBehaviour
{
    [Header("In game UI text objects")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverKillText;
    public TextMeshProUGUI gameOverTimeText;

    [Header("High Score Panel Text Objects")]
    public TextMeshProUGUI firstPlaceScore;
    public TextMeshProUGUI secondPlaceScore;
    public TextMeshProUGUI thirdPlaceScore;
    public TextMeshProUGUI fourthPlaceScore;
    public TextMeshProUGUI fifthPlaceScore;
    public TextMeshProUGUI bestTime;
    public TextMeshProUGUI totalKills;

    [Header("Panels")]
    public GameObject gamePlayUI;
    public GameObject menuPanel;
    public GameObject startMenu;
    public GameObject optionsMenu;
    public GameObject highScoreMenu;
    public GameObject creditsMenu;
    public GameObject gameOverScreen;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        musicSlider.value = PlayerPrefsManager.GetMusicVolume();
        sfxSlider.value = PlayerPrefsManager.GetSFXVolume();

        UpdateHighScoreDisplay();
    }

    private void Update()
    {
        scoreText.text = GameManager.Instance.playSessionManager.score.ToString();
    }

    public void EnableMenuPanel()
    {
        menuPanel.SetActive(true);
    }

    public void DisableMenuPanel()
    {
        menuPanel.SetActive(false);
    }

    public void OpenStartMenu()
    {
        startMenu.SetActive(true);
    }
    public void OpenOptionsMenu()
    {
        if(GameManager.Instance.playSessionManager.currentGameState != (int)GameState.StartMenu)
        {
            menuPanel.SetActive(true);
        }
        else
        {
            startMenu.SetActive(false);
        }

        optionsMenu.SetActive(true);
    }

    public void OpenCreditsMenu()
    {
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void OpenHighScoreMenu()
    {
        optionsMenu.SetActive(false);
        highScoreMenu.SetActive(true);
    }

    public void GoHome()
    {
        ResetUI();
        GameManager.Instance.playSessionManager.ResetGame();
        GameManager.Instance.playSessionManager.FadeToTargetScene(0);
    }

    public void StartGame()
    {
        GameManager.Instance.playSessionManager.FadeToTargetScene(2);
    }

    public void CloseCredits()
    {
        creditsMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        if(GameManager.Instance.playSessionManager.currentGameState == (int)GameState.Paused)
        {
            optionsMenu.SetActive(false);
            menuPanel.SetActive(false);
            GameManager.Instance.playSessionManager.currentGameState = (int)GameState.GamePlay;
        }
        else
        {
            optionsMenu.SetActive(false);
            startMenu.SetActive(true);
        }
    }

    public void CloseHighScore()
    {
        highScoreMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ResetUI()
    {
        menuPanel.SetActive(false);
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        highScoreMenu.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    public void PlayConfirm()
    {
        GameManager.Instance.audioManager.Play("UI Confirm");
    }

    public void PlayClose()
    {
        GameManager.Instance.audioManager.Play("UI Close");
    }

    public void DisplayGameOver()
    {
        ResetUI();
        menuPanel.SetActive(true);
        gameOverScreen.SetActive(true);

    }

    public void RestartLevel()
    {
        GameManager.Instance.playSessionManager.resetStats = true;
        GameManager.Instance.playSessionManager.QueueGameScene();
    }

    public void UpdateHighScoreDisplay()
    {
        firstPlaceScore.text = PlayerPrefsManager.GetFirstPlace().ToString();
        secondPlaceScore.text = PlayerPrefsManager.GetSecondPlace().ToString();
        thirdPlaceScore.text = PlayerPrefsManager.GetThirdPlace().ToString();
        fourthPlaceScore.text = PlayerPrefsManager.GetFourthPlace().ToString();
        fifthPlaceScore.text = PlayerPrefsManager.GetFifthPlace().ToString();
        bestTime.text = TimeConverter(PlayerPrefsManager.GetBestTime());
        totalKills.text = PlayerPrefsManager.GetEnemyKillTotal().ToString();
    }

    public void ReadyBlink()
    {
        animator.SetTrigger("ReadyBlink");
    }

    public void UpdateGameOverPanel(int score, int kills, float time)
    {
        gameOverScoreText.text = score.ToString();
        gameOverKillText.text = kills.ToString();
        gameOverTimeText.text = TimeConverter(time);
    }

    public string TimeConverter(float time)
    {
        string converted = "";
        if(time < 60)
        {
            converted = (int)time + " Sec";
        }
        else
        {
            int min = (int)(time / 60.0f);
            int sec = (int)(time % 60);

            converted = min + " Min " + sec + " Sec";
        }

        return converted;
    }
}
