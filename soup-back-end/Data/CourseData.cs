using soup_back_end.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace soup_back_end.Data
{
    public class CourseData
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public CourseData(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        //getAll
        public List<Course> GetAll()
        {
            List<Course> courses = new List<Course>();
            string query = "SELECT * FROM course";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                courses.Add(new Course
                                {
                                    Id = reader["Id"].ToString() ?? string.Empty,
                                    categoryId = reader["categoryId"].ToString() ?? string.Empty,
                                    course_Name = reader["course_Name"].ToString() ?? string.Empty,
                                    Description = reader["Description"].ToString(),
                                    img = reader["img"].ToString() ?? string.Empty,
                                    course_price = reader["course_price"] == DBNull.Value ? 0 : Convert.ToInt32(reader["course_price"]),
                                    Created = Convert.ToDateTime(reader["Created"]),
                                    Updated = Convert.ToDateTime(reader["Updated"]),
                                });
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
            }
            return courses;
        }
        public Course? GetById(string id)
        {
            Course? course = null;

            string query = $"SELECT * FROM course WHERE Id = @Id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();


                    using (MySqlDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            course = new Course
                            {
                                Id = reader["Id"].ToString() ?? string.Empty,
                                categoryId = reader["categoryId"].ToString() ?? string.Empty,
                                course_Name = reader["course_Name"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString(),
                                img = reader["img"].ToString() ?? string.Empty,
                                course_price = reader["course_price"] == DBNull.Value ? 0 : Convert.ToInt32(reader["course_price"]),
                                Created = Convert.ToDateTime(reader["Created"]),
                                Updated = Convert.ToDateTime(reader["Updated"]),
                            };
                        }
                    }
                }

                connection.Close();
            }

            return course;
        }

        public List<Course> GetByCategoryId(string categoryId)
        {
            List<Course> courses = new List<Course>();

            string query = $"SELECT * FROM course WHERE categoryId = @categoryId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@categoryId", categoryId);

                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                courses.Add(new Course
                                {
                                    Id = reader["Id"].ToString() ?? string.Empty,
                                    categoryId = reader["categoryId"].ToString() ?? string.Empty,
                                    course_Name = reader["course_Name"].ToString() ?? string.Empty,
                                    Description = reader["Description"].ToString(),
                                    img = reader["img"].ToString() ?? string.Empty,
                                    course_price = reader["course_price"] == DBNull.Value ? 0 : Convert.ToInt32(reader["course_price"]),
                                    Created = Convert.ToDateTime(reader["Created"]),
                                    Updated = Convert.ToDateTime(reader["Updated"]),
                                });
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
            }
            return courses;
        }

        public Course? GetByName(string course_Name)
        {
            Course? course = null;

            string query = $"SELECT * FROM course WHERE course_Name = @course_Name";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@course_Name", course_Name);

                    connection.Open();


                    using (MySqlDataReader reader = command.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            course = new Course
                            {
                                Id = reader["Id"].ToString() ?? string.Empty,
                                categoryId = reader["categoryId"].ToString() ?? string.Empty,
                                course_Name = reader["course_Name"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString(),
                                img = reader["img"].ToString() ?? string.Empty,
                                course_price = reader["course_price"] == DBNull.Value ? 0 : Convert.ToInt32(reader["course_price"]),
                                Created = Convert.ToDateTime(reader["Created"]),
                                Updated = Convert.ToDateTime(reader["Updated"]),
                            };
                        }
                    }
                }

                connection.Close();
            }

            return course;
        }

        public bool Insert(Course course)
        {
            bool result = false;

            string created = course.Created.Date.ToString("yyyy-MM-dd HH:mm:ss");
            string updated = course.Updated.Date.ToString("yyyy-MM-dd HH:mm:ss");

            string query = $"INSERT INTO course(Id, categoryId, course_Name, Description, img, Created, Updated) " + $"VALUES(@Id, @categoryId, @course_Name, @Description, @img, @Created, @Updated)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Id", course.Id);
                    command.Parameters.AddWithValue("@categoryId", course.categoryId);
                    command.Parameters.AddWithValue("@course_Name", course.course_Name);
                    command.Parameters.AddWithValue("@Description", course.Description);
                    command.Parameters.AddWithValue("@img", course.img);
                    command.Parameters.AddWithValue("@course_price", course.course_price);
                    command.Parameters.AddWithValue("@Created", course.Created);
                    command.Parameters.AddWithValue("@Updated", course.Updated);

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;

        }

        public bool Update(string id, Course course)
        {
            bool result = false;

            string updated = course.Updated.Date.ToString("yyyy-MM-dd HH:mm:ss");

            string query = $"UPDATE course SET course_Name = @course_Name, Description = @Description, img = @img,  Updated = @Updated " + $"WHERE Id = @Id";


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Id", course.Id);
                    command.Parameters.AddWithValue("@categoryId", course.categoryId);
                    command.Parameters.AddWithValue("@course_Name", course.course_Name);
                    command.Parameters.AddWithValue("@Description", course.Description);
                    command.Parameters.AddWithValue("@img", course.img);
                    command.Parameters.AddWithValue("@course_price", course.course_price);
                    command.Parameters.AddWithValue("@Updated", updated);

                    connection.Open();

                    result = command.ExecuteNonQuery() > 0 ? true : false;

                    connection.Close();
                }
            }

            return result;
        }

        public bool Delete(string id)
        {
            bool result = false;

            string query = $"DELETE FROM course WHERE Id = '{id}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
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
