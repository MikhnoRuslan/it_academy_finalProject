using System;
using CRUD.Interfaces;
using DataBase.Models;
using DataBase.Models.Repositories;
using MyLog.Models;

namespace CRUD.Models
{
    public class UserCrud : ICrud
    {
        private static readonly UserRepositories DatabaseUser = new UserRepositories();

        public void Create()
        {
            ConsoleKeyInfo key;
            do
            {
                var user = new User(EnterLogin());
                Log.Info($"Creation data.");
                DatabaseUser.Create(user);
                Console.WriteLine($"Do you want to create more (Y/N)?");
                key = Console.ReadKey();
                Console.WriteLine();
            } while (key.Key != ConsoleKey.N);
        }

        public void Delete()
        {
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine($"Enter user to delete");
                var user = Console.ReadLine();
                Log.Info($"Data deletion.");
                DatabaseUser.Delete(user);
                Console.WriteLine($"Do you want to delete more (Y/N)?");
                key = Console.ReadKey();
                Console.WriteLine();
            } while (key.Key != ConsoleKey.N);
        }

        public void GetDataByTask()
        {
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine($"Enter user:");
                var user = Convert.ToString(Console.ReadLine());
                var (login, password) = DatabaseUser.GetDataByItem(user);
                Console.WriteLine($"{login}\t{password}");
                
                Console.WriteLine($"\nDo you want to see more (Y/N)?");
                key = Console.ReadKey();
                Console.WriteLine();
            } while (key.Key != ConsoleKey.N);
        }

        public void GetAllData()
        {
            DatabaseUser.GetAllData();
        }

        private string EnterLogin()
        {
            Console.WriteLine($"Enter login:");
            return Console.ReadLine();
        }
    }
}
