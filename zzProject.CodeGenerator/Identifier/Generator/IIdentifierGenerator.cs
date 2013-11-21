using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zzProject.CodeGenerator.Identifier.Generator
{
    public interface IIdentifierGenerator<Seed, Result> where Result : IIdentifierGeneratorResult<Result>
    {
        Result Generate(Seed value);
    }
}
