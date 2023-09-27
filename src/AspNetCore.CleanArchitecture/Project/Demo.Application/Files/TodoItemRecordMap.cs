using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;
using CsvHelper.Configuration;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Files;

public class TodoItemRecordMap : ClassMap<Player>
{
    public TodoItemRecordMap()
    {
        //AutoMap(CultureInfo.InvariantCulture);

        //Map(m => m.Done).ConvertUsing(c => c.Done ? "Yes" : "No");
    }
}
