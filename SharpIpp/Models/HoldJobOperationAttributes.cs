﻿using SharpIpp.Mapping;
using SharpIpp.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpIpp.Models;
public class HoldJobOperationAttributes : CancelJobOperationAttributes
{
    public JobHoldUntil? JobHoldUntil { get; set; }

    public static new T Create<T>(Dictionary<string, IppAttribute[]> dict, IMapperApplier mapper) where T : HoldJobOperationAttributes, new()
    {
        var attributes = CancelJobOperationAttributes.Create<T>(dict, mapper);
        attributes.JobHoldUntil = mapper.MapFromDic<JobHoldUntil?>(dict, JobAttribute.JobHoldUntil);
        return attributes;
    }

    public override IEnumerable<IppAttribute> GetIppAttributes(IMapperApplier mapper)
    {
        foreach (var attribute in base.GetIppAttributes(mapper))
            yield return attribute;
        if (JobHoldUntil.HasValue)
            yield return new IppAttribute(Tag.Keyword, JobAttribute.JobHoldUntil, mapper.Map<string>(JobHoldUntil.Value));
    }
}