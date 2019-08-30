using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.SqlClient.CommandManager.Extensions
{
    public static class ExpressionExtensions
    {
        public static string GetMemberName<T, V>(this Expression<Func<T, V>> expression)
        {
            var member = expression.GetMember();
            return member.Name;
        }
        public static T GetAttribute<T>(this ICustomAttributeProvider provider)
            where T : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof(T), true);
            return attributes.Length > 0 ? attributes[0] as T : null;
        }

        public static MemberInfo GetMember<T, V>(this Expression<Func<T, V>> expression)
        {

            if (expression.Body == null)
            {
                throw new ArgumentException("Expression must be a member expression");
            }

            if (expression.Body is MemberExpression)
            {
                var memberExpression = (MemberExpression)expression.Body;
                return memberExpression.Member;
            }

            if (expression.Body is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression = (MethodCallExpression)expression.Body;
                return methodCallExpression.Method;
            }

            if (expression.Body is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression.Body;
                if (unaryExpression.Operand is MethodCallExpression)
                {
                    var methodExpression = (MethodCallExpression)unaryExpression.Operand;
                    return methodExpression.Method;
                }
                else if (unaryExpression.Operand is MemberExpression)
                {
                    var memberExpression = (MemberExpression)unaryExpression.Operand;
                    return memberExpression.Member;
                }
            }

            throw new ArgumentException("Expression must be a member expression");
        }
    }
}