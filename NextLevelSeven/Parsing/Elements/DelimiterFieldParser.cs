﻿using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Represents the special MSH-1 field, which contains the field delimiter for the rest of the segment.</summary>
    internal sealed class DelimiterFieldParser : StaticValueFieldParser
    {
        /// <summary>Create a field delimiter descendant.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        public DelimiterFieldParser(Parser ancestor)
            : base(ancestor, 0, 1)
        {
        }

        /// <summary>Get or set the value of the field delimiter.</summary>
        public override string Value
        {
            get
            {
                return new string(Ancestor.DescendantDivider.Value[3], 1);
            }
            set
            {
                var s = Ancestor.DescendantDivider.Value;
                if (HL7.NullValues.Contains(value))
                {
                    throw new ParserException(ErrorCode.FieldCannotBeNull);
                }
                var newValue = string.Concat(s.Substring(0, 3), value, (s.Length > 3 ? s.Substring(4) : string.Empty));
                Ancestor.DescendantDivider.Value = newValue;
            }
        }

        /// <summary>
        ///     Get or set the value of the field delimiter. Only the first value is considered.
        /// </summary>
        public override IEnumerable<string> Values
        {
            get { yield return Value; }
            set { Value = value.First(); }
        }
    }
}