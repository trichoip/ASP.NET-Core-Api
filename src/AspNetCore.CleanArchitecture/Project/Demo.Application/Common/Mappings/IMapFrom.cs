using AutoMapper;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType()).ReverseMap();
}
