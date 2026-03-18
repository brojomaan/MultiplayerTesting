using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class DeckController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private DeckVisual deckVisualPrefab;
        [SerializeField] private DeckVisual visual;


        private void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            visual = Instantiate(deckVisualPrefab, transform, false);
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

        public RectTransform SendTransform()
        {
            return transform as RectTransform;
        }
    }
}
