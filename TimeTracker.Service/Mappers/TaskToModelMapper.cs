using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Service.Models;
using TimeTracker.Service.ViewModels;
using AutoMapperAssist;

namespace TimeTracker.Service.Mappers
{
    public interface ITaskToModelMapper : IMapper<TaskViewModel, Task> { }

    public class TaskToModelMapper : Mapper<TaskViewModel, Task>, ITaskToModelMapper
    {
        public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<TaskViewModel, Task>()
                .ForMember(src => src.Id, opt => opt.Ignore())
                ;
        }
    }
}
