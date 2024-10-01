﻿using SharpIpp.Mapping;
using SharpIpp.Protocol.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpIpp.Protocol.Models;
public class MediaCol
{
    public static MediaCol Create(Dictionary<string, IppAttribute[]> dict, IMapperApplier mapper)
    {
        return new MediaCol
        {
            MediaBackCoating = mapper.MapFromDic<MediaCoating?>(dict, nameof(MediaBackCoating).ConvertCamelCaseToDash()),
            MediaBottomMargin = mapper.MapFromDic<int?>(dict, nameof(MediaBottomMargin).ConvertCamelCaseToDash()),
            MediaColor = mapper.MapFromDic<string?>(dict, nameof(MediaColor).ConvertCamelCaseToDash()),
            MediaFrontCoating = mapper.MapFromDic<MediaCoating?>(dict, nameof(MediaFrontCoating).ConvertCamelCaseToDash()),
            MediaGrain = mapper.MapFromDic<MediaGrain?>(dict, nameof(MediaGrain).ConvertCamelCaseToDash()),
            MediaHoleCount = mapper.MapFromDic<int?>(dict, nameof(MediaHoleCount).ConvertCamelCaseToDash()),
            MediaInfo = mapper.MapFromDic<string?>(dict, nameof(MediaInfo).ConvertCamelCaseToDash()),
            MediaKey = mapper.MapFromDic<string?>(dict, nameof(MediaKey).ConvertCamelCaseToDash()),
            MediaLeftMargin = mapper.MapFromDic<int?>(dict, nameof(MediaLeftMargin).ConvertCamelCaseToDash()),
            MediaOrderCount = mapper.MapFromDic<int?>(dict, nameof(MediaOrderCount).ConvertCamelCaseToDash()),
            MediaPrePrinted = mapper.MapFromDic<MediaPrePrinted?>(dict, nameof(MediaPrePrinted).ConvertCamelCaseToDash()),
            MediaRecycled = mapper.MapFromDic<MediaRecycled?>(dict, nameof(MediaPrePrinted).ConvertCamelCaseToDash()),
            MediaRightMargin = mapper.MapFromDic<int?>(dict, nameof(MediaRightMargin).ConvertCamelCaseToDash()),
            MediaSize = dict.ContainsKey(nameof(MediaSize).ConvertCamelCaseToDash()) ? MediaSize.Create(dict[nameof(MediaSize).ConvertCamelCaseToDash()].FromBegCollection().ToIppDictionary(), mapper) : null,
            MediaSizeName = mapper.MapFromDic<string?>(dict, nameof(MediaSizeName).ConvertCamelCaseToDash()),
            MediaSource = mapper.MapFromDic<MediaSource?>(dict, nameof(MediaSource).ConvertCamelCaseToDash()),
            MediaSourceProperties = dict.ContainsKey(nameof(MediaSourceProperties).ConvertCamelCaseToDash()) ? MediaSourceProperties.Create(mapper, dict[nameof(MediaSourceProperties).ConvertCamelCaseToDash()]) : null,
            MediaThickness = mapper.MapFromDic<int?>(dict, nameof(MediaThickness).ConvertCamelCaseToDash()),
            MediaTooth = mapper.MapFromDic<MediaTooth?>(dict, nameof(MediaTooth).ConvertCamelCaseToDash()),
            MediaTopMargin = mapper.MapFromDic<int?>(dict, nameof(MediaTopMargin).ConvertCamelCaseToDash()),
            MediaType = mapper.MapFromDic<string?>(dict, nameof(MediaType).ConvertCamelCaseToDash()),
            MediaWeightMetric = mapper.MapFromDic<int?>(dict, nameof(MediaWeightMetric).ConvertCamelCaseToDash())
        };
    }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// </summary>
    public MediaCoating? MediaBackCoating { get; set; }

    /// <summary>
    /// integer(0:MAX)
    /// </summary>
    public int? MediaBottomMargin { get; set; }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// PWG Media Standardized Names v2.0 (MSN2) [PWG5101.1]
    /// </summary>
    public string? MediaColor { get; set; }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// </summary>
    public MediaCoating? MediaFrontCoating { get; set; }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// </summary>
    public MediaGrain? MediaGrain { get; set; }

    /// <summary>
    /// integer(0:MAX)
    /// </summary>
    public int? MediaHoleCount { get; set; }

    /// <summary>
    /// text(255)
    /// </summary>
    public string? MediaInfo { get; set; }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// </summary>
    public string? MediaKey { get; set; }

    /// <summary>
    /// integer(0:MAX)
    /// </summary>
    public int? MediaLeftMargin { get; set; }

    /// <summary>
    /// integer(1:MAX)
    /// </summary>
    public int? MediaOrderCount { get; set; }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// </summary>
    public MediaPrePrinted? MediaPrePrinted { get; set; }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// </summary>
    public MediaRecycled? MediaRecycled { get; set; }

