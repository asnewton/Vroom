using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vroom.Models.ViewModels
{
    public class BikeViewModel
    {
        public Bike Bike { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Model> Models { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }

        private List<Currency> cl = new List<Currency>()
        {
            new Currency("USD", "USD"),
            new Currency("INR", "INR"),
            new Currency("EUR", "EUR")
        };

        public BikeViewModel()
        {
            Currencies = cl;
        }

    }

    public class Currency
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Currency(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
