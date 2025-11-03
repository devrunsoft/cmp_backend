// CMPNatural.Application/Mapper/ManifestFlatMapper.cs
using AutoMapper;
using System;

namespace CMPNatural.Application.Mapper
{
    public class ManifestMapper
    {
        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                c.AddProfile<ManifestMapperProfile>();
            });
            return cfg.CreateMapper();
        });

        public static IMapper Mapper => Lazy.Value;
    }
}