using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public LevelInfo level;

    [SerializeField]
    TMP_Text text;

    Button button;

    // Start is called before the first frame update
    void Start()
    {
        if (!text)
            text = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        if (TryGetComponent(out button))
        {
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnDisable()
    {
        if (TryGetComponent(out button))
        {
            button.onClick.RemoveListener(OnClick);
        }
    }

    public void OnClick()
    {
        SceneLoader.Instance.LoadLevel(level);
    }

    public void InitButton(int order)
    {
        if (!level)
            return;

        text.text = order.ToString();
    }
}
