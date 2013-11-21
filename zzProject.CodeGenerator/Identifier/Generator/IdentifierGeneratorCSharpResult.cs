using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zzProject.CodeGenerator.Identifier.Generator
{
    public class IdentifierGeneratorCSharpResult : IIdentifierGeneratorResult<IdentifierGeneratorCSharpResult>
    {
        private IdentifierGeneratorCSharp generator;
        public string OriginalValue { get; private set; }
        public string Result { get; private set; }
        public bool IsTruncated { get; private set; }
        public bool IsValid { get; private set; }
        public IdentifierGeneratorCSharpResultStateEnum State { get; private set; }
        internal int Iteration { get; private set; }

        public IdentifierGeneratorCSharpResult(IdentifierGeneratorCSharp generator, String originalValue, string result, bool isTruncated, IdentifierGeneratorCSharpResultStateEnum state)
        {
            this.OriginalValue = originalValue;
            this.Result = result;
            this.IsTruncated = isTruncated;
            this.IsValid = this.State == IdentifierGeneratorCSharpResultStateEnum.IsValid;
            this.generator = generator;
            this.Iteration = 0;
            this.State = state;
        }

        public IdentifierGeneratorCSharpResult Next()
        {
            this.Iteration++;
            var nextValue = this.generator.Next(this);
            return nextValue;
        }
    }
}
