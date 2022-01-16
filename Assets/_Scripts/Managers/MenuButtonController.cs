using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{

    public void LoadNextLevel()
    {
        SceneLoader.Instance.LoadNextLevel();
    }

    public void LoadPrevLevel()
    {
        SceneLoader.Instance.LoadPrevLevel();
    }
}
