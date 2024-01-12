using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Students
{
    public static class StudentEndpoint
    {
        public static void AddEndpointStudent(this WebApplication app)
        {
            var routes = app.MapGroup("student");

            routes.MapPost("", async (StudentRequest request, AppDbContext context) =>
            {
                var isStudentAlreadyRegistered = await context.Students
                        .AnyAsync(student => student.Name == request.Name);

                if (isStudentAlreadyRegistered)
                    return Results.Conflict("Student already registered");

                var newStudent = new Student(request.Name);
                await context.Students.AddAsync(newStudent);
                await context.SaveChangesAsync();

                var studentResponse = new StudentResponse(newStudent.Name, newStudent.Id);
                return Results.Ok(studentResponse);
            });

            routes.MapGet("", async (AppDbContext context) =>
            {
                var student = await context
                       .Students.Where(student => student.Active)
                       .Select(student => new StudentResponse(student.Name, student.Id))
                       .ToListAsync();

                return student;
            });

            routes.MapPut("{id}", 
                async (Guid id, AppDbContext context, StudentUpdateRequest request) =>
            {
                var student = await context.Students
                        .SingleOrDefaultAsync(student => student.Id == id);

                if (student == null)
                    return Results.NotFound();

                student.updateName(request.NewName);
                await context.SaveChangesAsync();

                var studentResponse = new StudentResponse(student.Name, student.Id);
                return Results.Ok(studentResponse);
            });

            routes.MapDelete("{id}", async (AppDbContext context, Guid id) =>
            {
                var student = await context.Students
                        .SingleOrDefaultAsync(student => student.Id == id); 

                if(student == null) return Results.NoContent();
                student.updateStatus(false);

                await context.SaveChangesAsync();
                return Results.Ok();
            });
        }
    }
}
