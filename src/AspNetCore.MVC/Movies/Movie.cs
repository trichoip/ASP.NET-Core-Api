using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.MVC.Movies
{
    public class Movie
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Name Required", AllowEmptyStrings = true)]
        [Display(Name = "Title fuck")]
        [StringLength(8, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        //          Title fuck length must be between 6 and 8.
        public string Title { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Release Date 123")]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Genre { get; set; } = string.Empty;
        [Range(0, 999.99)]
        public decimal Price { get; set; }

        public bool isActive { get; set; }

        [ValidateNever]
        public string Email { get; set; }

        public List<string> colors { get; set; } = new List<string>() { "red", "blue" };
        public AddressTest Address { get; set; }

        public CountryEnum EnumCountry { get; set; }

    }

    public class AddressTest
    {
        public string nameAddress { get; set; }
    }

    public enum CountryEnum
    {
        [Display(Name = "United Mexican States")]
        Mexico,
        [Display(Name = "United States of America")]
        USA,
        Canada,
        France,
        Germany,
        Spain
    }
}
