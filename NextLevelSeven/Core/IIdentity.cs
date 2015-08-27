﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    /// <summary>
    /// Interface that provides Application and Facility information.
    /// </summary>
    public interface IIdentity
    {
        /// <summary>
        /// Get or set the application name.
        /// </summary>
        string Application { get; set; }

        /// <summary>
        /// Get or set the facility name.
        /// </summary>
        string Facility { get; set; }
    }
}