using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SqlClient.CommandManager
{
    public class SqlCommandManager
    {
        private string _connectionString;
        public string ConnectionString
        {
            get { return _connectionString; }
            protected set { _connectionString = value; }
        }

        public SqlCommandManager(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public virtual int ExecuteNonQuery(string commandString, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    SqlParameter returnParam = SetupExecuteNonQueryCommand(parameters, command);

                    command.ExecuteNonQuery();

                    return (int)returnParam.Value;
                }
            }
        }

        public virtual async Task<int> ExecuteNonQueryAsync(string commandString, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    SqlParameter returnParam = SetupExecuteNonQueryCommand(parameters, command);

                    await command.ExecuteNonQueryAsync();

                    return (int)returnParam.Value;
                }
            }
        }

        /// <summary>
        /// Executes a Data Reader for the parameters provided.
        /// </summary>
        public virtual T ExecuteReader<T>(string commandString, SqlParameter[] parameters, Func<SqlDataReader, T> reader)
        {
            T result;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    SetupExecuteReaderCommand(parameters, command);

                    using (SqlDataReader dataReader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        result = reader(dataReader);
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Executes a Data Reader for the parameters provided.
        /// </summary>
        public virtual async Task ExecuteReaderAsync(string commandString, SqlParameter[] parameters, Action<SqlDataReader> reader)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    SetupExecuteReaderCommand(parameters, command);

                    await connection.OpenAsync();
                    using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        reader(dataReader);
                    }
                }
            }
        }
        /// <summary>
        /// Sets up the default fields for the Execute Reader Command.
        /// </summary>
        protected virtual void SetupExecuteReaderCommand(SqlParameter[] parameters, SqlCommand command)
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddRange(parameters.Where(x => x != null).ToArray());
        }
        /// <summary>
        /// Sets up the default fields for the Execute Non Query Command.
        /// </summary>
        protected virtual SqlParameter SetupExecuteNonQueryCommand(SqlParameter[] parameters, SqlCommand command)
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter returnParam = new SqlParameter();
            returnParam.SqlDbType = System.Data.SqlDbType.Int;
            returnParam.ParameterName = "@ReturnValue";
            returnParam.Direction = System.Data.ParameterDirection.ReturnValue;

            command.Parameters.Add(returnParam);

            command.Parameters.AddRange(parameters.Where(x => x != null).ToArray());

            return returnParam;
        }
    }
}