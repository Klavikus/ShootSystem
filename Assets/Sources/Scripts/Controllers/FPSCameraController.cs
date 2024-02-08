using UnityEngine;

namespace Sources.Scripts.Controllers
{
    public class FPSCameraController : MonoBehaviour
    {
        private const string VerticalAxisName = "Mouse Y";
        private const string HorizontalAxisName = "Mouse X";

        private float _mouseSensitivity;
        private float _minVerticalAngle;
        private float _maxVerticalAngle;
        private float _horizontalRotation;
        private float _verticalRotation;
        private Vector2 _recoilOffset;
        private float _recoilResetDuration;
        private bool _isInitialized;

        private void Update()
        {
            if (_isInitialized == false)
                return;

            float horizontal = (Input.GetAxis(HorizontalAxisName) + _recoilOffset.x) * _mouseSensitivity * Time.deltaTime;
            float vertical = (-Input.GetAxis(VerticalAxisName) + _recoilOffset.y) * _mouseSensitivity * Time.deltaTime;

            _recoilOffset = Vector2.Lerp(_recoilOffset, Vector2.zero, 1 / _recoilResetDuration * Time.deltaTime);

            _verticalRotation += vertical;
            _verticalRotation = Mathf.Clamp(_verticalRotation, _minVerticalAngle, _maxVerticalAngle);
            _horizontalRotation += horizontal;

            transform.localRotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0f);
        }

        public void Initialize(float mouseSensitivity, float minVerticalAngle, float maxVerticalAngle,
            float recoilResetDuration)
        {
            _mouseSensitivity = mouseSensitivity;
            _minVerticalAngle = minVerticalAngle;
            _maxVerticalAngle = maxVerticalAngle;
            _recoilResetDuration = recoilResetDuration;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _isInitialized = true;
        }

        public void AddRecoilOffset(Vector2 additionalOffset) =>
            _recoilOffset -= additionalOffset;
    }
}