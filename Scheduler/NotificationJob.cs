using System;
using System.Threading.Tasks;
using Crud.Data;
using Crud.Services;
using Microsoft.Extensions.Logging;
using Quartz;
namespace Crud.Scheduler
{
    public class NotificationJob : IJob
    {
        private readonly ILogger<NotificationJob> _logger;
        private readonly IPagoService _pagosService;
        private readonly IServiceProvider _serviceProvider;

        public NotificationJob(ILogger<NotificationJob> logger,IServiceProvider serviceProvider, IPagoService pagosService)
        {
            _logger = logger;
            _pagosService = pagosService;
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _pagosService.ActualizacionTarjetas();
            await _pagosService.ActualizacionSuscripciones();
            await _pagosService.CobroDePagos();
            await _pagosService.SuscripcionesNoPagas();
            _logger.LogInformation($"Notify User at {DateTime.Now} and Jobtype: {context.JobDetail.JobType}");
        }
    }
}
