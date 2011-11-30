using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Service.Storage.EntityFrameworkCodeFirst;
using TimeTracker.Service.Storage.InMemory;

namespace TimeTracker.Service.Storage.Context
{
    public interface ITimeTrackerSession : ISession { }

    public class TimeTrackerSession : EFCodeFirstSession, ITimeTrackerSession
    {
        public TimeTrackerSession()
            : base(new TimeTrackerContext())
        {

        }
    }

    public class TimeTrackerInMemorySession : InMemorySession, ITimeTrackerSession
    {
        public TimeTrackerInMemorySession()
        {

        }
    }

    public interface ITimeTrackerReadOnlySession : IReadOnlySession { }

    public class TimeTrackerReadOnlySession : EFCodeFirstReadOnlySession, ITimeTrackerReadOnlySession
    {
        public TimeTrackerReadOnlySession()
            : base(new TimeTrackerContext())
        {

        }
    }
}
