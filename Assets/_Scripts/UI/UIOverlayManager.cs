using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOverlayManager : Singleton<UIOverlayManager>
{
    public UIScreen pauseView;
    public UIScreen victoryScreen;
    public UIScreen gameplayUIScreen;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.PauseTriggered.AddListener(OpenPauseMenu);
        GameEvents.Instance.UnpauseTriggered.AddListener(ClosePauseMenu);

        GameEvents.Instance.GoalReached.AddListener(() => victoryScreen.Open());
        
        GameEvents.Instance.GameplayUIInit?.Invoke((GameplayUIScreen)gameplayUIScreen);
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
