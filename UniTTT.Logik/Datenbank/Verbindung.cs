using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Globalization;

namespace UniTTT.Datenbank
{
    public class Verbindung
    {
        string DbName;
        public Verbindung(string name)
        {
            DbName = name;
        }

        public bool Herstellen(ref SQLiteConnection conn)
        {
            conn.ConnectionString = string.Format(CultureInfo.InvariantCulture, "Data Source= {0}.db", DbName);
            conn.Open();
            return conn.ResultCode() == 0;
        }

        public bool Trennen(ref SQLiteConnection conn)
        {
            conn.Close();
            conn.Dispose();
            return true;
        }
    }
}
