using Player;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PickUpEffect", menuName = "Cards/Effects/PickUp", order = 0)]
    public class PickUpEffect : CardEffect
    {
        public override void Execute(NetworkPlayer player)
        {
            Debug.Log($"Player {player.OwnerClientId} Picked Up Something");
        }
    }
}