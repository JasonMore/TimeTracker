using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Service.ViewModels;
using TimeTracker.Service.Models;
using AutoMapperAssist;

namespace TimeTracker.Service.Mappers
{
    public interface ITrackerToViewMapper : IMapper<Tracker,TrackerViewModel> { }

    public class TrackerToViewMapper : Mapper<Tracker, TrackerViewModel>, ITrackerToViewMapper
    {
        private ITaskToViewMapper _taskToViewMapper;

        public TrackerToViewMapper(ITaskToViewMapper taskToViewMapper)
        {
            _taskToViewMapper = taskToViewMapper;
        }

        public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Tracker, TrackerViewModel>()
                .ForMember(src => src.currentTask, opt => opt.Ignore())
                .ForMember(src => src.tasks, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                    {
                        if (src == null)
                        {
                            return;
                        }
                        dest.currentTask = _taskToViewMapper.CreateInstance(src.CurrentTask);
                        dest.tasks = _taskToViewMapper.CreateSet(src.Tasks);
                    })
                ;
        }
    }
}
