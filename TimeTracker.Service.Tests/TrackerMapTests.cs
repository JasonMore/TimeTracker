using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TimeTracker.Service.Mappers;
using TimeTracker.Service.Models;
using TimeTracker.Service.ViewModels;

namespace TimeTracker.Service.Tests
{
    [TestFixture]
    public class TrackerMapTests
    {
        TrackerToModelMapper _trackerToModelMapper;
        TrackerToViewMapper _trackerToViewMapper;
        TaskToModelMapper _taskToModelMapper;
        TaskToViewMapper _taskToViewMapper;

        [SetUp]
        public void Setup()
        {
            _taskToModelMapper = new TaskToModelMapper();
            _taskToViewMapper = new TaskToViewMapper();
            _trackerToModelMapper = new TrackerToModelMapper(_taskToModelMapper);
            _trackerToViewMapper = new TrackerToViewMapper(_taskToViewMapper);
        }

        private TaskViewModel GetTestTaskViewModel()
        {
            return new TaskViewModel()
            {
                id = 123,
                name = "foo",
                projectName = "bar",
                elapsedTime = Convert.ToInt64(TimeSpan.FromMinutes(20).TotalMilliseconds),
                startTime = DateTime.Now.Ticks,
                endTime = DateTime.Now.AddMinutes(20).Ticks,
                date = DateTime.Now,
                timerRunning = true
            };
        }

        [TestCase]
        public void TaskViewToModel()
        {
            //Arrange
            TaskViewModel taskViewModel = GetTestTaskViewModel();
            Task dbTask = new Task();

            //Act
            _taskToModelMapper.LoadIntoInstance(taskViewModel, dbTask);

            //Assert
            Assert.That(dbTask.Id, Is.EqualTo(taskViewModel.id));
            Assert.That(dbTask.Name, Is.EqualTo(taskViewModel.name));
            Assert.That(dbTask.ProjectName, Is.EqualTo(taskViewModel.projectName));
            Assert.That(dbTask.ElapsedTime, Is.EqualTo(taskViewModel.elapsedTime));
            Assert.That(dbTask.StartTime, Is.EqualTo(taskViewModel.startTime));
            Assert.That(dbTask.EndTime, Is.EqualTo(taskViewModel.endTime));
            Assert.That(dbTask.Date, Is.EqualTo(taskViewModel.date));
            Assert.That(dbTask.TimerRunning, Is.EqualTo(taskViewModel.timerRunning));
        }

        [TestCase]
        public void TrackerViewToModel()
        {
            //Arrange
            TrackerViewModel trackerViewModel = new TrackerViewModel()
            {
                id = 456,
                userName = "jmore",
                currentTask = GetTestTaskViewModel(),
                tasks = new List<TaskViewModel> { GetTestTaskViewModel() }
            };
            Tracker dbTracker = new Tracker();

            //Act
            _trackerToModelMapper.LoadIntoInstance(trackerViewModel, dbTracker);

            //Assert
            Assert.That(dbTracker.Id,Is.EqualTo(trackerViewModel.id));
            Assert.That(dbTracker.UserName,Is.EqualTo(trackerViewModel.userName));
            Assert.That(dbTracker.CurrentTask,Is.Not.Null);
            Assert.That(dbTracker.CurrentTask.Name,Is.EqualTo(trackerViewModel.currentTask.name));
            Assert.That(dbTracker.Tasks,Is.Not.Null);
            Assert.That(dbTracker.Tasks.Count,Is.EqualTo(1));
            Assert.That(dbTracker.Tasks.First().Name,Is.EqualTo(trackerViewModel.tasks.First().name));
        }
    }
}
