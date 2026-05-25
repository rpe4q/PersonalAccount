using PersonalAccount.Models.Students;

public interface IStudentService
{
    public Task<StudentModel?> GetStudentByIdAsync(int id);
    public Task<StudentModel?> GetStudentByEmailAsync(string email);
    Task UpdateByIdAsync(int id, StudentModel student);
    Task<bool> UpdateStudentAsync(int id, StudentEditViewModel model);
}