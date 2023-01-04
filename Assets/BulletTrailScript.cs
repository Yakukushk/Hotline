using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BulletTrailScript : MonoBehaviour
{
    #region Values 
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private float _process;
    [SerializeField] private float _speed = 40f;
    public GameObject _bulletHole;
    public GameObject _bulletPoint;
    #endregion
    public BulletTrailScript (Vector3 _startPosition, Vector3 _targetPosition, float _process){
        this._startPosition = _startPosition;
        this._targetPosition = _targetPosition;
        this._process = _process;
    }
    
    private void Update()
    {
        MetodProcess();
    }
    private void MetodProcess() {
      

    }

    public void SetTargetPosition(Vector3 target) {
        _targetPosition = target.AxisWith(Axis.Z, - 1);
     }
}
public static class VectorExtension {
    public static Vector3 AxisWith(this Vector3 vector, Axis axis, float value) {
        return new Vector3(
        axis == Axis.X ? value : vector.x,
        axis == Axis.y ? value : vector.y,
        axis == Axis.Z ? value : vector.z
        );
        
    }

}
public enum Axis {
X, 
y,
Z
}