using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using passionProject1.Models;


namespace passionProject1.Models.ViewModels
{
    public class UpdateBestSeasonToGoController
    {
        public BestSeasonToGoDto SelectedBestSeasonToGo { get; set; }
        public IEnumerable<placeDto> PlaceInfo { get; set; }

    }
}