using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace COT.DAL
{
    public sealed class SingletonDB
    {
        private static readonly SingletonDB instance = new SingletonDB();
        private MySqlConnection con = null;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static SingletonDB()
        {

        }

        private SingletonDB()
        {

        }

        public static SingletonDB Instance
        {
            get
            {
                return instance;
            }
        }

        public MySqlConnection GetDBConnection(string connectionString)
        {
            if (con == null)
            {
                con = new MySqlConnection(connectionString);
            }
           
            return con;
        }
    }
}
