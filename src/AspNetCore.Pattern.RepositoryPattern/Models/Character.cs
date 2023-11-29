namespace AspNetCore.Pattern.RepositoryPattern.Models
{
    public class Character
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public virtual Backpack Backpack { get; set; }

        public virtual List<Weapon> Weapons { get; set; }

        public virtual List<Faction> Factions { get; set; }
    }
}
