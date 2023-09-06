namespace WarnerBros.Models
{
    public class Movies
    {
        public int id { get; set; }
        public string? img { get; set; }
        public string? nombre { get; set; }
        public string? sinopsis { get; set;  }
        public string? director { get; set; }
        public string? genero { get; set; }
        public int puntuacion { get; set; }
    }
}
