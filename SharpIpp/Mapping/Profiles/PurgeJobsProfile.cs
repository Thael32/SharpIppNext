﻿using SharpIpp.Models;
using SharpIpp.Protocol;
using SharpIpp.Protocol.Models;

namespace SharpIpp.Mapping.Profiles
{
    // ReSharper disable once UnusedMember.Global
    internal class PurgeJobsProfile : IProfile
    {
        public void CreateMaps(IMapperConstructor mapper)
        {
            mapper.CreateMap<PurgeJobsRequest, IppRequestMessage>((src, map) =>
            {
                var dst = new IppRequestMessage { IppOperation = IppOperation.PurgeJobs };
                map.Map<IIppPrinterRequest, IppRequestMessage>(src, dst);
                return dst;
            });

            mapper.CreateMap<IIppRequestMessage, PurgeJobsRequest>( ( src, map ) =>
            {
                var dst = new PurgeJobsRequest();
                map.Map<IIppRequestMessage, IIppPrinterRequest>( src, dst );
                return dst;
            } );

            mapper.CreateMap<IppResponseMessage, PurgeJobsResponse>((src, map) =>
            {
                var dst = new PurgeJobsResponse();
                map.Map<IppResponseMessage, IIppResponseMessage>(src, dst);
                return dst;
            });

            mapper.CreateMap<PurgeJobsResponse, IppResponseMessage>( ( src, map ) =>
            {
                var dst = new IppResponseMessage();
                map.Map<IIppResponseMessage, IppResponseMessage>( src, dst );
                return dst;
            } );
        }
    }
}
