using Sources.Scripts.Controllers;
using Sources.Scripts.Services;
using Sources.Scripts.UI;
using UnityEngine;

namespace Sources.Scripts.Infrastructure
{
    public class CompositionRoot : MonoBehaviour, ICoroutineRunner
    {
        [Header("General Parameters")]
        [SerializeField] private Camera _fpsCamera;

        [Header(nameof(FPSCameraController))]
        [SerializeField] private FPSCameraController _fpsCameraController;
        [SerializeField] private float _mouseSensitivity = 100;
        [SerializeField] private float _minVerticalAngle = -90;
        [SerializeField] private float _maxVerticalAngle = 90;
        [SerializeField] private float _recoilResetDuration = 0.2f;

        [Header(nameof(WeaponController))]
        [SerializeField] private WeaponController _weaponController;
        [SerializeField] private float _recoilAmount = 0.2f;
        [SerializeField] private float _recoilSpeed = 0.2f;

        [Header(nameof(HUD))]
        [SerializeField] private HUD _hud;

        private void Start()
        {
            _fpsCameraController.Initialize(_mouseSensitivity, _minVerticalAngle, _maxVerticalAngle,
                _recoilResetDuration);

            IRecoilService recoilService = new RecoilService(this, _fpsCameraController, _recoilAmount, _recoilSpeed);

            _weaponController.Initialize(recoilService, _fpsCamera);
            _hud.Initialize(_weaponController);
        }
    }
}