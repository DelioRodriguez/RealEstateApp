using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Application.ViewModels.Users;

namespace RealEstateApp.Application.ViewModels.Agents;

public class PropertyByAgentViewModel
{
    public AgentViewModel? Agent { get; set; }
    public List<PropertyListViewModel?>? Properties { get; set; }
}