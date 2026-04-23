using Microsoft.Data.SqlClient;

namespace E_Commerce_WebApp.Models.Concrete
{
    public class Connection
    {
        public static SqlConnection ServerConnect
        {
            get
            {
                SqlConnection sqlConnection = new SqlConnection("Server = AHMET\\SQLEXPRESS;Trusted_Connection=True;Database=TEKRARCore_ProjeDB;TrustServerCertificate=True;");
                return sqlConnection;
            }
        }
    }
}
