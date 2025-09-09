namespace NorthSouthSystems.IO;

using System.IO;
using System.Runtime.CompilerServices;

public static class PathX
{
    public static string GetDirectoryNameOfCallerFilePath([CallerFilePath] string? callerFilePath = null)
    {
        ArgumentNullException.ThrowIfNull(callerFilePath);

        if (!Path.IsPathRooted(callerFilePath))
            throw new ArgumentOutOfRangeException(nameof(callerFilePath));

        string? directory = Path.GetDirectoryName(callerFilePath);

        if (string.IsNullOrEmpty(directory))
            throw new ArgumentOutOfRangeException(nameof(callerFilePath));

        return directory;
    }

    public static string GetFullPathRelativeToCallerFilePath(string relativePath, [CallerFilePath] string? callerFilePath = null)
    {
        ArgumentNullException.ThrowIfNull(relativePath);

        if (Path.IsPathRooted(relativePath))
            throw new ArgumentOutOfRangeException(nameof(relativePath));

        // We use our helper method here because of its validations (DRY).
        string directory = GetDirectoryNameOfCallerFilePath(callerFilePath);

        return Path.Combine(directory, relativePath);
    }
}