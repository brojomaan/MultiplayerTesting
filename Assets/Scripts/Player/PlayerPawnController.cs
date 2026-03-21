using DG.Tweening;
using Grid;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerPawnController : MonoBehaviour
    {
        [SerializeField] private PlayerPawnVisual prefab;

        [SerializeField] private Transform root;
        [SerializeField] private PlayerPawnVisual visual;

        private Tween currentTween;
        
        public void Initialize(CellController cellController)
        {
            cellController.cell.SetPawnController(this);

            visual = Instantiate(prefab, transform, false);
            visual.Initialize();
        }
        public void OnHoverEnter()
        {
            visual.OnHoverEnter();
        }

        public void OnHoverExit()
        {
            visual.OnHoverExit();
            visual.ResetShake();
        }

        public void OnMouseClick()
        {
            visual.Shake();
        }

        public void Move(Vector2Int gridPosition)
        {
            currentTween?.Kill();

            Vector3 movePosition = GridManager.Instance.GetWorldPositionAtGridPosition(gridPosition);
            currentTween = root.DOMove(movePosition, 0.3f);
        }
    }
}