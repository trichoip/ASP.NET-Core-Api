﻿using System.Text.Json.Serialization;

namespace AspNetCore.Database.Mysql.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CharacterId { get; set; }
        [JsonIgnore]
        public Character Character { get; set; }
    }
}
