using System.Collections.Generic;

namespace Entities
{
    public class Role : IEntitiy
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
