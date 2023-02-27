using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace passionProject1.Models
{
    public class BestSeasonToGo
    {
        [Key]
        public int BestSeasonToGoID { get; set; }
        public string Season { get; set; }
        public ICollection<place> places { get; set; }

    }

    public class BestSeasonToGoDto
    {
        public int BestSeasonToGoID { get; set; }
        public string Season { get; set; }
    }
}