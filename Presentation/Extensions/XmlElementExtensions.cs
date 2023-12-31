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

        public static void SetUnderlyingType(this XmlElement xmlElement, string modifiers)
        {
            SetAttribute(xmlElement, "UnderlyingType", modifiers);
        }

        public static void SetIsNullable(this XmlElement xmlElement, bool isNullable)
        {
            SetAttribute(xmlElement, "IsNullable", isNullable.ToString().ToLower());
        }

        public static void SetIsPredefinedType(this XmlElement xmlElement, bool isPredefinedType)
        {
            SetAttribute(xmlElement, "IsPredefinedType", isPredefinedType.ToString().ToLower());
        }

        public static void SetIsReferenceType(this XmlElement xmlElement, bool isReferenceType)
        {
            SetAttribute(xmlElement, "IsReferenceType", isReferenceType.ToString().ToLower());
        }

        public static void SetHasGetter(this XmlElement xmlElement, bool hasGetter)
        {
            SetAttribute(xmlElement, "HasGetter", hasGetter.ToString().ToLower());
        }

        public static void SetHasSetter(this XmlElement xmlElement, bool HasSetter)
        {
            SetAttribute(xmlElement, "HasSetter", HasSetter.ToString().ToLower());
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

        public static void SetReturnType(this XmlElement xmlElement, string returnType)
        {
            SetAttribute(xmlElement, "ReturnType", returnType);
        }

        public static void SetGenericParameters(
            this XmlElement xmlElement,
            string genericParameters
        )
        {
            SetAttribute(xmlElement, "GenericParameters", genericParameters);
        }

        public static void SetParentClass(this XmlElement xmlElement, string parrentClass)
        {
            SetAttribute(xmlElement, "ParrentClass", parrentClass);
        }

        public static XmlElement AppendElement(this XmlElement parentElement, string elementName)
        {
            var newElement = parentElement.OwnerDocument.CreateElement(elementName);
            parentElement.AppendChild(newElement);

            return newElement;
        }

        public static XmlElement AppendClass(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Class");
        }

        public static XmlElement AppendBaseTypes(this XmlElement parentElement)
        {
            return parentElement.AppendElement("BaseTypes");
        }

        public static XmlElement AppendField(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Field");
        }

        public static XmlElement AppendProperty(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Property");
        }

        public static XmlElement AppendGetter(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Getter");
        }

        public static XmlElement AppendSetter(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Setter");
        }

        public static XmlElement AppendEvent(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Event");
        }

        public static XmlElement AppendMethod(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Method");
        }

        public static XmlElement AppendParameter(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Param");
        }

        public static XmlElement AppendInterface(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Interface");
        }

        public static XmlElement AppendStruct(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Struct");
        }

        public static XmlElement AppendEnum(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Enum");
        }

        public static XmlElement AppendRecord(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Record");
        }

        public static XmlElement AppendMember(this XmlElement parentElement)
        {
            return parentElement.AppendElement("Member");
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
