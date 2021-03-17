using System.ComponentModel.DataAnnotations;

namespace Application.Worlds.DTOs
{
    public class CreateWorldDto
    {
        [Required()]
        public string Name { get; set; }

        [Required()]
        [Range(typeof(bool), "true", "true")]
        public bool HasLife { get; set; }
    }
}
