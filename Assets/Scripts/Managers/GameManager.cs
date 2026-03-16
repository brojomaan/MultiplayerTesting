using System.Collections.Generic;
using Player;
using Structs;
using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Queue<GameCommand> CommandQueue = new Queue<GameCommand>();
        public Dictionary<ulong, NetworkPlayer> Players = new Dictionary<ulong, NetworkPlayer>();
        
        private int currentTurn = 0;
        private int maxTurns = 3;

        public NetworkVariable<GameState> gameState =
            new NetworkVariable<GameState>(GameState.WaitingForPlayers);
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("More than one GameManager instance!");
                Destroy(Instance);
            }

            Instance = this;
        }

        public override void OnNetworkSpawn()
        {
            gameState.OnValueChanged += OnGameStateChanged;

            if (!IsServer)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            }
        }

        public void RegisterPlayer(NetworkPlayer player)
        {
            ulong id = player.OwnerClientId;

            if (!Players.ContainsKey(id))
            {
                Players.Add(id, player);
                
                Debug.Log($"Player Registered: {id}");
            }
        }

        private void OnGameStateChanged(GameState oldState, GameState newState)
        {
            Debug.Log($"Game State changed to: {newState}");
            switch (newState)
            {
                case GameState.DealCards:
                    if (IsServer) DealCards();
                    break;
                case GameState.Programming:
                    Debug.Log($"Players can now submit cards");
                    break;
                case GameState.Reveal:
                    if (IsServer)
                        RevealCards();
                    break;
            }
        }

        private void SetGameState(GameState newState)
        {
            if (!IsServer) return;

            gameState.Value = newState;
        }

        public void CheckAllPlayersSubmitted()
        {
            if (!IsServer) return;
            
            foreach (var player in Players)
            {
                if (player.Value.programmedCards.Count <= currentTurn)
                {
                    return;
                }
            }
            
            Debug.Log($"All Players Submitted cards");

            currentTurn++;

            if (currentTurn >= maxTurns)
            {
                SetGameState(GameState.Reveal);
            }

        }

        private void RevealCards()
        {
            Debug.Log("=== REVEAL CARDS ===");

            BuildCommandQueue();

            ExecuteNextCommand();
        }

        private void DealCards()
        {
            Debug.Log($"Dealind Cards");
            
            NetworkPlayer[] players = FindObjectsByType<NetworkPlayer>(FindObjectsSortMode.None);

            foreach (NetworkPlayer player in players)
            {
                player.GenerateHand();
            }
            
            SetGameState(GameState.Programming);
        }

        private void BuildCommandQueue()
        {
            CommandQueue.Clear();
            
            NetworkPlayer[] players = FindObjectsByType<NetworkPlayer>(FindObjectsSortMode.None);

            for (int turn = 0; turn < maxTurns; turn++)
            {
                foreach (var player in players)
                {
                    GameCommand cmd = new GameCommand
                    {
                        PlayerId = player.OwnerClientId,
                        CardId = player.programmedCards[turn]
                    };
                    
                    Debug.Log($"PlayerID: {cmd.PlayerId}, CardID: {cmd.CardId}");
                    CommandQueue.Enqueue(cmd);
                }
            }
        }

        private void ExecuteNextCommand()
        {
            Debug.Log($"CommandQueue: {CommandQueue.Count}");
            if (CommandQueue.Count == 0)
            {
                SetGameState(GameState.RoundEnd);
                return;
            }

            GameCommand cmd = CommandQueue.Dequeue();
            ExecuteCommand(cmd);
        }

        private void ExecuteCommand(GameCommand cmd)
        {
            Debug.Log($"Command: Player: {cmd.PlayerId}, Card: {cmd.CardId}");

            ExecuteNextCommand();
        }

        private void OnClientDisconnected(ulong clientId)
        {
            if (Players.ContainsKey(clientId))
            {
                Players.Remove(clientId);
                
                Debug.Log($"Player Removed: {clientId}");
            }
        }

        private void Update()
        {
            if (!IsServer) return;

            if (gameState.Value == GameState.WaitingForPlayers)
            {
                if (Players.Count >= 2)
                {
                    StartGame();
                }
            }
        }

        private void StartGame()
        {
            SetGameState(GameState.DealCards);
        }
    }
}
