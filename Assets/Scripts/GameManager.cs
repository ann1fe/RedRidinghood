using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // ?

    public int mushroomCount = 0;

    public TMP_Text mushroomText;

    public GameObject gameOverUI;
    public GameObject gameWinUI;

    public GameObject handOverMushroomsUI;
    public GameObject player;
    public GameObject playerCamera;

    void Awake()
    {
        // Set up the singleton instance (use instance to easily access game manager)
        if (Instance == null)   
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Unpause the game because gameover paused the game
        Time.timeScale = 1f;
    }

    // Add a mushroom and update the UI
    public void AddMushroom()
    {
        mushroomCount++;
        UpdateMushroomUI();
    }

    void UpdateMushroomUI()
    {
        mushroomText.text = "Mushrooms Collected: " + mushroomCount;
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        Cursor.lockState = CursorLockMode.None;
    }

    public void WinGame()
    {
        gameWinUI.SetActive(true);
        SetPlayerFrozen(true);
        Cursor.lockState = CursorLockMode.None;
        handOverMushroomsUI.SetActive(false);
    }
    public void SetPlayerFrozen(bool frozen) {
        player.GetComponent<PlayerMovement>().enabled = !frozen; 
        playerCamera.GetComponent<PlayerCamera>().enabled = !frozen; 
    }    
    public void SetEnemiesFrozen(bool frozen) {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies) {
            enemy.GetComponent<EnemyFollow>().SetFrozen(frozen);
        }
    }
    public void RestartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        mushroomCount = 0;
        UpdateMushroomUI();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }

    public int MushroomsLeftCount()
    {
        return GameObject.FindGameObjectsWithTag("Mushroom").Length;
    }

    public void NotifyDialogueEnded()
    {
        if (MushroomsLeftCount() == 0)
        {
            WinGame();
        }   
    }
}
