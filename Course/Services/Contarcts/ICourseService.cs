using Course.Models;

namespace Course.Services.Contarcts
{
    public interface ICourseService
    {
        IEnumerable<Coursess> GetAllCourses();
        Coursess GetCourseById(int id);
        void AddCourse(Coursess course);
        void UpdateCourse(int id, Coursess course);
        void DeleteCourse(int id);
        bool IsCourseFull(int courseId);
    }
}
