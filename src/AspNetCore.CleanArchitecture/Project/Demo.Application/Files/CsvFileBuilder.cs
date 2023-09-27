using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
    public byte[] BuildTodoItemsFile(IEnumerable<Player> records)
    {
        using var memoryStream = new MemoryStream();
        //using (var streamWriter = new StreamWriter(memoryStream))
        //{
        //    using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

        //    csvWriter.Configuration.RegisterClassMap<TodoItemRecordMap>();
        //    csvWriter.WriteRecords(records);
        //}

        return memoryStream.ToArray();
    }
}
