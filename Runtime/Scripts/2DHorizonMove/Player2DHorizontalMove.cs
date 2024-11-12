using Meangpu.Setting;
using UnityEngine;

namespace Meangpu
{
    public class Player2DHorizontalMove : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float maxSpeed = 8f;
        [SerializeField] private float accelerationTime = 0.1f;
        [SerializeField] private float decelerationTime = 0.05f;
        [SerializeField] private float velocityPower = 0.9f; // Affects acceleration curve

        [Header("References")]
        [SerializeField] Rigidbody2D _rb;
        [SerializeField] FloatReference _horizontalMoveInput;
        private float velocityTarget;
        private float accelerationRate;
        private float decelerationRate;

        private void Start()
        {
            _horizontalMoveInput.Variable.SetValue(0);
            accelerationRate = maxSpeed / accelerationTime;
            decelerationRate = maxSpeed / decelerationTime;
        }

        private void FixedUpdate()
        {
            if (SettingManager.Instance.IsNowOpenSetting) return;

            velocityTarget = _horizontalMoveInput * maxSpeed;
            float currentVelocity = _rb.linearVelocityX;
            float velocityDiff = velocityTarget - currentVelocity;
            float acceleration = (Mathf.Abs(velocityTarget) > 0.01f) ? accelerationRate : decelerationRate;
            float movement = Mathf.Pow(Mathf.Abs(velocityDiff) * acceleration, velocityPower) * Mathf.Sign(velocityDiff);
            movement = Mathf.Clamp(movement, -Mathf.Abs(velocityDiff) * acceleration, Mathf.Abs(velocityDiff) * acceleration);
            _rb.AddForce(movement * Vector2.right);
            _rb.linearVelocityX = Mathf.Clamp(_rb.linearVelocityX, -maxSpeed, maxSpeed);
        }
    }

}