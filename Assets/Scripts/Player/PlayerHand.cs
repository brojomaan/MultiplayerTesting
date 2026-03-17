using System.Collections.Generic;

namespace Player
{
    [System.Serializable]
    public class PlayerHand
    {
        public List<int> Cards = new List<int>();

        public void Clear()
        {
            Cards.Clear();
        }

        public void AddCard(int cardId)
        {
            Cards.Add(cardId);
        }

        public int GetCard(int index)
        {
            return Cards[index];
        }
    }
}