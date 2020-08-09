using System;
using DataBase.Interfaces;
using Microsoft.Data.SqlClient;
using MyLog.Models;

namespace DataBase.Models.Repositories
{
    public class UserRepositories : IDataUser
    {
        private const string DataSource = Constants.DataSource;
        private const string Catalog = "User";
        private const string IntegratedSecurity = Constants.IntegratedSecurity;

        private const string NameTable = "UserInfo";
        private const string Login = Constants.ColumnLogin;
        private const string Password = Constants.ColumnPassword;

        public void Create(User user)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                OpenDb(connection);
                using (var cmd = new SqlCommand(GetInsertString(), connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue($"@{Login}", user.Login);
                        cmd.Parameters.AddWithValue($"@{Password}", user.Password);

                        Log.Info($"Writing data to the database...");
                        cmd.ExecuteNonQuery();
                        Log.Info($"Data written successfully.");
                    }
                    catch (Exception e)
                    {
                        Log.Error($"An error has occurred. Data not recorded to the database.");
                        Console.WriteLine($"Error: {e}");
                        Console.WriteLine(e.StackTrace);
                    }
                    finally
                    {
                        CloseDb(connection);
                    }
                }
            }
        }

        public void Delete<T>(T login)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                OpenDb(connection);
                using (var cmd = new SqlCommand(GetDeleteString(), connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue($"@{Login}", login);

                        Log.Info($"Data deletion from the database...");
                        cmd.ExecuteNonQuery();
                        Log.Info($"Data deleted successfully.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e}");
                        Console.WriteLine(e.StackTrace);
                        Log.Error($"An error has occurred. Data not deleted from the database.");
                    }
                    finally
                    {
                        CloseDb(connection);
                    }
                }
            }
        }

        public (string T, string T1) GetDataByItem(string inputLog)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand(GetSelectString(inputLog), connection))
                {
                    OpenDb(connection);
                    using(var reader = command.ExecuteReader())
                    {
                        Log.Info($"Reading data...");
                        try
                        {
                            while (reader.Read())
                            {
                                var login = reader.GetString(0);
                                var password = reader.GetString(1);

                                var rezult = (login, password);
                                return rezult;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error: {e}");
                            Console.WriteLine(e.StackTrace);
                        }
                        finally
                        {
                            CloseDb(connection);
                        }
                    }
                }
            }

            return default;
        }

        public void GetAllData()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand(GetSelectString(), connection))
                {
                    OpenDb(connection);
                    using (var reader = command.ExecuteReader())
                    {
                        Log.Info($"Reading data...");
                        try
                        {
                            while (reader.Read())
                            {
                                var login = reader.GetString(0);
                                var password = reader.GetString(1);

                                Console.WriteLine($"{login}\t{password}");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error: {e}");
                            Console.WriteLine(e.StackTrace);
                        }
                        finally
                        {
                            CloseDb(connection);
                        }
                    }
                }
            }
        }

        private static string GetConnectionString()
        {
            return $"Data Source={DataSource};Initial Catalog={Catalog};Integrated Security={IntegratedSecurity}";
        }

        private static string GetInsertString()
        {
            return $"INSERT INTO {NameTable} ({Login}, {Password}) VALUES (@{Login}, @{Password})";
        }

        private static string GetSelectString()
        {
            return $"SELECT * FROM {NameTable}";
        }

        private static string GetSelectString<T>(T inputLog)
        {
            return $"SELECT * FROM {NameTable} WHERE {Login}='{inputLog}'";
        }

        private static string GetDeleteString()
        {
            return $"DELETE {NameTable} WHERE {Login}=@{Login}";
        }

        private static void OpenDb(SqlConnection connection)
        {
            try
            {
                Log.Info($"Connecting to the database.");
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Log.Error($"The was an error! Database connection not possible.");
                throw;
            }
        }

        private static void CloseDb(SqlConnection connection)
        {
            connection.Close();
            Log.Info($"Database closed.");
        }
    }
}
