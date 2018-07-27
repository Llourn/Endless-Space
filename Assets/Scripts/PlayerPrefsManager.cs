using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    const string MUSIC_VOLUME_KEY = "music_volume";
    const string SFX_VOLUME_KEY = "sfx_volume";
    const string FIRST_PLACE_HIGHSCORE = "first_place";
    const string SECOND_PLACE_HIGHSCORE = "second_place";
    const string THIRD_PLACE_HIGHSCORE = "third_place";
    const string FOURTH_PLACE_HIGHSCORE = "fourth_place";
    const string FIFTH_PLACE_HIGHSCORE = "fifth_place";
    const string ENEMY_KILL_TOTAL = "enemy_kill_total";
    const string BEST_TIME = "best_time";
    const string LAUNCHED_BEFORE = "first_time_launching";
    

    public static void SetMusicVolume(float volume)
    {
        if(volume >= -80.0f && volume <= 20.0f)
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Music volume out of range");
        }
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
    }

    public static void SetSFXVolume(float volume)
    {
        if (volume >= -80.0f && volume <= 20.0f)
        {
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("SFX volume out of range");
        }
    }

    public static float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
    }

    public static void SetFirstPlace(int score)
    {
        if(score >= 0)
        {
            PlayerPrefs.SetInt(FIRST_PLACE_HIGHSCORE, score);
        }
        else
        {
            Debug.LogError("Score must be higher than 0");
        }
    }

    public static int GetFirstPlace()
    {
        return PlayerPrefs.GetInt(FIRST_PLACE_HIGHSCORE);
    }

    public static void SetSecondPlace(int score)
    {
        if (score >= 0)
        {
            PlayerPrefs.SetInt(SECOND_PLACE_HIGHSCORE, score);
        }
        else
        {
            Debug.LogError("Score must be higher than 0");
        }
    }

    public static int GetSecondPlace()
    {
        return PlayerPrefs.GetInt(SECOND_PLACE_HIGHSCORE);
    }

    public static void SetThirdPlace(int score)
    {
        if (score >= 0)
        {
            PlayerPrefs.SetInt(THIRD_PLACE_HIGHSCORE, score);
        }
        else
        {
            Debug.LogError("Score must be higher than 0");
        }
    }

    public static int GetThirdPlace()
    {
        return PlayerPrefs.GetInt(THIRD_PLACE_HIGHSCORE);
    }

    public static void SetFourthPlace(int score)
    {
        if (score >= 0)
        {
            PlayerPrefs.SetInt(FOURTH_PLACE_HIGHSCORE, score);
        }
        else
        {
            Debug.LogError("Score must be higher than 0");
        }
    }

    public static int GetFourthPlace()
    {
        return PlayerPrefs.GetInt(FOURTH_PLACE_HIGHSCORE);
    }

    public static void SetFifthPlace(int score)
    {
        if (score >= 0)
        {
            PlayerPrefs.SetInt(FIFTH_PLACE_HIGHSCORE, score);
        }
        else
        {
            Debug.LogError("Score must be higher than 0");
        }
    }

    public static int GetFifthPlace()
    {
        return PlayerPrefs.GetInt(FIFTH_PLACE_HIGHSCORE);
    }

    public static void SetEnemyKillTotal(int count)
    {
        PlayerPrefs.SetInt(ENEMY_KILL_TOTAL, count);
    }

    public static int GetEnemyKillTotal()
    {
        return PlayerPrefs.GetInt(ENEMY_KILL_TOTAL);
    }

    public static void SetBestTime(float time)
    {
        PlayerPrefs.SetFloat(BEST_TIME, time);
    }

    public static float GetBestTime()
    {
        return PlayerPrefs.GetFloat(BEST_TIME);
    }

    public static void SetLaunchedBefore(int oneMeansYes)
    {
        PlayerPrefs.SetInt(LAUNCHED_BEFORE, oneMeansYes);
    }

    public static int GetLaunchedBefore()
    {
        return PlayerPrefs.GetInt(LAUNCHED_BEFORE);
    }

}
