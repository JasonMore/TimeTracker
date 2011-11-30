using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using TimeTracker.Service.Models;
using System.IO;
using System.Data.SqlClient;

namespace TimeTracker.Service.Storage.Context
{
    public class TimeTrackerInitializer : DropCreateDatabaseAlways<TimeTrackerContext>
    {
        protected override void Seed(TimeTrackerContext context)
        {
            base.Seed(context);

            //seed data goes here

            context.SaveChanges();
        }
    }
}
