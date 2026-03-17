using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class CardDatabase : MonoBehaviour
    {
        public static CardDatabase Instance;

        public List<CardData> Cards = new List<CardData>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
            }

            Instance = this;
        }

        public CardData GetCard(int id)
        {
            if (id < 0 || id >= Cards.Count)
            {
                Debug.LogError($"Invalid card ID: {id}");
                return null;
            }
            
            return Cards[id];
        }
    }
}
