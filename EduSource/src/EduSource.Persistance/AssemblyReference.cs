using System.Reflection;

namespace EduSource.Persistence;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(Assembly).Assembly;
}
