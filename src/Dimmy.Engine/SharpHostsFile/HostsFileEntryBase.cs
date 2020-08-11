namespace SharpHostsFile
{
    /// <summary>
    ///     Provides the abstract base class for a hosts file entry.
    /// </summary>
    public abstract class HostsFileEntryBase
    {
        /// <summary>
        ///     Initializes a new instance of the SharpHostsFile.HostsFileEntryBase class.
        /// </summary>
        /// <param name="rawLine"></param>
        protected HostsFileEntryBase(string rawLine)
        {
            RawLine = rawLine;
        }

        /// <summary>
        ///     Entry line number.
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        ///     The raw host entry line.
        /// </summary>
        public string RawLine { get; set; }

        #region Overrides of Object

        /// <summary>
        ///     Returns the raw host entry line.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return RawLine;
        }

        #endregion

        /// <summary>
        ///     Returns the string reprsentation of the hosts entry.
        /// </summary>
        /// <param name="preserveFormatting">Preserves formatting, including whitespace of raw entry line.</param>
        /// <returns></returns>
        public abstract string ToString(bool preserveFormatting);
    }
}