using Meangpu.Setting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Meangpu
{
    public class InputDeviceTiltLeftRight : MonoBehaviour
    {
        [SerializeField] FloatReference _horizontalMove;
        [Header("Input Settings")]
        [SerializeField] private float tiltSensitivity = 2f;
        [SerializeField] private float smoothing = 0.1f;

        private Vector3 currentTilt;
        private Vector3 smoothTilt;

        void Start()
        {
            EnableAccelerometer();
        }

        private void EnableAccelerometer()
        {
            if (Accelerometer.current != null)
            {
                InputSystem.EnableDevice(Accelerometer.current);
                Debug.Log("Accelerometer enabled");
            }
            else
            {
                Debug.Log("No accelerometer found");
            }
        }

        void Update()
        {
            if (SettingManager.Instance.IsNowOpenSetting) return;

            Vector3 targetTilt = Vector3.zero;

            if (targetTilt.magnitude < 0.01f)
            {
                if (Accelerometer.current != null)
                {
                    Accelerometer accelerometer = Accelerometer.current;
                    if (accelerometer.enabled)
                    {
                        targetTilt = accelerometer.acceleration.ReadValue();
                    }
                }
                targetTilt *= tiltSensitivity;
            }

            smoothTilt = Vector3.Lerp(smoothTilt, targetTilt, smoothing);
            currentTilt = smoothTilt;
            _horizontalMove.Variable.SetValue(GetHorizontalTilt());
        }

        public float GetHorizontalTilt() => Mathf.Clamp(currentTilt.x, -1f, 1f);

        void OnDestroy()
        {
            if (Accelerometer.current != null) InputSystem.DisableDevice(Accelerometer.current);
        }
    }
}