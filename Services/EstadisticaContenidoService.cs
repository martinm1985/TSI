using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Crud.Models;

namespace Crud.Services
{
    public class EstadisticaContenidoService
    {
        private readonly IMongoCollection<EstadisticaContenido> _estContenido;

        public EstadisticaContenidoService(ICreadoresUYDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _estContenido = database.GetCollection<EstadisticaContenido>(settings.EstadisticasContenidoCollection);
        }

        public List<EstadisticaContenido> Get() =>
            _estContenido.Find(est => true).ToList();

        public EstadisticaContenido Get(string id) =>
            _estContenido.Find(est => est.Id == id).FirstOrDefault();

        public EstadisticaContenido GetByIdContenido(string idContenido) =>
            _estContenido.Find(est => est.IdContenido == idContenido).FirstOrDefault();

        public EstadisticaContenido Create(EstadisticaContenido est)
        {
            _estContenido.InsertOne(est);
            return est;
        }

        public void Update(string id, EstadisticaContenido estIn) =>
            _estContenido.ReplaceOne(est => est.Id == id, estIn);

        public void Remove(EstadisticaContenido estIn) =>
            _estContenido.DeleteOne(est => est.Id == estIn.Id);

        public void Remove(string id) =>
            _estContenido.DeleteOne(est => est.Id == id);


        public void AddVisualizacion(string idContenido)
        {
            var currentValue = GetByIdContenido(idContenido);
            if (currentValue != null)
            {
                var filter = Builders<EstadisticaContenido>.Filter
                    .Where(est => est.Fecha == currentValue.Fecha);
                var update = Builders<EstadisticaContenido>.Update
                    .Set(est => est.Visualizaciones, currentValue.Visualizaciones + 1);
                var options = new FindOneAndUpdateOptions<EstadisticaContenido>();
                _estContenido.FindOneAndUpdate(filter, update, options);
            }
            else
            {
                EstadisticaContenido estadisticaContenido = new()
                {
                    IdContenido = idContenido,
                    Visualizaciones = 1,
                    Fecha = DateTime.Today,


                };
                Create(estadisticaContenido);
            }
        }

    }
}