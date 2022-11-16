using UnityEngine;

namespace DestinyBlade
{
    public class SyncTransforms : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;

        private void FixedUpdate()
        {
            transform.position = (Vector2)_targetTransform.position;
        }
    }
}