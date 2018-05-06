using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace artfulplace.Ilisgate.V2.KerasExtension
{
    /// <summary>
    /// KerasのWeightを保存するのに使用する型
    /// </summary>
    [DataContract]
    public class WeightModel
    {
        /// <summary>
        /// レイヤー名
        /// </summary>
        [DataMember]
        public string[] LayerNames { get; set; }

        [IgnoreDataMember]
        public Dictionary<string, List<string>> WeightNameDictionary { get; set; }

        [DataMember]
        public WeightNameKeyPair[] WeightNames { get; set; }

        [DataMember]
        public WeightValueKeyPair[] WeightValues { get; set; }

        [IgnoreDataMember]
        public Dictionary<string, Array> WeightValueDictionary { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            WeightValueDictionary = WeightValues.ToDictionary(x => x.Name, x => x.Value);
            WeightNameDictionary = WeightNames.ToDictionary(x => x.Name, x => x.Value.ToList());
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext sc)
        {
            WeightValues = WeightValueDictionary.Select(x => new WeightValueKeyPair(x.Key, x.Value)).ToArray();
            WeightNames = WeightNameDictionary.Select(x => new WeightNameKeyPair(x.Key, x.Value.ToArray())).ToArray();
        }
    }

    [DataContract]
    public class WeightValueKeyPair
    {
        public WeightValueKeyPair()
        {
        }

        public WeightValueKeyPair(string name, Array value)
        {
            Name = name;
            Value = value;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Array Value { get; set; }
    }

    [DataContract]
    public class WeightNameKeyPair
    {
        public WeightNameKeyPair()
        {
        }

        public WeightNameKeyPair(string name, string[] value)
        {
            Name = name;
            Value = value;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string[] Value { get; set; }
    }
}
