using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The Singleton instance of the Game Manager
    /// </summary>
    public static GameManager i;
    public GameObject loadingScreen;
    private string currentScene;

    private void Awake()
    {
        // default singleton pattern
        if (i == null)
        {
            DontDestroyOnLoad(gameObject);
            i = this;
        }
        else if (i != this)
        {
            Destroy(gameObject);
        }

        SceneManager.LoadSceneAsync((int)SceneIndexes.START_SCREEN, LoadSceneMode.Additive);


    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadGame()
    {
        loadingScreen.gameObject.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync("Start"));
        scenesLoading.Add(SceneManager.LoadSceneAsync("Ludus", LoadSceneMode.Additive));
        currentScene = "Ludus";


        StartCoroutine(GetSceneLoadProgess());
    }

    public void LoadScene(string sceneName)
    {
        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
    }



    public IEnumerator GetSceneLoadProgess()
    {
        for (int n = 0; n < scenesLoading.Count; n++)
        {
            while (!scenesLoading[n].isDone)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.5f);


        loadingScreen.gameObject.SetActive(false);

    }


}
