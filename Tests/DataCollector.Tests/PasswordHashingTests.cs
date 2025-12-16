using DataCollector.Server.Services.Hashing;

namespace DataCollector.Tests;

public class PasswordHashingTests
{
    [Fact]
    public void Same_Passwords_Has_Different_Hashes()
    {
        var hasher = new DefaultHasher();
        string password1 = "Password1234";
        string password2 = "Password1234";

        string hash1 = hasher.Hash(password1);
        string hash2 = hasher.Hash(password2);

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Different_Passwords_Has_Different_Hashes()
    {
        var hasher = new DefaultHasher();
        string password1 = "Password1234";
        string password2 = "OtherPassword1234";

        string hash1 = hasher.Hash(password1);
        string hash2 = hasher.Hash(password2);

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Same_Passwords_Is_Verified()
    {
        var hasher = new DefaultHasher();
        string password1 = "Password1234";
        string password2 = "Password1234";

        string hash = hasher.Hash(password1);
        bool verify = hasher.Verify(password2, hash);

        Assert.True(verify);
    }

    [Fact]
    public void Different_Passwords_Is_Not_Verified()
    {
        var hasher = new DefaultHasher();
        string password1 = "Password1234";
        string password2 = "OtherPassword1234";

        string hash = hasher.Hash(password1);
        bool verify = hasher.Verify(password2, hash);

        Assert.False(verify);
    }
}
