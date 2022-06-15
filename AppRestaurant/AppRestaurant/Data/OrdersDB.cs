using Microsoft.AspNetCore.Mvc;
using AppRestaurant.Models;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using AppRestaurant.Models.NewFolder;

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
        public List<OrderModel> GetAll(FilterModel? filters)
        {
            List<OrderModel> orders =  new List<OrderModel>();
            int? userId = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sqlQuery = new StringBuilder("SELECT Id, UserId, AddressId, DishesIdsAsStr, TimePurchased, Status, Cost FROM Orders");
                    
                    if(filters != null) 
                    {
                        if (filters.Id != null) 
                        {
                            userId = filters.Id;
                        }
                        else if (filters.Email != null)
                        {
                            UsersDB usersDb = new UsersDB();
                            userId = usersDb.GetId(filters.Email);
                        }

                        if (userId == -1)
                            return orders;

                        sqlQuery.Append(" WHERE UserId=@userId");
                    }

                    using (SqlCommand cmd = new SqlCommand(sqlQuery.ToString(), connection))
                    {
                        if (userId != null)
                            cmd.Parameters.AddWithValue("userId", userId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderModel model;

                                if (reader.GetString(5) == "successful")
                                {
                                    model = new OrderModel(reader.GetInt32(0),
                                        reader.GetInt32(1),
                                        reader.GetInt32(2),
                                        reader.GetString(3),
                                        reader.GetDateTime(4),
                                        reader.GetString(5),
                                        reader.GetDecimal(6)
                                    );
                                } else
                                {
                                    model = new OrderModel(reader.GetInt32(0),
                                        reader.GetInt32(1),
                                        reader.GetString(3),
                                        reader.GetString(5),
                                        reader.GetDecimal(6)
                                    );
                                }

                                orders.Add(model);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return orders;
        }

        public OrderModel GetOne(int id)
        {
            OrderModel? model = null;

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

            return model;
        }

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

        public bool Delete(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"DELETE FROM Orders
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
    }
}
