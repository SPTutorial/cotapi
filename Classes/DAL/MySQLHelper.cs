using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace COT.DAL
{
    public class MySQLHelper
    {
        private MySqlConnection con = null;
        public MySQLHelper(string connectionString)
        {
            if(con == null)
            {
                con = SingletonDB.Instance.GetDBConnection(connectionString);
            }
        }

        /// <summary>
        /// To Execute Stored Procedure and GET DATA Information
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataset(CommandType commandType,string commandText, params MySqlParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlCommand cmd = new MySqlCommand(commandText, con);
                cmd.CommandType = commandType;
                if (commandType == CommandType.StoredProcedure)
                {
                    if (commandParameters != null)
                    {
                        foreach (MySqlParameter sqlPara in commandParameters)
                        {
                            if (sqlPara != null)
                            {
                                cmd.Parameters.AddWithValue(sqlPara.ParameterName, sqlPara.Value);
                            }
                        }
                    }
                }
                con.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(ds);
                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return ds;
        }

        /// <summary>
        /// To Insert/Update/Delete - Stored Procedures
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            int response = -1;
            try
            {
                MySqlCommand cmd = new MySqlCommand(commandText, con);
                cmd.CommandType = commandType;
                if (commandType == CommandType.StoredProcedure)
                {
                    if (commandParameters != null)
                    {
                        foreach (MySqlParameter sqlPara in commandParameters)
                        {
                            if (sqlPara != null)
                            {
                                cmd.Parameters.AddWithValue(sqlPara.ParameterName, sqlPara.Value);
                            }
                        }
                    }
                }
                con.Open();
                response = cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return response;
        }
    }
}