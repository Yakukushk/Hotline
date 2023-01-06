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
    public float blinkIntensity;
    public float blinkDuraction;
    float blinkTimer;
    MeshRenderer meshRenderer;
    
    #endregion

    public TestScript(Rigidbody rigidbody, GameObject gameObjects) {
        this.rb= rigidbody;
        this.gameObjects = gameObjects;
    }

    private void Start()
    {
        hplevel = maxhp;
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();   
    }
    //private void OnMouseDown()
    //{
    //    Instantiate(gameObjects, transform.position, Quaternion.identity);
    //    Destroy(this.gameObject);
    //}
    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuraction);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        meshRenderer.material.color = Color.gray * intensity;
    }
    public void TakeDamage(float damageAmount) {
        hplevel -= damageAmount;
        if (hplevel <= 0) {
            Instantiate(gameObjects, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        blinkTimer = blinkDuraction;     
    }
   

}
