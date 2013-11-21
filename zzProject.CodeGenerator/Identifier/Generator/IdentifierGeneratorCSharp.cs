using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zzProject.StringUtils;
using zzProject.StringUtils.Core.Infrastructure.Text;

namespace zzProject.CodeGenerator.Identifier.Generator
{
    public class IdentifierGeneratorCSharp : IIdentifierGenerator<String, IdentifierGeneratorCSharpResult>
    {
        public enum CapitalizationStyle
        {
            none = 0,
            PascalCase = 1,
            camelCase = 2,
            UPPERCASE = 3,
            lowercase = 4
        }

        private const int MAX_LENGTH_IDENTIFIER = 511;
        private CapitalizationStyle capitalizationStyle;
        private bool removeDiacritics;

        public IdentifierGeneratorCSharp(CapitalizationStyle capitalizationStyle, bool removeDiacritics)
        {
            this.capitalizationStyle = capitalizationStyle;
            this.removeDiacritics = removeDiacritics;
        }

        public IdentifierGeneratorCSharpResult Generate(string value)
        {
            //http://stackoverflow.com/questions/1904252/is-there-a-method-in-c-sharp-to-check-if-a-string-is-a-valid-identifier
            //http://blog.visualt4.com/2009/02/creating-valid-c-identifiers.html
            IdentifierGeneratorCSharpResult result;
            string tempValue = value;
            //Compliant with item 2.4.2 of the C# specification
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]");
            tempValue = regex.Replace(tempValue, "_");

            //Convertimos las palabras a mayúscula
            switch (this.capitalizationStyle)
            {
                case CapitalizationStyle.camelCase:
                    tempValue = TextNormalization.ToCamelCase(value);
                    break;
                case CapitalizationStyle.PascalCase:
                    tempValue = TextNormalization.ToPascalCase(value);
                    break;
                case CapitalizationStyle.lowercase:
                    tempValue = TextNormalization.ToLowerCase(value);
                    break;
                case CapitalizationStyle.UPPERCASE:
                    tempValue = TextNormalization.ToUpperCase(value);
                    break;
                default:
                    tempValue = value;
                    break;
            }
            tempValue.Replace("_", string.Empty);
            //Quitamos acentos
            if (removeDiacritics) tempValue = TextNormalization.RemoveDiacritics(tempValue);

            if (tempValue == string.Empty)
            {
                result = new IdentifierGeneratorCSharpResult(this, value, tempValue, false, IdentifierGeneratorCSharpResultStateEnum.IsEmpty);
            }
            else
            {
                //The identifier must start with a character or a "_"
                if (!char.IsLetter(tempValue, 0)
                    &&
                    !Microsoft.CSharp.CSharpCodeProvider.CreateProvider("C#").IsValidIdentifier(tempValue))
                {
                    tempValue = string.Concat("_", tempValue);
                }

                if (tempValue.Length >= MAX_LENGTH_IDENTIFIER)
                {
                    result = new IdentifierGeneratorCSharpResult(this, value, tempValue.Substring(0, MAX_LENGTH_IDENTIFIER), true, IdentifierGeneratorCSharpResultStateEnum.IsTooBig);
                }
                else
                {
                    result = new IdentifierGeneratorCSharpResult(this, value, tempValue, false, IdentifierGeneratorCSharpResultStateEnum.IsValid);
                }
            }
            return result;
        }

        internal IdentifierGeneratorCSharpResult Next(IdentifierGeneratorCSharpResult oldValue)
        {
            var suffixResult = StringPreSuffix.SuffixInt(oldValue.Result, oldValue.Iteration, MAX_LENGTH_IDENTIFIER);
            switch (suffixResult.State)
            {
                case StringPreSuffix.PreSuffixStateEnum.MatchSpecifications:
                    return new IdentifierGeneratorCSharpResult(this, oldValue.OriginalValue, suffixResult.Result, false, IdentifierGeneratorCSharpResultStateEnum.IsValid);
                case StringPreSuffix.PreSuffixStateEnum.OriginalValueTrimmed:
                    return new IdentifierGeneratorCSharpResult(this, oldValue.OriginalValue, suffixResult.Result, false, IdentifierGeneratorCSharpResultStateEnum.IsValid);
                //The identifier can't start with a number
                case StringPreSuffix.PreSuffixStateEnum.NoOriginalValueOnResult:
                    return new IdentifierGeneratorCSharpResult(this, oldValue.OriginalValue, suffixResult.Result, false, IdentifierGeneratorCSharpResultStateEnum.IsTooBig);
                case StringPreSuffix.PreSuffixStateEnum.MaxLengthTooShort:
                    return new IdentifierGeneratorCSharpResult(this, oldValue.OriginalValue, suffixResult.Result, false, IdentifierGeneratorCSharpResultStateEnum.IsTooBig);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
