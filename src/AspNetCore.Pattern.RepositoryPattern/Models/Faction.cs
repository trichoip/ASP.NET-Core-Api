﻿using System.Text.Json.Serialization;

namespace AspNetCore.Pattern.RepositoryPattern.Models;

public class Faction
{
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public virtual List<Character> Characters { get; set; }
}
