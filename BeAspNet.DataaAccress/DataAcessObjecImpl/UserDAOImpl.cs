﻿using BeAspNet.DataaAccress.DataAccessObject;
using BeAspNet.DataaAccress.DataObject;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BeAspNet.DataaAccress.DataAcessObjecImpl
{
    public class UserDAOImpl : IUserDAO
    {
        public User GetById(int id)
        {
            var user = new User();
            try
            {
                // Mở connectionstring
                var conn = DBHelper.GetSqlConnection();
                // Thực hiện lấy dữ liệu

                var cmd = new SqlCommand("SP_User_GetByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);
                // var cmd1 = new SqlCommand("SELECT * FROM ABC=1=1 ", conn);
                // cmd.CommandType = System.Data.CommandType.Text;

                // dọc dữ liệu
                var data = cmd.ExecuteReader();

                // đưa dữ liệu sang object  List<User>();
                while (data.Read())
                {

                    user.ID = data["ID"] != DBNull.Value ? Convert.ToInt32(data["ID"]) : 0;
                    user.FUllName = data["FUllName"].ToString();
                    user.UserName = data["UserName"].ToString();
                    user.UserAddress = data["UserAddress"].ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return user;
        }

        public List<User> GetUsers(int PageIndex, int PageSize, out int TotalCount)
        {
            var list = new List<User>();
            try
            {
                // Mở connectionstring
                var conn = DBHelper.GetSqlConnection();
                // Thực hiện lấy dữ liệu

                var cmd = new SqlCommand("SP_User_GetList", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@_PageIndex", PageIndex);
                cmd.Parameters.AddWithValue("@_PageSize", PageSize);
                cmd.Parameters.Add("@_TotalCount", System.Data.SqlDbType.Int).Direction
                   = System.Data.ParameterDirection.Output;
                // dọc dữ liệu
                var dataReader = cmd.ExecuteReader();

                // đưa dữ liệu sang object  List<User>();
                //while (data.Read())
                //{
                //    var user = new User
                //    {
                //        ID = data["ID"] != DBNull.Value ? Convert.ToInt32(data["ID"]) : 0,
                //        FUllName = data["FUllName"].ToString(),
                //        UserName = data["UserName"].ToString(),
                //        UserAddress = data["UserAddress"].ToString()
                //    };

                //    list.Add(user);
                //}

                list = DataReaderMapToList<User>(dataReader);

                dataReader.Close();

                TotalCount = cmd.Parameters["@_TotalCount"] != (object)DBNull.Value ? Convert.ToInt32(cmd.Parameters["@_TotalCount"].Value) : -1;
            }
            catch (Exception)
            {

                throw;
            }

            return list;
        }

        public int UserDelete(int UserId)
        {
            int rs = 0;
            try
            {  // Mở connectionstring
                var conn = DBHelper.GetSqlConnection();
                // Thực hiện lấy dữ liệu

                var cmd = new SqlCommand("SP_USER_DELETE", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@_Id", UserId);
                cmd.Parameters.Add("@_ResponseCode", System.Data.SqlDbType.Int).Direction
                    = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                rs = cmd.Parameters["@_ResponseCode"] != (object)DBNull.Value ? Convert.ToInt32(cmd.Parameters["@_ResponseCode"].Value) : -1;
            }
            catch (Exception ex)
            {

                throw;
            }

            return rs;
        }

        public int UserUpdate(User user)
        {
            int rs = 0;
            try
            {  // Mở connectionstring
                var conn = DBHelper.GetSqlConnection();
                // Thực hiện lấy dữ liệu

                var cmd = new SqlCommand("SP_User_Update", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@_Id", user.ID);
                cmd.Parameters.AddWithValue("@_UserName", user.UserName);
                cmd.Parameters.AddWithValue("@_FUllName", user.FUllName);
                cmd.Parameters.AddWithValue("@_UserAddress", user.UserAddress);
                cmd.Parameters.Add("@_ResponseCode", System.Data.SqlDbType.Int).Direction
                    = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                rs = cmd.Parameters["@_ResponseCode"] != (object)DBNull.Value ? Convert.ToInt32(cmd.Parameters["@_ResponseCode"].Value) : -1;
            }
            catch (Exception ex)
            {

                throw;
            }

            return rs;
        }

        public static List<T> DataReaderMapToList<T>(SqlDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
    }
}
