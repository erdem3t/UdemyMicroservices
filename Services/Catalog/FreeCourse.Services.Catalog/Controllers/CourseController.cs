using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.ServicesContract;
using FreeCourse.Shared.Controller;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Controllers
{
    public class CourseController : CustomBaseController
    {
        private readonly ICourseService courseService;
        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await courseService.GetAllAsync();
            return CreateActionResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await courseService.GetByIdAsync(id);
            return CreateActionResult(response);
        }

        [HttpGet]
        [Route("/api/[controller]/GetAllByUserId/{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await courseService.GetAllByUserIdAsync(userId);
            return CreateActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto createDto)
        {
            var response = await courseService.CreateAsync(createDto);
            return CreateActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto updateDto)
        {
            var response = await courseService.UpdateAsync(updateDto);
            return CreateActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await courseService.DeleteAsync(id);
            return CreateActionResult(response);
        }
    }
}
