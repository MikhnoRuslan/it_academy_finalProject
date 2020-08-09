using System;
using System.Collections.Generic;
using System.Security;
using DataBase.Interfaces;
using Microsoft.Data.SqlClient;
using MyLog.Models;

namespace DataBase.Models.Repositories
{
    public class ProductRepositories : IDataProduct
    {
        private const string NameTable = "ProductInformation";
        private const string Id = Constants.ColumnId;
        private const string Color = Constants.ColumnColor;
        private const string NumberOfSeats = Constants.ColumnNumOfSeats;
        private const string Task = Constants.ColumnTask;

        private const string DataSource = Constants.DataSource;
        private const string Catalog = "Checklist";
        private const string IntegratedSecurity = Constants.IntegratedSecurity;

        public void Create(ProductData product)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                OpenDb(connection);
                using (var cmd = new SqlCommand(GetInsertString(), connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue($"@{Id}", product.Id);
                        cmd.Parameters.AddWithValue($"@{Color}", product.Color);
                        cmd.Parameters.AddWithValue($"@{NumberOfSeats}", product.NumberOfSeats);
                        cmd.Parameters.AddWithValue($"@{Task}", product.Task);

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

        public void Delete<T>(T id)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                OpenDb(connection);
                using (var cmd = new SqlCommand(GetDeleteString(), connection))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue($"@{Id}", id);

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

        public Tuple<string, string, int, int>[] GetDataByTask(int inputTask)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand(GetSelectString(inputTask), connection))
                {
                    OpenDb(connection);
                    using (var reader = command.ExecuteReader())
                    {
                        Log.Info($"Reading data...");
                        var list = new List<Tuple<string, string, int, int>>();
                        try
                        {
                            while (reader.Read())
                            {
                                var id = reader.GetString(0);
                                var color = reader.GetString(1);
                                var numberOfSeats = reader.GetInt32(2);
                                var task = reader.GetInt32(3);

                                var tuple = Tuple.Create(id, color, numberOfSeats, task);
                                list.Add(tuple);
                            }
                            
                            return list.ToArray();
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

        public (string, string, int, int) GetDataById(string inputId)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                using (var command = new SqlCommand(GetSelectString(inputId), connection))
                {
                    OpenDb(connection);
                    using (var reader = command.ExecuteReader())
                    {
                        Log.Info($"Reading data...");
                        try
                        {
                            while (reader.Read())
                            {
                                var id = reader.GetString(0);
                                var color = reader.GetString(1);
                                var numberOfSeats = reader.GetInt32(2);
                                var task = reader.GetInt32(3);

                                var tuple = (id, color, numberOfSeats, task);

                                return tuple;
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
                                var id = reader.GetString(0);
                                var color = reader.GetString(1);
                                var numberOfSeats = reader.GetInt32(2);
                                var task = reader.GetInt32(3);

                                Console.WriteLine($"{id}\t{color}\t{numberOfSeats}\t{task}");
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
            return $@"Data Source={DataSource};Initial Catalog={Catalog};Integrated Security={IntegratedSecurity}";
        }

        private static string GetInsertString()
        {
            return
                $@"INSERT INTO {NameTable} ({Id}, {Color}, {NumberOfSeats}, {Task}) VALUES (@{Id}, @{Color}, @{NumberOfSeats}, @{Task})";
        }

        private static string GetSelectString()
        {
            return $@"SELECT * FROM {NameTable}";
        }

        private static string GetSelectString(int inputTask)
        {
            return $@"SELECT * FROM {NameTable} WHERE {Id} IN (SELECT {Id} FROM {NameTable} WHERE {Task}={inputTask})";
        }

        private static string GetSelectString(string id)
        {
            return $@"SELECT * FROM {NameTable} WHERE {Id} IN (SELECT {Id} FROM {NameTable} WHERE {Id}='{id}')";
        }

        private static string GetDeleteString()
        {
            return $"DELETE {NameTable} WHERE {Id}=@{Id}";
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
