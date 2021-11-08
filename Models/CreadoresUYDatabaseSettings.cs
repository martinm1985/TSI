namespace Crud.Models
{
    public class CreadoresUYDatabaseSettings : ICreadoresUYDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string EstadisticasContenidoCollection { get; set; }
        public string EstadisticasCreadorCollection { get; set; }
        public string EstadisticasPlataformaCollection { get; set; }
    }

    public interface ICreadoresUYDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string EstadisticasContenidoCollection { get; set; }
        string EstadisticasCreadorCollection { get; set; }
        string EstadisticasPlataformaCollection { get; set; }
    }
}
