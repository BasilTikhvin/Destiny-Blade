using UnityEngine;

namespace DestinyBlade
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Fighter : Destructible
    {
        [Header("Stamina")]
        [SerializeField] private float _maxStamina;
        public float MaxStamina => _maxStamina;
        [SerializeField] private float _staminaUsageValue;
        [SerializeField] private float _staminaRecoveryPerSecond;
        [SerializeField] private float _staminaRecoveryDelay;
        private float _currentStamina;
        public float CurrentStamina => _currentStamina;
        private float _staminaRecoveryTimer;

        [Header("Physics")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _rollForce;
        public int FaceDirection { get; set; }
        public int HorizontalDirection { get; set; }
        public int VerticalDirection { get; set; }
        public int RollDirection { get; set; }
        private Rigidbody2D _rigidbody;

        [Header("Combat")]
        [SerializeField] private float _attackRate;
        public float AttackRate => _attackRate;
        [SerializeField] private int _maxAttackCombo;
        public int MaxAttackCombo => _maxAttackCombo;
        public bool IsAttacking { get; set; }
        public bool IsBlocking { get; set; }

        protected override void Start()
        {
            base.Start();

            _currentStamina = _maxStamina;

            _rigidbody = transform.root.GetComponent<Rigidbody2D>();

            FaceDirection = 1;
        }

        private void FixedUpdate()
        {
            UpdateRigidbody();

            StaminaRecovery();
        }

        private void UpdateRigidbody()
        {
            _rigidbody.velocity = new Vector2(_moveSpeed * HorizontalDirection * Time.fixedDeltaTime, _rigidbody.velocity.y);

            _rigidbody.AddForce(_jumpForce * VerticalDirection * Time.fixedDeltaTime * transform.up, ForceMode2D.Impulse);

            _rigidbody.AddForce(_rollForce * RollDirection * Time.fixedDeltaTime * transform.right, ForceMode2D.Impulse);
        }

        private void StaminaRecovery()
        {
            if (_staminaRecoveryTimer > 0f)
            {
                _staminaRecoveryTimer -= Time.fixedDeltaTime;

                return;
            }

            if (_currentStamina < _maxStamina)
            {
                _currentStamina += _staminaRecoveryPerSecond * Time.fixedDeltaTime;
            }
            else
            {
                _currentStamina = _maxStamina;
            }
        }

        public bool StaminaUsage()
        {
            if (_currentStamina < _staminaUsageValue) return false;

            _currentStamina -= _staminaUsageValue;

            _staminaRecoveryTimer = _staminaRecoveryDelay;

            return true;
        }
    }
}