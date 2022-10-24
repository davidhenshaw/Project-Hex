using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : UIView
{
    [SerializeField]
    Button resumeButton;

    [SerializeField]
    Button mainMenuButton;

    // Start is called before the first frame update
    protected override void Start()
    {
        resumeButton.onClick.AddListener(OnResume);
        mainMenuButton.onClick.AddListener(OnMainMenu);

        base.Start();
    }

    void OnResume()
    {
        GameSession.Instance.Unpause();
    }

    void OnMainMenu()
    {
        // Load main menu
        SceneLoader.Instance.LoadLevel(0);
    }
}
