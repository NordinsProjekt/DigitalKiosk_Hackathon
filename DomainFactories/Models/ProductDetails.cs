using Entities.Enums;
using System;

public class ProductDetails(string name, string description, ShelfLocation shelfLocation, Section section, decimal price)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public ShelfLocation ShelfLocation { get; set; } = shelfLocation;
    public Section Section { get; set; } = section;
    public decimal Price { get; set; } = price;
}
