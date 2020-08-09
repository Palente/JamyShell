using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace JamyShell.Modules
{
    class Config
    {

        /**
         * BASE DE DONNEES
         * tickets:
         * id => ID_TICKET
         * AUTHOR_TICKET => ID_AUTHOR
         * CHANNEL_ID => Id du Channel
         * DESCRIPTION => Description du ticket
         * 
         */
        private readonly Logs _log = new Logs();
        private readonly SQLiteConnection _connection;
        public Config()
        {
            //http://zetcode.com/csharp/sqlite/
            _connection = new SQLiteConnection("Data Source=database.db;");
            _connection.Open();
            using var cmdCreate = new SQLiteCommand
            {
                CommandText = @"CREATE TABLE IF NOT EXISTS tickets (id INTEGER PRIMARY KEY AUTOINCREMENT, author BIGINT, channel BIGINT, description TEXT, status INTEGER, created TIMESTAMP)",
                Connection = _connection
            };
            _log.Debug("CREATE tickets A TOUCHER: "+cmdCreate.ExecuteNonQuery().ToString());
            using var cmdCreateWarn = new SQLiteCommand
            {
                CommandText = @"CREATE TABLE IF NOT EXISTS warnings (id INTEGER PRIMARY KEY AUTOINCREMENT, victime BIGINT, author BIGINT, reason TEXT, time TIMESTAMP)",
                Connection = _connection
            };
            _log.Debug("CREATE warnings A TOUCHER: " + cmdCreateWarn.ExecuteNonQuery().ToString());
        }
        public void TestInser()
        {
            using var cmdInser = new SQLiteCommand
            {
                CommandText = @"INSERT INTO warnings (victime, author, reason, time) VALUES (0000, 0001, 'UN SIMPLE TEST', 0)",
                Connection = _connection
                //CommandText = @"CREATE TABLE IF NOT EXISTS warnings (id INT PRIMARY KEY AUTO_INCREMENT, victime BIGINT, author BIGINT, reason TEXT, time TIMESTAMP)"
            };
            _log.Debug("INSERT A TOUCHER: "+cmdInser.ExecuteNonQuery().ToString());
        }
        public SQLiteDataReader TestGet()
        {
            using var cmdGet = new SQLiteCommand
            {
                CommandText = @"SELECT * FROM warnings WHERE id=1",
                Connection = _connection
            };
            return cmdGet.ExecuteReader();
        }
        public void Close() => _connection.Close();
        public ConnectionState GetState() => _connection.State;
    }
}
