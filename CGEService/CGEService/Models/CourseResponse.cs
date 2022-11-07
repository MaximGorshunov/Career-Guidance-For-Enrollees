namespace CGEService.Models
{
    public class CourseResponse
    {
        /// <summary>
        /// Identifier of the course
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of a university department to which course belongs
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Course name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Course registry code
        /// </summary>
        public string Code  { get; set; }

        /// <summary>
        /// Сourse description
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Amount of state-funded places committed for the course
        /// </summary>
        public int BudgetPlaces { get; set; }

        /// <summary>
        /// Amount of self-funded places committed for the course
        /// </summary>
        public int ContractPlaces { get; set; }

        /// <summary>
        /// The duration of the course
        /// </summary>
        public int Years { get; set; }

        /// <summary>
        /// Enumeration of admission examinations required for entry into the course
        /// </summary>
        public string Exams { get; set; }

        /// <summary>
        /// University to which course belongs
        /// </summary>
        public UniversityResponse University { get; set; }

        /// <summary>
        /// University department to which course belongs
        /// </summary>
        public DepartmentResponse Department { get; set; }
    }
}
