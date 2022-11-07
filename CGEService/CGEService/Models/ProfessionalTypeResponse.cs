using Entities;

namespace CGEService.Models
{
    public class ProfessionalTypeResponse
    {
        /// <summary>
        /// Type name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type description
        /// </summary>
        public string Description { get; set; }

        public ProfessionalTypeResponse(ProfessionalType professionalType)
        {
            Name = professionalType.Name;
            Description = professionalType.Info;
        }
    }
}
