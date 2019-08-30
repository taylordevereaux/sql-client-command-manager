
namespace System.Data.SqlClient.CommandManager
{
    public class QueryColumn
    {
        public QueryColumn(string propertyName, string columnName)
        {
            this.PropertyName = propertyName;
            this.ColumnName = columnName;

        }
        public string PropertyName { get; set; }
        public string ColumnName { get; set; }
    }
}