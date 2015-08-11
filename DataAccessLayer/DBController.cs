using System;
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
    public class DBController
    {
        string connString = Properties.Settings.Default.ConnString;
        public int getSalt(String username)
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

        public int authenticateUser(string username, string passwordHash, int nounce)
        {
            string retrievedHash = getPasswordHash(username);
            int nounceCheck;

            if (retrievedHash == "-1")
            {
                return -1;
            }
            else if (retrievedHash == passwordHash)
            {
                /*
                nounceCheck = checkNounce(nounce, username);
                if (nounceCheck == -1)
                {
                    int store = storeNounce(username, DateTime.UtcNow , nounce);
                    return store;
                }
                else
                {
                    return 0;
                }
                */
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int storePasswordDetails(string url, string username, string password, string iniValue, string keyValue, string user)
        {
            string query;

            query = "INSERT INTO stored (user, iniVal, url, password, username, key) " +
                "VALUES " +
                "('" + user + "', '" + iniValue + "','" + url + "','" + password + "','" + username + "', '" + keyValue + "')";

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

        public string getUsername(string url, string user)
        {
            string username;

            string query;

            query = "SELECT username FROM stored WHERE url = '" + url + "' AND user = '" + user + "'";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                username = (string)command.ExecuteScalar();

                return username;
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

        public string getPassword(string url, string user)
        {
            string password;

            string query;

            query = "SELECT password FROM stored WHERE url = '" + url + "' AND user = '" + user + "'";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                password = (string)command.ExecuteScalar();

                return password;
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

        public string getIniValue(string url, string user)
        {
            string IniValue;

            string query;

            query = "SELECT iniVal FROM stored WHERE url = '" + url + "' AND user = '" + user + "'";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                IniValue = (string)command.ExecuteScalar();

                return IniValue;
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

        public string getKey(string url, string user)
        {
            string key;

            string query;

            query = "SELECT key FROM stored WHERE url = '" + url + "' AND user = '" + user + "'";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                key = (string)command.ExecuteScalar();

                return key;
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

        private int storeNounce(string username, DateTime date, int nounce)
        {
            string query;

            query = "INSERT INTO nounce (nounce, user, date) "+
                "VALUES "+
                "('"+ nounce +"', '"+ username +"', '" + date + "')";

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

        private int checkNounce(int nounceEntered, string username)
        {
            int check;

            string query;

            query = "SELECT nounce FROM nounce WHERE nounce = '" + nounceEntered + "'";

            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                check = (int)command.ExecuteScalar();
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

            if (nounceEntered == check)
            {
                string query2;
                string retrievedUser;

                query2 = "SELECT username FROM nounce WHERE nounce = '" + nounceEntered + "'";
                SqlCommand command2 = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    retrievedUser = (string)command2.ExecuteScalar();
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

                if (username == retrievedUser)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                return 0;
            }
        }
}
    }

