using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using _7newprog;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;

namespace _7newprog
{
    public class Repository
    {

        public Worker[] workers;
        public string path;
        int index;
        string[] titles;
        Worker worker = new Worker();
        public Repository(string Path)
        {
            this.path = Path;
            this.index = 0;  //workers.Length;
            this.titles = new string[7];
            this.workers = GetAllWorkers(Path); //
        }
        private void Resize(bool Flag)
        {
            if (Flag)
            {
                Array.Resize(ref this.workers, this.workers.Length + 1);
            }
        }
        public void Add(Worker ConcreteWorker)
        {
            this.Resize(index >= this.workers.Length);
            this.workers[index] = ConcreteWorker;
            this.index++;
            //SaveData();
        }
        public int Count { get { return this.index; } }

        private void SaveData()
        {
            using (StreamWriter streamWriter = new StreamWriter("NotePage.csv", true, Encoding.UTF8))
            {
                streamWriter.WriteLine("ID, Время создания записи,  ФИО, Возраст, Рост, Дата рождения, месторождения ");
                foreach (Worker worker in workers)
                {
                    string line = $"{worker.id} {worker.creationTime} {worker.fullName} {worker.age} {worker.height} {worker.birthday} {worker.country}";
                    streamWriter.WriteLine(line);
                }
            }

        }

