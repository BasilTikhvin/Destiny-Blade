using UnityEngine;

namespace DestinyBlade
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        private LayerMask _enemyLayerMask;

        private Rigidbody2D _rigidbody;
        private int _direction;
        private Fighter _target;
        private float stepLenght;
        RaycastHit2D hit;

        private void Start()
        {
            _rigidbody = transform.GetComponent<Rigidbody2D>();

            transform.localScale = new Vector3(_direction, 1, 1);

            _rigidbody.AddForce(new Vector2(_direction * _speed, _speed / 10), ForceMode2D.Impulse);
        }

        private void FixedUpdate()
        {
            stepLenght = _speed * Time.fixedDeltaTime;
            
            hit = Physics2D.Raycast(transform.position, _direction * stepLenght * transform.right, stepLenght, _enemyLayerMask);

            if (hit)
            {
                if (hit.collider.TryGetComponent(out _target))
                {
                    if (_target.IsBlocking && _target.transform.localScale.x != _direction)
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
        }

        public void SetArrowSettings(int attackerFaceDirection, LayerMask enemyLayerMask)
        {
            _direction = attackerFaceDirection;

            _enemyLayerMask = enemyLayerMask;
        }
    }
}