using System.Security.Cryptography;
using System.Text;
using PersonalAccount.Models;
using PersonalAccount.Repositories;

namespace PersonalAccount.Services.Confirmation;

public class ConfirmationTokenService(IConfirmationTokenRepo confirmationTokenRepo) : IConfirmationTokenService
{
    public async Task<string> GenerateTokenAsync(int accountId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var confirmation = new ConfirmationTokenModel
        {
            AccountId = accountId,
            ExpiresAt = DateTime.UtcNow.AddMinutes(30),
            TokenHash = HashToken(token),
        };
        await confirmationTokenRepo.AddAsync(confirmation);
        return token;
    }

    public async Task<bool> ValidateTokenAsync(int accountId, string token)
    {
        var confirmationTokens = await confirmationTokenRepo.GetAllByAccountIdAsync(accountId);
        var tokenHash = HashToken(token);
        var confirmation = confirmationTokens.FirstOrDefault(confirmation =>
            confirmation.TokenHash == tokenHash
            && DateTime.UtcNow < confirmation.ExpiresAt
            && confirmation.ConfirmedAt == null);
        if (confirmation == null) return false;

        try
        {
            await confirmationTokenRepo.ConfirmByIdAsync(confirmation.Id, DateTime.UtcNow);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> HasAnyConfirmedTokenAsync(int accountId)
    {
        var confirmationTokens = await confirmationTokenRepo.GetAllByAccountIdAsync(accountId);
        return confirmationTokens.Any(confirmation => confirmation.ConfirmedAt != null);
    }

    private static string HashToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}