using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text movesText;

    private int moveCounter=30;
    private int score=0;

    public int Score 
    { 
        get => score; 
        set 
        {
            score = value;
            scoreText.text = $"Score: {score}";
        } 
    }
    public int MoveCounter 
    { 
        get => moveCounter;
        set
        {
            moveCounter = value;
            movesText.text = $"Moves: {moveCounter}";
        }
    }

    #region UNITY METHODS
    private void Awake()
    {
        if(!Instance)
            Instance= this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        scoreText.text = $"Score: {score}";
        movesText.text = $"Moves: {moveCounter}";
    }
    #endregion

}
