using UnityEngine;

public class BeelineSprite : MonoBehaviour
{
    private Camera activeCamera;

    void Update ( )
    {
        transform.LookAt ( activeCamera.transform, Vector3.up );
        transform.Rotate ( new Vector3 ( 100f, 0f, 0f ) );
        // 10 degrees more for debugging porpoises: so yewell bee able two sea tha sprite
    }

    void Start ( )
    {
        activeCamera = Camera.allCameras [ 0 ];
    }
}
