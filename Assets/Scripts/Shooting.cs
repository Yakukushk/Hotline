using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[RequireComponent(typeof(LineRenderer))]
public class Shooting : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] Camera cam;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform prefabSpawn;
    GameObject projectilePrefab;
    public Transform laserOrigin;
    private bool success;
    private Vector3 position;
    [SerializeField] private Transform aimedTransform;

 
    [Header("floats")]
    public float gunRange = 50f;
    public float fireRate = 0.2f;
    public float laserDuration = 0.05f;
    [SerializeField]LineRenderer laserLine;
    float fireTimer;

    public Shooting(bool success, Vector3 position) {
        this.success = success;
        this.position = position;
    }

    private void Start()
    {
        cam = Camera.main;
        laserLine = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        Aim();
        ShootingRaycast();
    }
    private void GetMousePostiton(bool suc, Vector3 position) {
        this.success = suc;
        this.position = position;
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            suc = true;
            position = hitInfo.point;
        }
        else {
            suc = false;
            position = Vector3.zero;
        }
    }
    private void Aim()
    {
        GetMousePostiton(success, position);
        if (success)
        {
            var direction = position - transform.position;
            direction.y = 0;
            transform.forward = direction;
        }
    }
    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var projectile = Instantiate(projectilePrefab, projectilePrefab.transform.position, Quaternion.identity);
            projectile.transform.forward = aimedTransform.forward;
        }
    }

    private void ShootingRaycast() {

        fireTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) && fireTimer > fireRate)
        {
            fireTimer = 0;
            laserLine.SetPosition(0, laserOrigin.position);
            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, gunRange))
            {
                laserLine.SetPosition(1, hit.point);
                //Destroy(hit.transform.gameObject);
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (cam.transform.forward * gunRange));
            }
            StartCoroutine(ShootLaser());
        }
    }
    IEnumerator ShootLaser() {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}
