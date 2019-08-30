namespace System.Data.SqlClient.CommandManager
{
    public class ExpressionMap
    {
        private string[] _names;
        private string[] _columnNames;
        public ExpressionMap(int length)
        {
            this._names = new string[length];
            this._columnNames = new string[length];
        }
        public string[] Names { get => this._names; }
        public string[] ColumnNames { get => this._columnNames; }
        public int Length => this._names.Length;
    }
}