using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnePlayerLevel()
    {
        StartCoroutine(LoadAsyncScene("SinglePlayer"));
    }

    public void TwoPlayerLevel()
    {
        StartCoroutine(LoadAsyncScene("TwoPlayer"));
    }

    public void ControlsPage()
    {
        StartCoroutine(LoadAsyncScene("Controls"));
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadAsyncScene(String sceneToSwitchTo)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToSwitchTo);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
