using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTracker.Service.ViewModels
{
    public class TaskViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string projectName { get; set; }
        public Int64 elapsedTime { get; set; }
        public Int64 startTime { get; set; }
        public Int64 endTime { get; set; }
        public DateTime? date { get; set; }
        public bool timerRunning { get; set; }
    }
}
