﻿using System;
using System.Collections.Generic;
using NextLevelSeven.Conversion;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>
    ///     Base class for message builders.
    /// </summary>
    internal abstract class BuilderBase : IElement, IComparable, IComparable<IElement>, IComparable<string>,
        IEquatable<IElement>, IEquatable<string>
    {
        /// <summary>
        ///     Encoding configuration for this message.
        /// </summary>
        internal readonly EncodingConfigurationBase EncodingConfiguration;

        /// <summary>
        ///     Initialize the message builder base class.
        /// </summary>
        internal BuilderBase()
        {
            EncodingConfiguration = new BuilderEncodingConfiguration(this);
            Index = 0;
        }

        /// <summary>
        ///     Initialize the message builder base class.
        /// </summary>
        /// <param name="config">Message's encoding configuration.</param>
        /// <param name="index">Index in the parent.</param>
        internal BuilderBase(EncodingConfigurationBase config, int index)
        {
            EncodingConfiguration = config;
            Index = index;
        }

        /// <summary>
        ///     Get or set the character used to separate component-level content.
        /// </summary>
        public virtual char ComponentDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to signify escape sequences.
        /// </summary>
        public virtual char EscapeDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to separate fields.
        /// </summary>
        public virtual char FieldDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to separate field repetition content.
        /// </summary>
        public virtual char RepetitionDelimiter { get; set; }

        /// <summary>
        ///     Get or set the character used to separate subcomponent-level content.
        /// </summary>
        public virtual char SubcomponentDelimiter { get; set; }

        /// <summary>
        ///     Compare this builder's value with another object's value. (IComparable support)
        /// </summary>
        /// <param name="obj">Other BuilderBase.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return obj == null
                ? 1
                : CompareTo(obj.ToString());
        }

        /// <summary>
        ///     Compare this builder's value with another element's value. (element IComparable support)
        /// </summary>
        /// <param name="other">Other element to compare to.</param>
        /// <returns></returns>
        public int CompareTo(IElement other)
        {
            return other == null
                ? 1
                : CompareTo(other.Value);
        }

        /// <summary>
        ///     Compare this builder's value with another string. (generic IComparable support)
        /// </summary>
        /// <param name="other">Other string to compare to.</param>
        /// <returns></returns>
        public int CompareTo(string other)
        {
            return string.Compare(Value, other, StringComparison.CurrentCulture);
        }

        /// <summary>
        ///     Get the index at which this builder is located in its descendant.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        ///     Deep clone this element.
        /// </summary>
        /// <returns>Cloned element.</returns>
        public abstract IElement Clone();

        /// <summary>
        ///     Get or set this element's value.
        /// </summary>
        public abstract string Value { get; set; }

        /// <summary>
        ///     Get or set this element's sub-values.
        /// </summary>
        public abstract IEnumerable<string> Values { get; set; }

        /// <summary>
        ///     Get a converter which will interpret this element's value as other types.
        /// </summary>
        public abstract IEncodedTypeConverter As { get; }

        /// <summary>
        ///     Get the number of sub-values in this element.
        /// </summary>
        public abstract int ValueCount { get; }

        /// <summary>
        ///     Get this element's section delimiter.
        /// </summary>
        public abstract char Delimiter { get; }

        /// <summary>
        ///     Get the descendant builder at the specified index.
        /// </summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Descendant builder.</returns>
        public IElement this[int index]
        {
            get { return GetGenericElement(index); }
        }

        /// <summary>
        ///     Get or set the value as a formatted string.
        /// </summary>
        public string FormattedValue
        {
            get { return TextConverter.ConvertToString(Value); }
            set { Value = TextConverter.ConvertFromString(value); }
        }

        /// <summary>
        ///     Get the ancestor element. Null if it's a root element.
        /// </summary>
        IElement IElement.Ancestor
        {
            get { return GetAncestor(); }
        }

        /// <summary>
        ///     Get descendant elements. For subcomponents, this will be empty.
        /// </summary>
        IEnumerable<IElement> IElement.Descendants
        {
            get { return GetDescendants(); }
        }

        /// <summary>
        ///     Determines whether this builder's value is equivalent to another element's value. (element IEquatable support)
        /// </summary>
        /// <param name="other">Object to compare to.</param>
        /// <returns>True, if objects are considered to be equivalent.</returns>
        public bool Equals(IElement other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Determine if this builder's value is equal to another string. (IEquatable support)
        /// </summary>
        /// <param name="other">Other string.</param>
        /// <returns>True, if the two are equivalent.</returns>
        public bool Equals(string other)
        {
            return string.Equals(Value, other, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Get the number of sub-values in this builder.
        /// </summary>
        /// <returns></returns>
        protected int GetValueCount()
        {
            return ValueCount;
        }

        /// <summary>
        ///     Get an HL7 escaped string.
        /// </summary>
        /// <param name="s">String to escape.</param>
        /// <returns>Escaped string.</returns>
        public string Escape(string s)
        {
            return EncodingConfiguration.Escape(s);
        }

        /// <summary>
        ///     Get an unescaped HL7 string.
        /// </summary>
        /// <param name="s">String to unescape.</param>
        /// <returns>Unescaped string.</returns>
        public string UnEscape(string s)
        {
            return EncodingConfiguration.UnEscape(s);
        }

        /// <summary>
        ///     Get the element at the specified index as an IElement.
        /// </summary>
        /// <param name="index">Index at which to get the element.</param>
        /// <returns>Generic element.</returns>
        protected abstract IElement GetGenericElement(int index);

        /// <summary>
        ///     Determines whether this object is equivalent to another object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>True, if objects are considered to be equivalent.</returns>
        public override sealed bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (ReferenceEquals(null, obj))
            {
                return Value == null;
            }
            return Value == obj.ToString();
        }

        /// <summary>
        ///     Get this builder's hash code.
        /// </summary>
        /// <returns>Hash code for the builder.</returns>
        public override sealed int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        ///     Get this builder's contents as a string.
        /// </summary>
        /// <returns>Builder's contents.</returns>
        public override sealed string ToString()
        {
            return Value;
        }

        /// <summary>
        ///     Get the ancestor element.
        /// </summary>
        /// <returns>Ancestor element.</returns>
        protected virtual IElement GetAncestor()
        {
            return null;
        }

        /// <summary>
        ///     Get descendant elements.
        /// </summary>
        /// <returns>Descendant elements.</returns>
        protected virtual IEnumerable<IElement> GetDescendants()
        {
            return new ProxyEnumerable<IElement>(index => this[index],
                null,
                GetValueCount,
                1);
        }
    }
}