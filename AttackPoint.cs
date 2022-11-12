
using UnityEngine;

namespace DestinyBlade
{
    [RequireComponent(typeof(Transform))]
    public class AttackPoint : MonoBehaviour
    {
        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private int attackDamage;
        [SerializeField] private float _attackRangeRadius;
        public float AttackRangeRadius => _attackRangeRadius;

        private Fighter _targetFighter;
        private Destructible _targetOther;

        public void Attack(int damageMultuplier)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, _attackRangeRadius, _enemyLayerMask);

            foreach (Collider2D enemy in hitEnemies)
            {
                _targetFighter = enemy.transform.root.GetComponent<Fighter>();
                _targetOther = enemy.transform.root.GetComponent<Destructible>();

                if (_targetFighter != null)
                {
                    if (_targetFighter.IsBlocking)
                    {
                        if (_targetFighter.StaminaUsage() == true) return;

                        _targetFighter.TakeDamage(attackDamage * damageMultuplier);

                        return;
                    }
                    else
                    {
                        _targetFighter.TakeDamage(attackDamage * damageMultuplier);

                        return;
                    }    
                }
                else if (_targetOther != null)
                {
                    _targetOther.TakeDamage(attackDamage * damageMultuplier);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _attackRangeRadius);
        }
#endif
    }
}