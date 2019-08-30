using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.SqlClient.CommandManager.Extensions
{
    public static class ExpressionExtensions
    {
        // public static string GetMemberName<T, V>(this Expression<Func<T, V>> expression)
        // {
        //     var member = expression.GetMember();
        //     return member.Name;
        // }
        public static T GetAttribute<T>(this ICustomAttributeProvider provider)
            where T : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof(T), true);
            return attributes.Length > 0 ? attributes[0] as T : null;
        }
        public static MemberExpression GetMemberExpression<T, V>(this Expression<Func<T, V>> expression)
        {
            if (expression.Body == null)
            {
                throw new ArgumentException("Expression must be a member expression");
            }

            if (expression.Body is MemberExpression)
            {
                return (MemberExpression)expression.Body;
            }

            if (expression.Body is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression.Body;
                if (unaryExpression.Operand is MemberExpression)
                {
                    return (MemberExpression)unaryExpression.Operand;
                }
            }
            throw new ArgumentException("Expression must be a member expression");
        }

        public static MemberExpression GetMemberExpression<T, V>(this MemberExpression expression)
        {
            if (expression.Expression == null)
            {
                throw new ArgumentException("Expression must be a member expression");
            }

            if (expression.Expression is MemberExpression)
            {
                return (MemberExpression)expression.Expression;
            }

            if (expression.Expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression.Expression;
                if (unaryExpression.Operand is MemberExpression)
                {
                    return (MemberExpression)unaryExpression.Operand;
                }
            }
            throw new ArgumentException("Expression must be a member expression");
        }

        public static (string Name, DbColumnAttribute Attribute) GetMemberDetails<T, V>(this Expression<Func<T, V>> expression)
        {
            DbColumnAttribute attribute = null;
            MemberInfo member = null;
            string memberName = null;

            if (expression.Body == null)
            {
                throw new ArgumentException("Expression must be a member expression");
            }

            if (expression.Body is MemberExpression)
            {
                var memberExpression = (MemberExpression)expression.Body;
                member = memberExpression.Member;
                memberName = GetMemberExpressionName(memberExpression);
            }

            if (expression.Body is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression = (MethodCallExpression)expression.Body;
                member = methodCallExpression.Method;
            }

            if (expression.Body is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression.Body;
                if (unaryExpression.Operand is MethodCallExpression)
                {
                    var methodExpression = (MethodCallExpression)unaryExpression.Operand;
                    member = methodExpression.Method;
                }
                else if (unaryExpression.Operand is MemberExpression)
                {
                    var memberExpression = (MemberExpression)unaryExpression.Operand;

                    member = memberExpression.Member;
                    memberName = GetMemberExpressionName(memberExpression);
                }
            }

            if (member != null)
            {
                attribute = member.GetAttribute<DbColumnAttribute>();

                if (string.IsNullOrWhiteSpace(memberName))
                {
                    memberName = member.Name;
                }

                return (memberName, attribute);
            }

            throw new ArgumentException("Expression must be a member expression");
        }

        private static string GetMemberExpressionName(MemberExpression memberExpression)
        {
            string name = "";
            if (memberExpression.Expression is MemberExpression)
            {
                name = GetMemberExpressionName((memberExpression.Expression as MemberExpression));
            }
            return !string.IsNullOrWhiteSpace(name) ? $"{name}.{memberExpression.Member.Name}" : memberExpression.Member.Name;
        }
    }
}