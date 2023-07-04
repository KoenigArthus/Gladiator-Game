using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private CameraZoom cameraZoom;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
                if (thirdPersonController != null)
                    thirdPersonController.EnableMovement();

                if (cameraZoom != null)
                    cameraZoom.EnableZoom();
            }
            else
            {
                inventory.SetActive(true);
                if (thirdPersonController != null)
                    thirdPersonController.DisableMovement();

                if (cameraZoom != null)
                    cameraZoom.DisableZoom();
            }
        }
    }
}
