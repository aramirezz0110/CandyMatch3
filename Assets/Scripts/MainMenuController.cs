using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text maxScoreText;
    [SerializeField] private GameInfoSaver gameInfoSaver;

    private void Start()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        int maxScore = gameInfoSaver ? gameInfoSaver.MaxScore() : 0;
        maxScoreText.text = $"Max score: {maxScore}"; 
    }
}
