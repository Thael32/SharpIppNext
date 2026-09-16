using SharpIpp;
using SharpIpp.Models;
using SharpIpp.Protocol.Models;

try
{
    var httpClient = new HttpClient(new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    });
    var client = new SharpIppClient(httpClient);
    var request = new GetPrinterAttributesRequest()
    {
        Version = new IppVersion(2, 0),
        OperationAttributes = new GetPrinterAttributesOperationAttributes
        {
            PrinterUri = new Uri("ipps://192.168.0.197:631/ipp/print"),
            RequestedAttributes = [
                PrinterAttribute.PrinterName,
                PrinterAttribute.PrinterUuid,
                PrinterAttribute.PrinterInfo,
                PrinterAttribute.PrinterLocation,
                PrinterAttribute.DocumentFormatDefault,
                PrinterAttribute.DocumentFormatPreferred,
                PrinterAttribute.DocumentFormatSupported,
                PrinterAttribute.PrinterMakeAndModel,
                PrinterAttribute.IppVersionsSupported,
                PrinterAttribute.MopriaCertified,
                PrinterAttribute.PrinterFirmwareName,
                PrinterAttribute.PrinterFirmwareStringVersion,
                PrinterAttribute.MediaSupported,
                PrinterAttribute.MediaTypeSupported,
                PrinterAttribute.PrinterUriSupported,
                PrinterAttribute.UriSecuritySupported,
                "printer-device-id"
                ]
        }
    };

    var response = await client.GetPrinterAttributesAsync(request);
    var text = System.Text.Json.JsonSerializer.Serialize(response);
    Console.WriteLine(text);

    /*
    var filePath = @"C:\example.pdf";
    await using var stream = File.Open(filePath, FileMode.Open);
    var printJobRequest = new PrintJobRequest
    {
        Document = stream,
        OperationAttributes = new()
        {
            PrinterUri = new Uri("ipp://localhost:631/"),
            DocumentName = "Document Name",
            DocumentFormat = "application/octet-stream",
            Compression = Compression.None,
            DocumentNaturalLanguage = "en",
            JobName = "Test Job",
            IppAttributeFidelity = false
        },
        JobTemplateAttributes = new()
        {
            Copies = 1,
            MultipleDocumentHandling = MultipleDocumentHandling.SeparateDocumentsCollatedCopies,
            Finishings = Finishings.None,
            PageRanges = [new SharpIpp.Protocol.Models.Range(1, 1)],
            Sides = Sides.OneSided,
            NumberUp = 1,
            OrientationRequested = Orientation.Portrait,
            PrinterResolution = new Resolution(600, 600, ResolutionUnit.DotsPerInch),
            PrintQuality = PrintQuality.Normal
        }
    };
    var printJobresponse = await client.PrintJobAsync(printJobRequest);
    */
    Console.WriteLine("Success!");
}
catch (Exception ex)
{
    Console.WriteLine( ex );
}
Console.ReadKey();