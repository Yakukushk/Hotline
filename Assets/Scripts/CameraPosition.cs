using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    GameObject player;
   public bool isFollow = true;
    Camera cam;
    movement move;
    Vector3 mousePos;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        cam = Camera.main;
        move = player.GetComponent<movement>();
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isFollow = false;
            move.SetMoving(false);      
        }
        else {
            isFollow = true;
        }
        if (isFollow == true)
        {
            camFollowPlayer();
        }   
        else
        {
            lookAround();           
            Debug.Log(gameObject.activeSelf);
        }
    }
    
    public void setFollowPlayer(bool val)
    {
        isFollow = val;
    }
    public void camFollowPlayer()
    {
            
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        this.transform.position = newPos;
    }
    public void lookAround() {
        Vector3 camPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        camPos.z = -10;
        Vector3 direction = camPos - this.transform.position;
        if (player.GetComponent<MeshFilter>().sharedMesh == true) {
            transform.Translate(direction * 2 * Time.deltaTime);
        }

    }
}
