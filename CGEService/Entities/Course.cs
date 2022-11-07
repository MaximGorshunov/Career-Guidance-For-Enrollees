using System.Collections.Generic;

namespace Entities
{
    public class Course : IEntitiy
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Info { get; set; }
        public int BudgetPlaces { get; set; }
        public int ContractPlaces { get; set; }
        public int Years { get; set; }
        public string Exams { get; set; }

        public Department Department { get; set; }  
        public ICollection<Profession> Professions { get; set; }
    }
}
