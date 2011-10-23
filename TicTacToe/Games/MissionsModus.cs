using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using TicTacToe.Datenbank;
using System.Globalization;

namespace TicTacToe.Konsole.Games
{
    class MissionsModus
    {
        private DB db = new DB();
        List<string> missionsdetails = new List<string>();
        public void Start()
        {
            do
            {
                
            } while (!HasEnd());
        }

        private bool HasEnd() 
        { 
            return true;
        }

        public List<String> MissionsWahl()
        {
            bool missiongewaehlt = false;
            List<string> titel = new List<string>();
            do
            {
                Console.Clear();
                titel = db.GetAllMissionTitel();
                for (int i = 0; i < titel.Count; i++)
                    Console.WriteLine("{0}: {1}", i + 1, titel[i]);
                Console.Write("Zahl der Mission: ");
                int missionsid = -1 + int.Parse(Console.ReadLine(), CultureInfo.CurrentCulture);
                missionsdetails = db.GetMissionDetails(missionsid);
                Console.WriteLine(missionsdetails[2]);
                Console.WriteLine();
                Console.WriteLine("Wollen sie diese Aufgabe anehmen? (J/N)");
                missiongewaehlt = Console.ReadKey().Key.ToString().ToUpper(CultureInfo.CurrentCulture).Trim() == "J";
            } while (!missiongewaehlt);

            missionsdetails.RemoveAt(0);
            return missionsdetails;
        }
    }

    class DB
    {
        SQLiteConnection conn;
        Verbindung verb = new Verbindung("missionen");

        public List<string> GetAllMissionTitel()
        {
            List<string> titel = new List<string>();
            if (verb.Herstellen(ref conn))
            {
                using (SQLiteTransaction traction = conn.BeginTransaction())
                {
                    using (SQLiteCommand sql = new SQLiteCommand(conn))
                    {
                        sql.CommandText = "Select Titel From Mission";
                        SQLiteDataReader reader = sql.ExecuteReader();
                        while (reader.Read())
                            titel.Add(reader.ToString());
                    }
                    traction.Commit();
                }
                verb.Trennen(ref conn);
            }
            return titel;
        }

        public List<string> GetMissionDetails(int id)
        {
            List<string> missiondetails = new List<string>();
            if (verb.Herstellen(ref conn))
            {
                using (SQLiteTransaction traction = conn.BeginTransaction())
                {
                    using (SQLiteCommand sql = new SQLiteCommand(conn))
                    {
                        sql.CommandText = string.Format(CultureInfo.CurrentCulture, "Select * From Mission wehre Id= {0}", id);
                        SQLiteDataReader reader = sql.ExecuteReader();
                        foreach (string x in reader)
                        {
                            missiondetails.Add(x);
                        }
                    }
                    traction.Commit();
                }
            }
            return missiondetails;
        }

        public void TabelleErstellen()
        {
            if (verb.Herstellen(ref conn))
            {
                using (SQLiteTransaction traction = conn.BeginTransaction())
                {
                    using (SQLiteCommand sql = new SQLiteCommand(conn))
                    {
                        sql.CommandText = "Create Table If Not Exists Missionen ";
                        sql.CommandText += "(Id Integer Primary Key Autoincrement, Titel Varchar(100), Beschreibung Varchar(500), Breite integer, Hoehe integer, SitCode integer, P1, P2, EndSitCode);";
                        sql.ExecuteNonQuery();
                    }
                    traction.Commit();
                }
                verb.Trennen(ref conn);
            }
        }
    }
}
