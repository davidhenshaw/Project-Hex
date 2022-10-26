using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOverlayManager : MonoBehaviour
{
    public UIScreen pauseView;
    public UIScreen victoryScreen;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.PauseTriggered.AddListener(OpenPauseMenu);
        GameEvents.Instance.UnpauseTriggered.AddListener(ClosePauseMenu);

        GameEvents.Instance.GoalReached.AddListener(() => victoryScreen.Open());
        //GameEvents.Instance.GoalReached.AddListener(() => victoryScreen.Open());
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
