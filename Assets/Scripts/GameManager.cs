using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public CharacterController CharacterController;
    public ObstacleGenerator ObstacleGenerator;

    public GameObject GameOverUI;
    public GameObject GameStartUI;

    public GameObject ScoreText;
    public GameObject ScoreMenuText;
    public GameObject HighScoreText;

    public int score;
    public int highscore;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("GameManager already exist!");
            return;
        }

        instance = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        GameStartUI.SetActive(true);
        Time.timeScale = 0.0f;
    }

    void Update()
    {
        if (score > highscore)
        {
            highscore = score;
        }

        ScoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
        ScoreMenuText.GetComponent<TextMeshProUGUI>().text = score.ToString();
        HighScoreText.GetComponent<TextMeshProUGUI>().text = highscore.ToString();

    }

    public void GameOver()
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ResetGame()
    {
        GameOverUI.SetActive(false);
        GameStartUI.SetActive(true);
        score = 0;
        CharacterController.PlayerReset();
        ObstacleGenerator.ObstacleReset();
    }
}
