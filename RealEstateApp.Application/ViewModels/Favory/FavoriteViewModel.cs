namespace RealEstateApp.Application.ViewModels.Favory;

public class FavoriteViewModel
{
    public int Id { get; set; }

    public string UserId { get; set; }
    public int PropertyId { get; set; }
    public DateTime AddedOn { get; set; } = DateTime.UtcNow;
}