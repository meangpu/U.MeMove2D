using UnityEngine;
using DG.Tweening;

namespace Meangpu.Move2D
{
    // modify from "GhostTail" from [mixandjam/Celeste-Movement: Recreating the movement and feel from Celeste](https://github.com/mixandjam/Celeste-Movement)
    public class DashGhostFx : MonoBehaviour
    {
        [SerializeField] Transform _playerParent;
        [Header("MainColor")]
        [SerializeField] Color _trailColor;
        [SerializeField] Color _fadeColor;
        [Header("Disable")]
        [SerializeField] Color _disableColor;
        [Header("Setting")]
        [SerializeField] float _ghostInterval = 0.05f;
        [SerializeField] float _fadeTime = .5f;

        private void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<SpriteRenderer>().material.color = _disableColor;
            }
        }

        public void ShowGhost()
        {
            Sequence s = DOTween.Sequence();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform currentGhost = transform.GetChild(i);
                s.AppendCallback(() => currentGhost.position = _playerParent.position);
                s.Append(currentGhost.GetComponent<SpriteRenderer>().material.DOColor(_trailColor, 0));
                s.AppendCallback(() => FadeSprite(currentGhost));
                s.AppendInterval(_ghostInterval);
            }
        }

        void FadeSprite(Transform current)
        {
            current.GetComponent<SpriteRenderer>().material.DOKill();
            current.GetComponent<SpriteRenderer>().material.DOColor(_fadeColor, _fadeTime);
        }

    }
}