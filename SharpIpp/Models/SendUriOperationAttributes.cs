﻿using SharpIpp.Mapping;
using SharpIpp.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpIpp.Models;
public class SendUriOperationAttributes : SendDocumentOperationAttributes
{
    public Uri? DocumentUri { get; set; }

    public static new T Create<T>(Dictionary<string, IppAttribute[]> dict, IMapperApplier mapper) where T : SendUriOperationAttributes, new()
    {
        var attributes = SendDocumentOperationAttributes.Create<T>(dict, mapper);
        if (Uri.TryCreate(mapper.MapFromDic<string?>(dict, JobAttribute.DocumentUri), UriKind.RelativeOrAbsolute, out Uri documentUri))
            attributes.DocumentUri = documentUri;
        else
            throw new ArgumentException($"{JobAttribute.DocumentUri} attribute must be set");
        return attributes;
    }

    public override IEnumerable<IppAttribute> GetIppAttributes(IMapperApplier mapper)
    {
        foreach (var attribute in base.GetIppAttributes(mapper))
            yield return attribute;
        if (DocumentUri != null)
            yield return new IppAttribute(Tag.Uri, JobAttribute.DocumentUri, DocumentUri.ToString());
    }
}
