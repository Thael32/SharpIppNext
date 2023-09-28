﻿using System;
using System.Collections.Generic;

using SharpIpp.Protocol.Models;

namespace SharpIpp.Models
{
    /// <summary>
    ///     <a href="https://datatracker.ietf.org/doc/html/rfc2911#section-3.2.6">Get-Jobs Operation</a>
    ///     This REQUIRED operation allows a client to retrieve the list of Job
    ///     objects belonging to the target Printer object.  The client may also
    ///     supply a list of Job attribute names and/or attribute group names.  A
    ///     group of Job object attributes will be returned for each Job object
    ///     that is returned.
    ///     This operation is similar to the Get-Job-Attributes operation, except
    ///     that this Get-Jobs operation returns attributes from possibly more
    ///     than one object.
    /// </summary>
    public class GetJobsRequest : IIppPrinterRequest
    {
        /// The client OPTIONALLY supplies this attribute.  The Printer
        /// object MUST support this attribute.  It indicates which Job
        /// objects MUST be returned by the Printer object. The values for
        /// this attribute are:
        public WhichJobs? WhichJobs { get; set; }

        /// The client OPTIONALLY supplies this attribute.  The Printer
        /// object MUST support this attribute. It is an integer value that
        /// determines the maximum number of jobs that a client will
        /// receive from the Printer even if "which-jobs" or "my-jobs"
        /// constrain which jobs are returned.  The limit is a "stateless
        /// limit" in that if the value supplied by the client is 'N', then
        /// only the first 'N' jobs are returned in the Get-Jobs Response.
        /// There is no mechanism to allow for the next 'M' jobs after the
        /// first 'N' jobs.  If the client does not supply this attribute,
        /// the Printer object responds with all applicable jobs.
        public int? Limit { get; set; }

        /// <summary>
        ///     The client OPTIONALLY supplies this attribute.  The Printer
        ///     object MUST support this attribute.  It is a set of Job
        ///     attribute names and/or attribute groups names in whose values
        ///     the requester is interested.  This set of attributes is
        ///     returned for each Job object that is returned.  The allowed
        ///     attribute group names are the same as those defined in the
        ///     Get-Job-Attributes operation in section 3.3.4.  If the client
        ///     does not supply this attribute, the Printer MUST respond as if
        ///     the client had supplied this attribute with two values: 'job-
        ///     uri' and 'job-id'.
        /// </summary>
        public string[]? RequestedAttributes { get; set; }

        /// <summary>
        ///     The client OPTIONALLY supplies this attribute.  The Printer
        ///     object MUST support this attribute.  It indicates whether jobs
        ///     from all users or just the jobs submitted by the requesting
        ///     user of this request MUST be considered as candidate jobs to be
        ///     returned by the Printer object.  If the client does not supply
        ///     this attribute, the Printer object MUST respond as if the
        ///     client had supplied the attribute with a value of 'false',
        ///     i.e., jobs from all users.  The means for authenticating the
        ///     requesting user and matching the jobs is described in section
        /// </summary>
        public bool? MyJobs { get; set; }

        public IppVersion Version { get; set; } = IppVersion.V1_1;

        public int RequestId { get; set; } = 1;

        public Uri PrinterUri { get; set; } = null!;

        /// <summary>
        ///     The "requesting-user-name" (name(MAX)) attribute SHOULD be
        ///     supplied by the client
        /// </summary>
        public string? RequestingUserName { get; set; }

        public IEnumerable<IppAttribute>? AdditionalOperationAttributes { get; set; }

        public IEnumerable<IppAttribute>? AdditionalJobAttributes { get; set; }
    }
}
