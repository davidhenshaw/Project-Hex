using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LineBoil : MonoBehaviour
{
    [SerializeField]
    float framesPerSecond = 60;

    [SerializeField]
    [Tooltip("Number of frames each sprite will be shown for")]
    int framesPerImage = 15;

    float secondsPerImage;

    [SerializeField]
    Sprite[] sprites;

    SpriteRenderer _spriteRenderer;

    float elapsed = 0;
    int currSprite = 0;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        secondsPerImage = framesPerImage / framesPerSecond;

        if(!_spriteRenderer)
            return;

        elapsed += Time.deltaTime;
        if(sprites != null && elapsed >= secondsPerImage)
        {
            _spriteRenderer.sprite = sprites[currSprite];

            currSprite += 1;
            elapsed = 0;
            if (currSprite > sprites.Length - 1)
                currSprite = 0;
        }
    }
}
