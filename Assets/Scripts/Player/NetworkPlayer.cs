using System.Collections.Generic;
using Managers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class NetworkPlayer : NetworkBehaviour
    {
        public List<int> hand = new List<int>();
        public List<int> programmedCards = new List<int>();

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;
            GameManager.Instance.RegisterPlayer(this);
            GenerateHand();
        }
        
        public void GenerateHand()
        {
            hand.Clear();

            for (int i = 0; i < 3; i++)
            {
                int card = Random.Range(0, 5);
                hand.Add(card);
            }
        
            SendHandClientRpc(hand.ToArray(), OwnerClientId);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void SendHandClientRpc(int[] cards, ulong targetClient)
        {
            if (NetworkManager.Singleton.LocalClientId != targetClient)
                return;

            hand = new List<int>(cards);

            /*Debug.Log($"My Hand: ");*/

            for (int i = 0; i < hand.Count; i++)
            {
                /*Debug.Log($"slot {i} card {hand[i]}");*/
            }
        }

        [Rpc(SendTo.Server)]
        public void SubmitCardServerRpc(int slot)
        {

            if (GameManager.Instance.gameState.Value != GameState.Programming) return;
            
            int card = hand[slot];

            programmedCards.Add(card);
        
            /*Debug.Log($"Player: {OwnerClientId} Submitted card {card}");*/

            GameManager.Instance.CheckAllPlayersSubmitted();
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
