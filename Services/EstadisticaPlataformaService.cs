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
    public class EstadisticaPlataformaService
    {
        private readonly IMongoCollection<EstadisticaPlataforma> _estPlataforma;

        public EstadisticaPlataformaService(ICreadoresUYDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _estPlataforma = database.GetCollection<EstadisticaPlataforma>(settings.EstadisticasPlataformaCollection);

        }

        public List<EstadisticaPlataforma> Get() =>
            _estPlataforma.Find(est => true).ToList();

        public EstadisticaPlataforma Get(string id) =>
            _estPlataforma.Find(est => est.Id == id).FirstOrDefault();

        public EstadisticaPlataforma Create(EstadisticaPlataforma est)
        {
            _estPlataforma.InsertOne(est);
            return est;
        }

        public void Update(string id, EstadisticaPlataforma estIn) =>
            _estPlataforma.ReplaceOne(est => est.Id == id, estIn);

        public void Remove(EstadisticaPlataforma estIn) =>
            _estPlataforma.DeleteOne(est => est.Id == estIn.Id);

        public void Remove(string id) =>
            _estPlataforma.DeleteOne(est => est.Id == id);

        static private DateTime CurrentMonth()
        {
            var today = DateTime.Today;
            return new DateTime(today.Year, today.Month, 1);

        }
        public EstadisticaPlataforma GetCurrentEstadisticaPlataforma()
        {

            return _estPlataforma.Find(est => est.FechaMes == CurrentMonth()).FirstOrDefault();
        }

        public void AddSeguidor()
        {
            var currentValue = GetCurrentEstadisticaPlataforma();
            if (currentValue != null)
            {
                var filter = Builders<EstadisticaPlataforma>.Filter.Where(est => est.FechaMes == currentValue.FechaMes);
                var update = Builders<EstadisticaPlataforma>.Update.Set(est => est.CantSeguidores, currentValue.CantSeguidores + 1);
                var options = new FindOneAndUpdateOptions<EstadisticaPlataforma>();
                _estPlataforma.FindOneAndUpdate(filter, update, options);
            }
            else
            {
                EstadisticaPlataforma estadisticaPlataforma = new()
                {
                    FechaMes = CurrentMonth(),
                    CantSeguidores = 1,
                    CantVisitasPerfil = 0,
                    CantSuscripciones = 0,
                };
                Create(estadisticaPlataforma);
            }
        }

        public void AddSuscripcion()
        {
            var currentValue = GetCurrentEstadisticaPlataforma();
            if (currentValue != null)
            {
                var filter = Builders<EstadisticaPlataforma>.Filter.Where(est => est.FechaMes == currentValue.FechaMes);
                var update = Builders<EstadisticaPlataforma>.Update.Set(est => est.CantSuscripciones, currentValue.CantSuscripciones + 1);
                var options = new FindOneAndUpdateOptions<EstadisticaPlataforma>();
                _estPlataforma.FindOneAndUpdate(filter, update, options);
            }
            else
            {
                EstadisticaPlataforma estadisticaPlataforma = new()
                {
                    FechaMes = CurrentMonth(),
                    CantSeguidores = 0,
                    CantVisitasPerfil = 0,
                    CantSuscripciones = 1,
                };
                Create(estadisticaPlataforma);
            }
        }

        public void AddVisitaPerfil()
        {
            var currentValue = GetCurrentEstadisticaPlataforma();
            if (currentValue != null)
            {
                var filter = Builders<EstadisticaPlataforma>.Filter.Where(est => est.FechaMes == currentValue.FechaMes);
                var update = Builders<EstadisticaPlataforma>.Update.Set(est => est.CantVisitasPerfil, currentValue.CantVisitasPerfil + 1);
                var options = new FindOneAndUpdateOptions<EstadisticaPlataforma>();
                _estPlataforma.FindOneAndUpdate(filter, update, options);
            }
            else
            {
                EstadisticaPlataforma estadisticaPlataforma = new()
                {
                    FechaMes = CurrentMonth(),
                    CantSeguidores = 0,
                    CantVisitasPerfil = 1,
                    CantSuscripciones = 0,
                };
                Create(estadisticaPlataforma);
            }
        }
    }
}
