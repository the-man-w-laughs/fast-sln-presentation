using System.Xml;

namespace Presentation.Extensions
{
    public static class XmlElementExtensions
    {
        public static void SetName(this XmlElement xmlElement, string name)
        {
            xmlElement.SetAttribute("Name", name);
        }

        public static void SetType(this XmlElement xmlElement, string type)
        {
            xmlElement.SetAttribute("Type", type);
        }

        public static void SetModifiers(this XmlElement xmlElement, string modifiers)
        {
            xmlElement.SetAttribute("Modifiers", modifiers);
        }

        public static void SetIsNullable(this XmlElement xmlElement, bool isNullable)
        {
            xmlElement.SetAttribute("IsNullable", isNullable.ToString());
        }

        public static void SetIsPredefinedType(this XmlElement xmlElement, bool isPredefinedType)
        {
            xmlElement.SetAttribute("IsPredefinedType", isPredefinedType.ToString());
        }

        public static void SetHasGetter(this XmlElement xmlElement, bool hasGetter)
        {
            xmlElement.SetAttribute("HasGetter", hasGetter.ToString());
        }

        public static void SetHasSetter(this XmlElement xmlElement, bool HasSetter)
        {
            xmlElement.SetAttribute("HasSetter", HasSetter.ToString());
        }

        public static void SetGetterModifiers(this XmlElement xmlElement, string getterModifiers)
        {
            xmlElement.SetAttribute("GetterModifiers", getterModifiers.ToString());
        }

        public static void SetSetterModifiers(this XmlElement xmlElement, string setterModifiers)
        {
            xmlElement.SetAttribute("SetterModifiers", setterModifiers.ToString());
        }

        public static void SetValue(this XmlElement xmlElement, string value)
        {
            xmlElement.SetAttribute("Value", value);
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

        public static XmlElement AppendClass(this XmlElement parentElement)
        {
            var classElement = parentElement.OwnerDocument.CreateElement("Class");
            parentElement.AppendChild(classElement);

            return classElement;
        }

        public static XmlElement AppendField(this XmlElement parentElement)
        {
            var fieldElement = parentElement.OwnerDocument.CreateElement("Field");
            parentElement.AppendChild(fieldElement);

            return fieldElement;
        }

        public static XmlElement AppendProperty(this XmlElement parentElement)
        {
            var fieldElement = parentElement.OwnerDocument.CreateElement("Property");
            parentElement.AppendChild(fieldElement);

            return fieldElement;
        }

        public static XmlElement AppendGetter(this XmlElement parentElement)
        {
            var getterElement = parentElement.OwnerDocument.CreateElement("Getter");
            parentElement.AppendChild(getterElement);

            return getterElement;
        }

        public static XmlElement AppendSetter(this XmlElement parentElement)
        {
            var setterElement = parentElement.OwnerDocument.CreateElement("Setter");
            parentElement.AppendChild(setterElement);

            return setterElement;
        }
    }
}
