﻿using System.Reflection;

namespace EduSource.Infrastructure.Dapper;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
