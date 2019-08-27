using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.CommandManager;
using System.Threading.Tasks;

namespace SqlClient.CommandManager.Tests
{
    public class SqlCommandRunner
    {
        public static async Task RunAsync(Func<SqlCommandManager, Task> action)
        {

            SqlConnection connection = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master");

            string folderPath = System.AppContext.BaseDirectory;
            String statement =
                $@"CREATE DATABASE 
                    SqlCommandDatabase 
                ON PRIMARY
                (
                    NAME = SqlCommandDatabase,
                    FILENAME = '{folderPath}\SqlCommandDatabaseData.mdf',
                    SIZE = 2MB, 
                    MAXSIZE = 10MB, 
                    FILEGROWTH = 10 %
                )
                LOG ON
                (
                    NAME = SqlCommandDatabase_Log,
                    FILENAME = '{folderPath}\SqlCommandDatabase_Log.ldf',
                    SIZE = 1MB,
                    MAXSIZE = 5MB,
                    FILEGROWTH = 10 %
                )";

            SqlCommand command = new SqlCommand(statement, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();

                await action(new SqlCommandManager("Server=localhost;Integrated security=SSPI;database=SqlCommandDatabase"));
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}