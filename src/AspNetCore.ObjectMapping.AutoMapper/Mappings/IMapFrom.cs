﻿using AutoMapper;

namespace AspNetCore.ObjectMapping.AutoMapper.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType()).ReverseMap();
}
