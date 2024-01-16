using System.Xml;
using Presentation.Consts;

namespace Presentation.Extensions
{
    public static class XmlElementExtensions
    {
        public static void SetName(this XmlElement xmlElement, string name)
        {
            SetAttribute(xmlElement, XmlAttributeNames.Name, name);
        }

        public static void SetType(this XmlElement xmlElement, string type)
        {
            SetAttribute(xmlElement, XmlAttributeNames.Type, type);
        }

        public static void SetModifiers(this XmlElement xmlElement, string modifiers)
        {
            SetAttribute(xmlElement, XmlAttributeNames.Modifiers, modifiers);
        }

        public static void SetUnderlyingType(this XmlElement xmlElement, string modifiers)
        {
            SetAttribute(xmlElement, XmlAttributeNames.UnderlyingType, modifiers);
        }

        public static void SetIsNullable(this XmlElement xmlElement, bool isNullable)
        {
            SetAttribute(xmlElement, XmlAttributeNames.IsNullable, isNullable.ToString().ToLower());
        }

        public static void SetIsPredefinedType(this XmlElement xmlElement, bool isPredefinedType)
        {
            SetAttribute(
                xmlElement,
                XmlAttributeNames.IsPredefinedType,
                isPredefinedType.ToString().ToLower()
            );
        }

        public static void SetIsReferenceType(this XmlElement xmlElement, bool isReferenceType)
        {
            SetAttribute(
                xmlElement,
                XmlAttributeNames.IsReferenceType,
                isReferenceType.ToString().ToLower()
            );
        }

        public static void SetHasGetter(this XmlElement xmlElement, bool hasGetter)
        {
            SetAttribute(xmlElement, XmlAttributeNames.HasGetter, hasGetter.ToString().ToLower());
        }

        public static void SetHasSetter(this XmlElement xmlElement, bool HasSetter)
        {
            SetAttribute(xmlElement, XmlAttributeNames.HasSetter, HasSetter.ToString().ToLower());
        }

        public static void SetGetterModifiers(this XmlElement xmlElement, string getterModifiers)
        {
            SetAttribute(xmlElement, XmlAttributeNames.GetterModifiers, getterModifiers.ToString());
        }

        public static void SetSetterModifiers(this XmlElement xmlElement, string setterModifiers)
        {
            SetAttribute(xmlElement, XmlAttributeNames.SetterModifiers, setterModifiers.ToString());
        }

        public static void SetValue(this XmlElement xmlElement, string value)
        {
            SetAttribute(xmlElement, XmlAttributeNames.Value, value);
        }

        public static void SetNamespace(this XmlElement xmlElement, string @namespace)
        {
            SetAttribute(xmlElement, XmlAttributeNames.Namespace, @namespace);
        }

        public static void SetReturnType(this XmlElement xmlElement, string returnType)
        {
            SetAttribute(xmlElement, XmlAttributeNames.ReturnType, returnType);
        }

        public static void SetGenericParameters(
            this XmlElement xmlElement,
            string genericParameters
        )
        {
            SetAttribute(xmlElement, XmlAttributeNames.GenericParameters, genericParameters);
        }

        public static void SetParentClass(this XmlElement xmlElement, string parentClass)
        {
            SetAttribute(xmlElement, XmlAttributeNames.ParentClass, parentClass);
        }

        public static XmlElement AppendElement(this XmlElement parentElement, string elementName)
        {
            var newElement = parentElement.OwnerDocument.CreateElement(elementName);
            parentElement.AppendChild(newElement);

            return newElement;
        }

        public static XmlElement AppendClass(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Class);
        }

        public static XmlElement AppendBaseTypes(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.BaseTypes);
        }

        public static XmlElement AppendType(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Type);
        }

        public static XmlElement AppendField(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Field);
        }

        public static XmlElement AppendProperty(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Property);
        }

        public static XmlElement AppendGetter(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Getter);
        }

        public static XmlElement AppendSetter(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Setter);
        }

        public static XmlElement AppendEvent(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Event);
        }

        public static XmlElement AppendMethod(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Method);
        }

        public static XmlElement AppendParameter(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Parameter);
        }

        public static XmlElement AppendInterface(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Interface);
        }

        public static XmlElement AppendStruct(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Struct);
        }

        public static XmlElement AppendEnum(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Enum);
        }

        public static XmlElement AppendRecord(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Record);
        }

        public static XmlElement AppendMember(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Member);
        }

        public static XmlElement AppendIf(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.If);
        }

        public static XmlElement AppendFor(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.For);
        }

        public static XmlElement AppendForeach(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Foreach);
        }

        public static XmlElement AppendWhile(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.While);
        }

        public static XmlElement AppendSwitch(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Switch);
        }

        public static XmlElement AppendCase(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Case);
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
