using System;
using Quartz;
using Quartz.Spi;
namespace Crud.Scheduler
{
    public class JobFactory: IJobFactory
    {
        private readonly IServiceProvider service;
        public JobFactory(IServiceProvider _service)
        {
            service = _service;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetaill = bundle.JobDetail;
            return (IJob)service.GetService(jobDetaill.JobType);
        }

        public void ReturnJob(IJob job)
        {
            
        }
    }
}
