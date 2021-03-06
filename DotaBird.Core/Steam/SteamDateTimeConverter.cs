﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using DotaBird.Core.Util;

namespace DotaBird.Core.Steam
{
    public class SteamDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
                throw new Exception("Wrong Token Type");

            long ticks = (long)reader.Value;

            // Steam returns date/times as the number of seconds since 1/1/1970 (aka, unix timestamp).
            return UnixTimestampHelper.FromUnixTimestamp(ticks);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime dt = (DateTime)value;
            writer.WriteValue(UnixTimestampHelper.ToUnixTimestamp(dt));
        }
    }
}
