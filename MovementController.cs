using UnityEngine;

namespace DestinyBlade
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private Fighter _player;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private SpriteRenderer _playerSpriteRenderer;
        [SerializeField] private Sensor _groundSensor;
        [SerializeField] private AttackPoint _playerAttackPoint;

        private Vector2 _attackPointPosition;

        private bool _isAttacking;
        private bool _isRolling;

        private float _rollTime;
        private int _currentAttack;
        private float _attackTimer;

        private void Start()
        {
            _attackPointPosition = _playerAttackPoint.transform.localPosition;

            //_target.EventOnDeath.AddListener(OnDeath);
        }

        private void Update()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            Idle();

            Run();

            Jump();

            Roll();

            Attack();

            Block();
        }    

        private void Idle()
        {
            _player.HorizontalDirection = 0;

            _playerAnimator.SetBool("isRunning", false);

            if (_player.IsBlocking) return;

            _playerAnimator.SetBool("isBlocking", false);
        }

        private void Run()
        {
            GetAttackPointPosition();

            if (_attackTimer >= 0 || _isRolling || _player.IsBlocking) return;

            if (Input.GetKey(KeyCode.D) == true)
            {
                _player.HorizontalDirection = 1;

                _player.FaceDirection = 1;

                _playerSpriteRenderer.flipX = false;

                _playerAnimator.SetBool("isRunning", true);
            }

            if (Input.GetKey(KeyCode.A) == true)
            {
                _player.HorizontalDirection = -1;

                _player.FaceDirection = -1;

                _playerSpriteRenderer.flipX = true;

                _playerAnimator.SetBool("isRunning", true);
            }
        }

        private void GetAttackPointPosition()
        {
            if (_player.FaceDirection == 1)
            {
                _playerAttackPoint.transform.localPosition = new Vector2(_attackPointPosition.x, _attackPointPosition.y);
            }
            else
            {
                _playerAttackPoint.transform.localPosition = new Vector2(-_attackPointPosition.x, _attackPointPosition.y);
            }
        }

        private void Jump()
        {
            if (_attackTimer >= 0 || _isRolling) return;

            if (Input.GetKeyDown(KeyCode.Space) == true && _groundSensor.IsTriggered == true)
            {
                if (_player.StaminaUsage() == false) return;

                _player.VerticalDirection = 1;

                _playerAnimator.SetTrigger("jump");
            }

            if (_groundSensor.IsTriggered == false)
            {
                _player.VerticalDirection = 0;
            }
        }

        private void Roll()
        {
            if (_rollTime > 1.0f)
            {
                _player.RollDirection = 0;

                _isRolling = false;
            }
            else
            {
                _rollTime += Time.deltaTime;

                return;
            }

            if (_attackTimer >= 0) return;

            if (Input.GetKeyDown(KeyCode.LeftShift) == true && _groundSensor.IsTriggered == true)
            {
                if (_player.StaminaUsage() == false) return;

                _rollTime = 0f;

                if (_playerSpriteRenderer.flipX == false)
                {
                    _player.RollDirection = 1;
                }
                else
                {
                    _player.RollDirection = -1;
                }

                _isRolling = true;

                _playerAnimator.SetTrigger("roll");
            }
        }

        private void Attack()
        {
            if (_attackTimer >= 0)
            {
                _attackTimer -= Time.deltaTime;

                if (_attackTimer < 0.6f)
                {
                    if (_isAttacking)
                    {
                        _playerAttackPoint.MeleeAttack(_currentAttack);
                    }

                    _isAttacking = false;
                }
                if (_attackTimer < 0.4f)
                {
                    _currentAttack = 0;
                }
            }

            if (_isRolling || _isAttacking) return;

            if (Input.GetMouseButtonDown(0) == true && _groundSensor.IsTriggered == true)
            {
                if (_player.StaminaUsage() == false) return;

                _attackTimer = _player.AttackRate;

                _currentAttack++;

                if (_currentAttack > _player.MaxAttackCombo || _currentAttack == 0)
                {
                    _currentAttack = 1;
                }

                if (_currentAttack >= 1)
                {
                    _playerAnimator.SetTrigger("attack" + _currentAttack);

                    _isAttacking = true;
                }
            }
        }

        private void Block()
        {
            _player.IsBlocking = false;

            if (_attackTimer >= 0 || _isRolling) return;

            if (Input.GetMouseButton(1) == true && _groundSensor.IsTriggered == true)
            {
                _playerAnimator.SetBool("isBlocking",true);

                _player.IsBlocking = true;
            }
        }
    }
}