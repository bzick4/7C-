using System;
using System;
using System.Linq;
using System.Text;
using System.Threading;



namespace _7newprog
{

    class Program
    {
        public static void Main(string[] args)
        {
            string path = @"NotePage.csv";
            NewFile(path);
            Repository re = new Repository(path);
            re.Menu();
            Console.WriteLine(re.Count);
            re.Delay();
        }
        public static void NewFile(string path)
        {
            if (File.Exists(path))

            {
                Console.WriteLine("Файл NotePage.csv найден");
            }
            else
            {
                File.Create(path).Close();
                string[] notepage = new string[] { };
                File.AppendAllLines(path, notepage);
                Console.WriteLine("Файл NotePage.csv Создан");
            }
        }
    } 

}


