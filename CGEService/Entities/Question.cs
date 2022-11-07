namespace Entities
{
     public class Question : IEntitiy
     {
        public int Id { get; set; }
        public int Number { get; set; }
        public int ProfessionIdFirst { get; set; }
        public int ProfessionIdSecond { get; set; }

        public Profession ProfessionFirst { get; set; }
        public Profession ProfessionSecond { get; set; }
     }
}
