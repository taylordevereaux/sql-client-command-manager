using System.Collections.Generic;
using System.Data.SqlClient.CommandManager.Extensions;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.SqlClient.CommandManager
{
    public class ExpressionMap<T>
    {
        private string[] _names;
        private string[] _columnNames;
        public string[] Names { get => this._names; }
        public string[] ColumnNames { get => this._columnNames; }
        public int Length => this._names.Length;
        private string _name;
        public string Name => this._name;
        public List<ExpressionMap<T>> Includes { get; set; }

        public ExpressionMap(params Expression<Func<T, object>>[] expressions)
        {
            this.Includes = new List<ExpressionMap<T>>();
            this.Build(GetMemberExpressions(expressions));
        }
        protected ExpressionMap(string name, MemberExpression[] expressions)
        {
            this.Includes = new List<ExpressionMap<T>>();
            this._name = name;
            this.BuildFromInclude(expressions);
        }

        private MemberExpression[] GetMemberExpressions(params Expression<Func<T, object>>[] expressions)
        {
            return expressions.Select(x => x.GetMemberExpression()).ToArray();
        }
        private void Build(MemberExpression[] expressions)
        {
            var memberExpressions = expressions.Where(x => !(x.Expression is MemberExpression)).ToArray();
            var childExpressions = expressions
                .Where(x => (x.Expression is MemberExpression))
                .GroupBy(x => (x.Expression as MemberExpression).Member.Name)
                .ToDictionary(x => x.Key, x => x.ToArray());

            ParseMemberExpressions(memberExpressions);

            if (childExpressions.Count > 0)
            {
                foreach (var item in childExpressions)
                {
                    this.Includes.Add(new ExpressionMap<T>(item.Key, item.Value));
                }
            }
        }

        private void BuildFromInclude(MemberExpression[] expressions)
        {
            ParseMemberExpressions(expressions);

            var childExpressions = expressions
                .Select(x => x.Expression as MemberExpression)
                .Where(x => (x.Expression is MemberExpression))
                .GroupBy(x => (x.Expression as MemberExpression).Member.Name)
                .ToDictionary(x => x.Key, x => x.ToArray());

            if (childExpressions.Count > 0)
            {
                foreach (var item in childExpressions)
                {
                    this.Includes.Add(new ExpressionMap<T>(item.Key, item.Value));
                }
            }
        }
        private void ParseMemberExpressions(MemberExpression[] memberExpressions)
        {
            this._names = new string[memberExpressions.Length];
            this._columnNames = new string[memberExpressions.Length];
            int length = memberExpressions.Length;
            for (int i = 0; i < length; ++i)
            {
                var memberExpression = memberExpressions[i];
                var member = memberExpression.Member;
                var attribute = member.GetAttribute<DbColumnAttribute>();
                this.Names[i] = member.Name;

                if (attribute != null)
                {
                    this.ColumnNames[i] = attribute.Name;
                }
                else
                {
                    this.ColumnNames[i] = member.Name;
                }
            }
        }
    }
}