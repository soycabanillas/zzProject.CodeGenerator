using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zzProject.CodeGenerator.Identifier
{
    public interface IIdentifierFactory<Seed>
    {
        string GenerateNewEntry(Seed value, IEnumerable<String> tabuIdentifiers);
        string GenerateNewEntry(Seed value, Func<String, bool> isValid);
    }
}
