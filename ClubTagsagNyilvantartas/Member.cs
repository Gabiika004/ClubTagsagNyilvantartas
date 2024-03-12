﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using ClubTagsagNyilvantartas;
//
//    var member = Member.FromJson(jsonString);

namespace ClubTagsagNyilvantartas
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Member
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("entry")]
        public string Entry { get; set; }

        [JsonProperty("rating")]
        public long Rating { get; set; }

        [JsonProperty("fullname")]
        public string Fullname { get; set; }

        [JsonProperty("interest")]
        public string Interest { get; set; }
    }

    public partial class Member
    {
        public static Member[] FromJson(string json) => JsonConvert.DeserializeObject<Member[]>(json, ClubTagsagNyilvantartas.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Member[] self) => JsonConvert.SerializeObject(self, ClubTagsagNyilvantartas.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}