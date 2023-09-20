using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Api.Record
{
    public record Person(
      [Required] string Name,
      [Range(0, 150)] int Age);
}
