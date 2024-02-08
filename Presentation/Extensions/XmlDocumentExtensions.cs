using System.Xml;

namespace Presentation.Extensions
{
    public static class XmlDocumentExtensions
    {
        public static XmlElement CreateClass(this XmlDocument xmlDocument)
        {
            var classElement = xmlDocument.CreateElement("Class");
            xmlDocument.DocumentElement?.AppendChild(classElement);

            return classElement;
        }
    }
}
