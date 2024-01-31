namespace NorthSouthSystems.IO;

using System.IO;
using System.Runtime.CompilerServices;

public static class PathX
{
    public static string GetDirectoryNameOfCallerFilePath([CallerFilePath] string callerFilePath = null)
    {
        if (callerFilePath == null)
            throw new ArgumentNullException(nameof(callerFilePath));

        if (!Path.IsPathRooted(callerFilePath))
            throw new ArgumentOutOfRangeException(nameof(callerFilePath));

        return Path.GetDirectoryName(callerFilePath);
    }

    public static string GetFullPathRelativeToCallerFilePath(string relativePath, [CallerFilePath] string callerFilePath = null)
    {
        if (relativePath == null)
            throw new ArgumentNullException(nameof(relativePath));

        if (Path.IsPathRooted(relativePath))
            throw new ArgumentOutOfRangeException(nameof(relativePath));

        // We use our helper method here because of its validations (DRY).
        string directory = GetDirectoryNameOfCallerFilePath(callerFilePath);

        return Path.Combine(directory, relativePath);
    }
}