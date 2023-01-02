using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
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
        else {
            print("Error");
            Debug.Log(gameObject.activeSelf);   
        }
    }
    //public void setFollowPlayerT(T val) {
    //    T isFollows = val;
    //}
    public void setFollowPlayer(bool val) {
        isFollow = val;
    }
    public void camFollowPlayer() {
        setFollowPlayer(true);
        Vector3 newPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        this.transform.position = newPos;
    }
}
