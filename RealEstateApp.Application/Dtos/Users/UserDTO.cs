﻿namespace RealEstateApp.Application.Dtos.Users;

public class UserDTO
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string UserName { get; set; }
    public string? ImagePath { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
}