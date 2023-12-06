using BeAspNet.DataaAccress.DataAccessObject;
using BeAspNet.DataaAccress.DataObject;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeAspNet.DataaAccress.DataAcessObjecImpl
{
    public class UserDAOImpl : IUserDAO
    {
        public List<User> GetUsers()
        {
            var list = new List<User>();
            try
            {
                // Mở connectionstring
                var conn = DBHelper.GetSqlConnection();
                // Thực hiện lấy dữ liệu

                var cmd = new SqlCommand("SP_User_GetList", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

               // var cmd1 = new SqlCommand("SELECT * FROM ABC=1=1 ", conn);
               // cmd.CommandType = System.Data.CommandType.Text;

                // dọc dữ liệu
                var data = cmd.ExecuteReader();

                // đưa dữ liệu sang object  List<User>();
                while (data.Read())
                {
                    var user = new User
                    {
                        ID = data["ID"] != DBNull.Value ? Convert.ToInt32(data["ID"]) : 0,
                        FUllName = data["FUllName"].ToString(),
                        UserName = data["UserName"].ToString(),
                        UserAddress= data["UserAddress"].ToString()
                    };

                    list.Add(user);
                }

            }
            catch (Exception)
            {

                throw;
            }

            return list;
        }
    }
}
