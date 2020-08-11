using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace SharpHostsFile
{
    /// <summary>
    ///     Represents a hosts file map entry.
    /// </summary>
    public class HostsFileMapEntry : HostsFileEntryBase
    {
        /// <summary>
        ///     Pattern to match hosts file map entry.
        /// </summary>
        public static readonly Regex Pattern = new Regex(@"^\s*(?<address>\S+)\s+(?<hostname>\S+)\s*(\#\s*(?<comment>\S+))?$", RegexOptions.Compiled);

        /// <summary>
        ///     Initializes a new instance of the SharpHostsFile.HostsFileMapEntry class.
        /// </summary>
        /// <param name="address">Host entry IP address.</param>
        /// <param name="hostname">Host entry hostname.</param>
        /// <param name="comment">Host entry comment.</param>
        public HostsFileMapEntry(IPAddress address, string hostname, string comment = null) : this(string.Empty, address, hostname, comment)
        {
        }

        /// <summary>
        ///     Initializes a new HostsFileMapEntry object.
        /// </summary>
        /// <param name="rawLine">The raw host entry line.</param>
        /// <param name="address">Host entry IP address.</param>
        /// <param name="hostname">Host entry hostname.</param>
        /// <param name="comment">Host entry comment.</param>
        public HostsFileMapEntry(string rawLine, IPAddress address, string hostname, string comment = null) : base(rawLine)
        {
            RawLine = rawLine;
            Address = address;
            Hostname = hostname;
            Comment = comment;
        }

        /// <summary>
        ///     Host entry IP address.
        /// </summary>
        public IPAddress Address { get; set; }

        /// <summary>
        ///     Host entry hostname.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        ///     Host entry comment.
        /// </summary>
        public string Comment { get; set; }

        #region Implementation of HostsFileEntryBase

        /// <summary>
        ///     Returns the string reprsentation of the hosts entry.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Address} {Hostname}{(Comment != null ? $" # {Comment}" : "")}";
        }

        /// <summary>
        ///     Returns the string reprsentation of the hosts entry.
        /// </summary>
        /// <param name="preserveFormatting">Preserves formatting, including whitespace of raw entry line.</param>
        /// <returns></returns>
        public override string ToString(bool preserveFormatting)
        {
            if (!string.IsNullOrEmpty(RawLine) && preserveFormatting)
            {
                var replaced = RegexHelper.ReplaceNamedGroups(RawLine, new Dictionary<string, string>
                    {
                        {"address", Address.ToString()},
                        {"hostname", Hostname},
                        {"comment", Comment}
                    },
                    Pattern);

                return replaced;
            }
            return ToString();
        }

        #endregion
    }
}