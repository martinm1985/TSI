using System;
namespace Crud.Models
{
    public class MediosCreados
    {
        public MediosCreados(string fechaCreacionI, int cantidadI)
        {
            fechaCreacion = fechaCreacionI;
            cantidad = cantidadI;
        }

        public string fechaCreacion { get; set; }
        public int cantidad { get; set; }

    }
}
