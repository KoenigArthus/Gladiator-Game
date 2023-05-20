using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager i;
    [SerializeField] private string startScene;
    [SerializeField] private string currentScene;
    [SerializeField] private GameObject loadingScreen;
    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private void Awake()
    {
        i = this;
        if (startScene != "" & SceneManager.sceneCount == 1)
            SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Additive);
        else if (SceneManager.sceneCount < 2)
            SceneManager.LoadSceneAsync("Start", LoadSceneMode.Additive);
        else
            startScene = SceneManager.GetActiveScene().name;


        currentScene = startScene;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadingScreen.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync(currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
        currentScene = sceneName;

        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        for (int n = 0; n < scenesLoading.Count; n++)
        {
            while (!scenesLoading[n].isDone)
            {
                yield return null;
            }
        }

        loadingScreen.SetActive(false);
    }



}


