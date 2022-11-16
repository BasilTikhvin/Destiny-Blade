using UnityEngine;

namespace DestinyBlade
{
    public class AIController : MonoBehaviour
    {
        public enum AIType
        {
            Idle,
            Patrol
        }

        [SerializeField] private AIType _behaviourType;
        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private PatrolPoint[] _patrolRoute;
        [SerializeField] private AttackPoint _npcAttackPoint;
        [SerializeField] private Sensor _groundSensor;
        [SerializeField] private float _npcSightDistance;
        [SerializeField] private float _minMoveStamina;

        private Fighter _npc;
        private Transform _npcTransform;
        private Animator _npcAnimator;

        private int _nextPatrolPoint;
        private Vector2 _movePoint;

        private RaycastHit2D _npcSight;
        private Fighter _attackTarget;
        private float _attackTimer;

        private void Start()
        {
            _npc = transform.GetComponent<Fighter>();
            _npc.EventOnDeath.AddListener(ActionOnDeath);

            _npcTransform = transform.GetComponent<Transform>();
            _npcAnimator = transform.GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            ActionIdle();

            ActionUpdateMovePoint();

            ActionFindAttackTarget();

            ActionAttack();

            //ActionBlock();
        }

        private void ActionIdle()
        {
            if (_npcTransform.localScale.x == 1)
            {
                _npc.FaceDirection = 1;
            }
            else
            {
                _npc.FaceDirection = -1;
            }

            _npc.HorizontalDirection = 0;

            _npcAnimator.SetBool("isRunning", false);

            if (_npc.IsBlocking) return;

            _npcAnimator.SetBool("isBlocking", false);
        }

        private void ActionUpdateMovePoint()
        {
            if (_attackTarget != null)
            {
                _movePoint = _attackTarget.transform.position + (transform.right * _npc.FaceDirection);

                ActionMove();

                return;
            }

            if (_behaviourType == AIType.Idle) return;

            bool isInsidePatrolPoint = Vector2.Distance(_patrolRoute[_nextPatrolPoint].transform.position, _npc.transform.position) < _patrolRoute[_nextPatrolPoint].Radius;

            if (isInsidePatrolPoint == true)
            {
                _nextPatrolPoint++;

                if (_nextPatrolPoint == _patrolRoute.Length)
                {
                    _nextPatrolPoint = 0;
                }
                _movePoint = _patrolRoute[_nextPatrolPoint].transform.position;
            }
            else
            {
                _movePoint = _patrolRoute[_nextPatrolPoint].transform.position;
            }
            ActionMove();
        }

        private void ActionMove()
        {
            if (_attackTimer >= 0 || _npc.IsBlocking || _npc.CurrentStamina < _minMoveStamina) return;

            SetDirection(_movePoint);

            _npcAnimator.SetBool("isRunning", true);
        }

        private void SetDirection(Vector2 point)
        {
            if ((point.x - _npc.transform.position.x) > 0)
            {
                _npc.FaceDirection = 1;

                _npc.HorizontalDirection = 1;

                _npcTransform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                _npc.FaceDirection = -1;

                _npc.HorizontalDirection = -1;

                _npcTransform.localScale = new Vector3(-1, 1, 1);
            }
        }    

        private void ActionFindAttackTarget()
        {
            if (_attackTarget != null) return;

            _npcSight = Physics2D.Raycast(transform.position, transform.position + (_npc.FaceDirection * _npcSightDistance * transform.right), 5, _enemyLayerMask);

            if (_npcSight)
            {
                _attackTarget = _npcSight.collider.transform.root.GetComponent<Fighter>();
            }
        }

        private void ActionAttack()
        {
            if (_attackTimer >= 0)
            {
                _attackTimer -= Time.deltaTime;

                if (_attackTimer <= _npc.AttackRate / 2)
                {
                    if (_npc.IsAttacking)
                    {
                        if (_npcAttackPoint.FighterAttackType == AttackPoint.AttackType.Melee)
                        {
                            _npcAttackPoint.MeleeAttack(_npc.MaxAttackCombo, _npc.FaceDirection);
                        }
                        else
                        {
                            _npcAttackPoint.DistantAttack(_npc.FaceDirection);
                        }
                    }
                    _npc.IsAttacking = false;
                }
                return;
            }

            if (_attackTarget == null) return;

            if (_groundSensor.IsTriggered == true && Vector2.Distance(_npcAttackPoint.transform.position, _attackTarget.transform.position) < _npcAttackPoint.AttackRangeRadius)
            {
                if (_npc.StaminaUsage() == false) return;

                SetDirection(_attackTarget.transform.position);

                _npc.IsAttacking = true;

                _attackTimer = _npc.AttackRate;

                _npcAnimator.SetTrigger("attack");
            }
        }

        private void ActionBlock()
        {
            _npc.IsBlocking = false;

            if (_attackTimer >= 0 || _attackTarget == null) return;

            if (_groundSensor.IsTriggered == true)
            {
                _npcAnimator.SetBool("isBlocking", true);

                _npc.IsBlocking = true;
            }
        }

        private void ActionOnDeath()
        {
            _npc.EventOnDeath.RemoveListener(ActionOnDeath);

            _npcAnimator.SetTrigger("dead");

            enabled = false;
            _npc.enabled = false;
        }
    }
}