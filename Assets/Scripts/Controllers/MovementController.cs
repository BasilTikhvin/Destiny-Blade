using UnityEngine;

namespace DestinyBlade
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private Fighter _player;
        
        private Transform _playerTransform;
        private Animator _playerAnimator;
        private Sensor _groundSensor;
        private AttackPoint _playerAttackPoint;

        private int _currentAttack;
        private float _attackTimer;

        private float _rollTimer;

        private void Start()
        {
            GetComponents(_player);

            enabled = true;
        }

        private void Update()
        {
            if (Time.timeScale > 0)
            {
                UpdateMovement();
            }
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
            if (_attackTimer >= 0 || _rollTimer >= 0f || _player.IsBlocking) return;

            if (Input.GetKey(KeyCode.D) == true)
            {
                _player.HorizontalDirection = 1;

                _playerTransform.localScale = new Vector3(1, 1, 1);

                _playerAnimator.SetBool("isRunning", true);
            }

            if (Input.GetKey(KeyCode.A) == true)
            {
                _player.HorizontalDirection = -1;

                _playerTransform.localScale = new Vector3(-1, 1, 1);

                _playerAnimator.SetBool("isRunning", true);
            }
        }

        private void Jump()
        {
            if (_attackTimer >= 0 || _rollTimer >= 0f) return;

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
            if (_rollTimer >= 0f)
            {
                _rollTimer -= Time.deltaTime;

                if (_rollTimer <= 0.5f)
                {
                    _player.RollDirection = 0;
                }

                return;
            }

            if (_attackTimer >= 0) return;

            if (Input.GetKeyDown(KeyCode.LeftShift) == true && _groundSensor.IsTriggered == true)
            {
                if (_player.StaminaUsage() == false) return;

                _rollTimer = 1f;

                if (_playerTransform.localScale.x == 1)
                {
                    _player.RollDirection = 1;
                }
                else
                {
                    _player.RollDirection = -1;
                }

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
                    if (_player.IsAttacking)
                    {
                        _playerAttackPoint.MeleeAttack(_currentAttack, (int)_player.transform.localScale.x);
                    }
                    _player.IsAttacking = false;
                }
                if (_attackTimer < 0.4f)
                {
                    _currentAttack = 0;
                }
            }

            if (_player.IsAttacking || _rollTimer >= 0f) return;

            if (Input.GetMouseButtonDown(0) == true && _groundSensor.IsTriggered == true)
            {
                if (_player.StaminaUsage() == false) return;

                _player.IsAttacking = true;

                _attackTimer = _player.AttackRate;

                _currentAttack++;

                if (_currentAttack > _player.MaxAttackCombo || _currentAttack == 0)
                {
                    _currentAttack = 1;
                }
                _playerAnimator.SetTrigger("attack" + _currentAttack);
            }
        }

        private void Block()
        {
            _player.IsBlocking = false;

            if (_attackTimer >= 0f || _rollTimer >= 0f) return;

            if (Input.GetMouseButton(1) == true && _groundSensor.IsTriggered == true)
            {
                _playerAnimator.SetBool("isBlocking",true);

                _player.IsBlocking = true;
            }
        }

        private void OnDeath()
        {
            _player.EventOnDeath.RemoveListener(OnDeath);

            _playerAnimator.SetTrigger("dead");
        }

        public void SetPlayer(Fighter player)
        {
            _player = player;

            GetComponents(player);
        }

        private void GetComponents(Fighter player)
        {
            //_player.EventOnDeath.AddListener(OnDeath);

            _playerTransform = player.GetComponent<Transform>();
            _playerAnimator = player.GetComponentInChildren<Animator>();
            _groundSensor = player.GetComponentInChildren<Sensor>();
            _playerAttackPoint = player.GetComponentInChildren<AttackPoint>();
        }
    }
}