using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMarkerWidget : MonoBehaviour
{
    Camera _camera;

    public Transform worldspaceTransform;
    Image image;

    private void Awake()
    {
        _camera = Camera.main;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(worldspaceTransform)
        {
            var screenPos = _camera.WorldToScreenPoint(worldspaceTransform.position);
            transform.position = screenPos;
        }
    }

    public void SetIcon(Sprite newIcon)
    {
        image.sprite = newIcon;
    }
}
