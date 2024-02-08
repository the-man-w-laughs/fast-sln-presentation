using System.Xml;
using Presentation.Consts;

namespace Presentation.Extensions
{
    public static class XmlElementExtensions
    {
        public static XmlElement SetName(this XmlElement xmlElement, string name)
        {
            return SetAttribute(xmlElement, XmlAttributeNames.Name, name);
        }

        public static XmlElement SetType(this XmlElement xmlElement, string type)
        {
            return SetAttribute(xmlElement, XmlAttributeNames.Type, type);
        }

        public static XmlElement SetModifiers(this XmlElement xmlElement, string modifiers)
        {
            return SetAttribute(xmlElement, XmlAttributeNames.Modifiers, modifiers);
        }

        public static XmlElement SetUnderlyingType(this XmlElement xmlElement, string modifiers)
        {
            return SetAttribute(xmlElement, XmlAttributeNames.UnderlyingType, modifiers);
        }

        public static XmlElement SetIsNullable(this XmlElement xmlElement, bool isNullable)
        {
            return SetAttribute(
                xmlElement,
                XmlAttributeNames.IsNullable,
                isNullable.ToString().ToLower()
            );
        }

        public static XmlElement SetIsPredefinedType(
            this XmlElement xmlElement,
            bool isPredefinedType
        )
        {
            return SetAttribute(
                xmlElement,
                XmlAttributeNames.IsPredefinedType,
                isPredefinedType.ToString().ToLower()
            );
        }

        public static XmlElement SetIsReferenceType(
            this XmlElement xmlElement,
            bool isReferenceType
        )
        {
            return SetAttribute(
                xmlElement,
                XmlAttributeNames.IsReferenceType,
                isReferenceType.ToString().ToLower()
            );
        }

        public static XmlElement SetHasGetter(this XmlElement xmlElement, bool hasGetter)
        {
            return SetAttribute(
                xmlElement,
                XmlAttributeNames.HasGetter,
                hasGetter.ToString().ToLower()
            );
        }

        public static XmlElement SetHasSetter(this XmlElement xmlElement, bool HasSetter)
        {
            return SetAttribute(
                xmlElement,
                XmlAttributeNames.HasSetter,
                HasSetter.ToString().ToLower()
            );
        }

        public static XmlElement SetGetterModifiers(
            this XmlElement xmlElement,
            string getterModifiers
        )
        {
            return SetAttribute(
                xmlElement,
                XmlAttributeNames.GetterModifiers,
                getterModifiers.ToString()
            );
        }

        public static XmlElement SetSetterModifiers(
            this XmlElement xmlElement,
            string setterModifiers
        )
        {
            return SetAttribute(
                xmlElement,
                XmlAttributeNames.SetterModifiers,
                setterModifiers.ToString()
            );
        }

        public static XmlElement SetValue(this XmlElement xmlElement, string value)
        {
            return SetAttribute(xmlElement, XmlAttributeNames.Value, value);
        }

        public static XmlElement SetNamespace(this XmlElement xmlElement, string @namespace)
        {
            return SetAttribute(xmlElement, XmlAttributeNames.Namespace, @namespace);
        }

        public static XmlElement SetReturnType(this XmlElement xmlElement, string returnType)
        {
            return SetAttribute(xmlElement, XmlAttributeNames.ReturnType, returnType);
        }

        public static XmlElement SetGenericParameters(
            this XmlElement xmlElement,
            string genericParameters
        )
        {
            return SetAttribute(xmlElement, XmlAttributeNames.GenericParameters, genericParameters);
        }

        public static XmlElement SetParentClass(this XmlElement xmlElement, string parentClass)
        {
            return SetAttribute(xmlElement, XmlAttributeNames.ParentClass, parentClass);
        }

        public static XmlElement SetOperator(this XmlElement xmlElement, string parentClass)
        {
            return SetAttribute(xmlElement, XmlAttributeNames.Operator, parentClass);
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
            return parentElement.AppendElement(XmlElementNames.ForEach);
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

        public static XmlElement AppendVariable(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Variable);
        }

        public static XmlElement AppendIncrementor(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Incrementor);
        }

        public static XmlElement AppendCondition(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Condition);
        }

        public static XmlElement AppendInitialization(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Initialization);
        }

        public static XmlElement AppendForEach(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.ForEach);
        }

        public static XmlElement AppendIfBlock(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.IfBlock);
        }

        public static XmlElement AppendElseBlock(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.ElseBlock);
        }

        public static XmlElement AppendLocalDeclaration(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.LocalDeclaration);
        }

        public static XmlElement AppendAssignment(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.Assignment);
        }

        public static XmlElement AppendPrefixExpression(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.PrefixExpression);
        }

        public static XmlElement AppendPostfixExpression(this XmlElement parentElement)
        {
            return parentElement.AppendElement(XmlElementNames.PostfixExpression);
        }

        public static XmlElement SetAttribute(XmlElement xmlElement, string name, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return xmlElement;
            }

            xmlElement.SetAttribute(name, value);
            return xmlElement;
        }
    }
}
