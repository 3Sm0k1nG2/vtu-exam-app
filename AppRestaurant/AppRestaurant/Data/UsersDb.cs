using Microsoft.Data.SqlClient;
using AppRestaurant.Models;
using System.Data;
using System.Data.SqlTypes;

namespace AppRestaurant.Data
{
    public class UsersDB : DbContext
    {
        public UserModel Connect(UserModel formModel)
        {
            UserModel? model = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"SELECT Id, Nickname, Email FROM Users
                        WHERE Email=@email AND Password=@password";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("email", formModel.Email);
                        command.Parameters.AddWithValue("password", formModel.Password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string? nickname;

                                try
                                {
                                    nickname = reader.GetString("Nickname");
                                }
                                catch (SqlNullValueException)
                                {
                                    nickname = null;
                                }

                                model = new UserModel(
                                    reader.GetInt32(0),
                                    nickname,
                                    reader.GetString(2));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return model;
        }

        public int GetId(string email)
        {
            int id = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"SELECT Id FROM Users
                        WHERE Email=@email";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("email", email);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id = reader.GetInt32("id");
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return id;
        }

        public List<int> GetAllIds()
        {
            List<int> ids = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"SELECT Id FROM Users";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ids.Add(reader.GetInt32("id"));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return ids;
        }

        public List<UserModel> GetAll()
        {
            List<UserModel> models = new List<UserModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = "SELECT Id, Nickname, Email FROM Users";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserModel model;
                                string? nickname;

                                try
                                {
                                    nickname = reader.GetString("Nickname");
                                }
                                catch (SqlNullValueException)
                                {
                                    nickname = null;
                                }

                                if (nickname == null)
                                    model = new UserModel(reader.GetInt32("id"), reader.GetString("Email"));
                                else
                                    model = new UserModel(reader.GetInt32("id"), nickname, reader.GetString("Email"));

                                models.Add(model);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return models;
        }

        public UserModel? GetOne(string email)
        {
            UserModel? model = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"SELECT Id, Nickname, Email FROM Users
                        WHERE Email=@email";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("email", email);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string? nickname;

                                try
                                {
                                    nickname = reader.GetString("Nickname");
                                }
                                catch (SqlNullValueException)
                                {
                                    nickname = null;
                                }

                                if (nickname == null)
                                    model = new UserModel(reader.GetInt32("id"), reader.GetString("Email"));
                                else
                                    model = new UserModel(reader.GetInt32("id"), nickname, reader.GetString("Email"));

                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return model;
        }

        public UserModel? GetOne(int id)
        {
            UserModel? model = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"SELECT Id, Nickname, Email FROM Users
                        WHERE Id=@id";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string? nickname;

                                try
                                {
                                    nickname = reader.GetString("Nickname");
                                }
                                catch (SqlNullValueException)
                                {
                                    nickname = null;
                                }

                                if (nickname == null)
                                    model = new UserModel(reader.GetInt32("id"), reader.GetString("Email"));
                                else
                                    model = new UserModel(reader.GetInt32("id"), nickname, reader.GetString("Email"));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return model;
        }

        public void Add(UserModel model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery;

                    if (model.Nickname == null)
                        sqlQuery = @"INSERT INTO Users (Email, Password)
                            Values (@email , @password)";
                    else
                        sqlQuery = @"INSERT INTO Users ( Nickname, Email, Password)
                            Values ( @nickname, @email , @password )";


                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        if (model.Nickname != null)
                            command.Parameters.AddWithValue("@nickname", model.Nickname);

                        command.Parameters.AddWithValue("@email", model.Email);
                        command.Parameters.AddWithValue("@password", model.Password);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void ChangePassword(UserModel model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = "UPDATE Users" +
                        "SET Password=@password" +
                        "WHERE Email=@email;";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("email", model.Password);
                        command.Parameters.AddWithValue("password", model.Password);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Delete(UserModel model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"DELETE FROM Users
                        WHERE Email=@email";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("email", model.Email);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public bool Delete(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"DELETE FROM Users
                        WHERE Email=@email";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        command.ExecuteNonQuery();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.RecordsAffected == 1)
                                return true;
                            else if (reader.RecordsAffected == 0)
                                return false;
                            else
                                throw new Exception("Negative records affected or more than two");
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"DELETE FROM Users
                        WHERE Id=@id";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        command.ExecuteNonQuery();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.RecordsAffected == 1)
                                return true;
                            else if (reader.RecordsAffected == 0)
                                return false;
                            else
                                throw new Exception("Negative records affected or more than two");
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public bool Exists(string email)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"SELECT [Id] FROM Users
                        WHERE Email = @email";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("email", email);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }
    }
}
