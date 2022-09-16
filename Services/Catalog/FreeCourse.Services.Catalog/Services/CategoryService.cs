using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.RepositoryContract;
using FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper mapper;
        private readonly IMongoRepository<Category> categoryRepository;

        public CategoryService(IMapper mapper, IMongoRepository<Category> categoryRepository)
        {
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await categoryRepository.FilterByAsync(p => true);
            return Response<List<CategoryDto>>.Success(mapper.Map<List<CategoryDto>>(categories), 200);
        }

        public async Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto categoryCreateDto)
        {
            var category = mapper.Map<Category>(categoryCreateDto);
            await categoryRepository.InsertOneAsync(category);
            return Response<CategoryDto>.Success(mapper.Map<CategoryDto>(category), 200);
        }

        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await categoryRepository.FindByIdAsync(id);

            if (category == null)
                return Response<CategoryDto>.Fail("Category Not Found", 404);

            return Response<CategoryDto>.Success(mapper.Map<CategoryDto>(category), 200);
        }
    }
}
