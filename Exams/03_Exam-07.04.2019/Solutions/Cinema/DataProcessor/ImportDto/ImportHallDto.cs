namespace Cinema.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportHallDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public bool Is4Dx { get; set; }

        [Required]
        public bool Is3D { get; set; }

        [Required]
        public int Seats { get; set; }
    }
}
