using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CardGameManager))]
public class UIUpdater : MonoBehaviour
{
    [SerializeField] private Image hpPlayer, hpEnemy;
    [SerializeField] private TextMeshProUGUI hpPlayerText, hpEnemyText;
    private CardGameManager cardGameManager;
    private UIValues initialValues;
    private bool isAnimationPlaying;
    private float progress;

    private void Awake()
    {
        cardGameManager = GetComponent<CardGameManager>();
    }


    private void Start()
    {
        initialValues = cardGameManager.uiChanges[0];
        cardGameManager.uiHasChanged.AddListener(UpdateValues);
        UpdateValues();
    }

    private void UpdateValues()
    {
        int chageAmount = cardGameManager.uiChanges.Count;

        for (int i = 0; i < chageAmount; i++)
        {

            //Debug.Log(cardGameManager.uiChanges.Count);
            if (cardGameManager.uiChanges.Count > 0)
            {
                UIValues uiValues = cardGameManager.uiChanges[0];

                hpPlayerText.text = uiValues.PlayerHealth.ToString() + "/" + initialValues.PlayerHealth.ToString();
                hpEnemyText.text = uiValues.EnemyHealth.ToString() + "/" + initialValues.EnemyHealth.ToString();

                //hpPlayer.fillAmount = uiValues.PlayerHealth / initialValues.PlayerHealth;
                // hpEnemy.fillAmount = uiValues.EnemyHealth / initialValues.EnemyHealth;
                AnimateHealthChange(uiValues);

                MoveToNextChange();
            }


        }
    }

    private void MoveToNextChange()
    {
        cardGameManager.uiChanges.RemoveAt(0);
    }

    private void AnimateHealthChange(UIValues values)
    {
        if (hpEnemy.fillAmount * initialValues.EnemyHealth != values.EnemyHealth)
        {
            float from = hpEnemy.fillAmount;
            float to = values.EnemyHealth / initialValues.EnemyHealth;
            LeanTween.value(from, to, .5f).setOnUpdate((float val) => { hpEnemy.fillAmount = val; });
        }

        if (hpPlayer.fillAmount * initialValues.PlayerHealth != values.PlayerHealth)
        {
            float from = hpPlayer.fillAmount;
            float to = values.PlayerHealth / initialValues.PlayerHealth;
            LeanTween.value(from, to, .5f).setOnUpdate((float val) => { hpPlayer.fillAmount = val; });
        }



    }



}
