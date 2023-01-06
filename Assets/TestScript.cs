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
    public float maxhp, hplevel = 10f;
    
    #endregion

    public TestScript(Rigidbody rigidbody, GameObject gameObjects) {
        this.rb= rigidbody;
        this.gameObjects = gameObjects;
    }

    private void Start()
    {
        hplevel = maxhp;
        rb = GetComponent<Rigidbody>();
    }
    //private void OnMouseDown()
    //{
    //    Instantiate(gameObjects, transform.position, Quaternion.identity);
    //    Destroy(this.gameObject);
    //}

    public void TakeDamage(float damageAmount) {
        hplevel -= damageAmount;
        if (hplevel <= 0) {
            Instantiate(gameObjects, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }


}
