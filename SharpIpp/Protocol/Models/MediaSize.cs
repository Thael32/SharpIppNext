﻿using SharpIpp.Mapping;
using SharpIpp.Protocol.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SharpIpp.Protocol.Models;
public class MediaSize
{
    /// <summary>
    /// integer(0:MAX))
    /// </summary>
    public int? XDimension { get; set; }

    /// <summary>
    /// integer(0:MAX))
    /// </summary>
    public int? YDimension { get; set; }

    public IEnumerable<IppAttribute> GetIppAttributes(IMapperApplier mapper)
    {
        if (XDimension.HasValue)
            yield return new IppAttribute(Tag.Integer, nameof(XDimension).ConvertCamelCaseToDash(), XDimension.Value);
        if (YDimension.HasValue)
            yield return new IppAttribute(Tag.Integer, nameof(YDimension).ConvertCamelCaseToDash(), YDimension.Value);
    }

    public static MediaSize Create(Dictionary<string, IppAttribute[]> dict, IMapperApplier mapper)
    {
        return new MediaSize
        {
            XDimension = mapper.MapFromDic<int?>(dict, nameof(XDimension).ConvertCamelCaseToDash()),
            YDimension = mapper.MapFromDic<int?>(dict, nameof(XDimension).ConvertCamelCaseToDash())
        };
    }
}
