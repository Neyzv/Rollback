using Rollback.Common.Extensions;

namespace Rollback.Common.DesignPattern.Instance
{
    public abstract class ParameterContainer
    {
        protected readonly string[] _parameters;

        public ParameterContainer(string[] parameters) =>
            _parameters = parameters;

        protected T? GetParameterValue<T>(ushort index)
        {
            var result = default(T?);
            var param = _parameters.ElementAtOrDefault(index);

            if (param is not null)
                result = param.ChangeType<T>();

            return result;
        }
    }
}
