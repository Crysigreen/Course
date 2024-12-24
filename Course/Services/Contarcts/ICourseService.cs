using Course.Models;

namespace Course.Services.Contarcts
{
    public interface ICourseService
    {
        IEnumerable<Courses> GetAllCourses();
        Courses GetCourseById(int id);
        void AddCourse(Courses course);
        void UpdateCourse(int id, Courses course);
        void DeleteCourse(int id);
        bool IsCourseFull(int courseId);
    }
}
