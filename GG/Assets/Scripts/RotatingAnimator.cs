using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingAnimator : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Rotator rotator;
    private float maxSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("RotationSpeed",rotator.currentSpeed / rotator.maxSpeed);
    }
}
