using System.Runtime.CompilerServices;
using System.Xml;

namespace Presentation.Extensions
{
    public static class XmlElementExtensions
    {
        public static void SetName(this XmlElement xmlElement, string name)
        {
            SetAttribute(xmlElement, "Name", name);
        }

        public static void SetType(this XmlElement xmlElement, string type)
        {
            SetAttribute(xmlElement, "Type", type);
        }

        public static void SetModifiers(this XmlElement xmlElement, string modifiers)
        {
            SetAttribute(xmlElement, "Modifiers", modifiers);
        }

        public static void SetIsNullable(this XmlElement xmlElement, bool isNullable)
        {
            SetAttribute(xmlElement, "IsNullable", isNullable.ToString());
        }

        public static void SetIsPredefinedType(this XmlElement xmlElement, bool isPredefinedType)
        {
            SetAttribute(xmlElement, "IsPredefinedType", isPredefinedType.ToString());
        }

        public static void SetIsReferenceType(this XmlElement xmlElement, bool isReferenceType)
        {
            SetAttribute(xmlElement, "IsReferenceType", isReferenceType.ToString());
        }

        public static void SetHasGetter(this XmlElement xmlElement, bool hasGetter)
        {
            SetAttribute(xmlElement, "HasGetter", hasGetter.ToString());
        }

        public static void SetHasSetter(this XmlElement xmlElement, bool HasSetter)
        {
            SetAttribute(xmlElement, "HasSetter", HasSetter.ToString());
        }

        public static void SetGetterModifiers(this XmlElement xmlElement, string getterModifiers)
        {
            SetAttribute(xmlElement, "GetterModifiers", getterModifiers.ToString());
        }

        public static void SetSetterModifiers(this XmlElement xmlElement, string setterModifiers)
        {
            SetAttribute(xmlElement, "SetterModifiers", setterModifiers.ToString());
        }

        public static void SetValue(this XmlElement xmlElement, string value)
        {
            SetAttribute(xmlElement, "Value", value);
        }

        public static void SetNamespace(this XmlElement xmlElement, string @namespace)
        {
            SetAttribute(xmlElement, "Namespace", @namespace);
        }

        public static void SetGenericParameters(
            this XmlElement xmlElement,
            string genericParameters
        )
        {
            SetAttribute(xmlElement, "GenericParameters", genericParameters);
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

        public static void SetAttribute(XmlElement xmlElement, string name, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            xmlElement.SetAttribute(name, value);
        }
    }
}
