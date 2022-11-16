using UnityEngine;

namespace DestinyBlade
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Sensor : MonoBehaviour
    {
        private bool _isTriggered;
        public bool IsTriggered => _isTriggered;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _isTriggered = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _isTriggered = false;
        }
    }
}