using UnityEngine;

namespace DestinyBlade
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelPassArea : MonoBehaviour
    {
        [SerializeField] private Fighter _player;
        [SerializeField] private MovementController _playerController;

        private bool _levelPassed;
        public bool LevelPassed => _levelPassed;

        private void Start()
        {
            _levelPassed = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.root.GetComponent<Fighter>() == _player)
            {
                _player.HorizontalDirection = 1;

                _playerController.enabled = false;

                _levelPassed = true;
            }
        }

        public void SetPlayer(Fighter player)
        {
            _player = player;
        }
    }
}