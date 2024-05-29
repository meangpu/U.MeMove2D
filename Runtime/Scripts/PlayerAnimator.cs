using UnityEngine;

namespace Meangpu.Move2D
{
    public class PlayerAnimator : MonoBehaviour
    {
        private PlayerMovement _moveScpt;
        private Animator _anim;
        private SpriteRenderer _spriteRend;

        [Header("Movement Tilt")]
        [SerializeField] private float _maxTilt = 20;
        [SerializeField][Range(0, 1)] private float _tiltSpeed = .08f;

        [Header("Particle FX")]
        [SerializeField] private ParticleSystem _jumpFX;
        [SerializeField] private ParticleSystem _landFX;
        [SerializeField] bool _isUseDashAnim;

        public bool StartedJumping { private get; set; }
        public bool JustLanded { private get; set; }
        public bool JustDash { private get; set; }

        private void Start()
        {
            _moveScpt = GetComponent<PlayerMovement>();
            _spriteRend = GetComponentInChildren<SpriteRenderer>();
            _anim = _spriteRend.GetComponent<Animator>();
        }

        private void LateUpdate()
        {
            #region Tilt
            float tiltProgress;

            int mult = -1;

            if (_moveScpt.IsSliding)
            {
                tiltProgress = 0.25f;
            }
            else
            {
                tiltProgress = Mathf.InverseLerp(-_moveScpt.Data.runMaxSpeed, _moveScpt.Data.runMaxSpeed, _moveScpt.RB.velocity.x);
                mult = _moveScpt.IsFacingRight ? 1 : -1;
            }

            float newRot = (tiltProgress * _maxTilt * 2) - _maxTilt;
            float rot = Mathf.LerpAngle(_spriteRend.transform.localRotation.eulerAngles.z * mult, newRot, _tiltSpeed);
            _spriteRend.transform.localRotation = Quaternion.Euler(0, 0, rot * mult);
            #endregion

            CheckAnimationState();
        }

        private void CheckAnimationState()
        {
            if (StartedJumping)
            {
                _anim.SetTrigger("Jump");
                _jumpFX?.Play();
                StartedJumping = false;
                return;
            }

            if (JustLanded)
            {
                _anim.SetTrigger("Land");
                _landFX?.Play();
                JustLanded = false;
                return;
            }

            if (JustDash && _isUseDashAnim)
            {
                _anim.SetTrigger("Dash");
                JustDash = false;
                return;
            }

            _anim.SetFloat("Vel Y", _moveScpt.RB.velocity.y);
        }
    }
}

