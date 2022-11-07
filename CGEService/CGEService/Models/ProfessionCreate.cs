using Entities;
using System;

namespace CGEService.Models
{
    /// <summary>
    /// Request model for creating profession
    /// </summary>
    public class ProfessionCreate
    {
        private ProfType profType;

        /// <summary>
        /// Name of proffesion
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
        public ProfType ProfType 
        {
            get { return profType; }
            set
            {
                if ((int)value < 1 || (int)value > 6)
                    throw new ArgumentException("Wrong profType value.", nameof(value));
                profType = value;
            } 
        }
    }
}
