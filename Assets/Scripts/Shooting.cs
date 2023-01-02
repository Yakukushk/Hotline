using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[RequireComponent(typeof(LineRenderer))]
public class Shooting : MonoBehaviour
{
    #region ForFirstRaycastMethod
    [Header("Items")]
    [SerializeField] Camera cam;
    [SerializeField] private LayerMask groundMask;
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
    #endregion
    #region ForSecondRaycastMethod
    [Header("New Method")]
    [SerializeField] private float _speed;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject _bulletTrail;
    [SerializeField] private float _weaponRange = 20f;
    movement Movement;
   // [SerializeField] private Animator _anim;
    #endregion
    public Shooting(bool success, Vector3 position) {
        this.success = success;
        this.position = position;
    }
    public Shooting(float _speed, Transform gunPoint, GameObject bulletTrail) {
        this._speed = _speed;
        this.gunPoint = gunPoint;
        this._bulletTrail = bulletTrail;
    }

    private void Start()
    {
        cam = Camera.main;
        laserLine = GetComponent<LineRenderer>();
        Movement = GetComponent<movement>();
    }
    private void Update()
    {
        //Aim();
        //ShootingRaycast();
        Shoot();
    }
    private void Shoot() {
      
        
        if (Input.GetMouseButtonDown(0)) {
            //// _anim.SetTrigger("Shoot");
            // var _hit = Physics2D.Raycast(gunPoint.position, transform.up, _weaponRange);
            // var trail = Instantiate(_bulletTrail, gunPoint.position, this.transform.rotation);
            // var trailScript = trail.GetComponent<BulletTrailScript>();
            // if (_hit.collider != null)
            // {
            //     trailScript.SetTargetPosition(_hit.point);
            //     // var hettable = _hit.collider.GetComponent<IHittable>();
            //     // hettable?.Hit();
            // }
            // else {
            //     var endPosition = gunPoint.position + transform.position * _weaponRange;
            //     trailScript.SetTargetPosition(endPosition);
            // }

            var bullet = Instantiate(_bulletTrail, gunPoint.position, gunPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(gunPoint.forward * _weaponRange, ForceMode2D.Impulse);
            Destroy(bullet, 2f);

        }
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
