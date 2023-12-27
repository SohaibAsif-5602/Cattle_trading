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

                        // Check for DBNull before conversion
                        user.Email = reader["email"] != DBNull.Value ? Convert.ToString(reader["email"]) : null;
                        user.Name = reader["name"] != DBNull.Value ? Convert.ToString(reader["name"]) : null;
                        user.Phone = reader["phone_number"] != DBNull.Value ? Convert.ToInt32(reader["phone_number"]) : 0;
                        user.Address = reader["address"] != DBNull.Value ? Convert.ToString(reader["address"]) : null;
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








    // TradeHistoryDetails class definition
    // Move the TradeHistoryDetails class to a separate file or section in your code
    public class TradeHistoryDetails
    {
        public string BuyerName { get; set; }
        public string SellerName { get; set; }
        public decimal Price { get; set; }
        public string Animal { get; set; }
        public string AnimalBreed { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        // Add other properties as needed for trade history details
    }
    public class TradeHistoryDAL
    {
        public async Task<List<TradeHistoryDetails>> GetTradeHistoryDetails(int userId)
        {
            List<TradeHistoryDetails> historyDetailsList = new List<TradeHistoryDetails>();

            using (SqlConnection connection = dbhelper.Getconnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetTradeHistoryDetails", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userId);

                    try
                    {
                        await connection.OpenAsync();
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();

                        while (reader.Read())
                        {
                            TradeHistoryDetails historyDetails = new TradeHistoryDetails();

                            historyDetails.BuyerName = reader["BuyerName"].ToString();
                            historyDetails.SellerName = reader["SellerName"].ToString();
                            historyDetails.Price = Convert.ToDecimal(reader["price"]);
                            historyDetails.Animal = reader["animal"].ToString();
                            historyDetails.AnimalBreed = reader["animal_breed"].ToString();
                            historyDetails.Age = Convert.ToInt32(reader["age"]);
                            historyDetails.Weight = Convert.ToDecimal(reader["weight"]);
                            historyDetails.Description = reader["description"].ToString();
                            // You can add more fields as per your trade history details model

                            historyDetailsList.Add(historyDetails);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (log, throw, etc.)
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }

            return historyDetailsList;
        }
    }




}
