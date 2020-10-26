using System;
using System.Collections.Generic;
using System.Text;

namespace ProBoost.Models
{
    public interface IProjet
    {
        string ProjectId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        IEnumerable<string> Categories { get; set; }
        DateTime Date { get; set; }
        string OwnerUId { get; set; }
    }
}
