using System;
using DataBase.Interfaces;
using DataBase.Models.Repositories;
using MyLog.Models;

namespace DataBase.Models
{
    public class User : IPassword
    {
        private const int SizeOfArray = 8;
        private const int SizeOfArrayUpperChar = 4;
        public string Login { get; set; }
        public string Password { get; }

        public User() {}

        public User(string login)
        {
            Login = login;
            Password = GetPassword();
        }

        public bool CheckLogin(string user, string inputPassword)
        {
            var databaseUser = new UserRepositories();
            var (login, password) = databaseUser.GetDataByItem(user);
            if (user.Equals(login) && inputPassword.Equals(password))
                return true;
            else
                return false;
        }

        private string GetPassword()
        {
            Log.Info($"Create a password.");
            var random = new Random();
            var arrayChars = new char[SizeOfArray];
            var pass = string.Empty;

            var indexToUpper = new int[SizeOfArrayUpperChar];

            for (int i = 0; i < indexToUpper.Length; i++)
            {
                indexToUpper[i] = random.Next(SizeOfArray);
            }

            for (int i = 0; i < arrayChars.Length; i++)
            {
                arrayChars[i] = Convert.ToChar(random.Next(97, 122));
                foreach (var j in indexToUpper)
                {
                    if (i == j)
                        arrayChars[i] = char.ToUpper(Convert.ToChar(random.Next(97, 122)));
                }
                
                pass += arrayChars[i];
            }

            return pass;
        }

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
                Console.WriteLine(Constants.WrongPassword);
                Log.Error($"Error while entering value");
            }

            return true;
        }
    }
}
