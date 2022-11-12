using UnityEngine;
using UnityEngine.UI;

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
        [Header("Physics")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _rollForce;
        [Header("Combat")]
        [SerializeField] private int _maxAttackCombo;
        public int MaxAttackCombo => _maxAttackCombo;
        public bool IsBlocking { get; set; }

        private float _currentStamina;
        public float CurrentStamina => _currentStamina;
        private float _staminaRecoveryTimer;

        public float FaceDirection { get; set; }
        public float HorizontalDirection { get; set; }
        public float VerticalDirection { get; set; }
        public float RollDirection { get; set; }

        private Rigidbody2D _rigidbody;

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
            _rigidbody.velocity = new Vector2(HorizontalDirection * _moveSpeed * Time.fixedDeltaTime, _rigidbody.velocity.y);

            _rigidbody.AddForce(VerticalDirection * transform.up * _jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);

            _rigidbody.AddForce(RollDirection * transform.right * _rollForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
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