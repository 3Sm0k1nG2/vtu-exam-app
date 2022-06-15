using AppRestaurant.Models;
using AppRestaurant.Models.Forms;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace AppRestaurant.Data
{
    public class AddressDb : DbContext
    {
        static public int recordsCount
        {
            get
            {
                SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();

                sqlConnectionStringBuilder.DataSource = "3SM0K1NG2\\VTU_SQLSERVER";
                sqlConnectionStringBuilder.UserID = "sa";
                sqlConnectionStringBuilder.Password = "sa";
                sqlConnectionStringBuilder.InitialCatalog = "ExamDB";
                sqlConnectionStringBuilder.TrustServerCertificate = true;
                sqlConnectionStringBuilder.IntegratedSecurity = true;

                try
                {
                    using (SqlConnection connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
                    {
                        connection.Open();
                        String sqlQuery = "SELECT COUNT(*) FROM Address";

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
                        String sqlQuery = "SELECT TOP 1 ID FROM Addresses ORDER BY ID DESC";

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

        public void Create(AddressFormDataModel addressFormDataModel)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"INSERT INTO Addresses (Street, City, PostCode, Phone)
                        VALUES (@street, @city, @postCode, @phone)";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("street", addressFormDataModel.Street);
                        cmd.Parameters.AddWithValue("city", addressFormDataModel.City);
                        cmd.Parameters.AddWithValue("postCode", addressFormDataModel.PostCode);
                        cmd.Parameters.AddWithValue("phone", addressFormDataModel.Phone);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public AddressModel Get(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    connection.Open();
                    String sqlQuery = @"SELECT Id, OrderId, Address, City, PostCode, Phone FROM Addresses
                        WHERE ID = @id";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AddressModel model = new AddressModel(id,
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.GetString(2)
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
    }
}
