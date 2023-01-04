using UnityEngine;

public class CrosshairWithGizmos : MonoBehaviour
{
    #region Datamembers
    
            #region Editor Settings
    
            [Header("Crosshair")] 
            [SerializeField] private GameObject crosshairRenderer;
            [SerializeField] private LayerMask crosshairMask;
    
            [Header("Aim")]
            [SerializeField] private Transform aimedTransform;
    
            [Header("Gizmos")]
            [SerializeField] private bool gizmo_cameraRay = false;
            [SerializeField] private bool gizmo_ground = false;
            [SerializeField] private bool gizmo_target = false;
            [SerializeField] private bool gizmo_ignoredHeightTarget = false;
    
            #endregion
            #region Private Fields
    
            private Camera mainCamera;
    
            #endregion
    
            #endregion
    
    
            #region Methods
    
            #region Unity Callbacks
    
            private void Awake()
            {
                Cursor.visible = false;
            }
            
            private void Start()
            {
                mainCamera = Camera.main;
            }
    
            private void Update()
            {
                RefreshCrosshair();
            }
    
            private void OnDrawGizmos()
            {
                if (Application.isPlaying == false)
                {
                    return;
                }
    
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, crosshairMask))
                {
                    if (gizmo_cameraRay)
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawLine(ray.origin, hitInfo.point);
                        Gizmos.DrawWireSphere(ray.origin, 0.5f);
                    }
    
                    var hitPosition = hitInfo.point;
                    var hitGroundHeight = Vector3.Scale(hitInfo.point, new Vector3(1, 0, 1)); ;
                    var hitPositionIngoredHeight = new Vector3(hitInfo.point.x, aimedTransform.position.y, hitInfo.point.z);
    
                    if (gizmo_ground)
                    {
                        Gizmos.color = Color.gray;
                        Gizmos.DrawWireSphere(hitGroundHeight, 0.5f);
                        Gizmos.DrawLine(hitGroundHeight, hitPosition);
                    }
    
                    if (gizmo_target)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireSphere(hitInfo.point, 0.5f);
                        Gizmos.DrawLine(aimedTransform.position, hitPosition);
                    }
    
                    if (gizmo_ignoredHeightTarget)
                    {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawWireSphere(hitPositionIngoredHeight, 0.5f);
                        Gizmos.DrawLine(aimedTransform.position, hitPositionIngoredHeight);
                    }
                }
            }
    
            #endregion
            
            private void RefreshCrosshair()
            {
                RaycastHit hit;
                
                var rayMainCamera = mainCamera.ScreenPointToRay(Input.mousePosition);
                
                Physics.Raycast(rayMainCamera, out var hitInfo, float.MaxValue);
                
                float koef = (transform.position.y - hitInfo.point.y) /
                             (mainCamera.transform.position.y - hitInfo.point.y);
                float distanceToCrosshair = hitInfo.distance * koef;
                
                Vector3 crosshairPos = new Vector3(
                    hitInfo.point.x + (mainCamera.transform.position.x - hitInfo.point.x) * distanceToCrosshair /
                    hitInfo.distance,
                    hitInfo.point.y + (mainCamera.transform.position.y - hitInfo.point.y) * distanceToCrosshair /
                    hitInfo.distance,
                    hitInfo.point.z + (mainCamera.transform.position.z - hitInfo.point.z) * distanceToCrosshair /
                    hitInfo.distance);
                
                float distanceFromPlayerToMousePos = Vector3.Distance(crosshairPos, transform.position);
                
                Ray ray = new Ray(transform.position, transform.forward);
                
                if (Physics.Raycast(ray, out hit, float.MaxValue, crosshairMask))
                {
                    if (distanceFromPlayerToMousePos < hit.distance)
                    {
                        crosshairRenderer.transform.position = crosshairPos;
                    }
                    else
                    {
                        crosshairRenderer.transform.position = hit.point;
                    }
                }
                else
                {
                    if(!double.IsNaN(crosshairPos.x) || !double.IsNaN(crosshairPos.y) || !double.IsNaN(crosshairPos.z))
                        crosshairRenderer.transform.position = crosshairPos;
                }
                crosshairRenderer.transform.position = new Vector3(crosshairRenderer.transform.position.x,
                    crosshairRenderer.transform.position.y + 0.01f, crosshairRenderer.transform.position.z );
            }
            #endregion
}
