using System;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using _7newprog;


    public class Worker
    {
    public Worker() { }
    public Worker(int Id, DateTime CreationTime, string FullName, int Age, int Height, DateTime Birthday, string Country)
        {
            this.id = Id;
            this.creationTime = CreationTime;
            this.fullName = FullName;
            this.age = Age;
            this.height = Height;
            this.birthday = Birthday;
            this.country = Country;
        }

   

        /// <summary>
    /// ID
    /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Время создание записи
        /// </summary>
        public DateTime creationTime { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string fullName { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>  
        public int age { get; set; }

        /// <summary>
        /// Рост
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// День рождения
        /// </summary>
        public DateTime birthday { get; set; }

        /// <summary>
        /// город рождения
        /// </summary>
        public string country { get; set; }

        public string Print()
        {
            return $"\nID: {id} | Время записи: {creationTime} | ФИО:{fullName} | Возраст: {age} | Рост: {height} | День рождения: {birthday} | Город рождения: {country}";
        }
        public string WriteInFile()
    {
        return $" {id}\t {creationTime}\t {fullName}\t {age}\t {height}\t {birthday}\t {country}\t";
    }

}



