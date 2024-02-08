using UnityEngine;

namespace Sources.Scripts.Behaviours
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _health = 100f;

        public void TakeDamage(float amount)
        {
            _health -= amount;

            if (_health <= 0f) 
                Die();
        }

        private void Die() =>
            Destroy(gameObject);
    }
}