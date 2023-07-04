using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Experimental.GraphView;

public class Focus : MonoBehaviour
{
    public EnemyIntension enemyIntension;
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
            case "enemyattack": StartAttackFocus();
                break;
            default:
                Debug.LogWarning("Invalid string recieved");
                break;
        }
    }

    public void PlayerDecideFocus(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.Attack:
                StartAttackFocus();
                break;
            case CardType.Block:
                //StartBlockFocus();
                Instantiate(bloodController.playerblockParticleSystem, bloodController.playerfloorSpawner.transform);
                break;
            case CardType.Aid:
                Instantiate(bloodController.playerhealParticleSystem, bloodController.playerchestParticleSpawner.transform);
                break;
            case CardType.Skill:
                Instantiate(bloodController.playerskillParticleSystem, bloodController.playerchestParticleSpawner.transform);
                break;
            case CardType.Ailment:
                Instantiate(bloodController.ailmentinstantParticleSystem, bloodController.playerchestParticleSpawner.transform);
            break;

            default:
                Debug.LogWarning("Invalid string recieved");
                break;
        }
        LeanTween.delayedCall(0.75f, rotator.StartMovement);
    }

    public void PlayerAilmentParticles(CardType cardType)
    {
        switch (cardType)
        {   
            case CardType.Ailment:
                Instantiate(bloodController.ailmentinstantParticleSystem, bloodController.playerchestParticleSpawner.transform);
                break;
        }
    }
    public void EnemyDecideAttackFocus()
    {
        if (enemyIntension == EnemyIntension.Attack)
        {
            StartAttackFocus();
        }
        LeanTween.delayedCall(0.75f, rotator.StartMovement);
    }

    public void EnemyDecideBlockFocus()
    {
        StartCoroutine(DelayedInstantiate());
    }

    private IEnumerator DelayedInstantiate()
    {
        yield return new WaitForSeconds(2f);

        Instantiate(bloodController.enemyblockParticleSystem, bloodController.enemyfloorSpawner.transform);

        rotator.StartMovement();
    }






    private void StopFocus()
    {
        zoomCam.Priority = defaultCam.Priority - 1;
        for(int i = 0; i< activationObjects.Length; i++)
        {
            activationObjects[i].SetActive(false);
        }


        LeanTween.delayedCall(0.75f, rotator.StartMovement);
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
