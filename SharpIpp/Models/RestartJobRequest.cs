﻿using System;
using System.Collections.Generic;

using SharpIpp.Protocol.Models;

namespace SharpIpp.Models
{
    /// <summary>
    ///     <a href="https://tools.ietf.org/html/rfc2911#section-3.3.7">Restart-Job Operation</a>
    ///     This OPTIONAL operation allows a client to restart a job that is
    ///     retained in the queue after processing has completed.
    ///     The job is moved to the 'pending' or 'pending-held' job state and
    ///     restarts at the beginning on the same IPP Printer object with the
    ///     same attribute values.  If any of the documents in the job were
    ///     passed by reference (Print-URI or Send-URI), the Printer MUST re-
    ///     fetch the data, since the semantics of Restart-Job are to repeat all
    ///     Job processing.  The Job Description attributes that accumulate job
    ///     progress, such as "job-impressions-completed", "job-media-sheets-
    ///     completed", and "job-k-octets-processed", MUST be reset to 0 so that
    ///     they give an accurate record of the job from its restart point.  The
    ///     job object MUST continue to use the same "job-uri" and "job-id"
    ///     attribute values.
    /// </summary>
    public class RestartJobRequest : IIppJobRequest
    {
        public IppVersion Version { get; set; } = IppVersion.V1_1;

        public int RequestId { get; set; } = 1;

        public Uri PrinterUri { get; set; } = null!;

        public Uri? JobUrl { get; set; }

        public int? JobId { get; set; }

        public string? RequestingUserName { get; set; }

        public IEnumerable<IppAttribute>? AdditionalOperationAttributes { get; set; }

        public IEnumerable<IppAttribute>? AdditionalJobAttributes { get; set; }
    }
}
