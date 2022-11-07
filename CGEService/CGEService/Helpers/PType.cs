namespace CGEService.Helpers
{
    public class PType
    {
        /// <summary>
        /// Profession type name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Survey result score for this type
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Text description of the Value
        /// </summary>
        public string Power { get; set; }

        public PType(string name, int value, string power)
        {
            Name = name;
            Value = value;
            Power = power;
        }
    }
}
