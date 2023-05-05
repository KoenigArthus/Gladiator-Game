using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ThirdPersonController : MonoBehaviour
{
    //input fields
    private ThirdPersonAA playerAA;
    private InputAction move;

    //movement fields
    [SerializeField]
    private bool movementEnabled;
    private Rigidbody rb;
    /*[SerializeField]
    private float movementForce = 1f;*/
    [SerializeField]
    private float maxSpeed = 5f;
    [SerializeField]
    private float turnTime = 1f;
    private float turnVelocity;
    private Vector3 forceDirection = Vector3.zero;
    [SerializeField]
    private new Camera camera;



    //Initialization of the controlls
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerAA = new ThirdPersonAA();
    }
    private void OnEnable()
    {
        EnableMovement();
    }
    private void OnDisable()
    {
        DisableMovement();
    }

    //Enabvling or Disabling the Movement. Is used in the Dialogue System
    public void EnableMovement()
    {
        move = playerAA.Player.Movement;
        playerAA.Player.Enable();
        movementEnabled = true;
    }
    public void DisableMovement()
    {
        playerAA.Player.Disable();
        movementEnabled = false;
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(camera);
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(camera);

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f;

        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        LookAt();
    }



    /// <summary>
    /// gets the forward direction relative to the <see cref="camera"/>
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    private Vector3 GetCameraForward(Camera camera)
    {
        Vector3 forward = camera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
    /// <summary>
    /// gets the right direction relative to the <see cref="camera"/>
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    private Vector3 GetCameraRight(Camera camera)
    {
        Vector3 right = camera.transform.right;
        right.y = 0;
        return right.normalized;
    }


    /// <summary>
    /// controlls where the Player Character is looking at
    /// and is also smoothing between the lookAt directions
    /// </summary>
    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float lookAtAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            rb.rotation = Quaternion.Euler(0f, lookAtAngle, 0f);
            //rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
            rb.angularVelocity = Vector3.zero;
    }

}

