//-------------------------------------------------------------------------------------------------
// <copyright file="InconclusiveTestAttribute.cs" company="Quantum">
//     Copyright (c) Quantum Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     An attribute for test case that is unstable enough to have its result considered inconclusive.
// </summary>
//
//-------------------------------------------------------------------------------------------------

namespace Quantum.NUnitExtension
{
    using System;

    /// <summary>
    /// An attribute for test case that is unstable enough to have its result considered inconclusive.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class InconclusiveTestAttribute : Attribute
    {
        /// <summary>
        /// Initializes an instance of InconclusiveTestAttribute.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        public InconclusiveTestAttribute(string ticket)
        {
            this.Ticket = ticket;
        }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        public string Ticket { get; set; }
    }
}
