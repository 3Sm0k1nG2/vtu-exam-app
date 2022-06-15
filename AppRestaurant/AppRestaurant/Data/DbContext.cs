using Microsoft.Data.SqlClient;

namespace AppRestaurant.Data
{
    public class DbContext
    {
        protected readonly SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

        public DbContext()
        {
            sqlBuilder.DataSource = "3SM0K1NG2\\VTU_SQLSERVER";
            sqlBuilder.UserID = "sa";
            sqlBuilder.Password = "sa";
            sqlBuilder.InitialCatalog = "ExamDB";
            sqlBuilder.TrustServerCertificate = true;
            sqlBuilder.IntegratedSecurity = true;
        }

        public string ConnectionString
        {
            get
            {
                return sqlBuilder.ConnectionString;
            }
        }
    }
}
