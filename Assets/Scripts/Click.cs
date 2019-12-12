using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    public bool clicked = false;

    public void OnClick()
    {
        clicked = true;
    }

    public void OnStopClick()
    {
        clicked = false;
    }
}
