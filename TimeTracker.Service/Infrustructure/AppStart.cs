using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using TimeTracker.Service.Storage.Context;
using Ninject.Extensions.Conventions;

namespace TimeTracker.Service.Infrustructure
{
    public static class AppStart
    {
        public static void RegisterAssemblies(IKernel kernel)
        {
            if (kernel == null)
                throw new ArgumentException("can not be null", "kernel");

            kernel.Bind<ITimeTrackerSession>().To<TimeTrackerSession>().InRequestScope();
            kernel.Bind<ITimeTrackerReadOnlySession>().To<TimeTrackerReadOnlySession>().InRequestScope();

            var scanner = new AssemblyScanner();
            scanner.WhereTypeIsNotInNamespace("TimeTracker.Service.Storage.Context");
            scanner.WhereTypeIsNotInNamespace("TimeTracker.Service.Mappers");
            scanner.FromCallingAssembly();
            scanner.BindWith<DefaultBindingGenerator>();
            kernel.Scan(scanner);

            //set mappers to singleton
            var mapScanner = new AssemblyScanner();
            mapScanner.FromCallingAssembly();
            mapScanner.WhereTypeIsInNamespace("TimeTracker.Service.Mappers");
            mapScanner.BindWith<DefaultBindingGenerator>();
            mapScanner.InSingletonScope();
            kernel.Scan(mapScanner);

        }
    }
}
