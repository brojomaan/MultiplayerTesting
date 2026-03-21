using Player;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class CellInstance
    {
        public Vector2Int iGridPosition;
        public int iGroupIndex;
        public GridTile iGridTile;
        public PlayerPawnController iPawnController;
        public bool isOccupied;


        public CellInstance(Vector2Int gridPosition, int gIndex)
        {
            iGridPosition = gridPosition;
            iGroupIndex = gIndex;
        }

        public void SetGridTile(GridTile gridTile)
        {
            iGridTile = gridTile;
        }

        public void SetPawnController(PlayerPawnController pawnController)
        {
            iPawnController = pawnController;
        }

        public void OnHover()
        {
            if (iGridTile != null)
                iGridTile.OnHoverEnter();
            if (iPawnController != null)
                iPawnController.OnHoverEnter();
        }

        public void OnHoverExit()
        {
            if (iGridTile != null)
                iGridTile.OnHoverExit();
            if (iPawnController != null)
                iPawnController.OnHoverExit();
        }

        public void OnClick()
        {
            if (iPawnController != null)
                iPawnController.OnMouseClick();
        }
        
    }
}
