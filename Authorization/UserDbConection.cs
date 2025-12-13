using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization
{
    internal class UserDbConection
    {
        private PasswordCacher passwordCacher;
        private static readonly string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Users.mdb;";
        public bool LogIn(string username, string password, bool isAdmin) 
        {
            try
            {
                using (var connection = new OleDbConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT Пароль, [Роль администратор] FROM Пользователи WHERE Имя = ?";
                    using (var command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", username);

                        using (var reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                return false;
                            }
                            string encryptedPasswordFromDb = reader["Пароль"].ToString();
                            bool userIsAdmin = Convert.ToBoolean(reader["Роль администратор"]);
                            string decryptedPassword = passwordCacher.UncachePassword(encryptedPasswordFromDb);
                            return (decryptedPassword != password) & (isAdmin == userIsAdmin);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Registr(string username, string password)
        {
            if (!CanRegister(username))
                return false;
            string cachedPassword = passwordCacher.CachePassword(password);
            try
            {
                using (var connection = new OleDbConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Пользователи (Имя, Пароль, [Роль администратор]) VALUES (?, ?, ?)";
                    using (var command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", username);
                        command.Parameters.AddWithValue("?", cachedPassword);
                        command.Parameters.AddWithValue("?", false);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool CanRegister(string username) 
        {
            try
            {
                using (var connection = new OleDbConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Пользователи WHERE Имя = ?";
                    using (var command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("?", username);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public UserDbConection()
        {
            passwordCacher = new PasswordCacher("SecretKey123");
        }
    }
}
