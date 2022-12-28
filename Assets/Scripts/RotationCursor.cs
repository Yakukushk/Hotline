using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCursor : MonoBehaviour
{

    Vector3 mousePos; // vector component 
    Camera cam; // camera 
    Rigidbody rig; // rigidbody
    [SerializeField] private bool isActiveRotation = false; // bool 
    private void Start()
    {
        rig = GetComponent<Rigidbody>(); // get valued rigidbody components 
        cam = Camera.main; // picked main camera 
    }
    private void Update()
    {
        OnRotationCamera(); // calling method 
    }
    public void OnRotationCamera()
    {
        if (isActiveRotation == false) // condition 
        {
            mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - cam.transform.position.z)); // valued parametrs for vector and developing input's for all axis (x,y,z) minus camera position z
            rig.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x)) * Mathf.Rad2Deg); // arctg((mousePos for y minus position hero for y and same method for x))^2
            Debug.Log(gameObject.activeSelf + "Active Rotation");
        }
        else
        {
            print("Error");
        }
    }
}
