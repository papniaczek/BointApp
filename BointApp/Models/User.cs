using System;
using System.Collections.Generic;

namespace BointApp.Models;

public class User
{
    public Guid UserID { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; set; }
    public List<Rental> RentalHistory { get; set; } = new();

    public User(string name, string surname, string email, string password, UserRole role = UserRole.User)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(surname)) throw new ArgumentException("Surname cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password cannot be null or whitespace.");
        
        Name = name;
        Surname = surname;
        Email = email;
        Role = role;
        PasswordHash = HashPassword(password); 
    }
    
    public static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
    
    public bool VerifyPassword(string password)
    {
        return PasswordHash == HashPassword(password);
    }
    
    public string FullName => $"{Name} {Surname}";
}