using System;

namespace Aex
{
    public class Fonds
    {
        private readonly string _name;
        private decimal _koers;
        private DateTime _datetime;

        public Fonds(string name, decimal koers)
        {
            _name = name;
            _koers = koers;
            _datetime = DateTime.Now;
        }

        public Fonds(string fonds)
        {
            var items = fonds.Split('|');

            _datetime = DateTime.Parse(items[0]);
            _name = items[1];
            _koers = Decimal.Parse(items[2]);
        }

        public string Name
        {
            get { return _name; }
        }

        public decimal Koers
        {
            get { return _koers; }
            set
            {
                _datetime = DateTime.Now;
                _koers = value;
            }
        }

        public DateTime Datetime
        {
            get { return _datetime; }
        }

        public override string ToString()
        {
            return String.Format("{0}|{1}|{2}", Datetime, Name, Koers);
        }
    }
}