using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CGEService.Models
{
    /// <summary>
    /// Request model for updating course
    /// All parametrs are optional
    /// </summary>
    public class CourseUpdate
    {
        /// <summary>
        /// Course's id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Name of course
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Code of course
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Some info about course
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Amount of budget places on this course
        /// </summary>
        public int? BudgetPlaces { get; set; }

        /// <summary>
        /// Amout of contract places on this course
        /// </summary>
        public int? ContractPlaces { get; set; }

        /// <summary>
        /// How long to will it takes to study
        /// </summary>
        public int? Years { get; set; }

        /// <summary>
        /// Entrance exams 
        /// </summary>
        public string Exams { get; set; }

        /// <summary>
        /// Ids of proffesions that is licked with course
        /// </summary>
        [Required]
        public List<int> ProfessionIds { get; set; }
    }
}
