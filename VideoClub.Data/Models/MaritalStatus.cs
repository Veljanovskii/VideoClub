using System.Collections.Generic;

namespace VideoClub.Data.Models
{
    public class MaritalStatus
    {
        public MaritalStatus()
        {
            Users = new HashSet<User>();
        }

        public int MaritalStatusId { get; set; }

        public string Caption { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}