namespace Rollback.Common.Extensions
{
    public static class TaskExtensions
    {
        public static async void FireAndForget(this Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
        }

        public static async void FireAndForget(this ValueTask task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
        }
    }
}
