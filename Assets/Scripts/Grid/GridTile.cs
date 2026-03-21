using UnityEngine;

namespace Grid
{
    public class GridTile : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GridTileVisual prefab;
    
    
        [Header("Visuals")]
        [SerializeField] private GridTileVisual visual;

        public void Initialize(bool lightColour, int groupIndex)
        {
            visual = Instantiate(prefab, transform, false);
            visual.Initialize(lightColour, groupIndex);
        }

        public void OnHoverEnter()
        {
            visual.OnHover();
        }

        public void OnHoverExit()
        {
            visual.OnReset();
        }
    }
}
