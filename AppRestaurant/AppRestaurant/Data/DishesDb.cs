using Microsoft.Data.SqlClient;
using AppRestaurant.Models;
using System.Data.SqlTypes;

namespace AppRestaurant.Data
{
    public class DishesDb : DbContext
    {
        public DishModel? GetOne(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"SELECT Name, Grams, Price, Description, ImageUrl FROM Dishes
                        WHERE ID = @id";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string? imgUrl;

                                try
                                {
                                    imgUrl = reader.GetString(4);
                                }
                                catch (SqlNullValueException)
                                {
                                    imgUrl = null;
                                }

                                DishModel model = new DishModel(id,
                                    reader.GetString(0),
                                    reader.GetInt32(1),
                                    reader.GetDecimal(2),
                                    reader.GetString(3),
                                    imgUrl);

                                return model;
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public List<DishModel> GetAll()
        {
            List<DishModel> modelCollection = new List<DishModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = "SELECT Id, Name, Grams, Price, Description, ImageUrl FROM Dishes";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string? imgUrl;

                                try
                                {
                                    imgUrl = reader.GetString(5);
                                }
                                catch (SqlNullValueException)
                                {
                                    imgUrl = null;
                                }

                                DishModel model = new DishModel(reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetInt32(2),
                                    reader.GetDecimal(3),
                                    reader.GetString(4),
                                    imgUrl);

                                modelCollection.Add(model);
                            }

                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return modelCollection;
        }

        public List<DishModel> GetAll(string? searchString)
        {
            List<DishModel> modelCollection = new List<DishModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"SELECT Id, Name, Grams, Price, Description, ImageUrl FROM Dishes
                        WHERE Name LIKE @srcStr OR Description LIKE @srcStr";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("srcStr", '%' + searchString + '%');

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string? imgUrl;

                                try
                                {
                                    imgUrl = reader.GetString(5);
                                }
                                catch (SqlNullValueException)
                                {
                                    imgUrl = null;
                                }

                                DishModel model = new DishModel(reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetInt32(2),
                                    reader.GetDecimal(3),
                                    reader.GetString(4),
                                    imgUrl);

                                modelCollection.Add(model);
                            }

                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return modelCollection;
        }

        public void Create(DishModel model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();

                    String sqlQuery = @"INSERT INTO Dishes (Name, Grams, Price, Description, ImageUrl)
                    VALUES (@name, @grams, @price, @description, @imgUrl)";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("name", model.Name);
                        command.Parameters.AddWithValue("grams", model.Grams);
                        command.Parameters.AddWithValue("price", model.Price);
                        command.Parameters.AddWithValue("imgUrl", model.ImageUrl);

                        if (model.Description == null)
                            command.Parameters.AddWithValue("description", "");
                        else
                            command.Parameters.AddWithValue("description", model.Description);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();

                    String sqlQuery = @"DELETE FROM Dishes
                    WHERE Id=@id";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("id", id);

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

        public void Update(DishModel oldModel, DishModel newModel)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();

                    String sqlQuery = @"UPDATE Dishes
                    SET Name=@newName, Grams=@newGrams, Price=@newPrice, Description=@newDescription, ImageUrl=@newImgUrl
                    WHERE ID=@id";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("newName", newModel.Name);
                        command.Parameters.AddWithValue("newGrams", newModel.Grams);
                        command.Parameters.AddWithValue("newPrice", newModel.Price);
                        command.Parameters.AddWithValue("newDescription", newModel.Description);
                        command.Parameters.AddWithValue("newImgUrl", newModel.ImageUrl);

                        command.Parameters.AddWithValue("id", oldModel.Id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Update(int id, DishModel newModel)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();

                    String sqlQuery = @"UPDATE Dishes
                    SET Name=@newName, Grams=@newGrams, Price=@newPrice, Description=@newDescription, ImageUrl=@newImgUrl
                    WHERE ID=@id";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("newName", newModel.Name);
                        command.Parameters.AddWithValue("newGrams", newModel.Grams);
                        command.Parameters.AddWithValue("newPrice", newModel.Price);
                        command.Parameters.AddWithValue("newDescription", newModel.Description);
                        command.Parameters.AddWithValue("newImgUrl", newModel.ImageUrl);

                        command.Parameters.AddWithValue("id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