        /// <summary>
        /// Метод МЕНЮ
        /// </summary>
        public void Menu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n>>>ГЛАВНОЕ МЕНЮ<<<\n");
                Console.WriteLine("1 - Просмотр всех записей в терминале");
                Console.WriteLine("2 - Просмотр записи по ID");
                Console.WriteLine("3 - Создание записи");
                Console.WriteLine("4 - Удаление записи");
                Console.WriteLine("5 - Загрузка записей в диапазоне дат");
                Console.WriteLine("0 - Выход");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        PrintAllWorkers();
                        break;
                    case "2": //просмотр записи по id 
                        GetWorkerById();
                        break;
                    case "3": //создание записи
                        AddWorker();
                        break;
                    case "4": //удаление записи 
                        DeleteWorker();
                        break;
                    case "5": //загрузка записи в диапазоне дат
                        PrintAllWorkersOrderedByDate();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.Write("Такого меню нет, повторите попытку!\n");
                        break;
                }
            }
        }

        /// <summary>
        /// Создание новой записи
        /// </summary>
        /// <param name="worker"></param>
        public void AddWorker()
        {

            using (StreamWriter streamWriter = new StreamWriter(path, true, Encoding.UTF8))
            {
                #region ID+
                Console.Write("Введите ID : ");
                int newId = 0;
                while (true)
                {
                    string id = Console.ReadLine();
                    newId = GetParseInt(id);
                    if (newId > 0)
                    {
                        break;
                    }
                    Console.WriteLine("Повторите попытку");
                    Console.Write("Введите ID: ");
                    id = Console.ReadLine();
                }
                #endregion
                #region time
                DateTime creationTime;
                creationTime = DateTime.UtcNow;
                Console.WriteLine($"Дата и время добавления записи: {creationTime} ");

                #endregion
                #region FullName+
                Console.Write("Введите ФИО сотрудника: ");
                string newName = Console.ReadLine();

                #endregion
                #region Age+
                Console.Write("Введите возраст: ");
                int newAge = 0;
                while (true)
                {
                    string age = Console.ReadLine();
                    newAge = GetParseInt(age);
                    if (newAge > 0)
                    {
                        break;
                    }

                    Console.WriteLine("Повторите попытку");
                    Console.Write("Введите возраст: ");
                    age = Console.ReadLine();

                }
                #endregion
                #region Height+
                Console.Write("Введите рост: ");
                int newHeight = 0;
                while (true)
                {
                    string height = Console.ReadLine();
                    newHeight = GetParseInt(height);
                    if (newHeight > 0)
                    {
                        break;
                    }
                    Console.WriteLine("Повторите попытку");
                    Console.Write("Введите рост: ");
                    height = Console.ReadLine();
                }
                #endregion
                #region Birthday +
                Console.Write("Введите дату рождения ММ/ДД/ГГГГ: ");
                DateTime newDate = DateTime.MinValue;
                while (true)
                {
                    string birthday = Console.ReadLine();
                    newDate = GetParseDate(birthday);
                    if (newDate != DateTime.MinValue)
                    {
                        break;
                    }
                    Console.WriteLine("Ошибка в формате даты. Повторите попытку :");
                }
                #endregion
                #region Country +
                Console.Write("Введите место рождения: ");
                string newCountry = Console.ReadLine();

                #endregion

                Worker newWorker = new Worker(newId, creationTime, newName, newAge, newHeight, newDate, newCountry);
                Add(newWorker);
                streamWriter.WriteLine(newWorker.WriteInFile());

                streamWriter.Close();
                ContinuedMenu();
            }
        }

        public int GetParseInt(string h)
        {
            bool result = int.TryParse(h, out int i);
            if (result)
            {
                return i;
            }
            return 0;
        }
        public DateTime GetParseDate(string d)
        {
            bool result = DateTime.TryParse(d, out DateTime i);
            if (result)
            {
                return i;
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// Метод продолжения ввода
        /// </summary>
        public void ContinuedMenu()
        {
            Console.WriteLine("\n>>>Продолжить ввод данных или перейти в главное меню?<<<\n");
            Console.WriteLine("1 - Продолжить ввод данных");
            Console.WriteLine("2 - Главное меню");
            string choice2 = Console.ReadLine();
            switch (choice2)
            {
                case "1":
                    AddWorker();
                    break;
                case "2":
                    Menu();
                    break;
                default:
                    Console.Write("Такого меню нет, повторите попытку!\n");
                    break;
            }
        }

        /// <summary>
        /// Чтение из файла
        /// </summary>
        /// <returns></returns>
        public Worker[] GetAllWorkers(string path)
        {
            List<Worker> workerList = new List<Worker>();
            using (StreamReader streamReader = new StreamReader(path))
            {
                while (!streamReader.EndOfStream)
                {
                    string[] data = streamReader.ReadLine().Split('\t');
                    Worker worker = new Worker
                    (
                        int.Parse(data[0]),
                        DateTime.Parse(data[1]),
                        data[2],
                        int.Parse(data[3]),
                        int.Parse(data[4]),
                        DateTime.Parse(data[5]),
                        data[6]
                    );
                    workerList.Add(worker);
                }
            }
            return workerList.ToArray();
        }     

        /// <summary>
        /// Поиск по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Worker GetWorkerById()
        {
            
            Console.Write("Введите ID для поиска : ");
            int searchId = Convert.ToInt32(Console.ReadLine());
            Worker searchWorker = new Worker();
            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                string[] data = line.Split('\t');

                if (data.Length >= 1 && int.TryParse(data[0], out int id))
                {
                    if (id == searchId)
                    {
                        searchWorker = new Worker
                        (
                        int.Parse(data[0]),
                        DateTime.Parse(data[1]),
                        data[2],
                        int.Parse(data[3]),
                        int.Parse(data[4]),
                        DateTime.Parse(data[5]),
                        data[6]
                        );
                        Console.WriteLine($"Сотрудник с ID {searchId} найден");
                        Console.WriteLine(searchWorker.Print());
                        break;
                    }   
                }
            }
            return searchWorker;
        }

        /// <summary>
            /// Удаление записи
            /// </summary>
            /// <param name="id"></param>
        public void DeleteWorker()
        {    
           
           Console.WriteLine("Введите ID для удаления : ");
           int deleteId = Convert.ToInt32(Console.ReadLine());
           var lines = File.ReadAllLines(path).ToList();
            for (int i = 0; i < lines.Count; i++)
             {
               string[] data = lines[i].Split('\t');
                if (data.Length >= 1 && int.TryParse(data[0], out int id))
                {
                    if (id == deleteId)
                    {
                        lines.RemoveAt(i);
                        File.WriteAllLines(path, lines);
                        this.workers = GetAllWorkers(path);
                        Console.WriteLine($"Сотрудник с ID {deleteId} удален");
                        break;
                    }
                   
                }
            }
           DeleteMenu();
        }
        public void DeleteMenu()
        {
            Console.WriteLine("\n>>>Продолжить удаление или перейти в главное меню?<<<\n");
            Console.WriteLine("1 - Продолжить удаление");
            Console.WriteLine("2 - Главное меню");
            string choice2 = Console.ReadLine();
            switch (choice2)
            {
                case "1":
                    DeleteWorker();
                    break;
                case "2":
                    Menu();
                    break;
                default:
                    Console.Write("Такого меню нет, повторите попытку!\n");
                    break;
            }
        }
    

        /// <summary>
        /// Загрузка записей в выбранном диапазоне дат
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        ///
        public Worker[] PrintAllWorkersOrderedByDate()
        {
            Console.Write("Введите первую дату в формате ММ/ДД/ГГГГ, для поиска в заданном диапазоне: ");
            DateTime dateFrom = DateTime.Parse(Console.ReadLine());
            Console.Write("Введите вторую дату в формате ММ/ДД/ГГГГ, для поиска в заданном диапазоне: ");
            DateTime dateTo = DateTime.Parse(Console.ReadLine());
            string[] lines = File.ReadAllLines(path);
            List<Worker> searchDate = new List<Worker>();
            foreach (var line in lines)
            {
                string[] data = line.Split('\t');
                DateTime dateAdded = DateTime.Parse(data[1]);
                if (dateAdded >= dateFrom && dateAdded <= dateTo)
                {
                    Worker foundDate = new Worker
                    (
                        int.Parse(data[0]),
                        DateTime.Parse(data[1]),
                        data[2],
                        int.Parse(data[3]),
                        int.Parse(data[4]),
                        DateTime.Parse(data[5]),
                        data[6]
                    );
                    searchDate.Add(foundDate);
                    Console.WriteLine(foundDate.Print());
                }
            }
            return searchDate.ToArray();
        }

        /// <summary>
        /// Вывод на консоль список
        /// </summary>
        /// <param name="workers"></param>
        public void PrintAllWorkers()
        {    
          Console.WriteLine($"{this.titles[0],8}{this.titles[1],30}{this.titles[2],30}" +
          $"{this.titles[3],30}{this.titles[4],30}{this.titles[5],30}{this.titles[6],30}");
           for (int i = 0; i < workers.Length; i++)
               {
                 Console.WriteLine(this.workers[i].Print());
               }              
            } 

        public void Delay()
        {
            Console.ReadKey();
        } 
      
        

    }
}

