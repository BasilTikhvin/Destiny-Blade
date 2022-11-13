using UnityEngine;

namespace DestinyBlade
{
    [RequireComponent(typeof(MeshRenderer))]
    public class BackgroundParallax : MonoBehaviour
    {
        [SerializeField][Range(0.0f, 5.0f)] private float parallaxPower;

        [SerializeField] private float textureScale;

        private Material quadMaterial;

        private void Start()
        {
            quadMaterial = GetComponent<MeshRenderer>().material;

            quadMaterial.mainTextureScale = Vector2.one * textureScale;
        }
        private void Update()
        {
            Vector2 offset = Vector2.one;

            offset.x += transform.position.x / transform.localScale.x / parallaxPower;
            offset.y += transform.position.y / transform.localScale.y / parallaxPower;

            quadMaterial.mainTextureOffset = offset;
        }
    }
}

