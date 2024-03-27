using System.Collections.Generic;

namespace VideoClub.Data.DataModels
{
    public class RentRequest
    {
        public List<int> Movies { get; set; }
        public string SelectedIDnumber { get; set; }
    }
}
