using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CGEService.Models
{
    /// <summary>
    /// Request model for creating course
    /// </summary>
    public class CourseCreate
    {
        /// <summary>
        /// id of course's department
        /// </summary>
        [Required]
        public int DepartmentId { get; set; }

        /// <summary>
        /// Name of course
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Code of course
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Some info about course
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Amount of budget places on this course
        /// </summary>
        public int BudgetPlaces { get; set; }
        
        /// <summary>
        /// Amout of contract places on this course
        /// </summary>
        public int ContractPlaces { get; set; }

        /// <summary>
        /// How long to will it takes to study
        /// </summary>
        [Required]
        public int Years { get; set; }

        /// <summary>
        /// Entrance exams 
        /// </summary>
        [Required]
        public string Exams { get; set; }

        /// <summary>
        /// Ids of proffesions that is licked with course
        /// </summary>
        [Required]
        public List<int> ProfessionIds { get; set; }
    }
}
