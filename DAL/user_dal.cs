using System;
using System.Data;
using System.Data.SqlClient;
using MODEL;

using ClassLibrarydal;

namespace DAL
{
    public class user_dal
    {


        public void RegisterUser(User_model user)
        {
            using (SqlConnection connection = dbhelper.Getconnection())
            {
                using (SqlCommand cmd = new SqlCommand("RegisterUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public User_model GetUserById(int userId)
        {
            User_model user = new User_model();

            using (SqlConnection connection = dbhelper.Getconnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetUserById", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        user.User_id = Convert.ToInt32(reader["user_id"]);
                        user.Email = Convert.ToString(reader["email"]);
                        user.Name = Convert.ToString(reader["name"]);
                        user.Phone = Convert.ToInt32(reader["phone_number"]);
                        user.Address = Convert.ToString(reader["address"]);
                    }
                    reader.Close();
                }
            }

            return user;
        }

        public void EditUserInfoById(int userId, string name, string phoneNumber, string address)
        {
            using (SqlConnection connection = dbhelper.Getconnection())
            {
                using (SqlCommand cmd = new SqlCommand("EditUserInfoById", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@Address", address);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task<bool> IsEmailAvailable(string email)
        {
            bool isAvailable = false;

            using (SqlConnection connection = dbhelper.Getconnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email = @Email", connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    await connection.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    if (result != null && Convert.ToInt32(result) == 0)
                    {
                        // Email is available (not registered in the Users table)
                        isAvailable = true;
                    }
                }
            }

            return isAvailable;
        }


        public async Task<(bool isValidUser, int userId)> ValidateUser(string email, string password)
        {
            bool isValidUser = false;
            int userId = 0;

            using (SqlConnection connection = dbhelper.Getconnection())
            {
                using (SqlCommand cmd = new SqlCommand("ValidateUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    await connection.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        isValidUser = Convert.ToInt32(reader["IsValid"]) == 1;
                        if (isValidUser)
                        {
                            userId = reader["UserID"] != DBNull.Value ? Convert.ToInt32(reader["UserID"]) : 0;
                        }
                    }
                    reader.Close();
                }
            }

            return (isValidUser, userId);
        }



    }
}
