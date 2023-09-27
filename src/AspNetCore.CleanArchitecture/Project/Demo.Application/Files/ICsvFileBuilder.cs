using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Files;

public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<Player> records);
}
