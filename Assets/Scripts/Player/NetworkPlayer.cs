using System.Collections.Generic;
using Cards;
using Managers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class NetworkPlayer : NetworkBehaviour
    {
        public Deck deck = new Deck();
        public PlayerHand hand = new PlayerHand();
        public List<int> programmedCards = new List<int>();

        private int cardAmount = 7;

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;
            GameManager.Instance.RegisterPlayer(this);
            List<int> startDeck = new List<int>()
            {
                0, 0, 0, 0, 0, 0,
                1, 1, 1,
                2, 2, 2
            };

            deck.Initialize(startDeck);

            deck.Shuffle();

        }
        
        public void DrawHand()
        {
            hand.Clear();

            for (int i = 0; i < cardAmount; i++)
            {
                int card = deck.DrawCard();

                if (card != -1)
                {
                    hand.AddCard(card);
                }
            }

            SendHandToClient();
        }

        [Rpc(SendTo.SpecifiedInParams)]
        private void SendHandRpc(int[] cards, RpcParams rpcParams = default)
        {
            if (!IsOwner) return;
            
            Debug.Log($"Recieved hand with {cards.Length} cards");

            CardDockController controller = FindFirstObjectByType<CardDockController>();

            controller.ShowHand(cards, this);
        }

        [Rpc(SendTo.Server)]
        public void SubmitCardServerRpc(int slot)
        {

            if (GameManager.Instance.gameState.Value != GameState.Programming) return;

            int card = hand.GetCard(slot);

            programmedCards.Add(card);
            
            GameManager.Instance.CheckAllPlayersSubmitted();
        }

        private void SendHandToClient()
        {
            SendHandRpc(
                hand.Cards.ToArray(),
                RpcTarget.Single(OwnerClientId, RpcTargetUse.Temp)
            );
        }

        private void Update()
        {
            if (!IsOwner) return;

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                Debug.Log($"Playing Card");
                SubmitCardServerRpc(0);
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                Debug.Log($"Playing Card");
                SubmitCardServerRpc(1);
            }

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                Debug.Log($"Playing Card");
                SubmitCardServerRpc(2);
            }
        }
    }
}
