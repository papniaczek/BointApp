using System;
using System.Collections.Generic;

namespace BointApp.Models;

public class User
{
    public Guid UserID { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public List<Rental> RentalHistory { get; set; } = new();

    public User(string name, string surname, string email, UserRole role = UserRole.User)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(surname)) throw new ArgumentException("Surname cannot be null or whitespace.");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or whitespace.");
        
        Name = name;
        Surname = surname;
        Email = email;
        Role = role;
    }
    
    public string FullName => $"{Name} {Surname}";
}