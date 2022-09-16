using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Helper;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Course;
using FreeCourse.Web.ServicesContract;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient httpClient;
        private readonly IPhotoStockService photoStockService;
        private readonly PhotoHelper photoHelper;

        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            this.httpClient = httpClient;
            this.photoStockService = photoStockService;
            this.photoHelper = photoHelper;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await httpClient.GetAsync("Category");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

            return result.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            var response = await httpClient.GetAsync("Course");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            foreach (var item in result.Data)
            {
                item.PictureUrl = photoHelper.GetPhotoStockUrl(item.Picture);
            }

            return result.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await httpClient.GetAsync($"Course/GetAllByUserId/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            foreach (var item in result.Data)
            {
                item.PictureUrl = photoHelper.GetPhotoStockUrl(item.Picture);
            }

            return result.Data;
        }

        public async Task<CourseViewModel> GetByCourseIdAsync(string courseId)
        {
            var response = await httpClient.GetAsync($"Course/{courseId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();

            result.Data.PictureUrl = photoHelper.GetPhotoStockUrl(result.Data.Picture);

            return result.Data;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput course)
        {
            var responsePhoto = await photoStockService.UploadPhoto(course.PhotoFormFile);
            if (responsePhoto != null)
            {
                course.Picture = responsePhoto.Url;
            }

            var response = await httpClient.PostAsJsonAsync("Course", course);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput course)
        {
            var responsePhoto = await photoStockService.UploadPhoto(course.PhotoFormFile);
            if (responsePhoto != null)
            {
                await photoStockService.DeletePhoto(course.Picture);
                course.Picture = responsePhoto.Url;
            }

            var response = await httpClient.PutAsJsonAsync<CourseUpdateInput>("Course", course);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await httpClient.DeleteAsync($"Course/{courseId}");

            return response.IsSuccessStatusCode;
        }
    }
}
