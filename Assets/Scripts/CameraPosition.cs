using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    #region CameraParametrs

    GameObject player;
    [SerializeField] private bool isFollow = true;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Vector3 targetOffsetPosition;
    [SerializeField] private float speed = 5f;
    Camera cam;
    movement move;

    #endregion

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
        move = player.GetComponent<movement>();
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

    public bool setFollowPlayer(bool val)
    {
        isFollow = val;
        return val;
    }

    public void camFollowPlayer()
    {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y,
            this.transform.position.z);
        this.transform.position = newPos;
    }

    public void lookAround()
    {
        Vector3 camPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        camPos.z = -10;
        Vector3 direction = camPos - this.transform.position;
        if (player.GetComponent<MeshRenderer>().isVisible == true)
        {
            transform.Translate(direction * 2 * Time.deltaTime);
        }
    }
}