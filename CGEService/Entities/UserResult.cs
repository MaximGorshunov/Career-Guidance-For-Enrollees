using System;
using System.Collections.Generic;
using System.Linq;

namespace Entities
{
    public class UserResult : IEntitiy
    {
        public int Id { get; set; }
        public int UserId {get; set;} 
        public DateTime Date { get; set; }
        public int R { get; set; }
        public int I { get; set; }
        public int A { get; set; }
        public int S { get; set; }
        public int E { get; set; }
        public int C { get; set; }

        public User User { get; set; }

        public ProfType GetHighst()
        {
            var list = new List<(ProfType type, int value)> 
            {
                (ProfType.R, R),
                (ProfType.I, I),
                (ProfType.A, A),
                (ProfType.S, S),
                (ProfType.E, E),
                (ProfType.C, C)
            };

            return list.OrderBy( u => u.value).Select(u => u.type).Last();
        } 
    }
}
