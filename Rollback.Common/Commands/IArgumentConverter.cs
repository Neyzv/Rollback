namespace Rollback.Common.Commands
{
    public interface IArgumentConverter<T>
    {
        public T? Convert(object value);
    }
}
