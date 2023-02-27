using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace passionProject1.Models
{
    public class place
    {
        [Key]
        public int PlaceID { get; set; }
        public string PlaceName { get; set; }


        //one place belongs to a season to go
        //one season belongs to many places
        [ForeignKey("BestSeasonToGo")]
        public int BestSeasonToGoID { get; set; }
        public virtual BestSeasonToGo BestSeasonToGo { get; set; }


        //one place belongs to a category
        //one category belongs to many places
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }

    //DATA TRANSFER OBJECT METHOD:
    public class placeDto
    {
        public int PlaceID { get; set; }
        public string PlaceName { get; set; }
        public int BestSeasonToGoID { get; set; }
        public int CategoryID { get; set; }


    }
}