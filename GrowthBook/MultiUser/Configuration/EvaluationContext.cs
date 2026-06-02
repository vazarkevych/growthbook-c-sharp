using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace GrowthBook.MultiUser.Configuration
{
    /// <summary>Combines global and user context for a single stateless evaluation pass.</summary>
    internal sealed class EvaluationContext
    {
        public GlobalContext Global { get; }
        public UserContext User { get; }
        public StackContext Stack { get; }

        public EvaluationContext(GlobalContext global, UserContext user)
        {
            Global = global;
            User = user;
            Stack = new StackContext();
        }

        /// <summary>Merges global and user forced variations. User values take precedence.</summary>
        public IDictionary<string, int> GetForcedVariations()
        {
            var result = new Dictionary<string, int>();
            if (Global.ForcedVariations != null)
                foreach (var kv in Global.ForcedVariations) result[kv.Key] = kv.Value;
            if (User.ForcedVariations != null)
                foreach (var kv in User.ForcedVariations) result[kv.Key] = kv.Value;
            return result;
        }

        /// <summary>Merges global and user forced feature values. User values take precedence.</summary>
        public IDictionary<string, JToken> GetForcedFeatureValues()
        {
            var result = new Dictionary<string, JToken>();
            if (Global.ForcedFeatureValues != null)
                foreach (var kv in Global.ForcedFeatureValues) result[kv.Key] = kv.Value;
            if (User.ForcedFeatureValues != null)
                foreach (var kv in User.ForcedFeatureValues) result[kv.Key] = kv.Value;
            return result;
        }

        /// <summary>Merges global and user attributes. User attributes and overrides take precedence.</summary>
        public JObject GetAttributes()
        {
            var result = Global.Attributes?.DeepClone() as JObject ?? new JObject();
            if (User.Attributes != null)
                foreach (var prop in User.Attributes.Properties())
                    result[prop.Name] = prop.Value; // user overrides global
            if (User.AttributeOverrides != null)
                foreach (var prop in User.AttributeOverrides.Properties())
                    result[prop.Name] = prop.Value;
            return result;
        }
    }
}
