using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAround : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] private float time;
    [SerializeField] private LeanTweenType easeType, loopType;
    [SerializeField] private float moveAmount, delay;
    private LTDescr tween, delayTween;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }



    private void OnEnable()
    {
        delayTween = LeanTween.delayedCall(gameObject, delay, () =>
        {
            tween = LeanTween.moveY(gameObject, startPosition.y + moveAmount, time)
            .setLoopPingPong()
            .setEase(easeType)
            .setRepeat(2)
            .setOnUpdate((float _) => StopTween());
        }).setRepeat(-1);


    }

    private void StopTween()
    {
        if (!gameObject.activeInHierarchy)
        {
            // Object is inactive, cancel the tween
            if (delayTween != null)
            {
                LeanTween.cancel(tween.id);
                LeanTween.cancel(delayTween.id);
                this.transform.SetPositionAndRotation(startPosition, startRotation);
            }
        }
    }
}
