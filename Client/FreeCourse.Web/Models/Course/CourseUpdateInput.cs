using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Course
{
    public class CourseUpdateInput
    {
        public string Id
        {
            get; set;
        }


        [Display(Name = "Kategori")]
        public string CategoryId
        {
            get; set;
        }

        public string UserId
        {
            get; set;
        }

        [Display(Name = "Kurs İsmi")]
        public string Name
        {
            get; set;
        }

        public string Picture
        {
            get; set;
        }


        [Display(Name = "Fiyatı")]
        public decimal Price
        {
            get; set;
        }

        [Display(Name = "Açıklama")]
        public string Description
        {
            get; set;
        }

        public FeatureViewModel Feature
        {
            get; set;
        }

        [Display(Name = "Kurs Resmi")]
        public IFormFile PhotoFormFile
        {
            get; set; 
        }
    }
}
