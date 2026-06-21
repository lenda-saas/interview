# PR: Feature - User Management System

## Description
This PR adds a complete User Management system including user registration, authentication, order processing, and user/order management endpoints. The implementation follows a straightforward service-based architecture with SQL Server data access.

## Changes Summary

### New Files
- `InterviewPrep/Models/User.cs` - User entity model
- `InterviewPrep/Models/Order.cs` - Order and OrderItem entity models
- `InterviewPrep/Services/UserService.cs` - User data access and authentication service
- `InterviewPrep/Services/OrderService.cs` - Order processing and data access service
- `InterviewPrep/Controllers/UsersController.cs` - REST API for user operations
- `InterviewPrep/Controllers/OrdersController.cs` - REST API for order operations

### Modified Files
- `InterviewPrep/Program.cs` - Registered new services and added controller support
- `InterviewPrep/appsettings.json` - Added SQL Server connection string
- `InterviewPrep/InterviewPrep.csproj` - Added System.Data.SqlClient package dependency

## Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/users` | Get all users |
| GET | `/api/users/{id}` | Get user by ID |
| POST | `/api/users` | Create a new user |
| POST | `/api/users/login` | User login |
| DELETE | `/api/users/{id}` | Delete a user |
| POST | `/api/orders/{orderId}/process` | Process an order |
| GET | `/api/orders/user/{userId}` | Get orders for a user |

## Notes for Reviewers
Please review the implementation and provide feedback on code quality, security, architecture, and any potential issues you can identify.
