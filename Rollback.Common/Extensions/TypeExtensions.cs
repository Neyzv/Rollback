using System.Reflection;

namespace Rollback.Common.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Return true if <paramref name="type"/> is a direct child of the generic type <paramref name="genericType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType">A generic Type.</param>
        /// <returns></returns>
        public static bool IsSubclassOfGenericType(this Type type, Type genericType) =>
            type != genericType && type != typeof(object) && genericType.IsGenericType && type.BaseType is not null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == genericType;

        /// <summary>
        /// Return true if <paramref name="type"/> is implementing the generic interface <paramref name="genericInterfaceType"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericInterfaceType"></param>
        /// <returns></returns>
        public static bool IsImplementingGenericInterface(this Type type, Type genericInterfaceType) =>
            type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType);

        /// <summary>
        /// Return the <see cref="PropertyInfo"/> from it's heritage, with name <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo? GetPropertyFromDerivedClass(this Type type, string propertyName)
        {
            var propertyInfo = default(PropertyInfo?);

            if (type is not null && type != typeof(object))
            {
                var property = type.GetProperty(propertyName);
                if (property is not null)
                    propertyInfo = property;
                else if (type.BaseType is not null)
                    propertyInfo = GetPropertyFromDerivedClass(type.BaseType, propertyName);
            }

            return propertyInfo;
        }

        /// <summary>
        /// Return the <see cref="MethodInfo"/> from it's heritage, with name <paramref name="methodName"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static MethodInfo? GetMethodFromDerivedClass(this Type type, string methodName)
        {
            var methodInfo = default(MethodInfo?);

            if (type is not null && type != typeof(object))
            {
                var property = type.GetMethod(methodName);
                if (property is not null)
                    methodInfo = property;
                else if (type.BaseType is not null)
                    methodInfo = GetMethodFromDerivedClass(type.BaseType, methodName);
            }

            return methodInfo;
        }

        /// <summary>
        /// Convert <paramref name="value"/> to <typeparamref name="T"/> type, including nullable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? ChangeType<T>(this object? value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                t = Nullable.GetUnderlyingType(t);

            if (t is null || value == null || value.ToString() == string.Empty)
                return default;

            return (T)Convert.ChangeType(value, t);
        }
    }
}
