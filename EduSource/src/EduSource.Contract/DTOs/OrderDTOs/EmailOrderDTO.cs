namespace EduSource.Contract.DTOs.OrderDTOs;

public class EmailOrderDTO
{
    public EmailOrderDTO()
    {
    }

    public EmailOrderDTO(string name, int quantity, int price, int total)
    {
        Name = name;
        Quantity = quantity;
        Price = price;
        Total = total;
    }

    public string Name { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public int Total { get; set; }


}
