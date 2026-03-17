using Player;
using UnityEngine;

namespace Data
{
    public abstract class CardEffect : ScriptableObject
    {
        public abstract void Execute(NetworkPlayer player);
    }
}