using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Service.ViewModels;
using TimeTracker.Service.Models;
using AutoMapperAssist;

namespace TimeTracker.Service.Mappers
{
    public interface ITaskToViewMapper : IMapper<Task,TaskViewModel> { }

    public class TaskToViewMapper : Mapper<Task, TaskViewModel>, ITaskToViewMapper
    {

    }
}
