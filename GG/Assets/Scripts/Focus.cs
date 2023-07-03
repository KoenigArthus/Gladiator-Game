using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera defaultCam, zoomCam;
    [SerializeField] GameObject[] activationObjects;
    [SerializeField] GameObject player, enemy;
    [SerializeField] Rotator rotator;
    [SerializeField] BloodController bloodController;

    public void DecideFocus(string notification)
    {
        switch (notification)
        {
            case "stop": StopFocus();
                break;
            default:
                Debug.LogWarning("Invalid string recieved");
                break;
        }
    }

    public void DecideFocus(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.Attack:
                StartAttackFocus();
                break;
            case CardType.Block:
                //StartBlockFocus();
                Instantiate(bloodController.blockParticleSystem, bloodController.floorSpawner.transform);
                break;
            case CardType.Aid:
                Instantiate(bloodController.healParticleSystem, bloodController.chestParticleSpawner.transform);
                break;
            case CardType.Skill:
                Instantiate(bloodController.skillParticleSystem, bloodController.chestParticleSpawner.transform);
                break;

            default:
                Debug.LogWarning("Invalid string recieved");
                break;
        }
        LeanTween.delayedCall(2f, rotator.StartMovement);
    }

    

    private void StopFocus()
    {
        zoomCam.Priority = defaultCam.Priority - 1;
        for(int i = 0; i< activationObjects.Length; i++)
        {
            activationObjects[i].SetActive(false);
        }

        rotator.StartMovement();
    }

    private void StartFocus()
    {
        zoomCam.Priority = defaultCam.Priority + 1;
        activationObjects[0].SetActive(true);
        activationObjects[1].SetActive(true);
    }

    private void StartAttackFocus()
    {
        StartFocus();
        activationObjects[2].SetActive(true);
    }

    private void StartBlockFocus()
    {
        StartFocus();
        activationObjects[3].SetActive(true);
    }





}
