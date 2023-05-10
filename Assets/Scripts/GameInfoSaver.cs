using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoSaver : MonoBehaviour
{
    public void SaveScore()
    {
        int currentScore = UIManager.Instance? UIManager.Instance.Score:0 ;
        int currentMaxScore = MaxScore();

        if (currentScore > currentMaxScore)
        {
            PlayerPrefs.SetInt(PlayerPrefsConst.MaxScore, currentScore);
        }        
    }
    public int MaxScore() => PlayerPrefs.GetInt(PlayerPrefsConst.MaxScore, 0);
}
