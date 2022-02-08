using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace ExamProject.Models
{
    public class ExamDb
    {
        public static SQLiteConnection connection = new SQLiteConnection("Data Source=D:\\examdb.db;Version=3;");
    }
}
