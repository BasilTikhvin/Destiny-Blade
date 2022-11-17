using UnityEngine;

namespace DestinyBlade
{
    [RequireComponent(typeof(Transform))]
    public class AttackPoint : MonoBehaviour
    {
        public enum AttackType
        {
            Melee,
            Distant
        }

        [SerializeField] private AttackType _fighterAttackType;
        public AttackType FighterAttackType => _fighterAttackType;

        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private Arrow _arrowPrefab;
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
                if (enemy.TryGetComponent(out _targetFighter))
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
                    if (enemy.TryGetComponent(out _targetOther))
                    {
                        _targetOther.TakeDamage(_attackDamage * damageMultuplier);
                    }
                }
            }
        }

        public void DistantAttack(int attackerFaceDirection)
        {
            Arrow _arrow = Instantiate(_arrowPrefab).GetComponent<Arrow>();

            _arrow.SetArrowSettings(attackerFaceDirection, _enemyLayerMask);

            _arrow.transform.position = transform.position;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _attackRangeRadius);
        }
#endif
    }
}