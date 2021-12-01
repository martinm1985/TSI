using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MongoDB.Driver;
using Crud.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;

namespace Crud.Services
{
    public class EstadisticaCreadorService
    {
        private readonly IMongoCollection<EstadisticaCreador> _estCreador;

        public EstadisticaCreadorService(ICreadoresUYDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _estCreador = database.GetCollection<EstadisticaCreador>(settings.EstadisticasCreadorCollection);

        }

        public List<EstadisticaCreador> Get() =>
            _estCreador.Find(est => true).ToList();

        public List<EstadisticaCreador> Get(string id) =>
            _estCreador.Find(est => est.CreadorId == id).ToList();

        public EstadisticaCreador Create(EstadisticaCreador est)
        {
            _estCreador.InsertOne(est);
            return est;
        }

        public void Update(string id, EstadisticaCreador estIn) =>
            _estCreador.ReplaceOne(est => est.Id == id, estIn);

        public void Remove(EstadisticaCreador estIn) =>
            _estCreador.DeleteOne(est => est.Id == estIn.Id);

        public void Remove(string id) =>
            _estCreador.DeleteOne(est => est.Id == id);

        static private DateTime CurrentMonth()
        {
            var today = DateTime.Today;
            return new DateTime(today.Year, today.Month, 1);

        }
        public EstadisticaCreador GetCurrentEstadisticaCreador(string id)
        {

            return _estCreador
                    .Find(est => est.FechaMes == CurrentMonth() && est.CreadorId == id)
                    .FirstOrDefault();
        }

        public void AddSeguidor(string id)
        {
            var currentValue = GetCurrentEstadisticaCreador(id);
            if (currentValue != null)
            {
                var filter = Builders<EstadisticaCreador>.Filter.Where(est => est.FechaMes == currentValue.FechaMes && est.CreadorId == id);
                var update = Builders<EstadisticaCreador>.Update.Set(est => est.CantSeguidores, currentValue.CantSeguidores + 1);
                var options = new FindOneAndUpdateOptions<EstadisticaCreador>();
                _estCreador.FindOneAndUpdate(filter, update, options);
            }
            else
            {
                EstadisticaCreador estadisticaCreador = new()
                {
                    CreadorId = id,
                    FechaMes = CurrentMonth(),
                    CantSeguidores = 1,
                    CantVisitasPerfil = 0,
                    CantSuscripciones = 0,
                };
                Create(estadisticaCreador);
            }
        }
        public void SubSeguidor(string id)
        {
            var currentValue = GetCurrentEstadisticaCreador(id);
            if (currentValue != null)
            {
                var filter = Builders<EstadisticaCreador>.Filter.Where(est => est.FechaMes == currentValue.FechaMes && est.CreadorId == id);
                var update = Builders<EstadisticaCreador>.Update.Set(est => est.CantSeguidores, currentValue.CantSeguidores - 1);
                var options = new FindOneAndUpdateOptions<EstadisticaCreador>();
                _estCreador.FindOneAndUpdate(filter, update, options);
            }
        }

        public void AddSuscripcion(string id)
        {
            var currentValue = GetCurrentEstadisticaCreador(id);
            if (currentValue != null)
            {
                var filter = Builders<EstadisticaCreador>.Filter.Where(est => est.FechaMes == currentValue.FechaMes && est.CreadorId == id);
                var update = Builders<EstadisticaCreador>.Update.Set(est => est.CantSuscripciones, currentValue.CantSuscripciones + 1);
                var options = new FindOneAndUpdateOptions<EstadisticaCreador>();
                _estCreador.FindOneAndUpdate(filter, update, options);
            }
            else
            {
                EstadisticaCreador estadisticaCreador = new()
                {
                    CreadorId = id,
                    FechaMes = CurrentMonth(),
                    CantSeguidores = 0,
                    CantVisitasPerfil = 0,
                    CantSuscripciones = 1,
                };
                Create(estadisticaCreador);
            }
        }

        public void SubSuscripcion(string id)
        {
            var currentValue = GetCurrentEstadisticaCreador(id);
            if (currentValue != null)
            {
                var filter = Builders<EstadisticaCreador>.Filter.Where(est => est.FechaMes == currentValue.FechaMes && est.CreadorId == id);
                var update = Builders<EstadisticaCreador>.Update.Set(est => est.CantSuscripciones, currentValue.CantSuscripciones - 1);
                var options = new FindOneAndUpdateOptions<EstadisticaCreador>();
                _estCreador.FindOneAndUpdate(filter, update, options);
            }
        }

        public void AddVisitaPerfil(string id)
        {
            var currentValue = GetCurrentEstadisticaCreador(id);
            if (currentValue != null)
            {
                var filter = Builders<EstadisticaCreador>.Filter.Where(est => est.FechaMes == currentValue.FechaMes && est.CreadorId == id);
                var update = Builders<EstadisticaCreador>.Update.Set(est => est.CantVisitasPerfil, currentValue.CantVisitasPerfil + 1);
                var options = new FindOneAndUpdateOptions<EstadisticaCreador>();
                _estCreador.FindOneAndUpdate(filter, update, options);
            }
            else
            {
                EstadisticaCreador estadisticaCreador = new()
                {
                    CreadorId = id,
                    FechaMes = CurrentMonth(),
                    CantSeguidores = 0,
                    CantVisitasPerfil = 1,
                    CantSuscripciones = 0,
                };
                Create(estadisticaCreador);
            }
        }
    }
}
