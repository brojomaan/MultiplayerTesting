using System.Net;
using Database;
using Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class CardController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CardData data;
        [SerializeField] private CardVisual visual;
        [SerializeField] private int indexInHand;
        [SerializeField] private NetworkPlayer owner;


        public void Initialize(int cardId, CardVisual cardVisual, NetworkPlayer player, int index)
        {
            data = CardDatabase.Instance.GetCard(cardId);

            owner = player;
            indexInHand = index;
            
            visual = cardVisual;
            visual.Wrap(data);
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"OnPointerClick: {data.id}");

            owner.SubmitCardServerRpc(indexInHand);
            
            Hide();
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            visual.Shake();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            visual.ResetShake();
        }
    }
}
