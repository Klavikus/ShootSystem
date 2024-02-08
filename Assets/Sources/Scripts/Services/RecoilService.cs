using System;
using System.Collections;
using Sources.Scripts.Controllers;
using Sources.Scripts.DataModels;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Scripts.Services
{
    public class RecoilService : IRecoilService
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly FPSCameraController _fpsCameraController;
        private readonly float _recoilAmount;
        private readonly float _recoilSpeed;

        private Transform _weaponTransform;
        private Vector3 _originalPosition;
        private Coroutine _recoilCoroutine;

        public RecoilService(ICoroutineRunner coroutineRunner, FPSCameraController fpsFPSCameraController,
            float recoilAmount, float recoilSpeed)
        {
            _coroutineRunner = coroutineRunner;
            _fpsCameraController = fpsFPSCameraController;
            _recoilAmount = recoilAmount;
            _recoilSpeed = recoilSpeed;
        }

        public void BindWeaponTransform(Transform weaponTransform)
        {
            _weaponTransform = weaponTransform;
            _originalPosition = _weaponTransform.localPosition;
        }

        public void ApplyRecoil(RecoilData recoilData)
        {
            Vector2 recoilOffset = Vector2.zero;
            recoilOffset.x = Random.Range(-recoilData.MaxHorizontalForce, recoilData.MaxHorizontalForce);
            recoilOffset.y = Random.Range(0f, recoilData.MaxVerticalForce);
            _fpsCameraController.AddRecoilOffset(recoilOffset);

            AnimateRecoil();
        }

        private void AnimateRecoil()
        {
            if (_recoilCoroutine != null)
            {
                _coroutineRunner.StopCoroutine(_recoilCoroutine);
                _weaponTransform.localPosition = _originalPosition;
            }

            _recoilCoroutine = _coroutineRunner.StartCoroutine(RecoilCoroutine());
        }

        private IEnumerator RecoilCoroutine()
        {
            Vector3 recoilPosition = _originalPosition - new Vector3(0, 0, _recoilAmount);

            while (Vector3.Distance(_weaponTransform.localPosition, recoilPosition) > Single.Epsilon)
            {
                _weaponTransform.localPosition = Vector3.Lerp(_weaponTransform.localPosition, recoilPosition,
                    Time.deltaTime * _recoilSpeed);

                yield return null;
            }

            while (Vector3.Distance(_weaponTransform.localPosition, _originalPosition) > Single.Epsilon)
            {
                _weaponTransform.localPosition = Vector3.Lerp(_weaponTransform.localPosition, _originalPosition,
                    Time.deltaTime * _recoilSpeed);

                yield return null;
            }
        }
    }
}