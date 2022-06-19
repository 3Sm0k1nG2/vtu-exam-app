using AppRestaurant.Models;
using Microsoft.Data.SqlClient;
using System.Text;

namespace AppRestaurant.Data
{
    public class OrderDb : DbContext
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
        public List<OrderModel> GetAll(int? userId)
        {
            List<OrderModel> orders =  new List<OrderModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sqlQuery = new StringBuilder("SELECT Id, UserId, AddressId, DishesIdsAsStr, TimePurchased, Status, Cost FROM Orders");
                    
                    if(userId != null) 
                    {
                        if (userId == -1)
                            return null;

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

        public List<OrderDetailedModel> GetAllDetailed(int? userId)
        {
            List<OrderDetailedModel> orders = new List<OrderDetailedModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sqlQuery = new StringBuilder(
                        @"SELECT
                             [Orders].Id
		                    ,[Addresses].Street
		                    ,[Addresses].Phone
		                    ,[Users].Email
		                    ,[DishesIdsAsStr]
		                    ,[TimePurchased]
		                    ,[Status]
		                    ,[Cost]
                          FROM [ExamDB].[dbo].[Orders]
                          LEFT JOIN Addresses
                          ON Orders.AddressId = Addresses.ID
                          LEFT JOIN Users
                          ON Orders.UserId = Users.ID"
                    );

                    if (userId != null)
                    {
                        if (userId == -1)
                            return null;

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
                                OrderDetailedModel model;

                                if (reader.GetString(6) == "successful")
                                {
                                    model = new OrderDetailedModel(reader.GetInt32(0),
                                        reader.GetString(1),
                                        reader.GetString(2),
                                        reader.GetString(3),
                                        reader.GetString(4),
                                        reader.GetDateTime(5),
                                        reader.GetString(6),
                                        reader.GetDecimal(7)
                                    );
                                }
                                else
                                {
                                    model = new OrderDetailedModel(reader.GetInt32(0),
                                        reader.GetString(3),
                                        reader.GetString(4),
                                        reader.GetString(6),
                                        reader.GetDecimal(7)
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

        public OrderDetailedModel GetOneDetailed(int orderId)
        {
            OrderDetailedModel? model = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    string sqlQuery = new StringBuilder(
                        @"SELECT
                             [Orders].Id
		                    ,[Addresses].Street
		                    ,[Addresses].Phone
		                    ,[Users].Email
		                    ,[DishesIdsAsStr]
		                    ,[TimePurchased]
		                    ,[Status]
		                    ,[Cost]
                          FROM [ExamDB].[dbo].[Orders]
                          LEFT JOIN Addresses
                          ON Orders.AddressId = Addresses.ID
                          LEFT JOIN Users
                          ON Orders.UserId = Users.ID
                          WHERE [Orders].Id=@orderId"
                    ).ToString();

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("orderId", orderId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.GetString(6) == "successful")
                                {
                                    model = new OrderDetailedModel(reader.GetInt32(0),
                                        reader.GetString(1),
                                        reader.GetString(2),
                                        reader.GetString(3),
                                        reader.GetString(4),
                                        reader.GetDateTime(5),
                                        reader.GetString(6),
                                        reader.GetDecimal(7)
                                    );
                                }
                                else
                                {
                                    model = new OrderDetailedModel(reader.GetInt32(0),
                                        reader.GetString(3),
                                        reader.GetString(4),
                                        reader.GetString(6),
                                        reader.GetDecimal(7)
                                    );
                                }

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

        public void PatchAddressFinal(OrderModel model)
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

        public bool DeleteOne(int id)
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
