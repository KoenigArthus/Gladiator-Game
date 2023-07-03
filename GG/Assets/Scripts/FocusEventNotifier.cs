using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusEventNotifier : MonoBehaviour
{
    Focus focus;

    private void Start()
    {
        focus = FindObjectOfType<Focus>();
    }


    public void NotifyFocus(string notification)
    {
        focus.DecideFocus(notification);
    }

}
