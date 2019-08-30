
namespace System.Data.SqlClient.CommandManager
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class DbColumnAttribute : Attribute
    {
        public DbColumnAttribute(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}