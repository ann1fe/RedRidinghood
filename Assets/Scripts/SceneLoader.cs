using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to load a scene when pressing a button
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("loading scene "+ sceneName) ;
        SceneManager.LoadScene(sceneName);
    }
}