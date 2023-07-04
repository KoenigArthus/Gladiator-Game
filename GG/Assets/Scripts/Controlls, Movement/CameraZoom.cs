using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    // Start is called before the first frame update
    private ThirdPersonAA playerAA;
    private CinemachineFollowZoom cineZoom;

    private void Awake()
    {
        playerAA = new ThirdPersonAA();
        cineZoom = gameObject.GetComponent<CinemachineFollowZoom>();
    }


    private void OnEnable()
    {
        playerAA.Player.Zoom.performed += ZoomCamera;
        playerAA.Enable();
    }

    public void EnableZoom()
    {
        playerAA.Player.Zoom.performed += ZoomCamera;
        playerAA.Enable();
    }

    public void DisableZoom()
    {
        playerAA.Player.Zoom.performed -= ZoomCamera;
        playerAA.Disable();
    }

    private void OnDisable()
    {
        playerAA.Player.Zoom.performed -= ZoomCamera;
        playerAA.Disable();
    }

    private void ZoomCamera(InputAction.CallbackContext obj)
    {
        float inputValue = -obj.ReadValue<Vector2>().y / 100f;
       
        

        if (Mathf.Abs(inputValue) > 0.1f)
        {
            cineZoom.m_Width += inputValue;
        }

        if (cineZoom.m_Width < 7.5f)
            cineZoom.m_Width = 7.5f;
        else if(cineZoom.m_Width > 15)
            cineZoom.m_Width = 15f;

    }

}
