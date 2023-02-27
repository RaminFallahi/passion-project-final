using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using passionProject1.Models;


namespace passionProject1.Models.ViewModels
{
    public class Updatecategory
    {
        public CategoryDto SelectedCategory { get; set; }
        public IEnumerable<CategoryDto> CategoriesOptions { get; set; }


    }
}