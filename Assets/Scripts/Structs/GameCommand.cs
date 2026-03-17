namespace Structs
{
    public struct GameCommand
    {
        public ulong PlayerId;
        public int CardId;
        
        public GameCommand(ulong playerId, int cardId)
        {
            PlayerId = playerId;
            CardId = cardId;
        }
    }
}