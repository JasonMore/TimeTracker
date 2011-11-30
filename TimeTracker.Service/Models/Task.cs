using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTracker.Service.Models
{
    public class Task
    {
        public int Id { get; set; }

        //public Tracker Tracker { get; set; }

        public string Name { get; set; }
        public string ProjectName { get; set; }
        public Int64 ElapsedTime { get; set; }
        public Int64 StartTime { get; set; }
        public Int64 EndTime { get; set; }
        public DateTime? Date { get; set; }
        public bool TimerRunning { get; set; }
    }
}
