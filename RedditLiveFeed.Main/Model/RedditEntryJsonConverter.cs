using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditLiveFeed.Main.Model
{
    public class RedditEntryJsonConverter : JsonConverter<RedditEntry>
    {
        public override bool CanWrite => false;

        public override RedditEntry ReadJson(JsonReader reader,
            Type objectType, RedditEntry existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var jsonEntry = jObject["data"];

            if(existingValue == null)
            {
                existingValue = Activator.CreateInstance<RedditEntry>();
            }

            var newReader = jsonEntry.CreateReader();
            serializer.Populate(newReader, existingValue);
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, RedditEntry value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
