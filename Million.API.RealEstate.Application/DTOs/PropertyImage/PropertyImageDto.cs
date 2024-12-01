namespace Million.API.RealEstate.Application.DTOs.PropertyImage
{
    public class PropertyImageDto
    {
        public string? Id { get; set; } // ID único de la imagen
        public string IdProperty { get; set; } // Relación con la propiedad
        public string File { get; set; } // URL o contenido del archivo
        public bool Enabled { get; set; } // Estado de la imagen
    }
}