using System;
using System.Collections.Generic;
using CheckList.Models;
using CRUD.Models;
using DataBase.Models;
using DataBase.Models.Repositories;
using E_mail;
using MyLog.Models;
using MyPdf.Models;

namespace CheckList
{
    class Program
    {
        static void Main()
        {
            Log.Info($"Start program.");
            ConsoleKeyInfo globalKey = default;

            do
            {
                Console.WriteLine($"1 - Admin;\n2 - User:");
                switch (SwitchMenu())
                {
                    //Admin
                    case 1:
                    {
                        var admin = new Admin();
                        Console.WriteLine($"Enter password:");
                        admin.CheckPassword();

                        ConsoleKeyInfo key;
                        do
                        {
                            Console.WriteLine($"1. Add task;\n2. Add new person;");
                            Console.WriteLine($"Choose an action:");
                            switch (SwitchMenu())
                            {
                                case 1: { SwitchToTask(); break; }
                                case 2: { SwitchToPerson(); break; }
                            }

                            Console.WriteLine($"Do you want to take another action (Y/N)?");
                            key = Console.ReadKey();
                            Console.WriteLine();
                        } while (key.Key != ConsoleKey.N);

                        Console.WriteLine($"Do you want to sweep a user (Y/N)?");
                        globalKey = Console.ReadKey();
                        Console.WriteLine();

                        break;
                    }
                
                    //User
                    case 2:
                    {
                        var login = EnterLoginAndPassForUser();
                        Console.Write($"\nEnter product id: ");
                        var inputId = Console.ReadLine();
                        var databaseProduct = new ProductRepositories();
                        (string id, string color, int numberOfSeats, int task) dataProduct = databaseProduct.GetDataById(inputId);
                    
                        Tuple<ushort, ushort, ushort, float>[] characteristics = SetPackageCharacteristics(dataProduct.numberOfSeats);

                        Pdf.Create(dataProduct, characteristics, login);
                        Mail.SendEmailAsync(dataProduct);

                        Console.WriteLine($"\nDo you want to sweep a user (Y/N)?");
                        globalKey = Console.ReadKey();
                        Console.WriteLine();

                        break;
                    }
                }
            } while (globalKey.Key != ConsoleKey.N);

            Log.Info($"Finish program\n");
        }

        #region Admin
        static void SwitchToTask()
        {
            var taskCrud = new TaskCrud();
            ConsoleKeyInfo key;
            do
            {
                Menu(Constants.Task);
                switch (SwitchMenu())
                {
                    case 1: { taskCrud.Create(); break; }
                    case 2: { taskCrud.Delete(); break; }
                    case 3: { taskCrud.GetDataByTask(); break; }
                    case 4: { taskCrud.GetAllData(); break; }
                }

                Console.WriteLine($"Do you want to take another action (Y/N)?");
                key = Console.ReadKey();
                Console.WriteLine();
            } while (key.Key != ConsoleKey.N);
        }

        static void SwitchToPerson()
        {
            var userCrud = new UserCrud();
            ConsoleKeyInfo key;
            do
            {
                Menu(Constants.User);
                switch (SwitchMenu())
                {
                    case 1: { userCrud.Create(); break; }
                    case 2: { userCrud.Delete(); break; }
                    case 3: { userCrud.GetDataByTask(); break; }
                    case 4: { userCrud.GetAllData(); break; }
                }

                Console.WriteLine($"Do you want to take another action (Y/N)?");
                key = Console.ReadKey();
                Console.WriteLine();
            } while (key.Key != ConsoleKey.N);
        }

        static void Menu(string item)
        {
            Console.WriteLine($"1 - Great {item};\n2 - Delete {item};\n3 - Show {item} data by value;\n4 - Show all data;");
            Console.WriteLine($"Choose an action:");
        }

        static int SwitchMenu()
        {
            Log.Info($"Select user...");
            int value;
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write($"Error, try again: ");
                Log.Error($"Error while entering value");
            }

            return value;
        }

        #endregion

        static string EnterLoginAndPassForUser()
        {
            Log.Info($"Input login and password for the user.");
            var user = new User();
            string login;
            string password;
            do
            {
                Console.Write($"Enter login: ");
                login = Console.ReadLine();
                Console.Write($"Enter password: ");
                password = user.EnterPassword();

                if (user.CheckLogin(login, password))
                {
                    Console.WriteLine($"Login and password entered successfully!");
                    Log.Info($"Login and password entered successfully.");
                    break;
                }
                else
                {
                    Console.WriteLine($"Try again. Login or password does not exist.");
                    Log.Error($"Login or password entry error.");
                }
            } while (!user.CheckLogin(login, password));

            return login;
        }

        static Tuple<ushort, ushort, ushort, float>[] SetPackageCharacteristics(int number)
        {
            var list = new List<Tuple<ushort, ushort, ushort, float>>();
            for (int i = 0; i < number; i++)
            {
                Console.WriteLine($"\nEnter data {i + 1} package:");
                var package = new Package();
                var tuple = Tuple.Create(package.Length, package.Width, package.Height, package.Weight);
                list.Add(tuple);
            }

            return list.ToArray();
        }
    }
}
