using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Service.Models;
using TimeTracker.Service.Storage.Context;
using System.Linq.Expressions;
using TimeTracker.Service.Mappers;
using TimeTracker.Service.ViewModels;

namespace TimeTracker.Service.Services
{
    public interface ITrackerService
    {
        Tracker Save(TrackerViewModel tracker);
        TrackerViewModel GetTracker(string userName);

        IEnumerable<string> GetProjects(string userName);
    }

    public class TrackerService : ITrackerService
    {
        private ITimeTrackerSession _session;
        private ITrackerToViewMapper _trackerToViewMapper;
        private ITrackerToModelMapper _trackerToModelMapper;

        public TrackerService(ITimeTrackerSession session, ITrackerToViewMapper trackerToViewMapper, ITrackerToModelMapper trackerToModelMapper)
        {
            _session = session;
            _trackerToViewMapper = trackerToViewMapper;
            _trackerToModelMapper = trackerToModelMapper;
        }

        private Tracker Find(Expression<Func<Tracker, bool>> whereClause)
        {
            return _session.Single(whereClause);
        }

        public Tracker Save(TrackerViewModel trackerViewModel)
        {
            Tracker tracker = Find(x => x.Id == trackerViewModel.id) ?? new Tracker();

            _trackerToModelMapper.LoadIntoInstance(trackerViewModel, tracker);

            if (trackerViewModel.id == 0)
                _session.Add(tracker);

            _session.CommitChanges();

            if (!tracker.Tasks.Contains(tracker.CurrentTask))
            {
                tracker.Tasks.Add(tracker.CurrentTask);
                _session.CommitChanges();
            }

            return tracker;
        }

        public TrackerViewModel GetTracker(string userName)
        {
            return _trackerToViewMapper.CreateInstance(Find(x => x.UserName == userName));
        }


        public IEnumerable<string> GetProjects(string userName)
        {
            var tracker = Find(x => x.UserName == userName);
            if (tracker != null)
	        {
                return tracker.Tasks
                    .Where(x=>x.ProjectName != null)
                    .Select(x => x.ProjectName)
                    .Distinct()
                    .ToList();
	        }

            return new List<string>();
                    
        }
    }
}
