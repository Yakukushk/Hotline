using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public bool isActive = false;
    public bool isActiveMovement = true;
    public float speed = 5f;

    public void Update()
    {
        PlayerMovement();
    }
    public void PlayerMovement() {
     
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
                isActive = true;
                Debug.Log(this.gameObject.activeSelf + "Active W");
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
                isActive = true;
                Debug.Log(this.gameObject.activeSelf + "Active A");
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
                isActive = true;
                Debug.Log(this.gameObject.activeSelf + "Active D");
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
                isActive = true;
                Debug.Log(this.gameObject.activeSelf + "Active S");
            }
            
        }
    
}
