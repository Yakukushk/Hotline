using UnityEngine;

public class CameraMove : MonoBehaviour
{
    #region CameraParametrs

    [SerializeField] private bool isFollow = true;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Vector3 targetOffsetPosition;
    [SerializeField] private float speed = 5f;
    Camera cam;

    #endregion

    private void Start()
    {
        this.cam = Camera.main;
    }

    public void Update()
    {
        MoveCamera();
    }

    public void MoveCamera()
    {
        transform.position = Vector3.Lerp(this.transform.position, targetPosition.position + targetOffsetPosition,
            speed * Time.deltaTime);
    }
}