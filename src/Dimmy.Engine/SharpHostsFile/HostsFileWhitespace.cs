namespace SharpHostsFile
{
    /// <summary>
    ///     Represents a whitespaced hosts file entry.
    /// </summary>
    public class HostsFileWhitespace : HostsFileEntryBase
    {
        /// <summary>
        ///     Initializes a new instance of the SharpHostsFile.HostsFileWhitespace class.
        /// </summary>
        /// <param name="rawLine"></param>
        public HostsFileWhitespace(string rawLine) : base(rawLine)
        {
        }

        #region Implementation of HostsFileEntryBase

        /// <summary>
        ///     Returns the string reprsentation of the hosts entry.
        /// </summary>
        /// <param name="preserveFormatting">Preserves formatting, including whitespace of raw entry line.</param>
        /// <returns></returns>
        public override string ToString(bool preserveFormatting)
        {
            return RawLine;
        }

        #endregion
    }
}