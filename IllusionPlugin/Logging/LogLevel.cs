using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IllusionPlugin.Logging
{
    /// <summary>
    /// Levels (types) of log for the logger.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// A debug message.
        /// </summary>
        Debug,
        /// <summary>
        /// An important message.
        /// </summary>
        Notice,
        /// <summary>
        /// A warning message from something that didn't cause an error but might cause one.
        /// </summary>
        Warning,
        /// <summary>
        /// An error message.
        /// </summary>
        Error
    }
}
