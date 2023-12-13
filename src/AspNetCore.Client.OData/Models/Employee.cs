namespace AspNetCore.Client.OData.Models;

public class Employee
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public decimal? Salary { get; set; }
    public string? JobRole { get; set; }

    public List<Weapon> Weapons { get; set; }
    public List<Faction> Factions { get; set; }
    public Backpack Backpack { get; set; }
}
