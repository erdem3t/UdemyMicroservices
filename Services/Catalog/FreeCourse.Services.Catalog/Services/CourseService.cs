using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.RepositoryContract;
using FreeCourse.Services.Catalog.ServicesContract;
using FreeCourse.Shared.Dtos;
using Mass = MassTransit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeCourse.Shared.Messages;

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoRepository<Course> courseRepository;
        private readonly IMongoRepository<Category> categoryRepository;
        private readonly IMapper mapper;
        private readonly Mass.IPublishEndpoint publishEndpoint;

        public CourseService(IMapper mapper, IMongoRepository<Course> courseRepository, IMongoRepository<Category> categoryRepository, Mass.IPublishEndpoint publishEndpoint)
        {
            this.mapper = mapper;
            this.courseRepository = courseRepository;
            this.categoryRepository = categoryRepository;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await courseRepository.FilterByAsync(p => true);

            if (courses.Any())
            {
                foreach (var item in courses)
                {
                    item.Category = await categoryRepository.FindByIdAsync(item.CategoryId);
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await courseRepository.FindByIdAsync(id);
            if (course == null)
                return Response<CourseDto>.Fail("Course Not Found", 404);

            course.Category = await categoryRepository.FindByIdAsync(course.CategoryId);

            return Response<CourseDto>.Success(mapper.Map<CourseDto>(course), 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await courseRepository.FilterByAsync(p => p.UserId == userId);

            if (courses.Any())
            {
                foreach (var item in courses)
                {
                    item.Category = await categoryRepository.FindByIdAsync(item.CategoryId);
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return Response<List<CourseDto>>.Success(mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseDto)
        {
            var newCourse = mapper.Map<Course>(courseDto);
            await courseRepository.InsertOneAsync(newCourse);
            return Response<CourseDto>.Success(mapper.Map<CourseDto>(newCourse), 200);
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseDto)
        {
            var updateCourse = mapper.Map<Course>(courseDto);
            await courseRepository.ReplaceOneAsync(updateCourse);

            await publishEndpoint.Publish(new CourseNameChangedEvent
            {
                UserId = courseDto.UserId,
                CourseId = courseDto.Id,
                UpdatedName = courseDto.Name,
            });

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            await courseRepository.DeleteByIdAsync(id);
            return Response<NoContent>.Success(204);
        }
    }
}
