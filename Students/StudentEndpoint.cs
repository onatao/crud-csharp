namespace ApiCrud.Students
{
    public class StudentEndpoint
    {
        public static void AddEndpointStudent(WebApplication app)
        {
            app.MapGet("student", () =>
                        new Student("Nathan"));
        }
    }
}
