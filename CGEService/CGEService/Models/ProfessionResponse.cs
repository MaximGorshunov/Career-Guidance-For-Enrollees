using Entities;

namespace CGEService.Models
{
    public class ProfessionResponse
    {
        /// <summary>
        /// Identifier of the profession
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Profession name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Professional type
        /// 1 - Realistic (Реалистичный)
        /// 2 - Investigative (Интеллектуальный)
        /// 3 - Artistic (Артистический)
        /// 4 - Social (Социальный)
        /// 5 - Conventional (Офисный)
        /// 6 - Enterprising (Предпринимательский)
        /// </summary>
        public ProfType ProfType { get; set; }

        /// <summary>
        /// Total amout of courses related to the profession
        /// </summary>
        public int CoursesAmount { get; set; }

        public ProfessionResponse () { }

        public ProfessionResponse(Profession profession)
        {
            Id = profession.Id;
            Name = profession.Name;
            ProfType = profession.ProfType;
        }

        public ProfessionResponse (Profession profession, int coursesAmount)
        {
            Id = profession.Id;
            Name = profession.Name;
            ProfType = profession.ProfType;
            CoursesAmount = coursesAmount;
        }
    }
}
