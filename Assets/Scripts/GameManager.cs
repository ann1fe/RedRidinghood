using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // ?

    public int collectedMushroomCount = 0;

    public TMP_Text mushroomText;

    public GameObject gameOverUI;
    public GameObject gameWinUI;

    public GameObject handOverMushroomsUI;
    public GameObject player;
    public GameObject playerCamera;

    int maxMushroomCount=0; 

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

    void Start()
    {
        //maxMushrooms = FindAnyObjectByType<CollectableItem>().Length;
    }

    void OnEnable()
    {
        // Unpause the game because gameover paused the game
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Called when player collects a mushroom
    /// </summary>
    public void CollectMushroom()
    {
        collectedMushroomCount++;
        UpdateMushroomUI();
    }

    /// <summary>
    /// Called once by each mushroom present in the scene
    /// </summary>
    public void RegisterMushroom()
    {
        maxMushroomCount++;
        UpdateMushroomUI();
    }

    private void UpdateMushroomUI()
    {
        mushroomText.text = "Mushrooms Collected: " + collectedMushroomCount+ "/"+maxMushroomCount;
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        Cursor.lockState = CursorLockMode.None;
    }

    public void WinGame()
    {
        SceneManager.LoadScene("EndScreen");
        Cursor.lockState = CursorLockMode.None;
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
        collectedMushroomCount = 0;
        UpdateMushroomUI();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }
    /// <summary>
    /// Checks if the player has collected all the mushrooms and calls WinGame if yes
    /// </summary>
    public void CheckEnded()
    {
        if (collectedMushroomCount == maxMushroomCount)
        {
            WinGame();
        }   
    }
}
