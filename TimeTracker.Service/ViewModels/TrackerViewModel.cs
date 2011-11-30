using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTracker.Service.ViewModels
{
    public class TrackerViewModel
    {
        public int id { get; set; }
        public string userName { get; set; }

        public IEnumerable<TaskViewModel> tasks { get; set; }
        public TaskViewModel currentTask { get; set; }
    }
}
