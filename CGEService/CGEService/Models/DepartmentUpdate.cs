namespace CGEService.Models
{
    /// <summary>
    /// Request model for updating department
    /// </summary>
    public class DepartmentUpdate
    {
        /// <summary>
        /// Department id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of university
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Univertsity's contacts
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// University's info
        /// </summary>
        public string Info { get; set; }
    }
}
