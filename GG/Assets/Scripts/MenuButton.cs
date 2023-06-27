using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(delegate { LevelLoader.i.LoadScene("Start"); });

    }
}
