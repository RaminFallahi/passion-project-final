using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using passionProject1.Models;

namespace passionProject1.Models.ViewModels
{
    public class Updateplace
    {
        public placeDto Selectedplace { get; set; }
        public IEnumerable<CategoryDto> CategoriesOptions { get; set; }
        // all categories to choose from when updating this places
        public IEnumerable<BestSeasonToGoDto> BestSeasonToGo { get; set; }

    }
}