/// <reference path="jquery-1.6.2-vsdoc.js"/>
/// <reference path="knockout-1.3.0beta.debug.js"/>
/// <reference path="knockout.mapping-latest.debug.js"/>
/// <reference path="jquery.validate-vsdoc.js"/>

var TimeTracker = {};
(function ($) {

    //setup timePicker
    ko.bindingHandlers.datepicker = {
        init: function(element, valueAccessor, allBindingsAccessor) {
            //initialize datepicker with some optional options
            var options = allBindingsAccessor().datepickerOptions || {};
            $(element).datepicker(options);

            //handle the field changing
            ko.utils.registerEventHandler(element, "change", function () {
                var observable = valueAccessor();
                observable($(element).datepicker("getDate"));
            });

            //handle disposal (if KO removes by the template binding)
            ko.utils.domNodeDisposal.addDisposeCallback(element, function() {
                $(element).datepicker("destroy");
            });

        },
        update: function(element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            $(element).datepicker("setDate", value);        
       } 
    };


    var projectNames = ko.observableArray();

    var task = function (name) {
        //private members
        var thisTask = this;
        var timeOut;
        var speed = 250;

        //private methods
        var zeroPad =  function (num, places) {
            var zero = places - num.toString().length + 1;
            return Array(+(zero > 0 && zero)).join("0") + num;
        }

        var timerLoop = function()
		{
			thisTask.elapsedTime(new Date().getTime() - thisTask.startTime());
			timeOut = setTimeout(function() { timerLoop(); }, speed);
		}

        //public properties
        this.id = ko.observable(0);
        this.name = ko.observable(name);
        this.projectName = ko.observable('');
        this.taskDate = ko.observable();
        this.startTime = ko.observable(0);
        this.elapsedTime = ko.observable(0);
        this.timerRunning = ko.observable(false);
        
        //dependant observables

        this.seconds = ko.dependentObservable(function() {
            return Math.floor((this.elapsedTime()/1000)%60);
        },this);

        this.minutes = ko.dependentObservable(function() {
            return Math.floor((this.elapsedTime()/(1000*60))%60);
        },this);

        this.hours = ko.dependentObservable(function() {
            return Math.floor((this.elapsedTime()/(1000*60*60))%24);
        },this);

        this.projectNames = ko.dependentObservable(function() {
            return projectNames();
        },this);

        this.elapsedTimeText = ko.dependentObservable(function() {
            var seconds = this.seconds();
            var minutes = this.minutes();
            var hours = this.hours();

            if (hours == 0) {
                if (minutes == 0) {
                    return $.format('{0} sec',zeroPad(seconds,2));
                }
                return $.format('{0}:{1} min', zeroPad(minutes,2), zeroPad(seconds,2));
            }
            return $.format('{0}:{1}:{2} hour', zeroPad(hours,2), zeroPad(minutes,2), zeroPad(seconds,2));

        }, this);

        this.startText = ko.dependentObservable({
            read: function() {
                return new Date(this.startTime()).format('shortTime');
            },
            write: function(value) {
                this.startTime(Date.parse(value).getTime());
            },
            owner: this
        });

        this.endText = ko.dependentObservable({
            read: function() {
                return new Date(this.startTime() + this.elapsedTime()).format('shortTime');
            },
            write: function(value) {
                this.elapsedTime(Date.parse(value) - this.startTime());
            },
            owner: this
        });

        this.date = ko.dependentObservable(function() {
            return this.taskDate() == null ? '' : this.taskDate().format();
        }, this);

        //public methods
        this.startTimer = function() {
            this.startTime(new Date().getTime());
            this.taskDate(new Date());
            timerLoop();
            this.timerRunning(true);
        }

        this.stopTimer = function() {
            clearTimeout(timeOut);
            this.timerRunning(false);
        }

        this.checkIfLoaded = function() {
            if(this.timerRunning() && timeOut == null) {
                timerLoop();
            }
        }

    }
    
    var viewModel = function(){
        //local variables
        var thisViewModel = this;
        var defaultTaskName = 'My Task';

        //properties
        this.id = ko.observable(0);
        this.newProjectName = ko.observable();
        this.tasks = ko.observableArray();
        this.currentTask = ko.observable(new task(defaultTaskName));

        //dependant observables
        this.timeButtonText = ko.dependentObservable(function() {
            return this.currentTask().timerRunning() ? 'stop' : 'start';
        },this);

        this.projectNames = ko.dependentObservable(function() {
            return projectNames();
        });

        this.projectName = ko.dependentObservable({
            read:function() {
                return this.currentTask().projectName();
            },
            write:function(value) {
                this.currentTask().projectName(value);
            },
            owner: this
        });

        this.elapsedTimeText = ko.dependentObservable(function() {
            return this.currentTask().elapsedTimeText();
        },this);

        this.taskName = ko.dependentObservable({
            read:function() {
                return this.currentTask().name();
            },
            write:function(value) {
                this.currentTask().name(value);
            },
            owner: this
        });

        this.taskDate = ko.dependentObservable({
            read:function() {
                return this.currentTask().taskDate();
            },
            write:function(value) {
                this.currentTask().taskDate(value);
            },
            owner: this
        });

        this.buttonStyle = ko.dependentObservable(function() {
            return this.currentTask().timerRunning() ? 'timeButtonRunning' : 'timeButtonStopped';
        },this);

        //methods
        this.taskNameClicked = function(e) {
            if (this.taskName() == defaultTaskName) {
                this.taskName('');
            }
        }
        
        this.taskNameBlur = function(e) {
            if(this.taskName() == '') {
                this.taskName(defaultTaskName);
            }
        }

        this.timeButtonClicked = function(e) {
            if(this.currentTask().timerRunning()) {
                this.tasks.unshift(this.currentTask());
                this.currentTask().stopTimer();
                this.currentTask(new task(defaultTaskName));
            } else {
                this.currentTask().startTimer();
            }
            saveModel();
        }

        var setupAddNewProject = function(){

        }

        this.addNewProject = function(e) {
            this.newProjectName('');

            var window = $("#addProjectWindow");
            var windowControl = window.data("tWindow");

            windowControl.center();
            windowControl.open();

            var add = function() {
                windowControl.close();
                if (projectNames.indexOf(thisViewModel.newProjectName()) == -1) {
                    projectNames.push(thisViewModel.newProjectName());
                }
                thisViewModel.projectName(thisViewModel.newProjectName());
            }

            window.find('input:first').focus();

            window.find('a.add').click(function(e) { 
                e.preventDefault();
                add();
            });

            window.find('a.cancel').click(function(e) {
                e.preventDefault(); 
                windowControl.close();
            });
        }
    }

    var currentViewModel;
    var options;
    var saveModel = function() {
        $.ajax({ 
            url: options.saveUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ tracker: ko.toJSON(currentViewModel) }),
            success: function (data, textStatus, jqXHR) {
                currentViewModel.id(data.trackerId);
                currentViewModel.currentTask().id(data.currentTaskId);
            }
        });
    }
    
    var updateModel = function(model, data){
        model.id(data.id);
        model.name(data.name);
        model.projectName(data.projectName);
        model.startTime(data.startTime);
        model.elapsedTime(data.elapsedTime);
        model.timerRunning(data.timerRunning);

        if (data.date != null) {
            model.taskDate(new Date(parseInt(data.date.substr(6))));
        }
    }

    var setModel = function() {
        $.ajax({ 
            url: options.getProjectsUrl,
            type: 'POST',
            contentType: 'application/json',
            success: function (data, textStatus, jqXHR) {
                    projectNames = ko.observableArray(data);
            }
        });
        
        $.ajax({ 
            url: options.getUrl,
            type: 'POST',
            contentType: 'application/json',
            success: function (data, textStatus, jqXHR) {
                currentViewModel = new viewModel();

                if (data.tracker != 'null') {
                    var saved = $.parseJSON(data.tracker);

                    currentViewModel.id(saved.id);
                    updateModel(currentViewModel.currentTask(),saved.currentTask);
                    currentViewModel.currentTask().checkIfLoaded();

                    //skip first saved task
                    for (var i = 0; i < saved.tasks.length - 1; i++) {
                        currentViewModel.tasks.push(new task(saved.currentTask.name));
                        updateModel(currentViewModel.tasks()[i],saved.tasks[i + 1]);
                    }

                    currentViewModel.tasks.reverse() 
                }

                ko.applyBindings(currentViewModel);
            }
        });
    }

    this.init = function(opt) {
        options = opt;
        
        setModel();
        

//        $('input, select').live('blur',function(e) {
//            saveModel();
//        });
    }

}).apply(TimeTracker, [jQuery]);
