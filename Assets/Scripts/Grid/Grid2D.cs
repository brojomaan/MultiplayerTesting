using Managers;
using UnityEngine;

namespace Grid
{
    public class Grid2D : MonoBehaviour
    {
        [SerializeField] private Transform root;
        [SerializeField] private int width;
        [SerializeField] private int height;

        [SerializeField] private CellController cellControllerPrefab;

        private bool isLight = false;
        private int groupIndex;
        [SerializeField] private GridManager gridManager;

        public void Initialize()
        {
            groupIndex = height;
            gridManager = GridManager.Instance;
            CreateGrid();
        }

        private int GetGroupIndex()
        {
            if (groupIndex == 0)
            {
                groupIndex = height;
            }
            return groupIndex;
        }
    
    
        public void CreateGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CellController cc = Instantiate(cellControllerPrefab, transform, false);
                    cc.transform.localPosition = new Vector3(x * gridManager.cellSize, y * gridManager.cellSize, 0);
                    Vector2Int gridPosition = new Vector2Int(x, y);
                    cc.Initialize(gridPosition, isLight, GetGroupIndex());
                    groupIndex--;
                    
                    gridManager.AddCellController(cc);

                
                    isLight = !isLight;
                }
            }
        }
    }
}
