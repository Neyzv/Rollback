namespace Rollback.Protocol.Enums
{
    public enum CharacterCreationResultEnum
    {
        OK = 0,
        ERR_NO_REASON = 1,
        ERR_INVALID_NAME = 2,
        ERR_NAME_ALREADY_EXISTS = 3,
        ERR_TOO_MANY_CHARACTERS = 4,
        ERR_NOT_SUBSCRIBER = 5,
        ERR_NEW_PLAYER_NOT_ALLOWED = 6
    }
}
