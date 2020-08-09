using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyLog.Models
{
    public static class Log
    {
        private const int SizeFile = 30_000;
        private static int _count = 0;
        private static readonly string Path = @$"F:\Уроки C#\MikhnoRuslan\it_academy_finalProject\CheckList\MyLog\Log";

        public static void Info(string message,
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                DateTime now = DateTime.Now;
                var m = (new StackTrace()).GetFrame(1)?.GetMethod();
                using (var file = new FileStream(@$"{Path}\log {now:d}_[{_count}].txt", FileMode.Append))
                {
                    if (file.Length <= SizeFile)
                    {
                        WriteData(file, message, m, sourceLineNumber);
                    }
                    else
                    {
                        _count++;
                        using (new FileStream(@$"{Path}\log {now:d}_[{_count}].txt", FileMode.Append))
                        {
                            WriteData(file, message, m, sourceLineNumber);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void Error(string message,
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                DateTime now = DateTime.Now;
                var m = (new StackTrace()).GetFrame(1)?.GetMethod();
                using (var file = new FileStream(@$"{Path}\log {now:d}_[{_count}].txt", FileMode.Append))
                {
                    if (file.Length <= SizeFile)
                    {
                        WriteData(file, message, m, sourceLineNumber);
                    }
                    else
                    {
                        _count++;
                        using (new FileStream(@$"{Path}\log {now:d}_[{_count}].txt", FileMode.Append))
                        {
                            WriteData(file, message, m, sourceLineNumber);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void WriteData(FileStream file, string message, MethodBase method, 
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            DateTime now = DateTime.Now;

            var array1 = Encoding.Default.GetBytes($"{now:G} ");
            var array2 = Encoding.Default.GetBytes($"|{memberName}| ");
            var array3 = Encoding.Default.GetBytes($"|{method?.DeclaringType?.FullName}.{method?.Name}| ");
            var array4 = Encoding.Default.GetBytes($"|{sourceLineNumber}| ");
            var array5 = Encoding.Default.GetBytes($"{message}\n");

            file.Write(array1, 0, array1.Length);
            file.Write(array2, 0, array2.Length);
            file.Write(array3, 0, array3.Length);
            file.Write(array4, 0, array4.Length);
            file.Write(array5, 0, array5.Length);
        }
    }
}
