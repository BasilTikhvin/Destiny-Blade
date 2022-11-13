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
        private Animator _npcAnimator;
        private SpriteRenderer _npcSpriteRenderer;

        private int _nextPatrolPoint;
        private Vector2 _movePoint;

        private RaycastHit2D _npcSight;
        private Destructible _attackTarget;
        private Vector2 _attackPointPosition;
        private float _attackTimer;

        private void Start()
        {
            _npc = transform.GetComponent<Fighter>();
            _npc.EventOnDeath.AddListener(ActionOnDeath);

            _npcAnimator = transform.GetComponentInChildren<Animator>();
            _npcSpriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();

            if (_npcAttackPoint != null)
            {
                _attackPointPosition = _npcAttackPoint.transform.localPosition;
            }
        }

        private void Update()
        {
            ActionIdle();

            ActionUpdateMovePoint();

            ActionFindAttackTarget();

            ActionAttack();
        }

        private void ActionIdle()
        {
            if (_npcSpriteRenderer.flipX == true)
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
            ActionGetAttackPointPosition();

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
            if (_npc.IsAttacking || _npc.IsBlocking || _npc.CurrentStamina < _minMoveStamina) return;

            if ((_movePoint.x - _npc.transform.position.x) > 0)
            {
                _npc.HorizontalDirection = 1;

                _npc.FaceDirection = 1;

                _npcSpriteRenderer.flipX = true;

                _npcAnimator.SetBool("isRunning", true);
            }

            if ((_movePoint.x - _npc.transform.position.x) < 0)
            {
                _npc.HorizontalDirection = -1;

                _npc.FaceDirection = -1;

                _npcSpriteRenderer.flipX = false;

                _npcAnimator.SetBool("isRunning", true);
            }
        }

        private void ActionGetAttackPointPosition()
        {
            if (_npc.FaceDirection == -1)
            {
                _npcAttackPoint.transform.localPosition = new Vector2(_attackPointPosition.x, _attackPointPosition.y);
            }
            else
            {
                _npcAttackPoint.transform.localPosition = new Vector2(-_attackPointPosition.x, _attackPointPosition.y);
            }
        }

        private void ActionFindAttackTarget()
        {
            _npcSight = Physics2D.Raycast(transform.position, transform.position + (_npc.FaceDirection * _npcSightDistance * transform.right), 5, _enemyLayerMask);

            if (_npcSight)
            {
                _attackTarget = _npcSight.collider.transform.root.GetComponent<Destructible>();

                if (_attackTarget != null)
                {
                    _movePoint = _attackTarget.transform.position + (transform.right * _npc.FaceDirection);
                }
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
                        _npcAttackPoint.MeleeAttack(_npc.MaxAttackCombo, _npc.FaceDirection);
                    }

                    _npc.IsAttacking = false;
                }
                return;
            }

            if (_attackTarget == null) return;

            if (_groundSensor.IsTriggered == true && Vector2.Distance(_npc.transform.position, _attackTarget.transform.position) < _npcAttackPoint.AttackRangeRadius * 2)
            {
                if (_npc.StaminaUsage() == false) return;

                _attackTimer = _npc.AttackRate;

                _npcAnimator.SetTrigger("attack");

                _npc.IsAttacking = true;
            }
        }

        private void ActionBlock()
        {
            _npc.IsBlocking = false;

            if (_attackTimer >= 0) return;

            if (Input.GetMouseButton(1) == true && _groundSensor.IsTriggered == true)
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