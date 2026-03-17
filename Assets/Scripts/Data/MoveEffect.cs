using Player;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "MoveEffect", menuName = "Cards/Effects/Move", order = 0)]
    public class MoveEffect : CardEffect
    {
        public override void Execute(NetworkPlayer player)
        {
            Debug.Log($"Player {player.OwnerClientId} Moves");
        }
    }
}