﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CliWrap.Builders
{
    /// <summary>
    /// Builder that helps generate a well-formed arguments string.
    /// </summary>
    public partial class ArgumentsBuilder
    {
        private static readonly CultureInfo DefaultCulture = CultureInfo.InvariantCulture;

        private readonly StringBuilder _buffer = new StringBuilder();

        /// <summary>
        /// Adds the specified value to the list of arguments.
        /// </summary>
        public ArgumentsBuilder Add(string value, bool escape)
        {
            if (_buffer.Length > 0)
                _buffer.Append(' ');

            _buffer.Append(escape
                ? Escape(value)
                : value
            );

            return this;
        }

        /// <summary>
        /// Adds the specified value to the list of arguments.
        /// </summary>
        // TODO: replace with optional argument when breaking changes are ok
        public ArgumentsBuilder Add(string value) =>
            Add(value, true);

        /// <summary>
        /// Adds the specified values to the list of arguments.
        /// </summary>
        public ArgumentsBuilder Add(IEnumerable<string> values, bool escape)
        {
            foreach (var value in values)
                Add(value, escape);

            return this;
        }

        /// <summary>
        /// Adds the specified values to the list of arguments.
        /// </summary>
        // TODO: replace with optional argument when breaking changes are ok
        public ArgumentsBuilder Add(IEnumerable<string> values) =>
            Add(values, true);

        /// <summary>
        /// Adds the specified value to the list of arguments.
        /// </summary>
        public ArgumentsBuilder Add(IFormattable value, CultureInfo cultureInfo, bool escape) =>
            Add(value.ToString(null, cultureInfo), escape);

        /// <summary>
        /// Adds the specified value to the list of arguments.
        /// </summary>
        // TODO: replace with optional argument when breaking changes are ok
        public ArgumentsBuilder Add(IFormattable value, CultureInfo cultureInfo) =>
            Add(value, cultureInfo, true);

        /// <summary>
        /// Adds the specified value to the list of arguments.
        /// The value is converted to string using invariant culture.
        /// </summary>
        public ArgumentsBuilder Add(IFormattable value, bool escape) =>
            Add(value, DefaultCulture, escape);

        /// <summary>
        /// Adds the specified value to the list of arguments.
        /// The value is converted to string using invariant culture.
        /// </summary>
        // TODO: replace with optional argument when breaking changes are ok
        public ArgumentsBuilder Add(IFormattable value) =>
            Add(value, true);

        /// <summary>
        /// Adds the specified values to the list of arguments.
        /// </summary>
        public ArgumentsBuilder Add(IEnumerable<IFormattable> values, CultureInfo cultureInfo, bool escape)
        {
            foreach (var value in values)
                Add(value, cultureInfo, escape);

            return this;
        }

        /// <summary>
        /// Adds the specified values to the list of arguments.
        /// </summary>
        // TODO: replace with optional argument when breaking changes are ok
        public ArgumentsBuilder Add(IEnumerable<IFormattable> values, CultureInfo cultureInfo) =>
            Add(values, cultureInfo, true);

        /// <summary>
        /// Adds the specified values to the list of arguments.
        /// The values are converted to string using invariant culture.
        /// </summary>
        public ArgumentsBuilder Add(IEnumerable<IFormattable> values, bool escape) =>
            Add(values, DefaultCulture, escape);

        /// <summary>
        /// Adds the specified values to the list of arguments.
        /// The values are converted to string using invariant culture.
        /// </summary>
        // TODO: replace with optional argument when breaking changes are ok
        public ArgumentsBuilder Add(IEnumerable<IFormattable> values) =>
            Add(values, true);

        /// <summary>
        /// Builds the resulting arguments string.
        /// </summary>
        public string Build() => _buffer.ToString();
    }

    public partial class ArgumentsBuilder
    {
        private static string Escape(string argument)
        {
            // Short circuit if argument is clean and doesn't need escaping
            if (argument.Length != 0 && argument.All(c => !char.IsWhiteSpace(c) && c != '"'))
                return argument;

            var buffer = new StringBuilder();

            buffer.Append('"');

            for (var i = 0; i < argument.Length;)
            {
                var c = argument[i++];

                if (c == '\\')
                {
                    var numBackSlash = 1;
                    while (i < argument.Length && argument[i] == '\\')
                    {
                        numBackSlash++;
                        i++;
                    }

                    if (i == argument.Length)
                    {
                        buffer.Append('\\', numBackSlash * 2);
                    }
                    else if (argument[i] == '"')
                    {
                        buffer.Append('\\', numBackSlash * 2 + 1);
                        buffer.Append('"');
                        i++;
                    }
                    else
                    {
                        buffer.Append('\\', numBackSlash);
                    }
                }
                else if (c == '"')
                {
                    buffer.Append('\\');
                    buffer.Append('"');
                }
                else
                {
                    buffer.Append(c);
                }
            }

            buffer.Append('"');

            return buffer.ToString();
        }
    }
}