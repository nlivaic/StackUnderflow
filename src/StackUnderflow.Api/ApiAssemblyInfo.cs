﻿using System.Reflection;

namespace StackUnderflow.Api
{
    // A convenient class to make the assembly information more accessible.
    public static class ApiAssemblyInfo
    {
        public static Assembly Value { get; } = typeof(ApiAssemblyInfo).Assembly;
    }
}
