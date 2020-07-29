using System.IO;
using System.Threading.Tasks;
using NuGet.Common;

namespace Dimmy.Engine.NuGet
{
    public class TextWriterLogger : ILogger
    {
        private readonly TextWriter _writer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextWriterLogger" /> class.
        /// </summary>
        /// <param name="writer">TextWriter to write output to</param>
        public TextWriterLogger(TextWriter writer)
        {
            _writer = writer;
        }

        public void LogDebug(string data)
        {
            Write(LogLevel.Debug, data);
        }

        public void LogVerbose(string data)
        {
            Write(LogLevel.Verbose, data);
        }

        public void LogInformation(string data)
        {
            Write(LogLevel.Information, data);
        }

        public void LogMinimal(string data)
        {
            Write(LogLevel.Minimal, data);
        }

        public void LogWarning(string data)
        {
            Write(LogLevel.Warning, data);
        }

        public void LogError(string data)
        {
            Write(LogLevel.Error, data);
        }

        public void LogInformationSummary(string data)
        {
            Write(LogLevel.Information, data);
        }

        public void Log(LogLevel level, string data)
        {
            Write(level, data);
        }
        
        public Task LogAsync(LogLevel level, string data)
        {
            return Task.Run(() => Write(level, data));
        }

        public void Log(ILogMessage message)
        {
            //swallowing the nuget logging for the moment 
            //https://github.com/gravypower/Dimmy/issues/3
        }

        public Task LogAsync(ILogMessage message)
        {
            return Task.Run(() => Log(message));
        }
        
        private void Write(LogLevel level, string message)
        {
            //swallowing the nuget logging for the moment 
            //https://github.com/gravypower/Dimmy/issues/3
        }
    }
}