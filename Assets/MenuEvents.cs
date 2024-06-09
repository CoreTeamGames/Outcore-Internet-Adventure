using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour
{
    public void OpenScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void OpenURL(string _URL)
    {
        Application.OpenURL(_URL);
    }

    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
