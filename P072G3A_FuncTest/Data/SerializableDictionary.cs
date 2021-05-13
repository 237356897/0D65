using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ToolKit;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace P072G3A_FuncTest.Data
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        public static SerializableDictionary<string, string> Instance = new SerializableDictionary<string, string>();

        #region 方法
        public void WriteXml(XmlWriter write)
        {
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            foreach (KeyValuePair<TKey, TValue> kv in this)
            {
                write.WriteStartElement("SerializableDictionary");
                write.WriteStartElement("key");
                KeySerializer.Serialize(write, kv.Key);
                write.WriteEndElement();
                write.WriteStartElement("value");
                ValueSerializer.Serialize(write, kv.Value);
                write.WriteEndElement();
                write.WriteEndElement();
            }
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            if (reader.Name == "SerializableDictionary")
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("SerializableDictionary");
                    reader.ReadStartElement("key");
                    TKey tk = (TKey)KeySerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    reader.ReadStartElement("value");
                    TValue vl = (TValue)ValueSerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    reader.ReadEndElement();
                    this.Add(tk, vl);
                    reader.MoveToContent();
                }
                reader.ReadEndElement();
            }

        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotImplementedException();
        }

        public void SaveDicXml(Object sender)
        {
            using (FileStream fileStream = new FileStream(AppConfig.PassWordPath, FileMode.Create))
            {
                XmlSerializer xmlFormatter = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                xmlFormatter.Serialize(fileStream, sender);
            }
        }

        public SerializableDictionary<string, string> LoadDicXml()
        {
            using (FileStream fileStream = new FileStream(AppConfig.PassWordPath, FileMode.Open))
            {
                XmlSerializer xmlFormatter = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                return (SerializableDictionary<string, string>)xmlFormatter.Deserialize(fileStream);
            }
        }

        #endregion

    }
}
