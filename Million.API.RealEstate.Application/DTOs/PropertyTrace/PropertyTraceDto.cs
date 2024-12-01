namespace Million.API.RealEstate.Application.DTOs.PropertyTrace
{
    public class PropertyTraceDto
    {
        public string? Id { get; set; } // ID único del registro de trazabilidad
        public string IdProperty { get; set; } // Relación con el ID de la propiedad
        public DateTime DateSale { get; set; } // Fecha de venta
        public string Name { get; set; } // Nombre del registro
        public decimal Value { get; set; } // Valor de la transacción
        public decimal Tax { get; set; } // Impuesto asociado
    }
}