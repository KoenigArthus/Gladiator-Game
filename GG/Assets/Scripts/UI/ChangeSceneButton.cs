using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private UnityEvent onBeforeChangeScene;
 
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ChangeScene);
    }

    private void ChangeScene()
    {
        onBeforeChangeScene?.Invoke();
        SceneManager.LoadScene(sceneName);
    }

}
