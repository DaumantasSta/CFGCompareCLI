namespace CFGCompareCLI.Models
{
    public class ParameterComparisonEntry
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public ParameterState State { get; set; }

        public static ParameterComparisonEntry CreateAddedParameterComparisonEntry(string id, string value)
        {
            return new ParameterComparisonEntry
            {
                Id = id,
                Value = value,
                State = ParameterState.Added
            };
        }

        public static ParameterComparisonEntry CreateRemovedParameterComparisonEntry(string id, string value)
        {
            return new ParameterComparisonEntry
            {
                Id = id,
                Value = value,
                State = ParameterState.Removed
            };
        }
    }
}
