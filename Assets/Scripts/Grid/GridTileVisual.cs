using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Grid
{
    public class GridTileVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer tileTop;
        [SerializeField] private SortingGroup sortingGroup;

        [SerializeField] private Transform root;

        [SerializeField] private Color lightColor;
        [SerializeField] private Color darkColor;

        private Tween currentTween;
    
        public void Initialize(bool lightColour, int groupIndex)
        {
            sortingGroup.sortingOrder = groupIndex;
            tileTop.color = lightColour ? lightColor : darkColor;
        }

        public void OnHover()
        {
            currentTween?.Kill();

            currentTween = tileTop.transform.DOLocalMoveY(-0.15f, 0.05f).SetEase(Ease.InOutQuad);
        }

        public void OnReset()
        {
            currentTween?.Kill();

            currentTween = tileTop.transform.DOLocalMoveY(0, 0.05f).SetEase(Ease.InOutQuad);
        }
    }
}
