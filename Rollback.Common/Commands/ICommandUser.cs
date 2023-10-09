namespace Rollback.Common.Commands
{
    public interface ICommandUser
    {
        byte Role { get; }

        public virtual string ToBold(string text) =>
            text;

        void Reply(string message);

        void ReplyError(string message);
    }
}
