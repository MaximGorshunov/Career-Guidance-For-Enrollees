using Entities;
using System.Collections.Generic;
using System.Linq;

namespace CGEService.Models
{
    public class UserResultStatistic
    {
        /// <summary>
        /// Total amount of survey results
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Information about professional types in which user(s) showed high results
        /// </summary>
        public Statistic High { get; set; }

        /// <summary>
        /// Information about professional types in which user(s) showed middle results
        /// </summary>
        public Statistic Middle { get; set; }

        /// <summary>
        /// Information about professional types in which user(s) showed low results
        /// </summary>
        public Statistic Low { get; set; }

        /// <summary>
        /// List of preferable professions according to surveys results
        /// </summary>
        public List<ProfessionResponse> PreferedProfessions { get; set; }
    }

    public class Statistic
    {
        /// <summary>
        /// Proportions of the survey results where Realistic type gain particular (High/Middle/Low) result
        /// </summary>
        public double Realistic { get; set; }

        /// <summary>
        /// Proportions of the survey results where Investigative type gain particular (High/Middle/Low) result
        /// </summary>
        public double Investigative { get; set; }

        /// <summary>
        /// Proportions of the survey results where Artistic type gain particular (High/Middle/Low) result
        /// </summary>
        public double Artistic { get; set; }

        /// <summary>
        /// Proportions of the survey results where Social type gain particular (High/Middle/Low) result
        /// </summary>
        public double Social { get; set; }

        /// <summary>
        /// Proportions of the survey results where Enterprising type gain particular (High/Middle/Low) result
        /// </summary>
        public double Enterprising { get; set; }

        /// <summary>
        /// Proportions of the survey results where Conventional type gain particular (High/Middle/Low) result
        /// </summary>
        public double Conventional { get; set; }

        public ProfType GetHighst()
        {
            var list = new List<(ProfType type, double value)>
            {
                (ProfType.R, Realistic),
                (ProfType.I, Investigative),
                (ProfType.A, Artistic),
                (ProfType.S, Social),
                (ProfType.E, Enterprising),
                (ProfType.C, Conventional)
            };

            return list.OrderBy(u => u.value).Select(u => u.type).Last();
        }
    }
}
