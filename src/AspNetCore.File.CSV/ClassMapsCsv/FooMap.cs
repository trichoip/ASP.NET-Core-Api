using AspNetCore.File.CSV.Models;
using CsvHelper.Configuration;

namespace AspNetCore.File.CSV.ClassMapsCsv;

public class FooMap : ClassMap<Foo>
{
    public FooMap()
    {
        Map(m => m.Id).Index(0).Name("id");
        Map(m => m.Name).Index(2).Name("name");
        Map(m => m.Licen).Index(1).Name("licen");
    }
}
