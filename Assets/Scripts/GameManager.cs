using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; 
using System.Collections.Generic;

/// <summary>
/// Keeps the game state and implements gameflow related helper functions 
/// </summary>
public class GameManager : MonoBehaviour
{
    // store GameManager Instance here to easily access it in other scripts (singleton design pattern)
    public static GameManager Instance; 

    public List<GameObject> collectedMushrooms = new List<GameObject>();

    public TMP_Text mushroomText;

    public GameObject gameOverUI;
    public GameObject gameWinUI;

    public GameObject handOverMushroomsUI;
    public GameObject player;
    public GameObject playerCamera;

    int maxMushroomCount=0; 

    void Awake()
    {
        // Set up the singleton instance
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

    /// <summary>
    /// Called when player collects a mushroom
    /// </summary>
    public void CollectMushroom(GameObject mushroom)
    {
        collectedMushrooms.Add(mushroom);
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
        mushroomText.text = "Mushrooms Collected: " + collectedMushrooms.Count + "/"+maxMushroomCount;
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
    public void RestartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        collectedMushrooms.Clear();
        UpdateMushroomUI();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
    }
    /// <summary>
    /// Checks if the player has collected all the mushrooms and calls WinGame if yes
    /// </summary>
    public void CheckEnded()
    {
        if (collectedMushrooms.Count == maxMushroomCount)
        {
            StartCoroutine(DropMushrooms());
        }   
    }
    IEnumerator DropMushrooms() {
        foreach (var mushroom in collectedMushrooms)
        {
            PickupScript pickup = player.GetComponent<PickupScript>();
            mushroom.transform.position = pickup.holdPos.transform.position;
            mushroom.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }
        WinGame();
    }
}
