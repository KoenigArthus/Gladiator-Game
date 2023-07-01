using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PopIn : MonoBehaviour
{
    [SerializeField] private LeanTweenType easeType;
    [SerializeField][Range(0.0f, 1.0f)] private float time;
    [SerializeField] private float delay;

    private void Awake()
    {
        gameObject.transform.localScale = Vector3.zero;
    }


    private void OnEnable()
    {
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), time).setEase(easeType).setDelay(delay);
    }

    private void OnDisable()
    {
        gameObject.transform.localScale = Vector3.zero;
    }


}
