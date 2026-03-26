using Entities.Enums;
using System;

namespace Factories.Models
{
    public class ProductDetails(string name, string description, ShelfLocation shelfLocation, Section section, decimal price)
    {
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public ShelfLocation ShelfLocation { get; set; } = shelfLocation;
        public Section Section { get; set; } = section;
        public decimal Price { get; set; } = price;
    }
}