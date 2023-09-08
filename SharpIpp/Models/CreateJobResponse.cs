﻿using System.Collections.Generic;

using SharpIpp.Protocol.Models;

namespace SharpIpp.Models
{
    public class CreateJobResponse : IIppJobResponse
    {
        public IppVersion Version { get; set; } = IppVersion.V11;

        public IppStatusCode StatusCode { get; set; }

        public int RequestId { get; set; } = 1;

        /// <summary>
        ///     job-uri
        /// </summary>
        public string JobUri { get; set; } = null!;

        /// <summary>
        ///     job-id
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        ///     job-state
        /// </summary>
        public JobState JobState { get; set; }

        /// <summary>
        ///     job-state-reasons
        /// </summary>
        public JobStateReason[] JobStateReasons { get; set; } = null!;

        /// <summary>
        ///     job-state-message
        /// </summary>
        public string? JobStateMessage { get; set; }

        /// <summary>
        ///     number-of-intervening-jobs
        /// </summary>
        public int? NumberOfInterveningJobs { get; set; }

        public List<IppSection> Sections { get; } = new List<IppSection>();
    }
}
