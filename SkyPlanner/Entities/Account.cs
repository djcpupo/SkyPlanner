using System.Collections.Generic;

namespace SkyPlanner.Entities
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
	
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
