﻿using SharpIpp.Mapping;
using SharpIpp.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpIpp.Models;
public class SendDocumentOperationAttributes : JobOperationAttributes
{
    /// <summary>
    ///     The client OPTIONALLY supplies this attribute.  The Printer
    ///     object MUST support this attribute.   It contains the client
    ///     supplied document name.  The document name MAY be different
    ///     than the Job name.  Typically, the client software
    ///     automatically supplies the document name on behalf of the end
    ///     user by using a file name or an application generated name.  If
    ///     this attribute is supplied, its value can be used in a manner
    ///     defined by each implementation.  Examples include: printed
    ///     along with the Job (job start sheet, page adornments, etc.),
    ///     used by accounting or resource tracking management tools, or
    ///     even stored along with the document as a document level
    ///     attribute.  IPP/1.1 does not support the concept of document
    ///     level attributes.
    /// </summary>
    /// <example>job63</example>
    /// <code>document-name</code>
    public string? DocumentName { get; set; }

    /// <summary>
    ///     The client OPTIONALLY supplies this attribute.  The Printer
    ///     object MUST support this attribute and the "compression-
    ///     supported" attribute (see section 4.4.32).  The client supplied
    ///     "compression" operation attribute identifies the compression
    ///     algorithm used on the document data. The following cases exist:
    /// </summary>
    /// <example>none</example>
    /// <code>compression</code>
    public Compression? Compression { get; set; }

    /// <summary>
    ///     The client OPTIONALLY supplies this attribute.  The Printer
    ///     object MUST support this attribute.  The value of this
    ///     attribute identifies the format of the supplied document data.
    ///     The following cases exist:
    /// </summary>
    /// <example>application/octet-stream</example>
    /// <code>document-format</code>
    public string? DocumentFormat { get; set; }

    /// <summary>
    ///     The client OPTIONALLY supplies this attribute.  The Printer
    ///     object OPTIONALLY supports this attribute. This attribute
    ///     specifies the natural language of the document for those
    ///     document-formats that require a specification of the natural
    ///     language in order to image the document unambiguously. There
    ///     are no particular values required for the Printer object to
    ///     support.
    /// </summary>
    public string? DocumentNaturalLanguage { get; set; }

    public bool LastDocument { get; set; }

    public static new T Create<T>(Dictionary<string, IppAttribute[]> dict, IMapperApplier mapper) where T : SendDocumentOperationAttributes, new()
    {
        var attributes = JobOperationAttributes.Create<T>(dict, mapper);
        attributes.DocumentName = mapper.MapFromDic<string?>(dict, JobAttribute.DocumentName);
        attributes.Compression = mapper.MapFromDic<Compression?>(dict, JobAttribute.Compression);
        attributes.DocumentFormat = mapper.MapFromDic<string?>(dict, JobAttribute.DocumentFormat);
        attributes.LastDocument = mapper.MapFromDic<bool>(dict, JobAttribute.LastDocument);
        attributes.DocumentNaturalLanguage = mapper.MapFromDic<string?>(dict, JobAttribute.DocumentFormat);
        return attributes;
    }

    public override IEnumerable<IppAttribute> GetIppAttributes(IMapperApplier mapper)
    {
        foreach (var attribute in base.GetIppAttributes(mapper))
            yield return attribute;
        if (DocumentName != null)
            yield return new IppAttribute(Tag.NameWithoutLanguage, JobAttribute.DocumentName, DocumentName);
        if (Compression != null)
            yield return new IppAttribute(Tag.Keyword, JobAttribute.Compression, mapper.Map<string>(Compression));
        if (DocumentFormat != null)
            yield return new IppAttribute(Tag.MimeMediaType, JobAttribute.DocumentFormat, DocumentFormat);
        yield return new IppAttribute(Tag.Boolean, JobAttribute.LastDocument, LastDocument);
        if (DocumentNaturalLanguage != null)
            yield return new IppAttribute(Tag.NaturalLanguage, JobAttribute.DocumentNaturalLanguage, DocumentNaturalLanguage);
    }
}
