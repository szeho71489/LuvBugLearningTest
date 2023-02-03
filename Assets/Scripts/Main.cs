using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Main : MonoBehaviour
{
    [SerializeField]
    private Shark shark;
    [SerializeField]
    private LevelSpawner levelSpawner;

    [Header("UI")]
    [SerializeField]
    private GameObject[] healthUI;
    [SerializeField]
    private GameObject gameOverPanel;

    private int playerHealth = 3;


    private bool isGameOver;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        shark.OnSick += OnSharkSickHandler;
    }

    private void OnDisable()
    {
        shark.OnSick -= OnSharkSickHandler;
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    private void OnSharkSickHandler(object sender, System.EventArgs eventArgs)
    {
        playerHealth--;

        if(playerHealth <= 0)
        {
            // End game
            GameOver();
        }

        for(int i = healthUI.Length - 1; i >= 0; i--)
        {
            if (playerHealth < i + 1)
                healthUI[i].SetActive(false);
        }
    }

    private void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        levelSpawner.StopLevel();

        gameOverPanel.SetActive(false);
    }
}
