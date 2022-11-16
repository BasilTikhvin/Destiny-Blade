using UnityEngine;
using UnityEngine.Events;

namespace DestinyBlade
{
    public class Destructible : MonoBehaviour
    {
        [SerializeField] private bool _indestructible;
        [SerializeField] private int _maxHitPoints;
        public int MaxHitPoints => _maxHitPoints;

        private int _currentHitpoints;
        public int CurrentHitPoints => _currentHitpoints;

        public UnityEvent EventOnDeath;

        protected virtual void Start()
        {
            _currentHitpoints = _maxHitPoints;
        }

        public void RestoreHitPoints(int restoreValue)
        {
            _currentHitpoints += restoreValue;

            if (_currentHitpoints > _maxHitPoints)
            {
                _currentHitpoints = _maxHitPoints;
            }
        }

        public void TakeDamage(int damageValue)
        {
            if (_indestructible) return;

            _currentHitpoints -= damageValue;

            if (_currentHitpoints <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            EventOnDeath.Invoke();

            Destroy(gameObject);
        }
    }
}