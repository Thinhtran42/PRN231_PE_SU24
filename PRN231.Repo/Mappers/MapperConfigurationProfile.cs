using AutoMapper;
using PRN231.Repo.Models;
using PRN231.Repo.ViewModels;

namespace PRN231.Repo.Mappers;

public class MapperConfigurationProfile
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<StudentViewModel, Student>().ReverseMap();
            CreateMap<Student, StudentDetailViewModel>();
        }
    }
}