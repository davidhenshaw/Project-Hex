using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOverlayManager : MonoBehaviour
{
    public UIView pauseView;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.PauseTriggered.AddListener(OpenPauseMenu);
        GameEvents.Instance.UnpauseTriggered.AddListener(ClosePauseMenu);
    }

    void OpenPauseMenu()
    {
        pauseView.Open();
    }

    void ClosePauseMenu()
    {
        pauseView.Close();
    }

}
