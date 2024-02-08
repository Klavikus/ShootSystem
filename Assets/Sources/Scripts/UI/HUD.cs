using Sources.Scripts.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Scripts.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TMP_Text _ammoText;
        [SerializeField] private Image _gunIcon;

        private WeaponController _weaponController;
        private bool _isInitialized;

        private void OnDestroy()
        {
            if (_isInitialized == false)
                return;

            _weaponController.AmmoChanged -= FillAmmoText;
        }

        public void Initialize(WeaponController weaponController)
        {
            _weaponController = weaponController;

            _gunIcon.sprite = _weaponController.Config.WeaponIcon;
            FillAmmoText(_weaponController.CurrentAmmo);

            _weaponController.AmmoChanged += FillAmmoText;

            _isInitialized = true;
        }

        private void FillAmmoText(int currentAmmo) =>
            _ammoText.text = $"{currentAmmo}/{_weaponController.Config.MaxAmmo}";
    }
}