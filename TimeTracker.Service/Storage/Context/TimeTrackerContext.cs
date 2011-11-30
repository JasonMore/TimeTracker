using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using TimeTracker.Service.Models;

namespace TimeTracker.Service.Storage.Context
{
    public class TimeTrackerContext : DbContext
    {
        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
