using UnityEngine;
using UnityEngine.EventSystems;

public class DiscardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private DiscardVisual discardVisualPrefab;
    
    [Header("Transforms")]
    [SerializeField] private RectTransform rootRect;

    [Header("visuals")]
    [SerializeField] private DiscardVisual visual;

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        visual = Instantiate(discardVisualPrefab, this.transform, false);
        visual.Initialize();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        visual.ShakeDeck();
        visual.BackgroundAlpha(0.6f, 1, 0.3f);
        visual.BorderScale(1.1f, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        visual.ResetPosition();
        visual.BackgroundAlpha(1f, 0.6f, 0.3f);
        visual.BorderScale(1f, 0.1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
