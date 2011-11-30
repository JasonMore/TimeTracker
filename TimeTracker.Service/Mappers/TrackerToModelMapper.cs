using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Service.Models;
using TimeTracker.Service.ViewModels;
using AutoMapperAssist;

namespace TimeTracker.Service.Mappers
{
    public interface ITrackerToModelMapper : IMapper<TrackerViewModel, Tracker> { }

    public class TrackerToModelMapper : Mapper<TrackerViewModel, Tracker>, ITrackerToModelMapper
    {
        ITaskToModelMapper _taskToModelMapper;

        public TrackerToModelMapper(ITaskToModelMapper taskToModelMapper)
        {
            _taskToModelMapper = taskToModelMapper;
        }

        public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<TrackerViewModel, Tracker>()
                .ForMember(src => src.Id, opt => opt.Ignore())
                .ForMember(src => src.CurrentTask, opt => opt.Ignore())
                .ForMember(src => src.Tasks, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.CurrentTask = dest.CurrentTask ?? new Task();

                    if (src.currentTask.id == 0)
                    {
                        dest.CurrentTask = _taskToModelMapper.CreateInstance(src.currentTask);
                    }
                    else
                    {
                        _taskToModelMapper.LoadIntoInstance(src.currentTask, dest.CurrentTask);
                    }

                    foreach (var task in src.tasks)
                    {
                        _taskToModelMapper.LoadIntoInstance(task, dest.Tasks.SingleOrDefault(x => x.Id == task.id));
                    }
                })
                ;
        }
    }
}
