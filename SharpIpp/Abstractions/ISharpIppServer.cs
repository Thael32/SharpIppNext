﻿using SharpIpp.Models;
using SharpIpp.Protocol;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpIpp;

public interface ISharpIppServer
{
    Task<IIppRequestMessage> ReceiveRawRequestAsync(Stream stream, CancellationToken cancellationToken = default);
    Task<IIppRequest> ReceiveRequestAsync(IIppRequestMessage request, CancellationToken cancellationToken = default);
    Task<IIppRequest> ReceiveRequestAsync(Stream stream, CancellationToken cancellationToken = default);
    Task SendRawResponseAsync(IIppResponseMessage ippResponseMessage, Stream stream, CancellationToken cancellationToken = default);
    Task SendResponseAsync<T>(T ippResponsMessage, Stream stream, CancellationToken cancellationToken = default) where T : IIppResponseMessage;
    void ValidateRawRequest(IIppRequestMessage request);
}