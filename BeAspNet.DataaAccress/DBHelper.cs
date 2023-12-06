using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAspNet.DataaAccress
{
    public static class DBHelper
    {
        public static SqlConnection GetSqlConnection()
        {
            SqlConnection sqlConnection = null;
            try
            {
                var constr = "Data Source=DESKTOP-IFRSV3F;Initial Catalog=BE_ASPNET_2911;User ID=sa;Password=123456;";
                sqlConnection = new SqlConnection(constr);
                if(sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
            }
            catch (Exception ex)
            {

                sqlConnection.Dispose();
            }

            return sqlConnection;
        }
    }
}
