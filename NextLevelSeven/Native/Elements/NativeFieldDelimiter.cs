﻿using NextLevelSeven.Core;
using NextLevelSeven.Native.Dividers;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents the special MSH-1 field, which contains the field delimiter for the rest of the segment.
    /// </summary>
    internal sealed class NativeFieldDelimiter : NativeElement, INativeField
    {
        public NativeFieldDelimiter(NativeElement ancestor)
            : base(ancestor, 0, 1)
        {
        }

        public override char Delimiter
        {
            get { return '\0'; }
        }

        public override bool HasSignificantDescendants
        {
            get { return false; }
        }

        public override string Value
        {
            get
            {
                var value = Ancestor.DescendantDivider.Value;
                if (value != null && value.Length > 3)
                {
                    return new string(value[3], 1);
                }
                return null;
            }
            set
            {
                // TODO: change the other delimiters in the segment
                var s = Ancestor.DescendantDivider.Value;
                if (s != null && s.Length >= 3)
                {
                    Ancestor.DescendantDivider.Value = string.Join(s.Substring(0, 3), value,
                        (s.Length > 3 ? s.Substring(4) : string.Empty));
                }
            }
        }

        public override INativeElement GetDescendant(int index)
        {
            return new NativeFieldDelimiter(this);
        }

        protected override IStringDivider GetDescendantDivider(NativeElement ancestor, int index)
        {
            return new ProxyStringDivider(() => Value, v => Value = v);
        }

        public override string ToString()
        {
            return Value;
        }

        public string GetValue(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return Value;
        }

        public System.Collections.Generic.IEnumerable<string> GetValues(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return Value.Yield();
        }

        public new INativeRepetition this[int index]
        {
            get { return new NativeRepetition(this, 0, index); }
        }

        override public IElement Clone()
        {
            return CloneInternal();
        }

        IField IField.Clone()
        {
            return CloneInternal();
        }

        NativeField CloneInternal()
        {
            return new NativeField(Value, EncodingConfiguration) { Index = Index };
        }
    }
}