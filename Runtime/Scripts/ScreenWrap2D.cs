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
            public FloatRange boundRange;
        }

        [Header("Wrap Settings")]
        [SerializeField] private bool useScreenBounds = true;
        [SerializeField] SpriteRenderer _objectSize;

        [SerializeField]
        private WrapAxis xAxis = new WrapAxis
        {
            enabled = true,
            offset = 0.5f,
            boundRange = new(-5.5f, 5.5f)
        };

        [SerializeField]
        private WrapAxis yAxis = new()
        {
            enabled = true,
            offset = 0.5f,
            boundRange = new(-5.5f, 5.5f)
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
            if (_objectSize != null)
            {
                objectSize = _objectSize.bounds.size;
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
            xAxis.boundRange.Minimum = -screenBounds.x;
            xAxis.boundRange.Maximum = screenBounds.x;
            yAxis.boundRange.Minimum = -screenBounds.y;
            yAxis.boundRange.Maximum = screenBounds.y;
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
                if (transform.position.x > xAxis.boundRange.Maximum + xAxis.offset + halfWidth)
                {
                    newPosition.x = xAxis.boundRange.Minimum - xAxis.offset;
                    positionChanged = true;
                }
                else if (transform.position.x < xAxis.boundRange.Minimum - xAxis.offset - halfWidth)
                {
                    newPosition.x = xAxis.boundRange.Maximum + xAxis.offset;
                    positionChanged = true;
                }
            }

            if (yAxis.enabled)
            {
                float halfHeight = objectSize.y * 0.5f;
                if (transform.position.y > yAxis.boundRange.Maximum + yAxis.offset + halfHeight)
                {
                    newPosition.y = yAxis.boundRange.Minimum - yAxis.offset;
                    positionChanged = true;
                }
                else if (transform.position.y < yAxis.boundRange.Minimum - yAxis.offset - halfHeight)
                {
                    newPosition.y = yAxis.boundRange.Maximum + yAxis.offset;
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
                Gizmos.DrawLine(new Vector3(xAxis.boundRange.Minimum, -10, 0), new Vector3(xAxis.boundRange.Minimum, 10, 0));
                Gizmos.DrawLine(new Vector3(xAxis.boundRange.Maximum, -10, 0), new Vector3(xAxis.boundRange.Maximum, 10, 0));
            }

            if (yAxis.enabled)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(new Vector3(-10, yAxis.boundRange.Minimum, 0), new Vector3(10, yAxis.boundRange.Minimum, 0));
                Gizmos.DrawLine(new Vector3(-10, yAxis.boundRange.Maximum, 0), new Vector3(10, yAxis.boundRange.Maximum, 0));
            }
        }
    }
}