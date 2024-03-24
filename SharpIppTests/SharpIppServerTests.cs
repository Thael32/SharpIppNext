﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpIpp;
using Moq;
using SharpIpp.Exceptions;
using SharpIpp.Models;
using SharpIpp.Protocol;
using SharpIpp.Protocol.Models;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System;
using SharpIppTests.Extensions;

namespace SharpIpp.Tests;

[TestClass]
[ExcludeFromCodeCoverage]
public class SharpIppServerTests
{
    [TestMethod]
    public void Constructor_Default_InstanceShouldBeCreated()
    {
        // Arrange & Act
        SharpIppServer server = new();
        // Assert
        server.Should().NotBeNull();
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_CancelJob_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.CancelJob,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "ipp://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 123 ),
            new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.RequestingUserName, "test-user" )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new CancelJobRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            JobId = 123,
            PrinterUri = new Uri( "ipp://127.0.0.1:631/" ),
            RequestingUserName = "test-user"
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_CreateJob_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.CreateJob,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "ipp://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 123 ),
            new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.RequestingUserName, "test-user" )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new CreateJobRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "ipp://127.0.0.1:631/" ),
            RequestingUserName = "test-user",
            NewJobAttributes = new()
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_GetCUPSPrinters_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.GetCUPSPrinters,
            Version = IppVersion.V1_1,
            RequestId = 123
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new CUPSGetPrintersRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user"
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_UnsupportedOperation_ShouldThrowError()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.Reserved1,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        await act.Should().ThrowAsync<IppRequestException>();
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_MessageIsNull_ShouldThrowError()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( request: null );
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestMethod()]
    public async Task ReceiveRawRequestAsync_Stream_ShouldReturnMessage()
    {
        // Arrange
        Mock<IIppProtocol> protocol = new();
        IppRequestMessage message = new();
        protocol.Setup( x => x.ReadIppRequestAsync( It.IsAny<Stream>(), It.IsAny<CancellationToken>() ) ).ReturnsAsync( message );
        SharpIppServer server = new( protocol.Object );
        // Act
        Func<Task<IIppRequestMessage>> act = async () => await server.ReceiveRawRequestAsync( Stream.Null );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().Be( message );
    }

    [TestMethod()]
    public async Task ReceiveRawRequestAsync_Null_ShouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Func<Task<IIppRequestMessage>> act = async () => await server.ReceiveRawRequestAsync( null );
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [TestMethod()]
    public void ValidateRawRequest_RequestIdIsInvalid_ShoouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage message = new()
        {
            IppOperation = IppOperation.CreateJob,
            Version = IppVersion.V1_1,
            RequestId = -123,
        };
        message.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "ipp://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 123 ),
            new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.RequestingUserName, "test-user" )
        ] );
        // Act
        Action act = () => server.ValidateRawRequest( message );
        // Assert
        act.Should().Throw<IppRequestException>()
            .WithMessage( "Bad request-id value" )
            .Which.RequestMessage.Should().Be( message );
    }

    [TestMethod()]
    public void ValidateRawRequest_EmptyListOfOperations_ShoouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage message = new()
        {
            IppOperation = IppOperation.CreateJob,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        // Act
        Action act = () => server.ValidateRawRequest( message );
        // Assert
        act.Should().Throw<IppRequestException>()
            .WithMessage( "No Operation Attributes" )
            .Which.RequestMessage.Should().Be( message );
    }

    [TestMethod()]
    public void ValidateRawRequest_InvalidFirstOperationAttribute_ShoouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage message = new()
        {
            IppOperation = IppOperation.CreateJob,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        message.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "ipp://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 123 ),
            new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.RequestingUserName, "test-user" )
        ] );
        // Act
        Action act = () => server.ValidateRawRequest( message );
        // Assert
        act.Should().Throw<IppRequestException>()
            .WithMessage( "attributes-charset MUST be the first attribute" )
            .Which.RequestMessage.Should().Be( message );
    }

    [TestMethod()]
    public void ValidateRawRequest_InvalidSecondOperationAttribute_ShoouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage message = new()
        {
            IppOperation = IppOperation.CreateJob,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        message.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "ipp://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 123 ),
            new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.RequestingUserName, "test-user" )
        ] );
        // Act
        Action act = () => server.ValidateRawRequest( message );
        // Assert
        act.Should().Throw<IppRequestException>()
            .WithMessage( "attributes-natural-language MUST be the second attribute" )
            .Which.RequestMessage.Should().Be( message );
    }

    [TestMethod()]
    public void ValidateRawRequest_MissingSecondOperationAttribute_ShoouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage message = new()
        {
            IppOperation = IppOperation.CreateJob,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        message.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" )
        ] );
        // Act
        Action act = () => server.ValidateRawRequest( message );
        // Assert
        act.Should().Throw<IppRequestException>()
            .WithMessage( "attributes-natural-language MUST be the second attribute" )
            .Which.RequestMessage.Should().Be( message );
    }

    [TestMethod()]
    public void ValidateRawRequest_InvalidIppVersion_ShoouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage message = new()
        {
            IppOperation = IppOperation.CreateJob,
            Version = new IppVersion( 0, 9 ),
            RequestId = 123,
        };
        message.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "ipp://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 123 ),
            new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.RequestingUserName, "test-user" )
        ] );
        // Act
        Action act = () => server.ValidateRawRequest( message );
        // Assert
        act.Should().Throw<IppRequestException>()
            .WithMessage( "Unsupported IPP version" )
            .Which.RequestMessage.Should().Be( message );
    }

    [TestMethod()]
    public void ValidateRawRequest_MissingPrinterUriAttribute_ShouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage message = new()
        {
            IppOperation = IppOperation.CreateJob,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        message.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 123 ),
            new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.RequestingUserName, "test-user" )
        ] );
        // Act
        Action act = () => server.ValidateRawRequest( message );
        // Assert
        act.Should().Throw<IppRequestException>()
            .WithMessage( "No printer-uri operation attribute" )
            .Which.RequestMessage.Should().Be( message );
    }

    [TestMethod()]
    public void ValidateRawRequest_MessageIsNull_ShouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Action act = () => server.ValidateRawRequest( null );
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod()]
    public void ValidateRawRequest_Message_ShouldBeSuccess()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage message = new()
        {
            IppOperation = IppOperation.CreateJob,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        message.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "ipp://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 123 ),
            new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.RequestingUserName, "test-user" )
        ] );
        // Act
        Action act = () => server.ValidateRawRequest( message );
        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_GetJobAttributes_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.GetJobAttributes,
            RequestId = 123,
            Version = IppVersion.V1_1
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 456 )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new GetJobAttributesRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user",
            JobId = 456
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_GetJobs_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.GetJobs,
            RequestId = 123,
            Version = IppVersion.V1_1
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new GetJobsRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user"
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_GetPrinterAttributes_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.GetPrinterAttributes,
            RequestId = 123,
            Version = IppVersion.V1_1
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new GetPrinterAttributesRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user"
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_HoldJob_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.HoldJob,
            RequestId = 123,
            Version = IppVersion.V1_1
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 234 )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new HoldJobRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user",
            JobId = 234
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_PausePrinter_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.PausePrinter,
            RequestId = 123,
            Version = IppVersion.V1_1
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new PausePrinterRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user"
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_PrintJob_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        using MemoryStream memoryStream = new();
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.PrintJob,
            RequestId = 123,
            Version = IppVersion.V1_1,
            Document = memoryStream
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new Models.PrintJobRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user",
            Document = memoryStream,
            NewJobAttributes = new(),
            DocumentAttributes = new()
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_PrintUri_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        var uri = new Uri( "http://test.com/document.pdf" );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.PrintUri,
            RequestId = 123,
            Version = IppVersion.V1_1
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" ),
            new IppAttribute( Tag.Uri, JobAttribute.DocumentUri, uri.AbsoluteUri )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new PrintUriRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user",
            DocumentUri = uri,
            NewJobAttributes = new(),
            DocumentAttributes = new()
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_PurgeJobs_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.PurgeJobs,
            RequestId = 123,
            Version = IppVersion.V1_1
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new PurgeJobsRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user"
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_ReleaseJob_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.ReleaseJob,
            RequestId = 123,
            Version = IppVersion.V1_1
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 234 )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new ReleaseJobRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user",
            JobId = 234
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_RestartJob_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.RestartJob,
            RequestId = 123,
            Version = IppVersion.V1_1
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 456 )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new RestartJobRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user",
            JobId = 456
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_ResumePrinter_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.ResumePrinter,
            RequestId = 123,
            Version = IppVersion.V1_1
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new ResumePrinterRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user"
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_SendDocument_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        using MemoryStream memoryStream = new();
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.SendDocument,
            RequestId = 123,
            Version = IppVersion.V1_1,
            Document = memoryStream
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 456 ),
            new IppAttribute( Tag.Boolean, JobAttribute.LastDocument, false )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new SendDocumentRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user",
            JobId = 456,
            Document = memoryStream,
            DocumentAttributes = new()
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_SendUri_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        var uri = new Uri( "http://test.com/document.pdf" );
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.SendUri,
            RequestId = 123,
            Version = IppVersion.V1_1
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" ),
            new IppAttribute( Tag.Uri, JobAttribute.DocumentUri, uri.AbsoluteUri ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 456 ),
            new IppAttribute( Tag.Boolean, JobAttribute.LastDocument, false )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new SendUriRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user",
            JobId = 456,
            DocumentUri = uri,
            DocumentAttributes = new()
        } );
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_ValidateJob_ShouldBeMapped()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        using MemoryStream memoryStream = new();
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.ValidateJob,
            RequestId = 123,
            Version = IppVersion.V1_1,
            Document = memoryStream
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, "test-user" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "http://127.0.0.1:631/" )
        ] );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( ippRequestMessage );
        // Assert
        (await act.Should().NotThrowAsync()).Which.Should().BeEquivalentTo( new ValidateJobRequest
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            PrinterUri = new Uri( "http://127.0.0.1:631" ),
            RequestingUserName = "test-user",
            Document = memoryStream,
            NewJobAttributes = new(),
            DocumentAttributes = new()
        } );
    }

    [TestMethod()]
    public void SendRawResponseAsync_MessageIsNull_ShouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Action act = () => server.SendRawResponseAsync( null, Stream.Null );
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod()]
    public void SendRawResponseAsync_StreamIsNull_ShouldThrowException()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Action act = () => server.SendRawResponseAsync( new IppResponseMessage(), null );
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod()]
    public void SendRawResponseAsync_Data_ShouldBeSuccess()
    {
        // Arrange
        SharpIppServer server = new( Mock.Of<IIppProtocol>() );
        // Act
        Action act = () => server.SendRawResponseAsync( new IppResponseMessage(), Stream.Null );
        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public async Task ReceiveRequestAsync_Stream_ShouldBeSuccess()
    {
        // Arrange
        Mock<IIppProtocol> ippProtocol = new();
        IppRequestMessage ippRequestMessage = new()
        {
            IppOperation = IppOperation.CreateJob,
            Version = IppVersion.V1_1,
            RequestId = 123,
        };
        ippRequestMessage.OperationAttributes.AddRange(
        [
            new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ),
            new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ),
            new IppAttribute( Tag.Uri, JobAttribute.PrinterUri, "ipp://127.0.0.1:631/" ),
            new IppAttribute( Tag.Integer, JobAttribute.JobId, 123 ),
            new IppAttribute( Tag.TextWithoutLanguage, JobAttribute.RequestingUserName, "test-user" )
        ] );
        ippProtocol.Setup( x => x.ReadIppRequestAsync( It.IsAny<Stream>(), It.IsAny<CancellationToken>() ) ).ReturnsAsync( ippRequestMessage );
        SharpIppServer server = new( ippProtocol.Object );
        // Act
        Func<Task<IIppRequest>> act = async () => await server.ReceiveRequestAsync( Stream.Null );
        // Assert
        await act.Should().NotThrowAsync();
    }

    [TestMethod()]
    public async Task SendResponseAsync_MesageIsNull_ShouldThrowException()
    {
        // Arrange
        SharpIppServer server = new(Mock.Of<IIppProtocol>());
        // Act
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        Func<Task> act = async () => await server.SendResponseAsync((IIppResponseMessage)null, Stream.Null);
#pragma warning restore CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [TestMethod()]
    public async Task SendResponseAsync_CreateJobResponse_ShouldBeMapped()
    {
        // Arrange
        Mock<IIppProtocol> ippProtocol = new();
        SharpIppServer server = new( ippProtocol.Object );
        var message = new CreateJobResponse
        {
            RequestId = 123,
            Version = IppVersion.V1_1,
            StatusCode = IppStatusCode.SuccessfulOk,
            JobId = 234,
            JobUri = "http://127.0.0.1:631/234",
            JobState = JobState.Pending,
            JobStateMessage = "custom state",
            NumberOfInterveningJobs = 0
        };
        var rawMessage = new IppResponseMessage
        {
            RequestId = 123
        };
        var operationSection = new IppSection { Tag = SectionTag.OperationAttributesTag };
        operationSection.Attributes.Add( new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ) );
        operationSection.Attributes.Add( new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ) );
        rawMessage.Sections.Add( operationSection );
        var jobSection = new IppSection { Tag = SectionTag.JobAttributesTag };
        jobSection.Attributes.Add(new IppAttribute(Tag.Uri, JobAttribute.JobUri, "http://127.0.0.1:631/234"));
        jobSection.Attributes.Add(new IppAttribute(Tag.Integer, JobAttribute.JobId, 234));
        jobSection.Attributes.Add(new IppAttribute(Tag.Enum, JobAttribute.JobState, (int)JobState.Pending));
        jobSection.Attributes.Add(new IppAttribute(Tag.TextWithoutLanguage, JobAttribute.JobStateMessage, "custom state"));
        jobSection.Attributes.Add(new IppAttribute(Tag.Integer, JobAttribute.TimeAtProcessing, 0));
        rawMessage.Sections.Add(jobSection);
        // Act
        await server.SendResponseAsync( message, Stream.Null );
        // Assert
        ippProtocol.Verify( x => x.WriteIppResponseAsync(
            It.Is<IppResponseMessage>( x => x.VerifyAssertionScope( _ => x.Should().BeEquivalentTo( rawMessage, "" ) ) ),
            It.IsAny<Stream>(),
            It.IsAny<CancellationToken>() ) );
    }
}