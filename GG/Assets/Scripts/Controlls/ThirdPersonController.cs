using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using JSAM;


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
    private Camera cam;



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

    #region movement enabling
    //Enabvling or Disabling the Movement. Is used for the Dialogue System
    /// <summary>
    /// enables movement
    /// </summary>
    public void EnableMovement()
    {
        move = playerAA.Player.Movement;
        playerAA.Player.Enable();
        movementEnabled = true;
    }
    /// <summary>
    /// disables movement
    /// </summary>
    public void DisableMovement()
    {
        playerAA.Player.Disable();
        movementEnabled = false;
    }
    /// <summary>
    /// enables movement and turn the player to a look at position
    /// </summary>
    /// <param name="lookAtPos">the position the character shoult look at</param>
    public void EnableMovement(Vector3 lookAtPos)
    {
        move = playerAA.Player.Movement;
        playerAA.Player.Enable();
        movementEnabled = true;
        LookAt(lookAtPos);
    }
    /// <summary>
    /// disables movement and turn the player to a look at position
    /// </summary>
    /// <param name="lookAtPos">the position the character shoult look at</param>
    public void DisableMovement(Vector3 lookAtPos)
    {
        playerAA.Player.Disable();
        movementEnabled = false;
        LookAt(lookAtPos);
    }
    /// <summary>
    /// enables movement and turn the player to a look at position as a string x,y,z
    /// </summary>
    /// <param name="lookAtPos">the position the character shoult look at</param>
    public void EnableMovement(string lookAtPos)
    {
        move = playerAA.Player.Movement;
        playerAA.Player.Enable();
        movementEnabled = true;
        Vector3 v3 = Vector3.zero;
        LookAt(CustomUtility.StringToVector3(lookAtPos));
    }
    /// <summary>
    /// disables movement and turn the player to a look at position
    /// </summary>
    /// <param name="lookAtPos">the position the character shoult look at</param>
    public void DisableMovement(string lookAtPos)
    {
        playerAA.Player.Disable();
        movementEnabled = false;
        LookAt(CustomUtility.StringToVector3(lookAtPos));
    }
    #endregion



    // Updates the player position
    private void FixedUpdate()
    {
        // gets the camera relative direction
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(cam);
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(cam);

        // moves the player
        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;


        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f;

        // clamps to the maximum movement speed
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        // controlls the view Direction 
        LookAt();


        // footsteps sound
        if (forceDirection.sqrMagnitude > 0 & !JSAM.AudioManager.IsSoundPlaying(Sounds.teststeps))
            JSAM.AudioManager.PlaySound(Sounds.teststeps);
        else
            JSAM.AudioManager.StopSound(Sounds.teststeps);
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
    /// <summary>
    /// makes the player look at the given position
    /// </summary>
    /// <param name="lookAtPos">the position the player should look</param>
    private void LookAt(Vector3 lookAtPos)
    {
        Vector3 direction = lookAtPos;
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

