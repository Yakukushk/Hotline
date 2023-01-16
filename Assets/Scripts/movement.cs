using UnityEngine;

[System.Serializable]
public class movement : MonoBehaviour
{
    [Header("Values")]

    #region Values

    private bool isActive = false;

    public bool isActiveMovement = true;
    public Camera cam;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float Mousespeed = 100;

    #endregion

    public void Start()
    {
        cam = Camera.main;
    }

    public void Update()
    {
        if (SetMoving(true) == true)
        {
            PlayerMovement();
            PlayerRotation();
        }
        else
        {
            checkMovement(false);
        }
    }

    public bool SetMoving(bool val)
    {
        isActiveMovement = val;
        return val;
    }

    public void PlayerMovement()
    {
        float _horizontal = Input.GetAxis("Horizontal");
        float _verctical = Input.GetAxis("Vertical");
        Vector3 pos = new Vector3(_horizontal, 0, _verctical);
        transform.Translate(pos * speed * Time.deltaTime, Space.World);
    }

    public void PlayerRotation()
    {
        RaycastHit _hit;
        Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hit))
        {
            var vector3 = new Vector3(_hit.point.x, transform.position.y, _hit.point.z);
            this.transform.LookAt(vector3);
        }
    }

    public void checkMovement(bool Active)
    {
        if (Input.GetKey(KeyCode.D) != true && Input.GetKey(KeyCode.A) != true && Input.GetKey(KeyCode.W) != true &&
            Input.GetKey(KeyCode.S) != true)
        {
            isActiveMovement = false;
        }
        else
        {
            isActiveMovement = true;
        }
    }
}