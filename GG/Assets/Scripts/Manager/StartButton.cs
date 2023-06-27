using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(delegate { SceneManager.LoadScene("Ludus"); });

    }
}
