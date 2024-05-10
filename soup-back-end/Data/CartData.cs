using soup_back_end.Models;
using MySql.Data.MySqlClient;
using MimeKit.Encodings;
using Mysqlx.Crud;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Security;
using MySqlX.XDevAPI.Common;

namespace soup_back_end.Data
{
    public class CartData
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CartData(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        

        //getAll
        public List<Cart> GetAll()
        {
            List<Cart> cart = new List<Cart>();
            string query =
         "SELECT " +
         "cr.cartId, " +
         "cr.UserId, " +
         "cr.courseId, " +
         "cr.categoryId, " +
         "cr.scheduleId, " +
         "co.course_Name, " +
         "cs.scheduleDate " +
         "FROM cart cr " +
         "JOIN course co ON co.Id = cr.courseId " +
         "JOIN course_schedule cs ON cs.scheduleId = cr.scheduleId " +
         "ORDER BY cr.cartId;";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) {
                                {
                                    cart.Add(new Cart
                                    {
                                        cartId = Guid.Parse(reader["cartId"].ToString() ?? string.Empty),
                                        courseId = reader["courseId"].ToString() ?? string.Empty,
                                        categoryId = reader["categoryId"].ToString() ?? string.Empty,
                                        userId = Guid.Parse(reader["UserId"].ToString() ?? string.Empty),
                                        scheduleId = reader["scheduleId"].ToString() ?? string.Empty,
                                    });
                                } 
                            }
                        }
                    }

                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return cart;
            }
        }

        //GetbyId
        public Cart? GetById(Guid cartid)
        {
            Cart? cart = null;

            string query = $"SELECT * FROM cart WHERE cartId = @cartid";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@cartid", cartid);

                    connection.Open();


                    using (MySqlDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            cart = new Cart
                            {
                                cartId = Guid.Parse(reader["cartId"].ToString() ?? string.Empty),
                                courseId = reader["courseId"].ToString() ?? string.Empty,
                                categoryId = reader["categoryId"].ToString() ?? string.Empty,
                                userId = Guid.Parse(reader["UserId"].ToString() ?? string.Empty),
                                scheduleId = reader["scheduleId"].ToString() ?? string.Empty,

                            };
                        }
                    }
                }

                connection.Close();
            }

            return cart;
        }

        //getByUserId
        public List<Cart> GetByUserId(Guid userId)
        {
            List<Cart> cart = new List<Cart>(); // Initialize the list

            string query = "SELECT * FROM cart WHERE UserId = @UserId";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cart.Add(new Cart
                            {
                                cartId = Guid.Parse(reader["cartId"].ToString() ?? string.Empty),
                                courseId = reader["courseId"].ToString() ?? string.Empty,
                                categoryId = reader["categoryId"].ToString() ?? string.Empty,
                                userId = Guid.Parse(reader["UserId"].ToString() ?? string.Empty),
                                scheduleId = reader["scheduleId"].ToString() ?? string.Empty,
                            });
                        }
                    }
                }

                connection.Close();
            }
            return cart;
        }


        public int GetItemAmount(Guid userId)
        {
            int itemAmount = 0;

            string query = "SELECT COUNT(*) AS item_amount FROM cart WHERE UserId = @UserId AND invoiceId = NULL";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@UserId", userId);
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        itemAmount = Convert.ToInt32(result);
                    }

                    connection.Close();
                }
            }

            return itemAmount;
        }

        public int GetTotalPrice(Guid userId)
        {
            int totalPrice = 0;

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"SELECT SUM(c.quantity * co.course_price) AS TotalPrice
                FROM cart c
                INNER JOIN course co ON c.courseId = co.courseId
                WHERE c.UserId = @UserId AND c.invoiceId IS NULL";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        totalPrice = Convert.ToInt32(result);
                    }
                }
            }

            return totalPrice;
        }


        public bool Insert(Cart cart)
        {
            bool result = false;

            string query = $"INSERT INTO cart(cartId, courseId, categoryId, UserId, scheduleId)" + $"VALUES(@cartId, @courseId, @categoryId, @UserId, @scheduleId)";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@cartId", cart.cartId);
                    command.Parameters.AddWithValue("@courseId", cart.courseId);
                    command.Parameters.AddWithValue("@categoryId", cart.categoryId);
                    command.Parameters.AddWithValue("@UserId", cart.userId);
                    command.Parameters.AddWithValue("@scheduleId", cart.scheduleId);

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool UpdateIsSelected(Guid userId, bool isSelected)
        {
            bool result = false;

            string updateQuery = "UPDATE cart SET isSelected = @isSelected WHERE userId = @userId";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@isSelected", isSelected);
                    command.Parameters.AddWithValue("@userId", userId);

                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    result = rowsAffected > 0; // Set result based on update success

                    connection.Close();
                }
            }

            return result;
        }


        public bool Delete(string cartId)
        {
            bool result = false;

            string query = $"DELETE FROM cart WHERE cartId = '{cartId}'";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }
    }
}
