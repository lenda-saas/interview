using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using InterviewPrep.Models;

namespace InterviewPrep.Services;

public class UserService
{
    private readonly string _connectionString;

    public UserService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public User GetUser(int id)
    {
        var conn = new SqlConnection(_connectionString);
        var cmd = new SqlCommand($"SELECT * FROM Users WHERE Id = {id}", conn);
        conn.Open();
        var reader = cmd.ExecuteReader();
        var user = new User();
        while (reader.Read())
        {
            user.Id = (int)reader["Id"];
            user.Username = (string)reader["Username"];
            user.Password = (string)reader["Password"];
            user.Email = (string)reader["Email"];
            user.Role = (string)reader["Role"];
            user.IsActive = (bool)reader["IsActive"];
            user.CreatedAt = (DateTime)reader["CreatedAt"];
            if (reader["LastLogin"] != DBNull.Value)
                user.LastLogin = (DateTime)reader["LastLogin"];
        }
        return user;
    }

    public List<User> GetAllUsers()
    {
        var users = new List<User>();
        var conn = new SqlConnection(_connectionString);
        var cmd = new SqlCommand("SELECT * FROM Users", conn);
        conn.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User
            {
                Id = (int)reader["Id"],
                Username = (string)reader["Username"],
                Password = (string)reader["Password"],
                Email = (string)reader["Email"],
                Role = (string)reader["Role"],
                IsActive = (bool)reader["IsActive"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            });
        }
        return users;
    }

    public void CreateUser(User user)
    {
        var hash = HashPassword(user.Password);
        var conn = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(
            $"INSERT INTO Users (Username, Password, Email, Role, IsActive, CreatedAt) VALUES ('{user.Username}', '{hash}', '{user.Email}', '{user.Role}', 1, GETDATE())",
            conn);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public bool Login(string username, string password)
    {
        var conn = new SqlConnection(_connectionString);
        var cmd = new SqlCommand($"SELECT Password FROM Users WHERE Username = '{username}'", conn);
        conn.Open();
        var result = cmd.ExecuteScalar();
        if (result == null) return false;
        var storedHash = result.ToString();
        return VerifyPassword(password, storedHash);
    }

    private string HashPassword(string password)
    {
        using var md5 = MD5.Create();
        var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        var sb = new StringBuilder();
        foreach (var b in bytes)
            sb.Append(b.ToString("x2"));
        return sb.ToString();
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        return HashPassword(password) == storedHash;
    }

    public void DeleteUser(int id)
    {
        var conn = new SqlConnection(_connectionString);
        var cmd = new SqlCommand($"DELETE FROM Users WHERE Id = {id}", conn);
        conn.Open();
        cmd.ExecuteNonQuery();
    }
}
