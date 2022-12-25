using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    GameObject player;
    bool isFollow = true;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Update()
    {
        if (isFollow == true)
        {
            camFollowPlayer();
        }
        else
        {
            print("Error");
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
}
