using Microsoft.AspNetCore.Mvc;
using AppRestaurant.Models;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace AppRestaurant.Data
{
    public class OrdersDB : DbContext
    {
        // To Implement

        static public int recordsCount
        {
            get
            {
                DbContext dbContext = new DbContext();

                try
                {
                    using (SqlConnection connection = new SqlConnection(dbContext.ConnectionString))
                    {
                        connection.Open();
                        String sqlQuery = "SELECT COUNT(*) FROM Orders";

                        using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    return Convert.ToInt32(reader[0]);
                                }
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                    return -1;
                }

                return 0;
            }
        }

        static public int lastId
        {
            get
            {
                DbContext dbContext = new DbContext();

                try
                {
                    using (SqlConnection connection = new SqlConnection(dbContext.ConnectionString))
                    {
                        connection.Open();
                        String sqlQuery = "SELECT TOP 1 ID FROM Orders ORDER BY ID DESC";

                        using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    return Convert.ToInt32(reader[0]);
                                }
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                    return -1;
                }

                return 0;
            }
        }

        public void Create(OrderModel orderModel)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"INSERT INTO Orders (UserId, DishesIdsAsStr, Status, Cost)
                        VALUES (@userId, @dishesIdsString, @status, @cost)";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("userId", orderModel.UserId);
                        cmd.Parameters.AddWithValue("dishesIdsString", orderModel.DishesIdsString);
                        cmd.Parameters.AddWithValue("status", orderModel.Status);
                        cmd.Parameters.AddWithValue("cost", orderModel.Cost);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public List<OrderModel> GetAll;
        public List<OrderModel> GetAllByUserId;
        public List<OrderModel> GetAllByUserEmail;
        public OrderModel GetOne(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"SELECT Id, UserId, AddressId, DishesIdsAsStr, TimePurchased, Status, Cost FROM Orders
                        WHERE ID = @id";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderModel model;

                                if (reader.GetString(5) == "successful")
                                {
                                    return model = new OrderModel(id,
                                        reader.GetInt32(1),
                                        reader.GetInt32(2),
                                        reader.GetString(3),
                                        reader.GetDateTime(4),
                                        reader.GetString(5),
                                        reader.GetDecimal(6)
                                    );
                                }

                                model = new OrderModel(id,
                                    reader.GetInt32(1),
                                    reader.GetString(3),
                                    reader.GetString(5),
                                    reader.GetDecimal(6)
                                    );

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

        public List<OrderModel> DeleteOne;
        public List<OrderModel> DeleteAllByUserId;
        public List<OrderModel> DeleteAllByUserEmail;
        public void Update(OrderModel model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"UPDATE Orders
                        SET AddressId=@addressId, TimePurchased=@timePurchased, Status=@status
                        WHERE ID = @id";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("id", model.Id);
                        cmd.Parameters.AddWithValue("addressId", model.AddressId);
                        cmd.Parameters.AddWithValue("timePurchased", model.TimePurchased);
                        cmd.Parameters.AddWithValue("status", model.Status);

                        cmd.ExecuteNonQuery();
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
