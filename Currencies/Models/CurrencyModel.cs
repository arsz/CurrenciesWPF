namespace Currencies.Models
{
    public class CurrencyModel
    {
        public CurrencyModel(string name, double rate, double value)
        {
            Name = name;
            Rate = rate;
            Value = value;
        }

        public string Name { get; }

        public double Rate { get; }

        public double Value { get; }
    }
}