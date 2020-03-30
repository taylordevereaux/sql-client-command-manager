using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.CommandManager;
using System.IO;
using System.Threading.Tasks;

namespace SqlClient.CommandManager.Tests
{
    public class SqlCommandRunner
    {
        public static async Task RunAsync(Func<SqlCommandManager, Task> action)
        {

            SqlConnection connection = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master");

            string folderPath = Path.Combine(System.AppContext.BaseDirectory, "Data");
            String statement =
                $@"
                IF db_id(N'SqlCommandManager.Tests') IS NULL
                CREATE DATABASE [SqlCommandDatabase.Tests]
                ON PRIMARY
                (
                    NAME = N'SqlCommandDatabase.Tests',
                    FILENAME = '{folderPath}\SqlCommandDatabase.Tests.mdf',
                    SIZE = 2MB, 
                    MAXSIZE = 10MB, 
                    FILEGROWTH = 10 %
                )
                LOG ON
                (
                    NAME = N'SqlCommandDatabase.Tests_log',
                    FILENAME = '{folderPath}\SqlCommandDatabase.Tests.ldf',
                    SIZE = 1MB,
                    MAXSIZE = 5MB,
                    FILEGROWTH = 10 %
                );
                GO";

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