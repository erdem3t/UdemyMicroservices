using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Web.ServicesContract
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCourseAsync();

        Task<List<CategoryViewModel>> GetAllCategoryAsync();

        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);

        Task<CourseViewModel> GetByCourseIdAsync(string courseId);

        Task<bool> CreateCourseAsync(CourseCreateInput course);

        Task<bool> UpdateCourseAsync(CourseUpdateInput course);
        
        Task<bool> DeleteCourseAsync(string courseId);
    }
}
