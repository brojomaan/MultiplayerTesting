using System.Collections.Generic;
using Database;
using Player;
using Structs;
using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance { get; private set; }

        private Queue<GameCommand> commandQueue = new Queue<GameCommand>();
        private Dictionary<ulong, NetworkPlayer> players = new Dictionary<ulong, NetworkPlayer>();
        
        private int currentTurn = 0;
        private int maxTurns = 3;

        private CardDatabase cardDatabase;

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

        private void Start()
        {
            cardDatabase = CardDatabase.Instance;
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

            if (!players.ContainsKey(id))
            {
                players.Add(id, player);
                
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
                    Debug.Log($"players can now submit cards");
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
            
            foreach (var player in players)
            {
                if (player.Value.programmedCards.Count <= currentTurn)
                {
                    return;
                }
            }
            
            Debug.Log($"All players Submitted cards");

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
            foreach (NetworkPlayer player in players.Values)
            {
                player.DrawHand();
            }

            SetGameState(GameState.Programming);
        }

        private void BuildCommandQueue()
        {
            commandQueue.Clear();
            
            for (int turn = 0; turn < maxTurns; turn++)
            {
                foreach (var player in players.Values)
                {
                    GameCommand cmd = new GameCommand
                    {
                        PlayerId = player.OwnerClientId,
                        CardId = player.programmedCards[turn]
                    };
                    
                    Debug.Log($"PlayerID: {cmd.PlayerId}, CardID: {cmd.CardId}");
                    commandQueue.Enqueue(cmd);
                }
            }
        }

        private void ExecuteNextCommand()
        {
            Debug.Log($"CommandQueue: {commandQueue.Count}");
            if (commandQueue.Count == 0)
            {
                Debug.Log($"All Commands Executed");
                SetGameState(GameState.RoundEnd);
                return;
            }

            GameCommand cmd = commandQueue.Dequeue();
            ExecuteCommand(cmd);

            ExecuteNextCommand();
        }

        private void ExecuteCommand(GameCommand cmd)
        {
            NetworkPlayer player = players[cmd.PlayerId];

            CardData card = cardDatabase.GetCard(cmd.CardId);
            
            card.effect.Execute(player);

        }

        private void OnClientDisconnected(ulong clientId)
        {
            if (players.ContainsKey(clientId))
            {
                players.Remove(clientId);
                
                Debug.Log($"Player Removed: {clientId}");
            }
        }

        private void Update()
        {
            if (!IsServer) return;

            if (gameState.Value == GameState.WaitingForPlayers)
            {
                if (players.Count >= 2)
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
