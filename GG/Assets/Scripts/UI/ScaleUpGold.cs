using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScaleUpGold : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    [SerializeField] LeanTweenType tweenType;
    [SerializeField] private float start, delay, time;


    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = start.ToString();
    }

    private void OnEnable()
    {
        textMesh.text = start.ToString();
        LeanTween.value(gameObject, start, 100, time)
            .setDelay(delay).setEase(tweenType)
            .setOnUpdate((float val) => { textMesh.text = Mathf.RoundToInt(val).ToString(); });
    }


}
