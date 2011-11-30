using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TimeTracker.Controllers;
using TimeTracker.Service.Services;
using NSubstitute;

namespace TimeTracker.Web.Tests
{
    [TestFixture]
    public class TrackerTests
    {
        private HomeController _home;
        private ITrackerService _trackerService;

        [SetUp]
        public void Setup()
        {
            _trackerService = Substitute.For<ITrackerService>();    
            _home = new HomeController(_trackerService);
        }

        [TestCase]
        public void NewSave()
        {
            //Arrange
            string jsonString = "{\"id\":0,\"timeButtonText\":\"stop\",\"projects\":[],\"currentTask\":{\"id\":0,\"name\":\"\",\"taskDate\":{\"_orient\":1,\"_is\":false},\"startTime\":1322069359562,\"elapsedTime\":1546,\"timerRunning\":true,\"endTime\":1322069361108,\"seconds\":1,\"minutes\":0,\"hours\":0,\"projectNames\":[],\"elapsedTimeText\":\"01 sec\",\"startText\":\"11:29 AM\",\"endText\":\"11:29 AM\",\"date\":\"Wed Nov 23 2011 11:29:19\"},\"projectNames\":[],\"elapsedTimeText\":\"01 sec\",\"taskName\":\"\"}";

            //Act
            var tracker = _home.convert(jsonString);

            //Assert
            Assert.That(tracker, Is.Not.Null);
            Assert.That(tracker.tasks, Is.Not.Null);
            Assert.That(tracker.tasks.Count(), Is.EqualTo(0));
        }
    }
}
