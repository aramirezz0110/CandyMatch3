using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("CONFIG")]
    [SerializeField] private int moveCounter = 30;

    [Header("UI REFERENCES")]
    [Header("Gameplay Panel")]
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text movesText;
    [Header("Pause Panel")]
    [SerializeField] private GameObject pausePanel;    
    [SerializeField] private GameObject candyParent;    
    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text currentScore;

    [Header("EVENTS")]
    [SerializeField] private UnityEvent OnGameOver;
    
    private int score=0;
    private bool isPaused=false;

    public int Score=> score;
    public int MoveCounter => moveCounter;

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
        ActivatePanel(gameplayPanel);
    }
    #endregion
    #region PUBLIC METHODS
    public void AddScore(int value)
    {
        score += value;
        scoreText.text = $"Score: {score}";
    }
    public void ReduceMoves()
    {
        moveCounter--;
        movesText.text = $"Moves: {moveCounter}";
        if (moveCounter==0)
        {
            moveCounter= 0;
            StartCoroutine(GameOver());
        }
    }
    public void OnPauseButton()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            ActivatePanel(pausePanel);
            candyParent.SetActive(false);
        }
        else
        {
            ActivatePanel(gameplayPanel);
            candyParent.SetActive(true);
        }
    }
    #endregion

    #region PRIVATE METHODS
    private IEnumerator GameOver()
    {
        yield return new WaitUntil(()=> !BoardManager.Instance.isShifting);
        yield return new WaitForSeconds(0.05f);   
        ShowScoreInfo();
        OnGameOver?.Invoke();
    }
    private void ShowScoreInfo()
    {
        currentScore.text = $"Score: {score}";
        ActivatePanel(gameOverPanel);
    }
    private void ActivatePanel(GameObject panel)
    {
        string panelName = panel.name;
        gameplayPanel?.SetActive(panelName.Equals(gameplayPanel.name));
        pausePanel?.SetActive(panelName.Equals(pausePanel.name));
        gameOverPanel?.SetActive(panelName.Equals(gameOverPanel.name));        
    }
    #endregion
}
