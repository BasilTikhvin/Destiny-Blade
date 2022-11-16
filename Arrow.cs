using UnityEngine;

namespace DestinyBlade
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;

        private Rigidbody2D _rigidbody;
        private int _direction;
        private Fighter _target;

        private void Start()
        {
            _rigidbody = transform.GetComponent<Rigidbody2D>();

            _rigidbody.AddForce(new Vector2(_direction * _speed, _speed / 10), ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _target = collision.transform.root.GetComponent<Fighter>();

            if (_target != null)
            {
                if (_target.IsBlocking && _target.FaceDirection != _direction)
                {
                    if (_target.StaminaUsage() == false)
                    {
                        _target.TakeDamage(_damage);
                    }
                }
                else
                {
                    _target.TakeDamage(_damage);
                }
            }
            Destroy(gameObject);
        }

        public void SetArrowDirection(int attackerFaceDirection)
        {
            _direction = attackerFaceDirection;
        }
    }
}