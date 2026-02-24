using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;

#if NET10_0_OR_GREATER

namespace MaxMind.Db;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public ref struct ValueCityResponse : IDecodableType<ValueCityResponse>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    [Constructor]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ValueCityResponse(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        ref ValueCity city,
        ref ValueContinent continent,
        ref ValueCountry country,
        ref ValueLocation location,
        [Parameter("registered_country")] ValueCountry registeredCountry)
    {
        City = city;
        Continent = continent;
        Country = country;
        Location = location;
        RegisteredCountry = registeredCountry;
    }

    /// <inheritdoc/>
    [CLSCompliant(false)]
    public DecodeState TryAssignToParameter(ReadOnlySpan<byte> name, Span<byte> route, out int written, out IDecodableType<ValueCityResponse>.ParameterAssignmentDelegate? returnValue)
    {
        written = 0;
        if (name.SequenceEqual("city"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCityResponse lhs, ReadOnlyMemory<byte> value) =>
            {
                // Complex type. Let the map populate
                lhs.City = new();
            };

            return DecodeState.Initialized;
        }
        else if (name.SequenceEqual("continent"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCityResponse lhs, ReadOnlyMemory<byte> value) =>
            {
                // Complex type. Let the map populate
                lhs.Continent = new();
            };

            return DecodeState.Initialized;
        }
        else if (name.SequenceEqual("country"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCityResponse lhs, ReadOnlyMemory<byte> value) =>
            {
                // Complex type. Let the map populate
                lhs.Country = new();
            };

            return DecodeState.Initialized;
        }
        else if (name.SequenceEqual("location"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCityResponse lhs, ReadOnlyMemory<byte> value) =>
            {
                // Complex type. Let the map populate
                lhs.Location = new();
            };

            return DecodeState.Initialized;
        }
        else if (name.SequenceEqual("registered_country"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCityResponse lhs, ReadOnlyMemory<byte> value) =>
            {
                // Complex type. Let the map populate
                lhs.RegisteredCountry = new();
            };

            return DecodeState.Initialized;
        }

        returnValue = null;
        return DecodeState.NotFound;
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ValueCity City;
    public ValueContinent Continent;
    public ValueCountry Country;
    public ValueLocation Location;
    public ValueCountry RegisteredCountry;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public ref struct ValueCity : IDecodableType<ValueCity>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    [Constructor]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ValueCity(int? confidence = null,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        [Parameter("geoname_id")] long? geoNameId = null,
        Dictionary<string, string>? names = null,
        List<string>? locales = null)
    {
        Confidence = confidence;
        ValueNamedEntity = new (geoNameId, names, locales);
    }

    /// <inheritdoc/>
    [CLSCompliant(false)]
    public DecodeState TryAssignToParameter(ReadOnlySpan<byte> name, Span<byte> route, out int written, out IDecodableType<ValueCity>.ParameterAssignmentDelegate? returnValue)
    {
        written = 0;
        if (name.SequenceEqual("confidence"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCity lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.Confidence = IDecodableType<ValueCity>.ReadVarInt(value.Span);
            };

            return DecodeState.Assigned;
        }
        else if (name.SequenceEqual("geoname_id"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCity lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.ValueNamedEntity.GeoNameId = IDecodableType<ValueContinent>.ReadLong(value.Span);
            };

            return DecodeState.Assigned;
        }
        else if (name.SequenceEqual("names"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCity lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.ValueNamedEntity.Names ??= new ();
                
                // This method is re-entrant
                int sepIndex = value.Span.IndexOf("\r\n"u8);
                string key = InternedStrings.GetString(value.Slice(0, sepIndex));
                string kvValue = InternedStrings.GetString(value.Slice(sepIndex + 2));
                lhs.ValueNamedEntity.Names.Add(key, kvValue);
            };

            "n"u8.CopyTo(route);
            written = 1;

            return DecodeState.Initialized;
        }
        else if (name.SequenceEqual("locales"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCity lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.ValueNamedEntity.Locales ??= new ();
                lhs.ValueNamedEntity.Locales.Add(Encoding.UTF8.GetString(value.Span));
            };

            return DecodeState.Initialized;
        }

        returnValue = null;
        return DecodeState.NotFound;
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ValueNamedEntity ValueNamedEntity = new();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public int? Confidence { get; internal set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public ref struct ValueNamedEntity
{
    [Constructor]
    public ValueNamedEntity(long? geoNameId = null, Dictionary<string, string>? names = null,
        List<string>? locales = null)
    {
        Names = names ?? new Dictionary<string, string>();
        GeoNameId = geoNameId;
        Locales = locales ?? new List<string> { "en" };
    }

    private Dictionary<string, string> _names = new Dictionary<string, string>();
    private List<string> _locales = new List<string> { "en" };

    public Dictionary<string, string> Names
    {
        get
        {
            _names ??= new ();
            return _names;
        }
        internal set
        {
            _names = value;
        }
    }

    public long? GeoNameId { get; internal set; }

    internal List<string> Locales
    {
        get
        {
            _locales ??= new List<string> { "en" };
            return _locales;
        }
        set
        {
            _locales = value;
        }
    }

    public string? Name
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        get
        {
            var names = this.Names;
            var locale = Locales.FirstOrDefault(l => names.ContainsKey(l));
            return locale == null ? null : Names[locale];
        }
    }
}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public ref struct ValueContinent : IDecodableType<ValueContinent>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    [Constructor]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ValueContinent(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        string? code = null,
        [Parameter("geoname_id")] long? geoNameId = null,
        Dictionary<string, string>? names = null,
        List<string>? locales = null)
    {
        Code = code;
        ValueNamedEntity = new (geoNameId, names, locales);
    }

    /// <inheritdoc/>
    [CLSCompliant(false)]
    public DecodeState TryAssignToParameter(ReadOnlySpan<byte> name, Span<byte> route, out int written, out IDecodableType<ValueContinent>.ParameterAssignmentDelegate? returnValue)
    {
        written = 0;
        if (name.SequenceEqual("code"u8))
        {
            returnValue = static (Span<byte> route, ref ValueContinent lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.Code = Encoding.UTF8.GetString(value.Span);
            };

            return DecodeState.Assigned;
        }
        else if (name.SequenceEqual("geoname_id"u8))
        {
            returnValue = static (Span<byte> route, ref ValueContinent lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.ValueNamedEntity.GeoNameId = IDecodableType<ValueContinent>.ReadLong(value.Span);
            };

            return DecodeState.Assigned;
        }
        else if (name.SequenceEqual("names"u8))
        {
            returnValue = static (Span<byte> route, ref ValueContinent lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.ValueNamedEntity.Names ??= new ();
                
                // This method is re-entrant
                int sepIndex = value.Span.IndexOf("\r\n"u8);
                string key = InternedStrings.GetString(value.Slice(0, sepIndex));
                string kvValue = InternedStrings.GetString(value.Slice(sepIndex + 2));
                lhs.ValueNamedEntity.Names.Add(key, kvValue);
            };

            "n"u8.CopyTo(route);
            written = 1;

            return DecodeState.Initialized;
        }
        else if (name.SequenceEqual("locales"u8))
        {
            returnValue = static (Span<byte> route, ref ValueContinent lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.ValueNamedEntity.Locales ??= new ();
                lhs.ValueNamedEntity.Locales.Add(Encoding.UTF8.GetString(value.Span));
            };

            return DecodeState.Initialized;
        }

        returnValue = null;
        return DecodeState.NotFound;
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ValueNamedEntity ValueNamedEntity;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public string? Code { get; internal set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public ref struct ValueCountry : IDecodableType<ValueCountry>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    [Constructor]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ValueCountry(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        int? confidence = null,
        [Parameter("geoname_id")] long? geoNameId = null,
        [Parameter("is_in_european_union")] bool isInEuropeanUnion = false,
        [Parameter("iso_code")] string? isoCode = null,
        Dictionary<string, string>? names = null,
        List<string>? locales = null)
    {
        Confidence = confidence;
        IsoCode = isoCode;
        IsInEuropeanUnion = isInEuropeanUnion;
        ValueNamedEntity = new (geoNameId, names, locales);
    }

    /// <inheritdoc/>
    [CLSCompliant(false)]
    public DecodeState TryAssignToParameter(ReadOnlySpan<byte> name, Span<byte> route, out int written, out IDecodableType<ValueCountry>.ParameterAssignmentDelegate? returnValue)
    {
        written = 0;
        if (name.SequenceEqual("code"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCountry lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.Confidence = IDecodableType<ValueCountry>.ReadVarInt(value.Span);
            };

            return DecodeState.Assigned;;
        }
        else if (name.SequenceEqual("iso_code"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCountry lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.IsoCode = Encoding.UTF8.GetString(value.Span);
            };

            return DecodeState.Assigned;
        }
        else if (name.SequenceEqual("is_in_european_union"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCountry lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.IsInEuropeanUnion = value.Span[0] == 0 ? false : true;
            };

            return DecodeState.Assigned;
        }
        else if (name.SequenceEqual("geoname_id"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCountry lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.ValueNamedEntity.GeoNameId = IDecodableType<ValueContinent>.ReadLong(value.Span);
            };

            return DecodeState.Assigned;
        }
        else if (name.SequenceEqual("names"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCountry lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.ValueNamedEntity.Names ??= new ();
                
                // This method is re-entrant
                int sepIndex = value.Span.IndexOf("\r\n"u8);
                string key = InternedStrings.GetString(value.Slice(0, sepIndex));
                string kvValue = InternedStrings.GetString(value.Slice(sepIndex + 2));
                lhs.ValueNamedEntity.Names.Add(key, kvValue);
            };

            "n"u8.CopyTo(route);
            written = 1;

            return DecodeState.Initialized;
        }
        else if (name.SequenceEqual("locales"u8))
        {
            returnValue = static (Span<byte> route, ref ValueCountry lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.ValueNamedEntity.Locales ??= new ();
                lhs.ValueNamedEntity.Locales.Add(InternedStrings.GetString(value));
            };

            return DecodeState.Initialized;
        }

        returnValue = null;
        return DecodeState.NotFound;
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ValueNamedEntity ValueNamedEntity;
    public int? Confidence { get; internal set; }
    public bool IsInEuropeanUnion { get; internal set; }
    public string? IsoCode { get; internal set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public ref struct ValueLocation : IDecodableType<ValueLocation>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    [Constructor]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ValueLocation(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        [Parameter("accuracy_radius")] int? accuracyRadius = null,
        double? latitude = null,
        double? longitude = null,
        [Parameter("time_zone")] string? timeZone = null)
    {
        AccuracyRadius = accuracyRadius;
        Latitude = latitude;
        Longitude = longitude;
        TimeZone = timeZone;
    }

    /// <inheritdoc/>
    [CLSCompliant(false)]
    public DecodeState TryAssignToParameter(ReadOnlySpan<byte> name, Span<byte> route, out int written, out IDecodableType<ValueLocation>.ParameterAssignmentDelegate? returnValue)
    {
        written = 0;
        if (name.SequenceEqual("accuracy_radius"u8))
        {
            returnValue = static (Span<byte> route, ref ValueLocation lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.AccuracyRadius = IDecodableType<ValueCity>.ReadVarInt(value.Span);
            };

            return DecodeState.Assigned;
        }
        else if (name.SequenceEqual("latitude"u8))
        {
            returnValue = static (Span<byte> route, ref ValueLocation lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.Latitude = IDecodableType<ValueCity>.ReadDouble(value.Span);
            };

            return DecodeState.Assigned;
        }
        else if (name.SequenceEqual("longitude"u8))
        {
            returnValue = static (Span<byte> route, ref ValueLocation lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.Longitude = IDecodableType<ValueCity>.ReadDouble(value.Span);
            };

            return DecodeState.Assigned;
        }
        else if (name.SequenceEqual("time_zone"u8))
        {
            returnValue = static (Span<byte> route, ref ValueLocation lhs, ReadOnlyMemory<byte> value) =>
            {
                lhs.TimeZone = Encoding.UTF8.GetString(value.Span);
            };

            return DecodeState.Assigned;
        }

        returnValue = null;
        return DecodeState.NotFound;
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public int? AccuracyRadius { get; internal set; }
    public int? AverageIncome { get; internal set; }
    public bool HasCoordinates => Latitude.HasValue && Longitude.HasValue;
    public double? Latitude { get; internal set; }
    public double? Longitude { get; internal set; }
    public int? PopulationDensity { get; internal set; }
    public string? TimeZone { get; internal set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public ref struct ValueSubdivision
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    [Constructor]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ValueSubdivision(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        int? confidence = null,
        [Parameter("geoname_id")] long? geoNameId = null,
        [Parameter("iso_code")] string? isoCode = null,
        Dictionary<string, string>? names = null,
        List<string>? locales = null)
    {
        Confidence = confidence;
        IsoCode = isoCode;
        namedEntity = new(geoNameId, names, locales);
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public ValueNamedEntity namedEntity;
    public int? Confidence { get; internal set; }
    public string? IsoCode { get; internal set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
#endif