using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace passionProject1.Models.ViewModels
{
    public class DetailsBestSeasonToGo
    {
        public BestSeasonToGoDto SelectedBestSeasonToGo { get; set; }
        public IEnumerable<placeDto> RelatedPlaces { get; set; }
        public IEnumerable<placeDto> AvailablePlaces { get; set; }

    }
}