using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTracker.Service.Models
{
    public class Tracker
    {
        public Tracker()
        {
            Tasks = new List<Task>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
        public virtual Task CurrentTask { get; set; }
    }
}
