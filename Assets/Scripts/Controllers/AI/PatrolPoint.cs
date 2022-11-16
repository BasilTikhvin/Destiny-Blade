using UnityEngine;

[RequireComponent(typeof(Transform))]
public class PatrolPoint : MonoBehaviour
{
    [SerializeField] private float _radius;
    public float Radius => _radius;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
#endif
}
