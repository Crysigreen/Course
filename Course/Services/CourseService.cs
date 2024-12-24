using Course.Data;
using Course.Models;
using Course.Services.Contarcts;

namespace Course.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;
        public CourseService(ApplicationDbContext context) => _context = context;

        public IEnumerable<Courses> GetAllCourses() => _context.Courses.ToList();
        public Courses GetCourseById(int id) => _context.Courses.Find(id);
        public void AddCourse(Courses course) { _context.Courses.Add(course); _context.SaveChanges(); }
        public void UpdateCourse(int id, Courses updatedCourse)
        {
            var course = GetCourseById(id);
            if (course != null)
            {
                course.Title = updatedCourse.Title;
                course.Description = updatedCourse.Description;
                course.Price = updatedCourse.Price;
                course.MaxParticipants = updatedCourse.MaxParticipants;
                _context.SaveChanges();
            }
        }
        public void DeleteCourse(int id) { _context.Courses.Remove(GetCourseById(id)); _context.SaveChanges(); }
        public bool IsCourseFull(int courseId)
        {
            var course = GetCourseById(courseId);
            return course != null && course.CurrentParticipants >= course.MaxParticipants;
        }
    }
}
