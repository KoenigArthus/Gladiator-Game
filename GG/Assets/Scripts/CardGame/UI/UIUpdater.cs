using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardGameManager))]
public class UIUpdater : MonoBehaviour
{
    [SerializeField] private Image hpPlayer, hpEnemy;
    private CardGameManager cardGameManager;
    private UIValues initialValues;

    private void Awake()
    {
        cardGameManager = GetComponent<CardGameManager>();
    }


    private void Start()
    {
        cardGameManager.uiHasChanged.AddListener(UpdateValues);
        initialValues = cardGameManager.uiChanges[0];
        UpdateValues();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("U");
            UpdateValues();
        }
    }


    private void UpdateValues()
    {
        int initialChanges = cardGameManager.uiChanges.Count;

        for (int i = 0; i < initialChanges; i++)
        {
            Debug.Log(cardGameManager.uiChanges.Count);
            if (cardGameManager.uiChanges.Count > 0)
            {
                UIValues firstValue = cardGameManager.uiChanges[0];

                hpPlayer.fillAmount = firstValue.PlayerHealth / initialValues.PlayerHealth;
                hpEnemy.fillAmount = firstValue.EnemyHealth / initialValues.EnemyHealth;

                cardGameManager.uiChanges.RemoveAt(0);
            }
        }


        


    }





}
