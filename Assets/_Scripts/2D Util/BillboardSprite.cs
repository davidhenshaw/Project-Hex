using DG.Tweening;
using metakazz.Hex;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    private Camera activeCamera;

    bool isFacingRight;

    [SerializeField]
    Vector3 _rotationOffset = new Vector3(100, 0, 0);

    MovementController _movementController;

    void Update ( )
    {
        transform.LookAt(activeCamera.transform, Vector3.up);
        transform.Rotate ( _rotationOffset );
        // 10 degrees more for debugging porpoises: so yewell bee able two sea tha sprite
    }

    private void Awake()
    {
        _movementController = GetComponentInParent<MovementController>();
        if(_movementController)
        {
            _movementController.Moved += OnMove;
        }
    }

    void Start ( )
    {
        activeCamera = Camera.allCameras [ 0 ];

    }

    private void OnMove(Vector3Int from, Vector3Int to)
    {
        var moveDir = to.YXZ() - from.YXZ();
        var dotProduct = Vector3.Dot(Vector3.right, moveDir);

        if (dotProduct > 0 && !isFacingRight || dotProduct < 0 && isFacingRight)
            FlipSprite();
    }


    [ContextMenu("Flip")]
    protected void FlipSprite()
    {

        Vector3 target = isFacingRight ?
            new Vector3(0, 180, 0)
            :
            new Vector3(0, 0, 0);


        StartCoroutine(FlipSprite(target, 0.5f));
        isFacingRight = !isFacingRight;

        //transform
        //    .DOLocalRotate(target, 0.5f)
        //    .SetEase(Ease.OutBack)
        //    .OnComplete(() =>
        //    {
        //        isFacingRight = !isFacingRight;
        //        _rotationOffset = target;
        //    });
    }

    IEnumerator FlipSprite(Vector3 targetRotation, float time)
    {
        var startRotation = _rotationOffset;
        var rotationPerSecond = (targetRotation - startRotation ) / time;
        float t = 0;
        float elapsed = 0;

        while(t <= 1)
        {
            t = elapsed / time;
            var lerpValue = Vector3.Slerp(_rotationOffset, targetRotation, t);
            _rotationOffset = lerpValue;

            yield return new WaitForFixedUpdate();
            elapsed += Time.fixedDeltaTime;
        }
    }
}
