using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models
{
    public class FeatureViewModel
    {
        [Display(Name = "Süresi")]
        public int Duration
        {
            get; set;
        }
    }
}
