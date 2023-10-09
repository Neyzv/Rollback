using System.Linq.Expressions;

namespace Rollback.Common.DesignPattern.Instance
{
    public abstract class Singleton<T> where T : class
    {
        private static readonly Lazy<T> _instance = new(() => Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile()(),
            LazyThreadSafetyMode.ExecutionAndPublication);

        public static T Instance =>
            _instance.Value;
    }
}
