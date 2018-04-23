using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;

namespace Days.Model
{
    public class ObjectSerializer
    {
        public static string ToXml(ObservableCollection<Events> value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Events>));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true,
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                serializer.Serialize(xmlWriter, value);
            }

            return stringBuilder.ToString();
        }

        public static ObservableCollection<Events> FromXml(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Events>));
            ObservableCollection<Events> value;

            using (StringReader stringReader = new StringReader(xml))
            {
                object deserialized = serializer.Deserialize(stringReader);
                value = (ObservableCollection<Events>)deserialized;
            }

            return value;
        }

        public static string CoverEventsToXml(ObservableCollection<CoverEvents> value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<CoverEvents>));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true,
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                serializer.Serialize(xmlWriter, value);
            }

            return stringBuilder.ToString();
        }

        public static ObservableCollection<CoverEvents> CoverEventsFromXml(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<CoverEvents>));
            ObservableCollection<CoverEvents> value;

            using (StringReader stringReader = new StringReader(xml))
            {
                object deserialized = serializer.Deserialize(stringReader);
                value = (ObservableCollection<CoverEvents>)deserialized;
            }

            return value;
        }
    }
}
