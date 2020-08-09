using System;
using DataBase.Interfaces;
using MyLog.Models;

namespace CheckList.Models
{
    class Admin : IPassword
    {
        public string Password { get; } = Constants.Password.AdminPassword;
        public string EnterPassword()
        {
            var password = string.Empty;
            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                    break;

                Console.Write("*");
                password += key.KeyChar;
            }

            Console.WriteLine();

            return password;
        }

        public bool CheckPassword()
        {
            while (!EnterPassword().Equals(Password))
            {
                Console.WriteLine(Constants.Password.WrongPassword);
                Log.Error($"Error while entering value");
            }

            return true;
        }
    }
}
