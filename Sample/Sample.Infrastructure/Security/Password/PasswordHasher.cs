using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Sample.Application.Core.Abstractions.Security;

namespace Sample.Infrastructure.Security.Password;

public class PasswordHasher : IPasswordHasher
{
    const int Iterations = 100_000;
    const int SaltSize = 128 / 8;
    
    private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

    public string HashPassword(string password)
    {
        var hash = HashPassword(password,
            Rng,
            prf: KeyDerivationPrf.HMACSHA512,
            iterCount: Iterations,
            saltSize: SaltSize,
            numBytesRequested: 256 / 8);

        return Convert.ToBase64String(hash);
    }
    
    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        var decodedHashedPassword = Convert.FromBase64String(hashedPassword);
        return VerifyHashedPasswordV3(decodedHashedPassword, providedPassword);
    }

    private static byte[] HashPassword(string password, RandomNumberGenerator rng, KeyDerivationPrf prf, int iterCount, int saltSize, int numBytesRequested)
    {
        var salt = new byte[saltSize];
        rng.GetBytes(salt);
        var subKey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

        var output = new byte[13 + salt.Length + subKey.Length];
        WriteNetworkByteOrder(output, 1, (uint)prf);
        WriteNetworkByteOrder(output, 5, (uint)iterCount);
        WriteNetworkByteOrder(output, 9, (uint)saltSize);
        Buffer.BlockCopy(salt, 0, output, 13, salt.Length);
        Buffer.BlockCopy(subKey, 0, output, 13 + saltSize, subKey.Length);
        return output;
    }


    private static bool VerifyHashedPasswordV3(byte[] hashedPassword, string password)
    {
        try
        {
            // Read header information
            var prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
            var iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
            var saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

            // Read the salt: must be >= 128 bits
            if (saltLength < 128 / 8)
            {
                return false;
            }

            var salt = new byte[saltLength];
            Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

            // Read the subKey (the rest of the payload): must be >= 128 bits
            var subKeyLength = hashedPassword.Length - 13 - salt.Length;
            if (subKeyLength < 128 / 8)
            {
                return false;
            }

            var expectedSubKey = new byte[subKeyLength];
            Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubKey, 0, expectedSubKey.Length);

            // Hash the incoming password and verify it
            var actualSubKey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subKeyLength);

            return CryptographicOperations.FixedTimeEquals(actualSubKey, expectedSubKey);
        }
        catch
        {
            // This should never occur except in the case of a malformed payload, where
            // we might go off the end of the array. Regardless, a malformed payload
            // implies verification failed.
            return false;
        }
    }

    private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }



    private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
    {
        return ((uint)buffer[offset + 0] << 24)
               | ((uint)buffer[offset + 1] << 16)
               | ((uint)buffer[offset + 2] << 8)
               | buffer[offset + 3];
    }
}