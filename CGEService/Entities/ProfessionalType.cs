namespace Entities
{
    public class ProfessionalType : IEntitiy
    {
        public int Id { get; set; }
        public ProfType ProfType { get; set; } 
        public string Name { get; set; }
        public string Info { get; set; }
    }
}
