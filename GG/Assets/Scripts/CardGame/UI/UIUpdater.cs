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
    private float progress = 0f;

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

                //------------- Animation methods start
                AnimateHealthChange(uiValues);
                //------------- Animation methods end

                // Wait for the animation methods to complete
                StartCoroutine(WaitForAnimations());

                // Reset progress for the next iteration
                progress = 0f;

                MoveToNextChange();
            }
        }
    }


    private IEnumerator WaitForAnimations()
    {
        while (progress < 2f)
        {
            // Wait for a short duration before checking progress again
            yield return new WaitForSeconds(0.1f);
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
            LeanTween.value(from, to, .5f)
                .setOnUpdate((float val) => { hpEnemy.fillAmount = val; })
                .setOnComplete(() => { progress += 1; });
        }

        if (hpPlayer.fillAmount * initialValues.PlayerHealth != values.PlayerHealth)
        {
            float from = hpPlayer.fillAmount;
            float to = values.PlayerHealth / initialValues.PlayerHealth;
            LeanTween.value(from, to, .5f)
                .setOnUpdate((float val) => { hpPlayer.fillAmount = val; })
                .setOnComplete(() => { progress += 1; });
        }
    }



}
