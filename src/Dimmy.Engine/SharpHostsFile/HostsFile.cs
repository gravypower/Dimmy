using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SharpHostsFile
{
    /// <summary>
    ///     Represents a Windows hosts file.
    /// </summary>
    public class HostsFile : IList<HostsFileEntryBase>
    {
        private readonly List<HostsFileEntryBase> _entries = new List<HostsFileEntryBase>();

        /// <summary>
        ///     Hosts file entries.
        /// </summary>
        public IReadOnlyCollection<HostsFileEntryBase> Entries => _entries.AsReadOnly();

        /// <summary>
        ///     Reads a hosts file from the specified location.
        /// </summary>
        /// <param name="fileName">The file path for the hosts file.</param>
        public void Load(string fileName)
        {
            _entries.Clear();

            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            if (!File.Exists(fileName))
                throw new FileNotFoundException($"Hosts file not found at {fileName}");

            var counter = 1;

            using (TextReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var type = GetHostsFileEntryType(line);

                    HostsFileEntryBase entry = null;

                    if (type != null)
                        if (type == typeof(HostsFileWhitespace))
                        {
                            entry = new HostsFileWhitespace(line) {LineNumber = counter};
                        }

                        else if (type == typeof(HostsFileComment))
                        {
                            entry = new HostsFileComment(line, line) {LineNumber = counter};
                        }

                        else if (type == typeof(HostsFileMapEntry))
                        {
                            var match = HostsFileMapEntry.Pattern.Match(line);

                            if (match.Success)
                                entry = new HostsFileMapEntry(line,
                                    IPAddress.Parse(match.Groups["address"].Value),
                                    match.Groups["hostname"].Value,
                                    match.Groups["comment"].Value) {LineNumber = counter};
                        }

                        else
                        {
                            entry = new HostsFileUnknownEntry(line);
                        }

                    if (entry != null)
                        _entries.Add(entry);

                    counter++;
                }
            }
        }

        /// <summary>
        ///     Saves a hosts file to the specified location.
        /// </summary>
        /// <param name="fileName">The file path for the hosts file.</param>
        /// <param name="preserveFormatting">Preserves any formatting, including whitespace for hosts file entries.</param>
        public void Save(string fileName, bool preserveFormatting = true)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            using TextWriter writer = new StreamWriter(fileName);
            
            foreach (var entry in _entries)
            {
                writer.WriteLine(entry.ToString(preserveFormatting));
            }
        }

        /// <summary>
        ///     Searches for an entry that matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The System.Predicate`1 delegate that defines the conditions of the elements to search for.</param>
        /// <returns></returns>
        public HostsFileEntryBase Find(Predicate<HostsFileEntryBase> match)
        {
            return _entries.Find(match);
        }

        /// <summary>
        ///     Retrieves all the entries that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The System.Predicate`1 delegate that defines the conditions of the elements to search for.</param>
        /// <returns></returns>
        public List<HostsFileEntryBase> FindAll(Predicate<HostsFileEntryBase> match)
        {
            return _entries.FindAll(match);
        }

        /// <summary>
        ///     Returns the default system hosts file path.
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultHostsFilePath()
        {
            var hostsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers\\etc\\hosts");

            if (!File.Exists(hostsPath))
                throw new FileNotFoundException($"Hosts file not found at {hostsPath}");
            return hostsPath;
        }

        private static Type GetHostsFileEntryType(string line)
        {
            if (string.IsNullOrEmpty(line) || line.Trim().Length == 0)
                return typeof(HostsFileWhitespace);
            return line.TrimStart().StartsWith("#")
                ? typeof(HostsFileComment)
                : typeof(HostsFileMapEntry);
        }

        /// <summary>
        /// </summary>
        /// <param name="entries"></param>
        public void RemoveAll(IEnumerable<HostsFileEntryBase> entries)
        {
            foreach (var entry in entries)
            {
                Remove(entry);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="entries"></param>
        public void AddAll(IEnumerable<HostsFileEntryBase> entries)
        {
            foreach (var entry in entries)
            {
                Add(entry);
            }
        }

        #region Implementation of IEnumerable

        /// <summary>
        ///     Returns an enumerator that iterates through the host entries.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<HostsFileEntryBase> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<HostsFileEntryBase>

        /// <summary>
        ///     Adds an entry to the hosts file.
        /// </summary>
        /// <param name="entry">The hosts entry to add.</param>
        public void Add(HostsFileEntryBase entry)
        {
            _entries.Add(entry);
        }

        /// <summary>
        ///     Clears all host entries.
        /// </summary>
        public void Clear()
        {
            _entries.Clear();
        }

        /// <summary>
        ///     Determines whether an entry exists in the hosts file.
        /// </summary>
        /// <param name="entry">The host entry to check for.</param>
        public bool Contains(HostsFileEntryBase entry)
        {
            return _entries.Contains(entry);
        }

        /// <summary>
        ///     Copies the entire System.Collections.Generic.List`1 to a compatible one-dimensional array, starting at the
        ///     specified index of the target array.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional System.Array that is the destination of the elements copied from
        ///     System.Collections.Generic.List`1. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(HostsFileEntryBase[] array, int arrayIndex)
        {
            _entries.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Removes a host entry from the hosts file.
        /// </summary>
        /// <param name="entry">The host entry to remove.</param>
        /// <returns>Returns true if the item is removed, otherwise false.</returns>
        public bool Remove(HostsFileEntryBase entry)
        {
            return _entries.Remove(entry);
        }

        /// <summary>
        ///     Gets the number of host entries.
        /// </summary>
        public int Count => _entries.Count;

        /// <summary>
        ///     Determines if the hosts file is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        #endregion

        #region Implementation of IList<HostsFileEntryBase>

        /// <summary>
        ///     Searches for the specified entry and returns the zero-based index of the first occurance.
        /// </summary>
        /// <param name="entry">The entry to search for.</param>
        /// <returns></returns>
        public int IndexOf(HostsFileEntryBase entry)
        {
            return _entries.IndexOf(entry);
        }

        /// <summary>
        ///     Inserts an entry at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert the entry.</param>
        /// <param name="entry">The entry to insert.</param>
        public void Insert(int index, HostsFileEntryBase entry)
        {
            _entries.Insert(index, entry);
        }

        /// <summary>
        ///     Removes the entry at the specified index;
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            _entries.RemoveAt(index);
        }

        /// <summary>
        ///     Gets or sets the entry at the specified index.
        /// </summary>
        /// <param name="index">The index of the entry to get or set.</param>
        public HostsFileEntryBase this[int index]
        {
            get => _entries[index];
            set => _entries[index] = value;
        }

        #endregion
    }
}