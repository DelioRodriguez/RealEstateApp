
namespace RealEstateApp.Application.Dtos.Agents
{
    public class AgentPropertiesDto
    {
        public int PropertyId { get; set; }
        public string? Code { get; set; }
        public string? PropertyType { get; set; }
        public string? SaleType { get; set; }
        public decimal Price { get; set; }
        public double Size { get; set; }
        public int Rooms { get; set; }
        public int Bathrooms { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public List<string>? Improvements { get; set; }
    }
}
