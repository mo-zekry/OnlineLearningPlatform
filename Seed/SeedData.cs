using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Ensure the database is created and apply any pending migrations
        context.Database.Migrate();

        // Seed Roles
        string[] roleNames = { "Admin", "Student", "Guest" };

        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        // Add Admin user
        var adminUser = new ApplicationUser
        {
            UserName = "admin@plat.com",
            Email = "admin@plat.com",
            FirstName = "Admin",
            LastName = "User"
        };

        var adminPassword = "Admin@123";
        var admin = await userManager.FindByEmailAsync(adminUser.Email);

        if (admin == null)
        {
            var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdminUser.Succeeded)
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        // Check if data has already been seeded
        if (context.Categories.Any() || context.Courses.Any())
        {
            return; // Data already seeded, exit the method
        }

        // Seed Categories and retrieve their IDs
        var webDevCategory = new Category { Name = "Web Development" };
        var dataScienceCategory = new Category { Name = "Data Science" };
        var progLangsCategory = new Category { Name = "Programming Languages" };

        context.Categories.AddRange(webDevCategory, dataScienceCategory, progLangsCategory);
        await context.SaveChangesAsync();

        // Seed Courses
        var courses = new List<Course>
        {
            new()
            {
                Name = "Introduction to Web Development",
                Description =
                    "Learn the basics of web development using HTML, CSS, and JavaScript.",
                Price = 100,
                ImageUrl =
                    "https://images.pexels.com/photos/39284/macbook-apple-imac-computer-39284.jpeg",
                IsProgressLimited = true,
                CategoryId = webDevCategory.Id
            },
            new()
            {
                Name = "Advanced Data Analysis",
                Description = "Master data analysis techniques using Python and R.",
                Price = 200,
                ImageUrl = "https://images.pexels.com/photos/669619/pexels-photo-669619.jpeg",
                IsProgressLimited = true,
                CategoryId = dataScienceCategory.Id
            },
            new()
            {
                Name = "Modern JavaScript",
                Description = "Learn modern JavaScript features and frameworks.",
                Price = 150,
                ImageUrl =
                    "https://cdn.pixabay.com/photo/2015/04/23/17/41/javascript-736400_960_720.png",
                IsProgressLimited = false,
                CategoryId = progLangsCategory.Id
            }
        };

        await context.Courses.AddRangeAsync(courses);
        await context.SaveChangesAsync();

        // Seed Modules and Lessons for each Course
        var modules = new List<Module>
        {
            new()
            {
                CourseId = courses.First(c => c.Name == "Introduction to Web Development").Id,
                Name = "HTML Basics",
                Number = 1,
                Lessons = new List<Lesson>
                {
                    new()
                    {
                        Name = "Introduction to HTML",
                        Number = 1,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/1321208/1321208-uhd_2560_1440_30fps.mp4",
                        LessonDetails = "Learn the basics of HTML structure and syntax.",
                        CourseOrder = 1
                    },
                    new()
                    {
                        Name = "HTML Elements",
                        Number = 2,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/1093662/1093662-hd_1920_1080_30fps.mp4",
                        LessonDetails = "Explore various HTML elements and their uses.",
                        CourseOrder = 2
                    }
                }
            },
            new()
            {
                CourseId = courses.First(c => c.Name == "Introduction to Web Development").Id,
                Name = "CSS Fundamentals",
                Number = 2,
                Lessons = new List<Lesson>
                {
                    new()
                    {
                        Name = "CSS Basics",
                        Number = 1,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/1536322/1536322-hd_1920_1080_30fps.mp4",
                        LessonDetails = "Learn the basics of CSS styling and selectors.",
                        CourseOrder = 3
                    },
                    new()
                    {
                        Name = "CSS Layouts",
                        Number = 2,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/1536315/1536315-hd_1920_1080_30fps.mp4",
                        LessonDetails =
                            "Understand the different ways to layout web pages using CSS.",
                        CourseOrder = 4
                    }
                }
            },
            new()
            {
                CourseId = courses.First(c => c.Name == "Advanced Data Analysis").Id,
                Name = "Python for Data Analysis",
                Number = 1,
                Lessons = new List<Lesson>
                {
                    new()
                    {
                        Name = "Introduction to Pandas",
                        Number = 1,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/2239241/2239241-hd_1920_1080_24fps.mp4",
                        LessonDetails = "Learn how to use Pandas for data manipulation.",
                        CourseOrder = 1
                    },
                    new()
                    {
                        Name = "Data Visualization with Matplotlib",
                        Number = 2,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/2895788/2895788-uhd_2560_1440_24fps.mp4",
                        LessonDetails = "Explore data visualization techniques with Matplotlib.",
                        CourseOrder = 2
                    }
                }
            },
            new()
            {
                CourseId = courses.First(c => c.Name == "Advanced Data Analysis").Id,
                Name = "R for Data Analysis",
                Number = 2,
                Lessons = new List<Lesson>
                {
                    new()
                    {
                        Name = "Introduction to R",
                        Number = 1,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/3126661/3126661-uhd_2560_1440_24fps.mp4",
                        LessonDetails = "Learn the basics of R programming for data analysis.",
                        CourseOrder = 3
                    },
                    new()
                    {
                        Name = "Data Manipulation with dplyr",
                        Number = 2,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/979689/979689-hd_1920_1080_30fps.mp4",
                        LessonDetails = "Understand how to manipulate data using dplyr.",
                        CourseOrder = 4
                    }
                }
            },
            new()
            {
                CourseId = courses.First(c => c.Name == "Modern JavaScript").Id,
                Name = "ES6 Features",
                Number = 1,
                Lessons = new List<Lesson>
                {
                    new()
                    {
                        Name = "Introduction to ES6",
                        Number = 1,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/3048183/3048183-uhd_2560_1440_24fps.mp4",
                        LessonDetails = "Explore the new features introduced in ES6.",
                        CourseOrder = 1
                    },
                    new()
                    {
                        Name = "Arrow Functions",
                        Number = 2,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/2890236/2890236-hd_1920_1080_30fps.mp4",
                        LessonDetails = "Learn about arrow functions and their syntax.",
                        CourseOrder = 2
                    }
                }
            },
            new()
            {
                CourseId = courses.First(c => c.Name == "Modern JavaScript").Id,
                Name = "JavaScript Frameworks",
                Number = 2,
                Lessons = new List<Lesson>
                {
                    new()
                    {
                        Name = "Introduction to React",
                        Number = 1,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/4199353/4199353-uhd_2560_1440_25fps.mp4",
                        LessonDetails = "Learn the basics of building web applications with React.",
                        CourseOrder = 3
                    },
                    new()
                    {
                        Name = "Vue.js Basics",
                        Number = 2,
                        VideoUrl =
                            "https://videos.pexels.com/video-files/9783690/9783690-uhd_2732_1440_25fps.mp4",
                        LessonDetails =
                            "Understand how to use Vue.js for building interactive user interfaces.",
                        CourseOrder = 4
                    }
                }
            }
        };

        await context.Modules.AddRangeAsync(modules);
        await context.SaveChangesAsync();

        // Seed Quizzes and Quiz Questions/Answers for each Course
        var quizzes = new List<Quiz>
        {
            new()
            {
                CourseId = courses.First(c => c.Name == "Introduction to Web Development").Id,
                Name = "HTML Basics Quiz",
                Number = 1,
                CourseOrder = 5,
                MinPassScore = 60,
                IsPassRequired = true,
                QuizQuestions = new List<QuizQuestion>
                {
                    new()
                    {
                        QuestionTitle = "What does HTML stand for?",
                        QuizAnswers = new List<QuizAnswer>
                        {
                            new() { AnswerText = "HyperText Markup Language", IsCorrect = true },
                            new() { AnswerText = "HyperTool Markup Language", IsCorrect = false },
                            new() { AnswerText = "HighText Machine Language", IsCorrect = false }
                        }
                    },
                    new()
                    {
                        QuestionTitle = "Which tag is used for creating hyperlinks?",
                        QuizAnswers = new List<QuizAnswer>
                        {
                            new() { AnswerText = "<link>", IsCorrect = false },
                            new() { AnswerText = "<a>", IsCorrect = true },
                            new() { AnswerText = "<href>", IsCorrect = false }
                        }
                    }
                }
            },
            new()
            {
                CourseId = courses.First(c => c.Name == "Advanced Data Analysis").Id,
                Name = "Python Basics Quiz",
                Number = 1,
                CourseOrder = 5,
                MinPassScore = 70,
                IsPassRequired = true,
                QuizQuestions = new List<QuizQuestion>
                {
                    new()
                    {
                        QuestionTitle = "What is Pandas primarily used for?",
                        QuizAnswers = new List<QuizAnswer>
                        {
                            new() { AnswerText = "Data visualization", IsCorrect = false },
                            new() { AnswerText = "Data manipulation", IsCorrect = true },
                            new() { AnswerText = "Machine learning", IsCorrect = false }
                        }
                    },
                    new()
                    {
                        QuestionTitle = "Which library is used for data visualization in Python?",
                        QuizAnswers = new List<QuizAnswer>
                        {
                            new() { AnswerText = "NumPy", IsCorrect = false },
                            new() { AnswerText = "Matplotlib", IsCorrect = true },
                            new() { AnswerText = "SciPy", IsCorrect = false }
                        }
                    }
                }
            },
            new()
            {
                CourseId = courses.First(c => c.Name == "Modern JavaScript").Id,
                Name = "JavaScript Basics Quiz",
                Number = 1,
                CourseOrder = 5,
                MinPassScore = 65,
                IsPassRequired = true,
                QuizQuestions = new List<QuizQuestion>
                {
                    new()
                    {
                        QuestionTitle = "Which keyword is used to declare variables in ES6?",
                        QuizAnswers = new List<QuizAnswer>
                        {
                            new() { AnswerText = "var", IsCorrect = false },
                            new() { AnswerText = "let", IsCorrect = true },
                            new() { AnswerText = "define", IsCorrect = false }
                        }
                    },
                    new()
                    {
                        QuestionTitle = "Which of the following is a JavaScript framework?",
                        QuizAnswers = new List<QuizAnswer>
                        {
                            new() { AnswerText = "Angular", IsCorrect = true },
                            new() { AnswerText = "Django", IsCorrect = false },
                            new() { AnswerText = "Flask", IsCorrect = false }
                        }
                    }
                }
            }
        };

        await context.Quizzes.AddRangeAsync(quizzes);
        await context.SaveChangesAsync();
    }
}
