using UnityEngine;

namespace DestinyBlade
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _leftBorder;
        [SerializeField] private Transform _rightBorder;

        private Vector2 _screenResolution;
        private Vector2 _cameraOffset;
        private Vector3 _cameraStartPosition;

        private void Start()
        {
            _screenResolution.x = Screen.width;
            _screenResolution.y = Screen.height;

            _cameraOffset = _camera.ScreenToWorldPoint(_screenResolution);
            _cameraStartPosition = new Vector3(_leftBorder.transform.position.x + _cameraOffset.x, -1, _camera.transform.position.z);
            _camera.transform.position = _cameraStartPosition;

            _camera.orthographicSize = 4;
        }

        private void FixedUpdate()
        {
            if (_camera == null || _target == null) return;

            if (_target.transform.position.x < _leftBorder.transform.position.x + _cameraOffset.x)
            {
                _camera.transform.position = new Vector3(_camera.transform.position.x, -1, _camera.transform.position.z);
            }
            else if (_target.transform.position.x > _rightBorder.transform.position.x - _cameraOffset.x)
            {
                _camera.transform.position = new Vector3(_camera.transform.position.x, -1, _camera.transform.position.z);
            }
            else
            {
                _camera.transform.position = new Vector3(_target.transform.position.x, -1, _camera.transform.position.z);
            }
        }

        public void SetTarget(Transform target)
        {
            _target = target;

            _camera.transform.position = _cameraStartPosition;
        }
    }
}