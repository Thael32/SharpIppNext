using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

using NUnit.Framework;

using SharpIpp.Mapping;
using SharpIpp.Mapping.Profiles;
using SharpIpp.Models;
using SharpIpp.Protocol;
using SharpIpp.Protocol.Models;
using SharpIpp.Tests.Extensions;
using SharpIpp.Tests.Models;
using Snapper;

using Range = SharpIpp.Protocol.Models.Range;

namespace SharpIpp.Tests;

//[Snapper.Attributes.UpdateSnapshots(true)]
public class ConstructTests
{
    private const int JobId = 63;
    private static readonly Uri PrinterUrl = new("ipp://127.0.0.1:631");
    private static readonly Uri DocumentUri = new("http://example.com/document.pdf");

    private IMapper _mapper;

    [Test]
    public void ValidateJobAsync_Simple()
    {
        var file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Resources", "word-2-pages.pdf");
        using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
        var request = new ValidateJobRequest { PrinterUri = PrinterUrl, Document = fileStream };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void ValidateJobAsync_Full()
    {
        var file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Resources", "word-2-pages.pdf");
        using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);

        var request = new ValidateJobRequest
        {
            PrinterUri = PrinterUrl,
            Document = fileStream,
            DocumentAttributes = new DocumentAttributes
            {
                DocumentName = "Document Name",
                DocumentFormat = "application/octet-stream",
                DocumentNaturalLanguage = "en",
            },
            NewJobAttributes = new NewJobAttributes
            {
                JobName = "Test Job",
                IppAttributeFidelity = false,
                // JobPriority = 50, //unsupported most of the time
                // JobHoldUntil = JobHoldUntil.NoHold //unsupported most of the time
                MultipleDocumentHandling = MultipleDocumentHandling.SeparateDocumentsCollatedCopies,
                Copies = 1,
                Finishings = Finishings.None,
                PageRanges = new[] { new Range(1, 2) },
                Sides = Sides.OneSided,
                NumberUp = 1,
                OrientationRequested = Orientation.Portrait,
                PrinterResolution = new Resolution(600, 600, ResolutionUnit.DotsPerInch),
                PrintQuality = PrintQuality.Normal,
                PrintScaling = PrintScaling.Fit,
            },
        };


        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void CreateJobAsync_Simple()
    {
        var request = new CreateJobRequest { PrinterUri = PrinterUrl };

        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void CreateJobAsync_Full()
    {
        var request = new CreateJobRequest
        {
            PrinterUri = PrinterUrl,
            NewJobAttributes = new NewJobAttributes
            {
                JobName = "Test Job",
                IppAttributeFidelity = false,
                // JobPriority = 50, //unsupported most of the time
                // JobHoldUntil = JobHoldUntil.NoHold //unsupported most of the time
                MultipleDocumentHandling = MultipleDocumentHandling.SeparateDocumentsCollatedCopies,
                Copies = 1,
                Finishings = Finishings.None,
                PageRanges = new[] { new Range(1, 2) },
                Sides = Sides.OneSided,
                NumberUp = 1,
                OrientationRequested = Orientation.Portrait,
                PrinterResolution = new Resolution(600, 600, ResolutionUnit.DotsPerInch),
                PrintQuality = PrintQuality.Normal,
                PrintScaling = PrintScaling.Fit,
            },
        };

        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void PrintJobAsync_Simple()
    {
        var file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Resources", "word-2-pages.pdf");
        using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
        var request = new PrintJobRequest { PrinterUri = PrinterUrl, Document = fileStream };

        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void PrintJobAsync_Full()
    {
        var file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Resources", "EmptyTwoPage.pdf");
        using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);

        var request = new PrintJobRequest
        {
            PrinterUri = PrinterUrl,
            Document = fileStream,
            DocumentAttributes = new DocumentAttributes
            {
                DocumentName = "Document Name",
                DocumentFormat = "application/octet-stream",
                DocumentNaturalLanguage = "en",
            },
            NewJobAttributes = new NewJobAttributes
            {
                JobName = "Test Job",
                IppAttributeFidelity = false,
                // JobPriority = 50, //unsupported most of the time
                // JobHoldUntil = JobHoldUntil.NoHold //unsupported most of the time
                MultipleDocumentHandling = MultipleDocumentHandling.SeparateDocumentsCollatedCopies,
                Copies = 1,
                Finishings = Finishings.None,
                PageRanges = new[] { new Range(1, 2) },
                Sides = Sides.OneSided,
                NumberUp = 1,
                OrientationRequested = Orientation.Portrait,
                PrinterResolution = new Resolution(600, 600, ResolutionUnit.DotsPerInch),
                PrintQuality = PrintQuality.Normal,
                PrintScaling = PrintScaling.Fit,
            },
        };

        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void PrintUriAsync_Simple()
    {
        var request = new PrintUriRequest { PrinterUri = PrinterUrl, DocumentUri = DocumentUri };

        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void PrintUriAsync_Full()
    {
        var request = new PrintUriRequest
        {
            PrinterUri = PrinterUrl,
            DocumentUri = DocumentUri,
            DocumentAttributes = new DocumentAttributes
            {
                DocumentName = "Document Name",
                DocumentFormat = "application/octet-stream",
                DocumentNaturalLanguage = "en",
            },
            NewJobAttributes = new NewJobAttributes
            {
                JobName = "Test Job",
                IppAttributeFidelity = false,
                // JobPriority = 50, //unsupported most of the time
                // JobHoldUntil = JobHoldUntil.NoHold //unsupported most of the time
                MultipleDocumentHandling = MultipleDocumentHandling.SeparateDocumentsCollatedCopies,
                Copies = 1,
                Finishings = Finishings.None,
                PageRanges = new[] { new Range(1, 2) },
                Sides = Sides.OneSided,
                NumberUp = 1,
                OrientationRequested = Orientation.Portrait,
                PrinterResolution = new Resolution(600, 600, ResolutionUnit.DotsPerInch),
                PrintQuality = PrintQuality.Normal,
                PrintScaling = PrintScaling.Fit,
            },
        };

        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void GetPrinterAttributesAsync_Full()
    {
        var request = new GetPrinterAttributesRequest
        {
            PrinterUri = PrinterUrl, RequestedAttributes = new[] { PrinterAttribute.PagesPerMinute },
        };

        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void GetPrinterAttributesAsync_Simple()
    {
        var request = new GetPrinterAttributesRequest { PrinterUri = PrinterUrl };

        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void GetJobAttributesAsync()
    {
        var request = new GetJobAttributesRequest { PrinterUri = PrinterUrl, JobId = JobId };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void GetJobsAsync_Default()
    {
        var request = new GetJobsRequest { PrinterUri = PrinterUrl, WhichJobs = WhichJobs.Completed };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void GetJobsAsync_Full()
    {
        var request = new GetJobsRequest
        {
            PrinterUri = PrinterUrl,
            WhichJobs = WhichJobs.Completed,
            RequestedAttributes = new[]
            {
                JobAttribute.Compression,
                JobAttribute.Copies,
                JobAttribute.DateTimeAtCompleted,
                JobAttribute.DateTimeAtCreation,
                JobAttribute.DateTimeAtProcessing,
                JobAttribute.DocumentFormat,
                JobAttribute.DocumentName,
                JobAttribute.Finishings,
                JobAttribute.IppAttributeFidelity,
                JobAttribute.JobId,
                JobAttribute.JobImpressions,
                JobAttribute.JobImpressionsCompleted,
                JobAttribute.JobKOctetsProcessed,
                JobAttribute.JobMediaSheets,
                JobAttribute.JobMediaSheetsCompleted,
                JobAttribute.JobName,
                JobAttribute.JobOriginatingUserName,
                JobAttribute.JobOriginatingUserName,
                JobAttribute.JobPrinterUpTime,
                JobAttribute.JobPrinterUri,
                JobAttribute.JobSheets,
                JobAttribute.JobState,
                JobAttribute.JobStateMessage,
                JobAttribute.JobStateReasons,
                JobAttribute.Media,
                JobAttribute.MultipleDocumentHandling,
                JobAttribute.NumberUp,
                JobAttribute.OrientationRequested,
                JobAttribute.PrinterResolution,
                JobAttribute.PrintQuality,
                JobAttribute.Sides,
                JobAttribute.TimeAtCompleted,
                JobAttribute.TimeAtCreation,
                JobAttribute.TimeAtProcessing,
            },
        };

        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void PausePrinterAsync()
    {
        var request = new PausePrinterRequest { PrinterUri = PrinterUrl };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void ResumePrinterAsync()
    {
        var request = new ResumePrinterRequest { PrinterUri = PrinterUrl };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void PurgeJobsAsync()
    {
        var request = new PurgeJobsRequest { PrinterUri = PrinterUrl };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void CancelJobAsync()
    {
        var request = new CancelJobRequest { PrinterUri = PrinterUrl, JobId = JobId };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void HoldJobAsync()
    {
        var request = new HoldJobRequest { PrinterUri = PrinterUrl, JobId = JobId };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void ReleaseJobAsync()
    {
        var request = new ReleaseJobRequest { PrinterUri = PrinterUrl, JobId = JobId };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void RestartJobAsync()
    {
        var request = new RestartJobRequest { PrinterUri = PrinterUrl, JobId = JobId };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void SendUriAsync_Simple()
    {
        var request = new SendUriRequest { PrinterUri = PrinterUrl, JobId = JobId, DocumentUri = DocumentUri };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void SendUriAsync_Full()
    {
        var request = new SendUriRequest
        {
            PrinterUri = PrinterUrl,
            JobId = JobId,
            DocumentUri = DocumentUri,
            DocumentAttributes = new DocumentAttributes
            {
                DocumentName = "Document Name",
                DocumentFormat = "application/octet-stream",
                DocumentNaturalLanguage = "en",
            },
        };

        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void SendDocumentAsync_Simple()
    {
        var file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Resources", "word-2-pages.pdf");
        using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
        var request = new SendDocumentRequest { PrinterUri = PrinterUrl, JobId = JobId, Document = fileStream };
        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [Test]
    public void SendDocumentAsync_Full()
    {
        var file = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Resources", "word-2-pages.pdf");
        using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);

        var request = new SendDocumentRequest
        {
            PrinterUri = PrinterUrl,
            JobId = JobId,
            Document = fileStream,
            DocumentAttributes = new DocumentAttributes
            {
                DocumentName = "Document Name",
                DocumentFormat = "application/octet-stream",
                DocumentNaturalLanguage = "en",
            },
        };

        var result = _mapper.Map<IppRequestMessage>(request);
        CheckResult(result);
    }

    [OneTimeSetUp]
    public void TearUp()
    {
        _mapper = new SimpleMapper();
        var assembly = Assembly.GetAssembly(typeof(TypesProfile));
        _mapper.FillFromAssembly(assembly!);
    }

    public void CheckResult(IIppRequestMessage request)
    {
        Assert.NotNull(request);
        Test.AddJsonAttachment(request, "request.json");

        var options = new JsonSerializerOptions { WriteIndented = true, Converters = { new StreamConverter(), new IppVersionJsonConverter() } };
        var serialized = JsonSerializer.Serialize(request, options);

        Console.WriteLine(serialized);
        serialized.ShouldMatchSnapshot();
    }
}
