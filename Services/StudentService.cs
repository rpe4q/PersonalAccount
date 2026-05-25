using PersonalAccount.Models.Students;
using PersonalAccount.Repositories;

namespace PersonalAccount.Services;

public class StudentService(IStudentRepo<StudentModel> studentRepo) : IStudentService
{
    public async Task<StudentModel?> GetStudentByIdAsync(int id)
    {
        return await studentRepo.GetByIdAsync(id);
    }

    public async Task<StudentModel?> GetStudentByEmailAsync(string email)
    {
        return await studentRepo.GetByEmailAsync(email);
    }

    public async Task UpdateByIdAsync(int id, StudentModel student)
    {
        await studentRepo.UpdateByIdAsync(id, student);
    }

    public async Task<bool> UpdateStudentAsync(int id, StudentEditViewModel model)
    {
        var student = await GetStudentByIdAsync(id);
        if (student == null) return false;

        student.FullName = model.FullName;
        student.GroupName = model.GroupName;
        student.PhotoUrl = model.PhotoUrl is null ? null : new Uri(model.PhotoUrl);

        await studentRepo.UpdateByIdAsync(id, student);
        return true;
    }
}