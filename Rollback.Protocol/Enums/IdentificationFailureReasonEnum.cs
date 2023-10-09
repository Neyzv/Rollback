namespace Rollback.Protocol.Enums
{
    public enum IdentificationFailureReasonEnum
    {
        BAD_VERSION = 1,
        WRONG_CREDENTIALS = 2,
        BANNED = 3,
        KICKED = 4,
        IN_MAINTENANCE = 5,
        UNKNOWN_AUTH_ERROR = 99
    }
}
