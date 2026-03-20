using Grid;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Grid2D grid;

    public void Initialize()
    {
        grid.Initialize();
    }
}
