using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionToServer
{
    public static class Connection
    {
        private static MySqlConnection sqlConnection;
        private static MySqlCommand sqlCommand;
        private static DataSet dataSet;
        private static readonly String connectstr = "Server=solove00.mysql.ukraine.com.ua;" +
                                                    "Database=solove00_db;" +
                                                    "Uid=solove00_db;" +
                                                    "Pwd=2UHk3kBR;" +
                                                    "CharSet = cp1251; ";
        public static void Connect()
        {
            sqlConnection = new MySqlConnection(connectstr);
            sqlConnection.Open();
        }

        public static void CloseConnection()
        {
            if (sqlConnection == null)
                return;
            if(sqlConnection.State == ConnectionState.Open)
                sqlConnection.Close();
        }

        public static String CheckConnection()
        {
            if (sqlConnection == null)
                return "null connection";
            else if (sqlConnection.State == ConnectionState.Closed)
                return "closed";
            else if (sqlConnection.State == ConnectionState.Open)
                return "open";
            else if (sqlConnection.State == ConnectionState.Connecting)
                return "connecting";
            return sqlConnection.State.ToString();
        }

        public static DataSet GetRequest(String SQLRequest)
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                try
                {
                    if (sqlCommand == null)
                        sqlCommand = new MySqlCommand();
                    sqlCommand.CommandText = SQLRequest;
                    sqlCommand.Connection = sqlConnection;

                    sqlCommand.ExecuteNonQuery();
                    MySqlDataAdapter da = new MySqlDataAdapter(sqlCommand);
                    if (dataSet == null)
                        dataSet = new DataSet();
                    else
                        dataSet.Clear();

                    da.Fill(dataSet);
                    String str = "";
                    foreach(DataTable t in dataSet.Tables)
                        foreach(DataRow r in t.Rows)
                            str = r["email"].ToString();

                }
                catch
                {
                    
                }
            }
            else
            {
                return null;
            }
            return dataSet;
        }

        public static Boolean SetRequest(String SQLRequest)
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                try
                {
                    if (sqlCommand == null)
                        sqlCommand = new MySqlCommand();
                    sqlCommand.CommandText = SQLRequest;
                    sqlCommand.Connection = sqlConnection;

                    sqlCommand.ExecuteNonQuery();
                    
                }
                catch
                {
                    return false;
                }
                return true;
            }
            
            return false;
        }
    }
}
