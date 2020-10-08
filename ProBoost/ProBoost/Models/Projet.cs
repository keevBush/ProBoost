using System;
using System.Collections.Generic;
using System.Text;

namespace ProBoost.Models
{
    public class Projet
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable <string> Categories { get; set; }
        public DateTime Date { get; set; }
        public string OwnerUId { get; set; }
    }
}
