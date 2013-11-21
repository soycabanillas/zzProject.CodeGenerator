using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zzProject.CodeGenerator.Identifier.Generator
{
    public interface IIdentifierGeneratorResult<ReturnType>
    {
        string Result { get; }
        bool IsTruncated { get; }
        bool IsValid { get; }
        ReturnType Next();
    }
}
