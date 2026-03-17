using System.Collections.Generic;
using UnityEngine;


namespace Cards
{
    [System.Serializable]
    public class Deck
    {
        private List<int> cards = new List<int>();

        public void Initialize(List<int> startingCards)
        {
            cards = new List<int>(startingCards);
            
        }

        public void Shuffle()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                int rand = Random.Range(i, cards.Count);
                
                int temp = cards[i];
                cards[i] = cards[rand];
                cards[rand] = temp;
            }
        }

        public int DrawCard()
        {
            if (cards.Count == 0)
            {
                Debug.LogWarning("Deck is empty!");
                return -1;
            }

            int card = cards[0];
            cards.RemoveAt(0);

            return card;
        }
    }
}
