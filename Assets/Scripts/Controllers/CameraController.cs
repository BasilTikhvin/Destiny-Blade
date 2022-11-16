using DestinyBlade;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private float _cameraOffsetX;
    [SerializeField] private float _cameraOffsetY;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = _camera.transform.position;
    }

    private void FixedUpdate()
    {
        if (_camera == null || _target == null) return;

        if (_target.transform.position.x < _leftBorder.transform.position.x + _cameraOffsetX)
        {
            _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, _camera.transform.position.z);
        }
        else if (_target.transform.position.x > _rightBorder.transform.position.x - _cameraOffsetX)
        {
            _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, _camera.transform.position.z);
        }
        else
        {
            _camera.transform.position = new Vector3(_target.transform.position.x, _camera.transform.position.y, _camera.transform.position.z);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;

        _camera.transform.position = _startPosition;
    }
}
