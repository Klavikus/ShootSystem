using UnityEngine;

namespace Sources.Scripts.DataModels
{
    [CreateAssetMenu(fileName = "NewWeaponConfig", menuName = "Weapon System/Weapon Config")]
    public class WeaponConfig : ScriptableObject
    {
        [Header("General Parameters")]
        public Sprite WeaponIcon;

        [Header("Shooting Parameters")] 
        public float Damage;
        public float FireRate;
        public float Range;

        [Header("Ammo Parameters")] 
        public int MaxAmmo;
        public float ReloadTime;

        [Header("Effects")] 
        public ParticleSystem MuzzleFlashEffect;
        public ParticleSystem HitEffect;

        [Header("Sound")] public AudioClip ShootClip;
        public AudioClip ReloadClip;

        [Header("Recoil")] 
        public RecoilData Recoil;
    }
}