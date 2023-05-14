using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Yarn.Unity
{
    public class OptionsListViewCustom : DialogueViewBase
    {
        [SerializeField] CanvasGroup canvasGroup;

        [SerializeField] OptionViewCustom optionViewPrefab;
        [SerializeField] OptionViewCustom thoughtOptionViewPrefab;

        [SerializeField] TextMeshProUGUI lastLineText;

        [SerializeField] float fadeTime = 0.1f;

        [SerializeField] bool showUnavailableOptions = false;

        [SerializeField] Color unavailableColor = Color.gray;
        [SerializeField] Color availableColor = Color.white;

        // A cached pool of OptionView objects so that we can reuse them
        // edit: we wont reuse them because we have to change them to thought options sometimes
        [SerializeField] List<OptionViewCustom> optionViews = new List<OptionViewCustom>();

        // The method we should call when an option has been selected.
        Action<int> OnOptionSelected;

        // The line we saw most recently.
        LocalizedLine lastSeenLine;

        public void Start()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void Reset()
        {
            canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            // Don't do anything with this line except note it and
            // immediately indicate that we're finished with it. RunOptions
            // will use it to display the text of the previous line.
            lastSeenLine = dialogueLine;
            onDialogueLineFinished();
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            // Hide all existing option views
            foreach (var optionView in optionViews)
            {
                Debug.Log(optionView + "1111111");
                optionView.gameObject.SetActive(false);
            }

            // If we don't already have enough option views, create more
            while (dialogueOptions.Length > optionViews.Count)
            {
                var optionView = CreateNewOptionView();
                optionView.gameObject.SetActive(false);
            }

            // Set up all of the option views
            int optionViewsCreated = 0;

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                var optionView = optionViews[i];
                var option = dialogueOptions[i];

                if (option.IsAvailable == false && showUnavailableOptions == false)
                {
                    // Don't show this option.
                    continue;
                }

                optionView.gameObject.SetActive(true);

                // colors available options
                if (option.IsAvailable == true)
                {
                    TextMeshProUGUI optionText = new TextMeshProUGUI();
                    optionText = optionView.gameObject.GetComponent<TextMeshProUGUI>();
                    optionText.color = availableColor;
                }
                // colors unavailable options
                else if (showUnavailableOptions == true && option.IsAvailable == false)
                {
                    TextMeshProUGUI optionText = new TextMeshProUGUI();
                    optionText = optionView.gameObject.GetComponent<TextMeshProUGUI>();
                    optionText.color = unavailableColor;
                }

                optionView.Option = option;

                // The first available option is selected by default
                if (optionViewsCreated == 0)
                {
                    optionView.Select();
                }

                optionViewsCreated += 1;
            }

            // Update the last line, if one is configured
            if (lastLineText != null)
            {
                if (lastSeenLine != null)
                {
                    lastLineText.gameObject.SetActive(true);
                    lastLineText.text = lastSeenLine.Text.Text;
                }
                else
                {
                    lastLineText.gameObject.SetActive(false);
                }
            }

            // Note the delegate to call when an option is selected
            OnOptionSelected = onOptionSelected;

            // Fade it all in
            StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeTime));

            /// <summary>
            /// Creates and configures a new <see cref="OptionView"/>, and adds
            /// it to <see cref="optionViews"/>.
            /// </summary>
            OptionViewCustom CreateNewOptionView()
            {
                //change the optionview if the last line was a thought (aka. had no name)
                OptionViewCustom localOptionView = new OptionViewCustom();

                if (lastSeenLine.CharacterName != null)
                    localOptionView = optionViewPrefab;
                else
                    localOptionView = thoughtOptionViewPrefab;


                var optionView = Instantiate(localOptionView);
                optionView.transform.SetParent(transform, false);
                optionView.transform.SetAsLastSibling();

                optionView.OnOptionSelected = OptionViewWasSelected;
                optionViews.Add(optionView);

                return optionView;
            }

            /// <summary>
            /// Called by <see cref="OptionView"/> objects.
            /// </summary>
            void OptionViewWasSelected(DialogueOption option)
            {
                StartCoroutine(OptionViewWasSelectedInternal(option));

                IEnumerator OptionViewWasSelectedInternal(DialogueOption selectedOption)
                {
                    yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 1, 0, fadeTime));
                    OnOptionSelected(selectedOption.DialogueOptionID);

                    // Deleting each Option as wen don't need them anymore
                    foreach (var optionView in optionViews)
                    {
                        Destroy(optionView.gameObject);
                    }
                    optionViews.Clear();
                }
            }
        }
    }
}
