using UnityEngine;

namespace Grid
{
    public class Grid2D : MonoBehaviour
    {
        [SerializeField] private Transform root;
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float cellSize;

        [SerializeField] private CellController cellControllerPrefab;

        private bool isLight = false;
        private int groupIndex;

        public void Initialize()
        {
            CreateGrid();
            groupIndex = height;
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
            float middleX = cellSize * width / 2 - 1;
            float middleY = cellSize * height / 2 - 3;
        
            root.transform.position = new Vector3(-middleX, -middleY, 0);
        
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CellController cc = Instantiate(cellControllerPrefab, transform, false);
                    cc.transform.localPosition = new Vector3(x * cellSize, y * cellSize, 0);
                    Vector2Int gridPosition = new Vector2Int(x, y);
                    cc.Initialize(gridPosition, isLight, GetGroupIndex());
                    groupIndex--;

                
                    isLight = !isLight;
                }
            }
        }
    }
}
