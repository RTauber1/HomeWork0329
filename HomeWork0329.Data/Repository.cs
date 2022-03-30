using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork0329.Data
{
    public class Repository
    {
        private string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddPost(string title, string phoneNumber, string description, int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO SimpleBlog VALUES (@Title, @PhoneNumber, @Description, @userId)";
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@userId", id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            connection.Close();
            connection.Dispose();
        }
        public List<Posts> GetPosts()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM SimpleBlog";
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Posts> PostList = new List<Posts>();
            while (reader.Read())
            {
                PostList.Add(new Posts
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Description = (string)reader["Description"],
                    UserId=(int)reader["UserId"]
                });
            }
            connection.Close();
            connection.Dispose();
            return PostList;
        }
        public bool AddUser(string name, string email, string password)
        {
            if (IsEmailThereYet(email))
            {
                using var connection = new SqlConnection(_connectionString);
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO SimpleBlogPasswords (Name, Email, Password) " +
                    "VALUES (@name, @email, @password)";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(password));

                connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            return false;
        }
        private bool IsEmailThereYet(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM simpleBlogPasswords WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return true;
            }
            return false;
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isCorrect = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return isCorrect ? user : null;

        }

        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM simpleBlogPasswords WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                Email = (string)reader["Email"],
                Name = (string)reader["Name"],
                Password = (string)reader["Password"]
            };
        }
        public List<Posts> GetUsersPosts(string email)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT s.*, sbp.* FROM SimpleBlog s
                                JOIN simpleBlogPasswords sbp
                                ON s.UserId= sbp.Id
                                Where UserId = @userId";
            cmd.Parameters.AddWithValue("@userId", GetUsersId(email));
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Posts> PostList = new List<Posts>();
            while (reader.Read())
            {
                PostList.Add(new Posts
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Description = (string)reader["Description"],
                });
            }
            connection.Close();
            connection.Dispose();
            return PostList;
        }
        public int GetUsersId(string email)
        {
            int id=0;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"select * from simpleBlogPasswords where Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id = (int)reader["Id"];
            }
            connection.Close();
            connection.Dispose();
            return id;
        }
        public void Erase(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "delete from SimpleBlog where Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            connection.Close();
            connection.Dispose();
        }
    }

}
