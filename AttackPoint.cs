using UnityEngine;

namespace DestinyBlade
{
    [RequireComponent(typeof(Transform))]
    public class AttackPoint : MonoBehaviour
    {
        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private int _attackDamage;
        [SerializeField] private float _attackRangeRadius;
        public float AttackRangeRadius => _attackRangeRadius;

        private Fighter _targetFighter;
        private Destructible _targetOther;

        public void MeleeAttack(int damageMultuplier, int attackerFaceDirection)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, _attackRangeRadius, _enemyLayerMask);

            foreach (Collider2D enemy in hitEnemies)
            {
                _targetFighter = enemy.transform.root.GetComponent<Fighter>();

                if (_targetFighter != null)
                {
                    if (_targetFighter.IsBlocking && _targetFighter.FaceDirection != attackerFaceDirection)
                    {
                        if (_targetFighter.StaminaUsage() == false)
                        {
                            _targetFighter.TakeDamage(_attackDamage * damageMultuplier);
                        }
                    }
                    else
                    {
                        _targetFighter.TakeDamage(_attackDamage * damageMultuplier);
                    }    
                }
                else
                {
                    _targetOther = enemy.transform.root.GetComponent<Destructible>();

                    if (_targetOther != null)
                    {
                        _targetOther.TakeDamage(_attackDamage * damageMultuplier);
                    }
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