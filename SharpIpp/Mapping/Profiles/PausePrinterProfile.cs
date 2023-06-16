﻿using SharpIpp.Models;
using SharpIpp.Protocol;
using SharpIpp.Protocol.Models;

namespace SharpIpp.Mapping.Profiles
{
    // ReSharper disable once UnusedMember.Global
    internal class PausePrinterProfile : IProfile
    {
        public void CreateMaps(IMapperConstructor mapper)
        {
            mapper.CreateMap<PausePrinterRequest, IppRequestMessage>((src, map) =>
            {
                var dst = new IppRequestMessage { IppOperation = IppOperation.PausePrinter };
                map.Map<IIppPrinterRequest, IppRequestMessage>(src, dst);
                return dst;
            });

            mapper.CreateMap<IppRequestMessage, PausePrinterRequest>( ( src, map ) =>
            {
                var dst = new PausePrinterRequest();
                map.Map<IppRequestMessage, IIppPrinterRequest>( src, dst );
                return dst;
            } );

            mapper.CreateMap<IppResponseMessage, PausePrinterResponse>((src, map) =>
            {
                var dst = new PausePrinterResponse();
                map.Map<IppResponseMessage, IIppResponseMessage>(src, dst);
                return dst;
            });

            mapper.CreateMap<PausePrinterResponse, IppResponseMessage>( ( src, map ) =>
            {
                var dst = new IppResponseMessage();
                map.Map<IIppResponseMessage, IppResponseMessage>( src, dst );
                return dst;
            } );
        }
    }
}
