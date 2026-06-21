using System.Data.SqlClient;
using InterviewPrep.Models;

namespace InterviewPrep.Services;

public class OrderService
{
    private readonly string _connectionString;

    public OrderService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public decimal CalculateOrderTotal(int orderId)
    {
        var conn = new SqlConnection(_connectionString);
        var cmd = new SqlCommand($"SELECT * FROM OrderItems WHERE OrderId = {orderId}", conn);
        conn.Open();
        var reader = cmd.ExecuteReader();
        decimal total = 0;
        while (reader.Read())
        {
            total += (decimal)reader["Price"] * (int)reader["Quantity"];
        }
        return total;
    }

    public void ProcessOrder(int orderId)
    {
        var conn = new SqlConnection(_connectionString);
        conn.Open();

        var total = CalculateOrderTotal(orderId);
        
        var updateCmd = new SqlCommand($"UPDATE Orders SET TotalAmount = {total}, Status = 'Processed' WHERE Id = {orderId}", conn);
        updateCmd.ExecuteNonQuery();

        var insertCmd = new SqlCommand($"INSERT INTO OrderLogs (OrderId, Action, Timestamp) VALUES ({orderId}, 'Processed', GETDATE())", conn);
        insertCmd.ExecuteNonQuery();
    }

    public List<Order> GetUserOrders(int userId)
    {
        var orders = new List<Order>();
        var conn = new SqlConnection(_connectionString);
        var cmd = new SqlCommand($"SELECT * FROM Orders WHERE UserId = {userId}", conn);
        conn.Open();
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            orders.Add(new Order
            {
                Id = (int)reader["Id"],
                UserId = (int)reader["UserId"],
                TotalAmount = (decimal)reader["TotalAmount"],
                Status = (string)reader["Status"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            });
        }
        return orders;
    }
}
