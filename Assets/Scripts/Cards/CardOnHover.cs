using UnityEngine;

namespace Cards
{
    public class CardOnHover : MonoBehaviour
    {
        [SerializeField] private Transform tooltip;
    
        public void Show()
        {
            tooltip.gameObject.SetActive(true);
        }

        public void Hide()
        {
            tooltip.gameObject.SetActive(false);
        }
    }
}
