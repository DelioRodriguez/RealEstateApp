namespace RealEstateApp.Application.Dtos.Agents;

public class AgentViewDto
{
    public string? AgentId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int Properties { get; set; }
    public bool IsActive { get; set; }
}