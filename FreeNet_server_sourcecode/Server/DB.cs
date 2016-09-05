using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql;
using MySql.Data.MySqlClient;
using System.Data;

namespace Logic
{
    class DB
    {
        static MySqlConnection conn;
        static DataSet ds;

        public void InitDB(string serverName, string dbName, string id, string pw)
        {
            string strconn = "server=" + serverName + ";Database=" + dbName
                + ";Uid=" + id + ";Pwd=" + pw;
            conn = new MySqlConnection(strconn);
            ds = new DataSet();
        }

        void ExecuteQuery(string query)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void UpdateGameResult(string id, bool isWin)
        {

            string sql = "UPDATE user_info SET ";
            string column;
            if (isWin)
                column = "numWin";
            else
                column = "numLose";

            sql += column + " = " + column + " + 1 WHERE id = '" + id + "'";
            ExecuteQuery(sql);


        }

        public bool RegisterUser(string id, string pw)
        {
            if (IsUserExist(id))
                return false;

            string sql = "INSERT INTO user_info (id, password) VALUES ('" + id + "', '" + pw + "') ";
            ExecuteQuery(sql);

            return true;
        }

        public bool IsUserExist(string id)
        {

            string sql = "SELECT * FROM user_info WHERE id = '" + id + "'";

            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);


            MySqlDataReader rdr = cmd.ExecuteReader();


            bool isExist = rdr.Read();
            rdr.Close();
            conn.Close();

            if (isExist)
                return true;
            else
                return false;


        }

        public bool IsUserExist(string id, string pw)
        {
            string sql = "SELECT * FROM user_info WHERE id = '" + id + "' AND password = '" + pw + "'";

            conn.Open();
            MySqlCommand cmd = new MySqlCommand(sql, conn);


            MySqlDataReader rdr = cmd.ExecuteReader();


            bool isExist = rdr.Read();
            rdr.Close();
            conn.Close();

            if (isExist)
                return true;
            else
                return false;
        }
        

    public void Insert(string table, List<string> columns, List<dynamic> values)
        {
            string sql = "INSERT INTO " + table + " (";

            int index = 0;
            foreach(string column in columns)
            {
                sql += column;
                if(index != columns.Count-1)
                    sql += ", ";

                index++;
            }

            sql += ") VALUES (";

            index = 0;
            foreach(var value in values)
            {
                if (value is string)
                    sql += " '";

                sql += value;

                if (value is string)
                    sql += "'";

                if (index != values.Count - 1)
                    sql += ", ";

                index++;
            }

            ExecuteQuery(sql);
        }

        public void Delete(string table, string condition = null)
        {

        }

        public void UpdateGameResult(string table, string id, string pw, bool isWin)
        {
            string sql = "Update " + table +
                " SET  ";

            string WinOrLose;
            if (isWin)
                WinOrLose = "numWin";
            else
                WinOrLose = "numLose";

            sql += WinOrLose + " = " + WinOrLose +  " + 1 WHERE id = " + id;
            ExecuteQuery(sql);


        }
    }
}
