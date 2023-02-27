using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace passionProject1.Models.ViewModels
{
    public class DetailsPlace
    {
        public placeDto Selectedplace { get; set; }
        public IEnumerable<CategoryDto> ResponsibleCategories { get; set; }

        public IEnumerable<CategoryDto> AvailableCategories { get; set; }

        public IEnumerable<BestSeasonToGoDto> ResponsibleBestSeasonToGoes { get; set; }

        public IEnumerable<BestSeasonToGoDto> AvailableBestSeasonToGoes { get; set; }
    }
}