namespace SharpHostsFile
{
    /// <summary>
    ///     Represents a hosts file comment entry.
    /// </summary>
    public class HostsFileComment : HostsFileEntryBase
    {
        /// <summary>
        ///     Initializes a new instance of the SharpHostsFile.HostsFileComment class.
        /// </summary>
        /// <param name="comment">The hosts file comment.</param>
        public HostsFileComment(string comment) : this(string.Empty, comment)
        {
        }

        /// <summary>
        ///     Initializes a new HostsFileComment object.
        /// </summary>
        /// <param name="rawLine">The raw host entry line.</param>
        /// <param name="comment">The hosts file comment.</param>
        public HostsFileComment(string rawLine, string comment) : base(rawLine)
        {
            RawLine = rawLine;
            Comment = comment;
        }

        /// <summary>
        ///     Hosts file comment.
        /// </summary>
        public string Comment { get; set; }

        #region Overrides of HostsFileEntryBase

        /// <summary>
        ///     Returns the hosts file comment.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Comment;
        }

        #endregion

        #region Implementation of HostsFileEntryBase

        /// <summary>
        ///     Returns the string reprsentation of the hosts entry.
        /// </summary>
        /// <param name="preserveFormatting">Preserves formatting, including whitespace of raw entry line.</param>
        /// <returns></returns>
        public override string ToString(bool preserveFormatting)
        {
            return preserveFormatting ? RawLine : Comment;
        }

        #endregion
    }
}