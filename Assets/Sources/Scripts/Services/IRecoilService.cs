using Sources.Scripts.DataModels;
using UnityEngine;

namespace Sources.Scripts.Services
{
    public interface IRecoilService
    {
        void ApplyRecoil(RecoilData recoilData);
        void BindWeaponTransform(Transform weaponTransform);
    }
}