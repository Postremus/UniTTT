using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.AI
{
    public class ReinforcementMysqlWriterReader : IReinforcementDataWriterReader
    {
        private string _host;
        private int _port;
        private string _userName;
        private string _password;
        private string _database;
        public ReinforcementMysqlWriterReader(string host, int port, string userName, string password, string database)
        {
            _host = host;
            _port = port;
            _userName = userName;
            _password = password;
            _database = database;
        }

        public void Write(int[,] zuege, int[,] sitCodes, int[] wertungen)
        {

            throw new NotImplementedException();
        }

        public int[] Read(string sitcode)
        {
            throw new NotImplementedException();
        }
    }
}
