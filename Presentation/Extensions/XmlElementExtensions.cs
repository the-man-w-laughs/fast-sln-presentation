using System.Xml;

namespace Presentation.Extensions
{
    public static class XmlElementExtensions
    {
        public static void SetName(this XmlElement xmlElement, string name)
        {
            xmlElement.SetAttribute("Name", name);
        }

        public static void SetModifiers(this XmlElement xmlElement, string modifiers)
        {
            xmlElement.SetAttribute("Modifiers", modifiers);
        }

        public static void SetNamespace(this XmlElement xmlElement, string @namespace)
        {
            xmlElement.SetAttribute("Namespace", @namespace);
        }

        public static void SetGenericParameters(
            this XmlElement xmlElement,
            string genericParameters
        )
        {
            xmlElement.SetAttribute("GenericParameters", genericParameters);
        }

        public static XmlElement CreateClass(this XmlElement parentElement)
        {
            var classElement = parentElement.OwnerDocument.CreateElement("Class");
            parentElement.AppendChild(classElement);

            return classElement;
        }

        public static XmlElement CreateField(this XmlElement parentElement)
        {
            var fieldElement = parentElement.OwnerDocument.CreateElement("Field");
            parentElement.AppendChild(fieldElement);

            return fieldElement;
        }
    }
}
