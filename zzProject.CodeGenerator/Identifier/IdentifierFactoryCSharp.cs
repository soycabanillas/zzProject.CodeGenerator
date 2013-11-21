using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zzProject.CodeGenerator.Identifier.Generator;

namespace zzProject.CodeGenerator.Identifier
{
    public class IdentifierFactoryCSharp : IIdentifierFactory<String>
    {
        private IdentifierGeneratorCSharp normalizer;

        public IdentifierFactoryCSharp(IdentifierGeneratorCSharp.CapitalizationStyle capitalizationStyle, bool removeDiacritics)
        {
            this.normalizer = new IdentifierGeneratorCSharp(capitalizationStyle, removeDiacritics);
        }

        public virtual string GenerateNewEntry(String value, IEnumerable<String> tabuIdentifiers)
        {
            var normalization = this.normalizer.Generate(value);
            //if (normalization.Error | ErrorNormalizationEnum.NoError)
            while (tabuIdentifiers.Any(el => el == normalization.Result))
            {
                normalization = normalization.Next();
            }
            if (String.IsNullOrWhiteSpace(normalization.Result)) return null;
            return normalization.Result;
        }

        public virtual string GenerateNewEntry(String value, Func<String, bool> isValid) {
            var normalization = this.normalizer.Generate(value);
            //if (normalization.Error | ErrorNormalizationEnum.NoError)
            while (!isValid(normalization.Result)) {
                normalization = normalization.Next();
            }
            if (String.IsNullOrWhiteSpace(normalization.Result)) return null;
            return normalization.Result;
        }
    }
}