    /// <summary>
    /// integer(0:MAX)
    /// </summary>
    public int? MediaRightMargin { get; set; }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// </summary>
    public MediaSize? MediaSize { get; set; }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// PWG media size name [PWG5101.1]
    /// </summary>
    public string? MediaSizeName { get; set; }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// </summary>
    public MediaSource? MediaSource { get; set; }

    /// <summary>
    /// collection
    /// </summary>
    public MediaSourceProperties? MediaSourceProperties { get; set; }

    /// <summary>
    /// integer(1:MAX)
    /// </summary>
    public int? MediaThickness { get; set; }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// </summary>
    public MediaTooth? MediaTooth { get; set; }

    /// <summary>
    /// integer(0:MAX)
    /// </summary>
    public int? MediaTopMargin { get; set; }

    /// <summary>
    /// type2 keyword | name(MAX)
    /// </summary>
    public string? MediaType { get; set; }

    /// <summary>
    /// integer(0:MAX)
    /// </summary>
    public int? MediaWeightMetric { get; set; }

    public IEnumerable<IppAttribute> GetIppAttributes(IMapperApplier mapper)
    {
        if (MediaBackCoating.HasValue)
            yield return new IppAttribute(Tag.Keyword, nameof(MediaBackCoating).ConvertCamelCaseToDash(), mapper.Map<string>(MediaBackCoating.Value));
        if (MediaBottomMargin.HasValue)
            yield return new IppAttribute(Tag.Integer, nameof(MediaBottomMargin).ConvertCamelCaseToDash(), MediaBottomMargin.Value);
        if (MediaFrontCoating.HasValue)
            yield return new IppAttribute(Tag.Keyword, nameof(MediaFrontCoating).ConvertCamelCaseToDash(), mapper.Map<string>(MediaFrontCoating.Value));
        if (MediaGrain.HasValue)
            yield return new IppAttribute(Tag.Keyword, nameof(MediaGrain).ConvertCamelCaseToDash(), mapper.Map<string>(MediaGrain.Value));
        if (MediaHoleCount.HasValue)
            yield return new IppAttribute(Tag.Integer, nameof(MediaHoleCount).ConvertCamelCaseToDash(), MediaHoleCount.Value);
        if (MediaInfo != null)
            yield return new IppAttribute(Tag.TextWithoutLanguage, nameof(MediaInfo).ConvertCamelCaseToDash(), MediaInfo);
        if (MediaKey != null)
            yield return new IppAttribute(Tag.Keyword, nameof(MediaKey).ConvertCamelCaseToDash(), MediaKey);
        if (MediaLeftMargin.HasValue)
            yield return new IppAttribute(Tag.Integer, nameof(MediaLeftMargin).ConvertCamelCaseToDash(), MediaLeftMargin.Value);
        if (MediaOrderCount.HasValue)
            yield return new IppAttribute(Tag.Integer, nameof(MediaOrderCount).ConvertCamelCaseToDash(), MediaOrderCount.Value);
        if (MediaPrePrinted.HasValue)
            yield return new IppAttribute(Tag.Keyword, nameof(MediaPrePrinted).ConvertCamelCaseToDash(), mapper.Map<string>(MediaPrePrinted.Value));
        if (MediaRecycled.HasValue)
            yield return new IppAttribute(Tag.Keyword, nameof(MediaRecycled).ConvertCamelCaseToDash(), mapper.Map<string>(MediaRecycled.Value));
        if (MediaRightMargin.HasValue)
            yield return new IppAttribute(Tag.Integer, nameof(MediaRightMargin).ConvertCamelCaseToDash(), MediaRightMargin.Value);
        if (MediaSize != null)
            foreach (var attribute in MediaSize.GetIppAttributes(mapper).ToBegCollection(nameof(MediaSize).ConvertCamelCaseToDash()))
                yield return attribute;
        if (MediaSizeName != null)
            yield return new IppAttribute(Tag.Keyword, nameof(MediaSizeName).ConvertCamelCaseToDash(), MediaSizeName);
        if (MediaSource.HasValue)
            yield return new IppAttribute(Tag.Keyword, nameof(MediaSource).ConvertCamelCaseToDash(), mapper.Map<string>(MediaSource.Value));
        if (MediaSourceProperties != null)
            foreach (var attribute in MediaSourceProperties.GetIppAttributes(mapper).ToBegCollection(nameof(MediaSourceProperties).ConvertCamelCaseToDash()))
                yield return attribute;
        if (MediaThickness.HasValue)
            yield return new IppAttribute(Tag.Integer, nameof(MediaThickness).ConvertCamelCaseToDash(), MediaThickness.Value);
        if (MediaTooth.HasValue)
            yield return new IppAttribute(Tag.Keyword, nameof(MediaTooth).ConvertCamelCaseToDash(), mapper.Map<string>(MediaTooth.Value));
        if (MediaWeightMetric.HasValue)
            yield return new IppAttribute(Tag.Integer, nameof(MediaWeightMetric).ConvertCamelCaseToDash(), MediaWeightMetric.Value);
    }
}
