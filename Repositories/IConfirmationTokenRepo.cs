using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public interface IConfirmationTokenRepo
{
    Task CreateAsync(ConfirmationTokenModel token);
    Task<List<ConfirmationTokenModel>> GetByStudentIdAsync(int studentId);
    Task ConfirmByIdAsync(int id);
}