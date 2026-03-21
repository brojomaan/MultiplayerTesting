using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerPawnVisual : MonoBehaviour
    {
        [SerializeField] private Transform root;
        [SerializeField] private Transform pawn;
        [SerializeField] private Transform pawnHead;
        [SerializeField] private Transform shield;
        [SerializeField] private Transform sword;
        private Vector3 shieldStartPos;
        private Vector3 swordStartPos;
        private Vector3 pawnHeadStartPos;

        private Tween currentTween;
        private Sequence currentSequence;

        public void Initialize()
        {
            Debug.Log("Initializing PlayerPawnVisual");
            shieldStartPos = shield.localPosition;
            swordStartPos = sword.localPosition;
            pawnHeadStartPos = pawn.localPosition;
        }

        public void OnHoverEnter()
        {
            currentTween?.Kill();

            currentTween = pawn.transform.DOLocalMoveY(-0.15f, 0.1f).SetEase(Ease.InOutQuad);
        }

        public void Shake()
        {
            currentSequence?.Kill();

            Sequence s = DOTween.Sequence();
            currentSequence = s;
            s.Append(pawnHead.transform.DOShakePosition(0.5f, 0.1f)).SetEase(Ease.InOutQuad);
            s.Join(shield.transform.DOShakePosition(0.5f, 0.1f)).SetEase(Ease.InOutQuad);
            s.Join(sword.transform.DOShakePosition(0.5f, 0.1f)).SetEase(Ease.InOutQuad);
        }

        public void ResetShake()
        {
            currentSequence?.Kill();
            
            Sequence s = DOTween.Sequence();
            currentSequence = s;
            
            s.Append(pawnHead.transform.DOMove(pawnHeadStartPos, 0.1f)).SetEase(Ease.InOutQuad);
            s.Join(shield.transform.DOMove(shieldStartPos, 0.1f)).SetEase(Ease.InOutQuad);
            s.Join(sword.transform.DOMove(swordStartPos, 0.1f)).SetEase(Ease.InOutQuad);
            
        }
        
        public void OnHoverExit()
        {
            currentTween?.Kill();
            
            currentTween = pawn.transform.DOLocalMoveY(0f, 0.1f).SetEase(Ease.InOutQuad);
        }
    }
}