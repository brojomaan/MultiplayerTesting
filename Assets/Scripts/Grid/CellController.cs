using UnityEngine;

namespace Grid
{
    public class CellController : MonoBehaviour
    {
        public CellInstance cell;
    
        [SerializeField] private GridTile gridTilePrefab;


        public void Initialize(Vector2Int gridPosition, bool lightColour, int groupIndex)
        {
            cell = new CellInstance(gridPosition, groupIndex);
        
            GridTile gridTile = Instantiate(gridTilePrefab, transform, false);
            gridTile.Initialize(lightColour, groupIndex);

            cell.SetGridTile(gridTile);
        }

        private void OnMouseEnter()
        {
            cell.OnHover();
        }

        private void OnMouseExit()
        {
            cell.OnHoverExit();
        }

        private void OnMouseDown()
        {
            cell.OnClick();
        }
    }
}
