using System.Security.Cryptography;
using System.Text;
using PersonalAccount.Models;
using PersonalAccount.Repositories;

namespace PersonalAccount.Services.Confirmation;

public class ConfirmationTokenService(IConfirmationTokenRepo confirmations) : IConfirmationTokenService
{
    public async Task<string> GenerateTokenAsync(int studentId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var confirmation = new ConfirmationTokenModel
        {
            StudentId = studentId,
            ExpiresAt = DateTime.UtcNow.AddSeconds(30),
            TokenHash = HashToken(token),
        };
        await confirmations.CreateAsync(confirmation);
        return token;
    }

    public async Task<bool> ValidateTokenAsync(int studentId, string token)
    {
        var confirmationTokens = await confirmations.GetByStudentIdAsync(studentId);
        var tokenHash = HashToken(token);
        var confirmation = confirmationTokens.FirstOrDefault(confirmation =>
            confirmation.TokenHash == tokenHash
            && DateTime.UtcNow < confirmation.ExpiresAt
            && confirmation.ConfirmedAt == null);
        if (confirmation == null) return false;

        try
        {
            await confirmations.ConfirmByIdAsync(confirmation.Id);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string HashToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}