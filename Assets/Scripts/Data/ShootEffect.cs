using Player;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ShootEffect", menuName = "Cards/Effects/Shoot", order = 0)]
    public class ShootEffect : CardEffect
    {
        public override void Execute(NetworkPlayer player)
        {
            Debug.Log($"player {player.OwnerClientId} Shot there gun");
        }
    }
}