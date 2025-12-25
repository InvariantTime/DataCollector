using System.Security.Cryptography;

namespace DataCollector.Server.Services.Hashing;

public class DefaultHasher : IPasswordHasher, IDisposable
{
    private const int _saltSize = 32;
    private const int _hashSize = 32;
    private const int _iterationsCount = 10_000;

    private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

    public string Hash(string password)
    {
        byte[] salt = new byte[_saltSize];
        _rng.GetBytes(salt);

        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterationsCount, HashAlgorithmName.SHA256, _hashSize);

        var result = new byte[_saltSize + _hashSize];

        Buffer.BlockCopy(salt, 0, result, 0, _saltSize);
        Buffer.BlockCopy(hash, 0, result, _saltSize, _hashSize);

        return Convert.ToHexString(result);
    }

    public bool Verify(string password, string passwordHash)
    {
        if (password == null)
            return false;

        if (passwordHash == null)
            return false;

        byte[] hash = Convert.FromHexString(passwordHash);
        byte[] salt = new byte[_saltSize];

        Buffer.BlockCopy(hash, 0, salt, 0, _saltSize);

        var newHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterationsCount, HashAlgorithmName.SHA256, _hashSize);

        for (int i = 0; i < _hashSize; i++)
        {
            if (newHash[i] != hash[i + _saltSize])
                return false;
        }

        return true;
    }

    public void Dispose()
    {
        _rng.Dispose();
    }
}
