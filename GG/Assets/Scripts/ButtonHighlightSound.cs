using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JSAM;

public class ButtonHighlightSound : MonoBehaviour, IPointerEnterHandler
{
    public Sounds highlightSound = Sounds.Click;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.PlaySound(Sounds.Hover);
    }
}

