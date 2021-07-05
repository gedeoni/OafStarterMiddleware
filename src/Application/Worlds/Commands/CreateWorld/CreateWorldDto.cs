using System.ComponentModel.DataAnnotations;

namespace Application.Worlds.Commands
{
    public class CreateWorldDto
    {
        [Required()]
        public string Name { get; set; }

        [Required()]
        public bool HasLife { get; set; }
    }
}
