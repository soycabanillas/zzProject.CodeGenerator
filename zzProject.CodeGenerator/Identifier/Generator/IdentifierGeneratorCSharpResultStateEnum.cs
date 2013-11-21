using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zzProject.CodeGenerator.Identifier.Generator
{
    [Flags]
    public enum IdentifierGeneratorCSharpResultStateEnum
    {
        IsValid = 0,
        IsEmpty = 1, //The result is an empty string and an empty string is not a valid identifier for the generator used
        IsTooBig = 2, //The result is too big to be a valid identifier for the generator used
    }
}
