using AutoMapper;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.ViewModels;

namespace OnlineLearningPlatform.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            
            CreateMap<Course, CourseViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap();

            CreateMap<Enrollment, EnrollmentViewModel>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(
                    dest => dest.StudentName,
                    opt => opt.MapFrom(src => $"{src.Student.FirstName} {src.Student.LastName}")
                )
                .ReverseMap();
            CreateMap<Lesson, LessonViewModel>()
                .ForMember(dest => dest.ModuleName, opt => opt.MapFrom(src => src.Module.Name))
                .ReverseMap();
            CreateMap<Module, ModuleViewModel>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ReverseMap();
            CreateMap<Quiz, QuizViewModel>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ReverseMap();
            CreateMap<QuizAnswer, QuizAnswerViewModel>()
                .ForMember(
                    dest => dest.QuestionTitle,
                    opt => opt.MapFrom(src => src.QuizQuestion.QuestionTitle)
                )
                .ReverseMap();
            CreateMap<QuizQuestion, QuizQuestionViewModel>()
                .ForMember(dest => dest.QuizName, opt => opt.MapFrom(src => src.Quiz.Name))
                .ReverseMap();
            CreateMap<StudentLesson, StudentLessonViewModel>()
                .ForMember(
                    dest => dest.StudentName,
                    opt => opt.MapFrom(src => $"{src.Student.FirstName} {src.Student.LastName}")
                )
                .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.Lesson.Name))
                .ReverseMap();
            CreateMap<StudentQuizAttempt, StudentQuizAttemptViewModel>()
                .ForMember(
                    dest => dest.StudentName,
                    opt => opt.MapFrom(src => $"{src.Student.FirstName} {src.Student.LastName}")
                )
                .ForMember(dest => dest.QuizName, opt => opt.MapFrom(src => src.Quiz.Name))
                .ReverseMap();
        }
    }
}
