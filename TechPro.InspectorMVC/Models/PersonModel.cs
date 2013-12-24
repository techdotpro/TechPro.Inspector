namespace TechPro.InspectorMVC.Models
{
    using System;
    using System.Collections.Generic;
    using FakeO;

    public class PersonModel
    {
        public DateTime Birthday { get; set; }

        public string Biography { get; set; }

        public IDictionary<string, string> Contacts { get; set; }

        public double Credit { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int UUID { get; set; }

        public PersonModel()
        {
            this.Biography = FakeO.Lorem.Paragraph(2);
            this.Birthday = FakeO.Data.Random<DateTime>();
            this.Credit = FakeO.Number.NextDouble(-5000, 5000);
            this.Name = FakeO.Name.FullName(NameFormats.Standard);
            this.Email = FakeO.Internet.Email(this.Name);
            this.UUID = FakeO.Distinct.Number();

            this.Contacts = new SortedDictionary<string, string>();
            var n = (new Random()).Next(3, 8);
            for (var i = 0; i < n; i++) {
                this.Contacts.Add(FakeO.Name.FullName(), FakeO.Phone.Number());
            }
        }
    }
}