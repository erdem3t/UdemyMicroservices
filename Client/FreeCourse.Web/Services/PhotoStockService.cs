using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.PhotoStock;
using FreeCourse.Web.ServicesContract;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
            var response = await httpClient.DeleteAsync($"Photos?photoUrl={photoUrl}");

            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return null;

            var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";

            using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);

            var multipartContent = new MultipartFormDataContent
            {
                { new ByteArrayContent(ms.ToArray()), "photo", randomFileName }
            };

            var response = await httpClient.PostAsync("Photos", multipartContent);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();

            return result.Data;
        }
    }
}
