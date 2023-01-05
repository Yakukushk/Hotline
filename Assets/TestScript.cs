using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    #region Parametrs
    [Header("Parametrs")]
    //private Rigidbody[] rigidbody;
    private Rigidbody rb;
    [SerializeField] private GameObject gameObjects;
    #endregion

    public TestScript(Rigidbody rigidbody, GameObject gameObjects) {
        this.rb= rigidbody;
        this.gameObjects = gameObjects;
    }

    private void Start()
    {
       
        rb = GetComponent<Rigidbody>();
    }
    private void OnMouseDown()
    {
        Instantiate(gameObjects, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }


}
