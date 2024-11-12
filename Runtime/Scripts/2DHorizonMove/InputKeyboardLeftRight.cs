using Meangpu.Setting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Meangpu
{
    public class InputKeyboardLeftRight : MonoBehaviour
    {
        [SerializeField] FloatReference _horizontalMove;
        [Header("Input Settings")]
        [SerializeField] private float keyboardSensitivity = 2f;
        [SerializeField] private float smoothing = 0.1f;

        [SerializeField] InputActionReference _leftInput;
        [SerializeField] InputActionReference _rightInput;

        private Vector3 currentTilt;
        private Vector3 smoothTilt;

        void Update()
        {
            if (SettingManager.Instance.IsNowOpenSetting) return;

            Vector3 targetTilt = Vector3.zero;
            float horizontalInput = 0f;

            if (_leftInput.action.IsPressed()) horizontalInput -= 1f;
            if (_rightInput.action.IsPressed()) horizontalInput += 1f;
            if (horizontalInput != 0f)
            {
                targetTilt.x = horizontalInput * keyboardSensitivity;
            }

            smoothTilt = Vector3.Lerp(smoothTilt, targetTilt, smoothing);
            currentTilt = smoothTilt;
            _horizontalMove.Variable.SetValue(GetHorizontalTilt());
        }

        public float GetHorizontalTilt() => Mathf.Clamp(currentTilt.x, -1f, 1f);

    }
}