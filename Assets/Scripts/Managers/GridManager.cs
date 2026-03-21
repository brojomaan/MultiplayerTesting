using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;
        public float cellSize = 2.05f;
        [SerializeField] private Grid2D grid;
        private Dictionary<Vector2Int, CellController> cellControllerDictionary = 
            new Dictionary<Vector2Int, CellController>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
            }

            Instance = this;
        }
        public void Initialize()
        {
            grid.Initialize();
        }

        public void AddCellController(CellController cellController)
        {
            cellControllerDictionary.Add(cellController.cell.iGridPosition, cellController);
        }

        public CellController GetCellAtGridPosition(Vector2Int gridPosition)
        {
            cellControllerDictionary.TryGetValue(gridPosition, out CellController cellController);
            return cellController;
        }

        public Vector3 GetWorldPositionAtGridPosition(Vector2Int gridPosition)
        {
            float x = gridPosition.x * cellSize;
            float y = gridPosition.y * cellSize;
            
            return new Vector3(x, y, 0);
        }
    }
}
