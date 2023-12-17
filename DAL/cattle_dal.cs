using ClassLibrarydal;
using MODEL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class cattle_dal
    {
        public static async Task<List<Cattle_model>> GetCattle()
        {
            SqlConnection sqlConnection = dbhelper.Getconnection();
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand("GetCattle", sqlConnection); // Assuming 'get_cattle_data' is the stored procedure for retrieving cattle data
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();
            List<Cattle_model> cattleList = new List<Cattle_model>();
            while (reader.Read())
            {
                Cattle_model cattle = new Cattle_model();
                cattle.CattleId = Convert.ToInt32(reader["cattle_id"]);
                cattle.UserId = Convert.ToInt32(reader["user_id"]);
                cattle.Animal = reader["animal"].ToString();
                cattle.AnimalBreed = reader["animal_breed"].ToString();
                cattle.Age = Convert.ToInt32(reader["age"]);
                cattle.Weight = Convert.ToDecimal(reader["weight"]);
                cattle.Price = Convert.ToDecimal(reader["price"]);
                cattle.Description = reader["description"].ToString();
                cattleList.Add(cattle);
            }
            sqlConnection.Close();
            return cattleList;
        }



        public async Task InsertNewCattle(int userId, string animal, string animalBreed, int age, decimal weight, decimal price, string description)
        {
            using (SqlConnection connection = dbhelper.Getconnection())
            {
                using (SqlCommand cmd = new SqlCommand("InsertNewCattle", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Animal", animal);
                    cmd.Parameters.AddWithValue("@AnimalBreed", animalBreed);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Weight", weight);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Description", description);

                    try
                    {
                        await connection.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (log, throw, etc.)
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }
        }



        public async Task DeleteCattle(int cattleId)
        {
            using (SqlConnection connection = dbhelper.Getconnection())
            {
                using (SqlCommand cmd = new SqlCommand("DeleteCattle", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CattleId", cattleId);

                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }






    }
}
