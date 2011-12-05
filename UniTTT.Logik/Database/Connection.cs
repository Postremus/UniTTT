using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Globalization;

namespace UniTTT.Logik.Database
{
    public class Connection
    {
        public string DbName { get; private set; }

        public Connection(string dname)
        {
            DbName = dname;
        }

        public bool Connect(ref SQLiteConnection conn)
        {
            conn.ConnectionString = string.Format(CultureInfo.InvariantCulture, "Data Source= {0}.db", DbName);
            conn.Open();
            return conn.ResultCode() == 0;
        }

        public bool Close(ref SQLiteConnection conn)
        {
            conn.Close();
            conn.Dispose();
            return true;
        }
    }
}
