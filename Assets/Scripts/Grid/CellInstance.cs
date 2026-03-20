using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class CellInstance
    {
        public Vector2Int iGridPosition;
        public int iGroupIndex;
        public GridTile iGridTile;


        public CellInstance(Vector2Int gridPosition, int gIndex)
        {
            iGridPosition = gridPosition;
            iGroupIndex = gIndex;
        }

        public void SetGridTile(GridTile gridTile)
        {
            iGridTile = gridTile;
        }
        
    }
}
