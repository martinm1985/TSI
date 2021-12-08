using System;
using System.Threading;
using Quartz;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz.Spi;

namespace Crud.Scheduler
{
    public class MyScheduler: IHostedService
    {
        public IScheduler Scheduler { get; set; }
        private readonly IJobFactory jobFactory;
        private readonly JobMetadata jobMetadata;
        private readonly ISchedulerFactory schedulerFactory;

        public MyScheduler(ISchedulerFactory _schedulerFactory, JobMetadata _jobMetadata, IJobFactory _jobFactory)
        {
            schedulerFactory = _schedulerFactory;
            jobMetadata = _jobMetadata;
            jobFactory = _jobFactory;

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //Creating Scheduler
            Scheduler = await schedulerFactory.GetScheduler();
            Scheduler.JobFactory = jobFactory;

            //Create Job
            IJobDetail jobDetail = CreateJob(jobMetadata);

            //Create trigger
            ITrigger trigger = CreateTrigger(jobMetadata);

            //Schedule Job
            await Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);

            //Start the Scheduler
            await Scheduler.Start(cancellationToken);
        
        }

        private ITrigger CreateTrigger(JobMetadata jobMetadata)
        {
            return TriggerBuilder.Create()
                .WithIdentity(jobMetadata.JobId.ToString())
                .WithCronSchedule(jobMetadata.CronExpression)
                .WithDescription(jobMetadata.JobName).Build();
        }

        private IJobDetail CreateJob(JobMetadata jobMetadata)
        {
            return JobBuilder.Create(jobMetadata.JobType)
                .WithIdentity(jobMetadata.JobId.ToString())
                .WithDescription(jobMetadata.JobName).Build();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
