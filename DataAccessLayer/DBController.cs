﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace DataAccessLayer
{
    class DBController
    {
        string connString = Properties.Settings.Default.ConnString;
        private int getSalt(String username)
        {
            int salt;
            
            string query;

            query = "SELECT salt FROM users WHERE username = '" + username + "'";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                salt = (int)command.ExecuteScalar();

                return salt;
            }
            catch
            {
                return -1;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        private string getPasswordHash(string username)
        {
            string hash;

            string query;

            query = "SELECT passwordHash FROM users WHERE username = '" + username + "'";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                hash = (string)command.ExecuteScalar();

                return hash;
            }
            catch
            {
                return "-1";
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        public int registerUser(string username, string passwordHash, int salt)
            
        { 
            string query;

            query = "INSERT INTO users (username, passwordHash, salt) "+
                "VALUES "+
                "('"+ username +"', '"+ passwordHash +"', '" + salt + "')";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                int returnValue = 0;
                returnValue = command.ExecuteNonQuery();
                return returnValue;
            }
            catch
            {
                return -1;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
}
    }
}
