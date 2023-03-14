using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeaMart
{
    internal class connections
    {
        public static MySqlConnection getConnection()
        {
            MySqlConnection connection = null;

            try
            {
                string sConnstring = "server = localhost;database=db_ragnarok;uid=root;password=;";
                connection = new MySqlConnection(sConnstring);
            }
            catch (MySqlException sqlex)
            {
                throw new Exception(sqlex.Message.ToString());
            }
            return connection;
        }
    }
}