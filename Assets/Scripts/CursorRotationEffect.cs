using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorRotationEffect : MonoBehaviour
{
    private movement PlayerMovement;
    GameObject player;
    float timer = 0.1f;
    float mod = 0.1f;
    float zValue = 0.0f;

    private void Start()
    {
        PlayerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<movement>();
    }
    private void Update()
    {
        EffectRotation();
    }
    public void EffectRotation() {
        if (PlayerMovement.isActiveMovement == true) {
            Vector3 rot = new Vector3(0, 0, zValue);
            this.transform.eulerAngles = rot;
            zValue += mod;
            if (transform.eulerAngles.z >= 5.0f && transform.eulerAngles.z < 10.0f)
            {
                mod = -0.1f;
                Debug.Log("First con working!" + " " + mod);
            }
            else if (transform.eulerAngles.z < 355.0f && transform.eulerAngles.z > 350.0f) {
                mod = 0.1f;
                Debug.Log("Second con working!" + " " + mod);   
            } 

        }
    }
}
