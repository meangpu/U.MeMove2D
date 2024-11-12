using UnityEngine;

namespace Meangpu
{
    public class ScreenWrap2D : MonoBehaviour
    {
        [System.Serializable]
        private struct WrapAxis
        {
            public bool enabled;
            public float offset;
            public float minBound;
            public float maxBound;
        }

        [Header("Wrap Settings")]
        [SerializeField] private bool useScreenBounds = true;
        [SerializeField]
        private WrapAxis xAxis = new WrapAxis
        {
            enabled = true,
            offset = 0.5f,
            minBound = -8.5f,
            maxBound = 8.5f
        };

        [SerializeField]
        private WrapAxis yAxis = new WrapAxis
        {
            enabled = true,
            offset = 0.5f,
            minBound = -4.5f,
            maxBound = 4.5f
        };

        private Camera mainCamera;
        private Vector2 objectSize;
        private Vector2 screenBounds;
        private bool isInitialized;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (isInitialized) return;
            mainCamera = Camera.main;
            if (!mainCamera)
            {
                Debug.LogWarning("No main camera found!");
                enabled = false;
                return;
            }

            if (TryGetComponent(out SpriteRenderer sprite))
            {
                objectSize = sprite.bounds.size;
            }
            else
            {
                objectSize = Vector2.one;
            }

            if (useScreenBounds)
            {
                CalculateScreenBounds();
            }
            isInitialized = true;
        }

        private void CalculateScreenBounds()
        {
            screenBounds = mainCamera.ScreenToWorldPoint(
                new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z)
            );

            // Update bounds based on screen size
            xAxis.minBound = -screenBounds.x;
            xAxis.maxBound = screenBounds.x;
            yAxis.minBound = -screenBounds.y;
            yAxis.maxBound = screenBounds.y;
        }

        private void LateUpdate()
        {
            if (!isInitialized)
            {
                Initialize();
                return;
            }

            Vector3 newPosition = transform.position;
            bool positionChanged = false;

            if (xAxis.enabled)
            {
                float halfWidth = objectSize.x * 0.5f;
                if (transform.position.x > xAxis.maxBound + xAxis.offset + halfWidth)
                {
                    newPosition.x = xAxis.minBound - xAxis.offset;
                    positionChanged = true;
                }
                else if (transform.position.x < xAxis.minBound - xAxis.offset - halfWidth)
                {
                    newPosition.x = xAxis.maxBound + xAxis.offset;
                    positionChanged = true;
                }
            }

            if (yAxis.enabled)
            {
                float halfHeight = objectSize.y * 0.5f;
                if (transform.position.y > yAxis.maxBound + yAxis.offset + halfHeight)
                {
                    newPosition.y = yAxis.minBound - yAxis.offset;
                    positionChanged = true;
                }
                else if (transform.position.y < yAxis.minBound - yAxis.offset - halfHeight)
                {
                    newPosition.y = yAxis.maxBound + yAxis.offset;
                    positionChanged = true;
                }
            }

            if (positionChanged)
            {
                transform.position = newPosition;
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !isInitialized) return;

            if (xAxis.enabled)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(new Vector3(xAxis.minBound, -10, 0), new Vector3(xAxis.minBound, 10, 0));
                Gizmos.DrawLine(new Vector3(xAxis.maxBound, -10, 0), new Vector3(xAxis.maxBound, 10, 0));
            }

            if (yAxis.enabled)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(new Vector3(-10, yAxis.minBound, 0), new Vector3(10, yAxis.minBound, 0));
                Gizmos.DrawLine(new Vector3(-10, yAxis.maxBound, 0), new Vector3(10, yAxis.maxBound, 0));
            }
        }
    }
}