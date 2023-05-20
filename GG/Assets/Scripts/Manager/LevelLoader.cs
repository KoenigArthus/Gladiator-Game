using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using Yarn;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader i;
    public GameObject loadingScreen;
    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

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
    }

    public async void LoadScene(string sceneName)
    {

        loadingScreen.gameObject.SetActive(true);
        Debug.Log("a");


        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;


       // StartCoroutine(Loading());
        await Task.Delay(1000);


        scene.allowSceneActivation = true;

        loadingScreen.gameObject.SetActive(false);
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