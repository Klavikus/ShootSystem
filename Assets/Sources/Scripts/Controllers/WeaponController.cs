using System;
using System.Collections;
using Sources.Scripts.Behaviours;
using Sources.Scripts.DataModels;
using Sources.Scripts.Services;

namespace Sources.Scripts.Controllers
{
    using UnityEngine;

    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponConfig _config;
        [SerializeField] private LayerMask _hitLayers;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private AudioSource _audioSource;

        private const string FireButton = "Fire1";

        private IRecoilService _recoilService;
        private Camera _fpsCamera;
        private ParticleSystem _muzzleFlash;
        private WaitForSeconds _reloadDelay;
        private bool _isReloading;
        private float _nextTimeToFire;
        private bool _isInitialized;

        public int CurrentAmmo { get; private set; }
        public WeaponConfig Config => _config;

        public event Action<int> AmmoChanged;

        private void Update()
        {
            if (_isInitialized == false)
                return;

            bool isFireButtonPressed = Input.GetButton(FireButton);
            bool isTimeToShoot = Time.time >= _nextTimeToFire;

            if (isFireButtonPressed && isTimeToShoot && !_isReloading)
            {
                _nextTimeToFire = Time.time + 1f / _config.FireRate;
                Shoot();
            }
        }

        public void Initialize(IRecoilService recoilService, Camera fpsCamera)
        {
            _recoilService = recoilService;
            _fpsCamera = fpsCamera;

            _recoilService.BindWeaponTransform(transform);
            
            _reloadDelay = new WaitForSeconds(_config.ReloadTime);
            _isInitialized = true;

            _muzzleFlash = Instantiate(_config.MuzzleFlashEffect, _shootPoint);

            CurrentAmmo = _config.MaxAmmo;
        }

        private void Shoot()
        {
            if (CurrentAmmo <= 0)
                return;

            CurrentAmmo--;

            _muzzleFlash.Play();
            _audioSource.PlayOneShot(_config.ShootClip);

            if (Physics.Raycast(_fpsCamera.transform.position, _fpsCamera.transform.forward,
                    out RaycastHit hitInfo, _config.Range, _hitLayers))
            {
                if (hitInfo.collider.TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage(_config.Damage);

                Instantiate(_config.HitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)).Play(true);
            }

            _recoilService.ApplyRecoil(_config.Recoil);
            AmmoChanged?.Invoke(CurrentAmmo);

            if (CurrentAmmo == 0 && _isReloading == false)
                StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            _isReloading = true;

            _audioSource.PlayOneShot(_config.ReloadClip);
            yield return _reloadDelay;

            CurrentAmmo = _config.MaxAmmo;

            AmmoChanged?.Invoke(CurrentAmmo);

            _isReloading = false;
        }
    }
}