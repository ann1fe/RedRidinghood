using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("loading scene "+ sceneName) ;
        SceneManager.LoadScene(sceneName);
    }
}