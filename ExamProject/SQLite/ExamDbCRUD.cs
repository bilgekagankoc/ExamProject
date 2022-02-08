using ExamProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace ExamProject.SQLite
{
    public class ExamDbCRUD
    {
        public bool TabloVarmi(SQLiteConnection openConnection, string tabloAdi)
        {
            var sql = "SELECT name FROM sqlite_master  WHERE type='table' AND name='" + tabloAdi + "';";
            if (openConnection.State == System.Data.ConnectionState.Open)
            {
                SQLiteCommand command = new SQLiteCommand(sql, openConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                var usersCreateTable = @"CREATE TABLE Users(
                               ID INTEGER PRIMARY KEY AUTOINCREMENT ,
                               KullaniciAdi           TEXT      NOT NULL,
                               Sifre            TEXT       NOT NULL,
                               RolTipi          INTEGER NOT NULL
                            );";
                var soruMetniCreateTable = @"CREATE TABLE SoruText(
                               ID INTEGER PRIMARY KEY AUTOINCREMENT ,
                               SoruMetni           TEXT      NOT NULL,
                               SoruBasligi          TEXT      NOT NULL
                            );";
                var SoruVeCevaplarCreateTable = @"CREATE TABLE SoruCevap(
                               ID INTEGER PRIMARY KEY AUTOINCREMENT ,
                               SoruTextId INTEGER NOT NULL,
                               A TEXT NOT NULL,
                               B TEXT NOT NULL,
                               C TEXT NOT NULL,
                               D TEXT NOT NULL,
                               DOGRUCEVAP TEXT NOT NULL
                            );";
                SQLiteCommand cmd = new SQLiteCommand(usersCreateTable, ExamDb.connection);
                cmd.ExecuteNonQuery();
                SQLiteCommand cmd1 = new SQLiteCommand(soruMetniCreateTable, ExamDb.connection);
                cmd1.ExecuteNonQuery();
                SQLiteCommand cmd2 = new SQLiteCommand(SoruVeCevaplarCreateTable, ExamDb.connection);
                cmd2.ExecuteNonQuery();
                return true;
            }

            else
            {
                throw new System.ArgumentException("Data.ConnectionState must be open");
            }
        }
        public bool KullaniciVarmi(SQLiteConnection openConnection)
        {

            var sql = "SELECT * FROM Users";
            SQLiteCommand cmd = new SQLiteCommand(sql, ExamDb.connection);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows == false)
            {
                SQLiteCommand cmd2 = new SQLiteCommand(ExamDb.connection);
                cmd2.CommandText = "INSERT INTO Users (KullaniciAdi,Sifre,RolTipi) VALUES ('admin','12345',1)";
                cmd2.ExecuteNonQuery();
                cmd2.CommandText = "INSERT INTO Users (KullaniciAdi,Sifre,RolTipi) VALUES ('user','12345',2)";
                cmd2.ExecuteNonQuery();
                return true;
            }
            else if (rdr.HasRows == true)
            {
                return true;
            }
            else
            {
                throw new System.ArgumentException("Data.ConnectionState must be open");
            }

        }
        public void VeriTabanıKontrol()
        {

            var tabloSonuc = TabloVarmi(ExamDb.connection, "Users");
            var veriSonuc = KullaniciVarmi(ExamDb.connection);

        }

        public UserModel KullaniciBilgileri(UserModel user)
        {
            SQLiteCommand cmd = new SQLiteCommand(ExamDb.connection);
            cmd.CommandText = $"SELECT * FROM Users Where KullaniciAdi = '{user.KullaniciAdi}' and Sifre = '{user.Sifre}'";
            var sqlQuery = cmd.ExecuteReader();
            UserModel sqlUser = new UserModel();
            while (sqlQuery.Read())
            {
                sqlUser = new UserModel()
                {
                    KullaniciAdi = sqlQuery["KullaniciAdi"].ToString(),
                    id = Convert.ToInt32(sqlQuery["Id"]),
                    RolTipi = Convert.ToInt32(sqlQuery["RolTipi"]),
                    Sifre = sqlQuery["Sifre"].ToString()
                };
                return sqlUser;
            }
            if (!sqlQuery.Read())
            {

                return sqlUser;
            }
            return null;

        }
        public bool AddWiredText(string text, string header)
        {

            SQLiteCommand cmd = new SQLiteCommand(ExamDb.connection);
            cmd.CommandText = $"INSERT INTO SoruText (SoruMetni,SoruBasligi) VALUES ('{text}','{header}')";
            cmd.ExecuteNonQuery();
            return true;
        }
        public bool YaziEklendiMi()
        {
            var sql = "SELECT count(*) FROM SoruText";
            SQLiteCommand cmd = new SQLiteCommand(sql, ExamDb.connection);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            int rowCount = 0;
            while (rdr.Read())
            {
                rowCount = Convert.ToInt32(rdr["count(*)"]);

            }
            if (rowCount == 5)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public List<SoruTextModel> GetTextDb()
        {
            List<SoruTextModel> stms = new List<SoruTextModel>();
            if (!(ExamDb.connection.State == ConnectionState.Open))
            {
                ExamDb.connection.Open();
            }
            var sql = "SELECT * FROM SoruText";
            SQLiteCommand cmd = new SQLiteCommand(sql, ExamDb.connection);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                foreach (var item in rdr)
                {
                    SoruTextModel stm = new SoruTextModel()
                    {
                        id = rdr.GetInt32(0),
                        SoruBasligi = rdr.GetString(2),
                        SoruMetni = rdr.GetString(1),
                    };
                    stms.Add(stm);
                }

            }
            return stms;
        }

        public void SaveExam(SoruTextModels model)
        {
            if (!(ExamDb.connection.State == ConnectionState.Open))
            {
                ExamDb.connection.Open();
            }

            foreach (var item in model.SoruCevapModel.SoruModels)
            {
                SQLiteCommand cmd2 = new SQLiteCommand(ExamDb.connection);
                cmd2.CommandText = $"INSERT INTO SoruCevap (SoruTextId,A,B,C,D,DOGRUCEVAP) VALUES ({model.SoruCevapModel.SoruTextId},'{item.A}','{item.B}','{item.C}','{item.D}','{item.Dogru}')";
                cmd2.ExecuteNonQuery();
            }
        }
        //ÇALIŞMIYOR
        //public SoruTextModels GetExamDb()
        //{
        //    SoruTextModels stm = new SoruTextModels();
        //    var soruTextId = 0;
        //    if (!(ExamDb.connection.State == ConnectionState.Open))
        //    {
        //        ExamDb.connection.Open();
        //    }
        //    var sql = "SELECT * FROM SoruCevap";
        //    SQLiteCommand cmd = new SQLiteCommand(sql, ExamDb.connection);
        //    SQLiteDataReader rdr = cmd.ExecuteReader();
        //    while (rdr.Read())
        //    {
        //        foreach (var item in rdr)
        //        {
        //            stm.SoruTextId = Convert.ToInt32(rdr.GetInt32(0));

        //            var A = rdr.GetString(1);
        //            var B = rdr.GetString(2);
        //            var C = rdr.GetString(3);
        //            var D = rdr.GetString(4);
        //            var Dogru = rdr.GetString(6);
                 
        //          //  stm.SoruCevapModel.SoruModels.Add(soruModel);
        //        }
                
                

        //    }
        //    return stm;

        }
    }
}
