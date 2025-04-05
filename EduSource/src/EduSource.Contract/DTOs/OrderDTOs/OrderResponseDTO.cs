using EduSource.Contract.Enumarations.Authentication;

namespace EduSource.Contract.DTOs.OrderDTOs;

public static class OrderResponseDTO
{
    public class AccountResponseDTO
    {
        public AccountResponseDTO(Guid id, string firstName, string lastName, string email, string? cropAvatarUrl, GenderType genderType)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CropAvatarUrl = cropAvatarUrl;
            GenderType = genderType;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? CropAvatarUrl { get; set; }
        public GenderType GenderType { get; set; }
    }
    public class OrderDetailsResponseDTO
    {
        public OrderDetailsResponseDTO(Guid id, string productName, int price, int quantity)
        {
            Id = id;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
