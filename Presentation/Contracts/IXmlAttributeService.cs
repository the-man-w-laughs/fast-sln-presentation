using System.Xml;

namespace Presentation.Contracts
{
    public interface IXmlService
    {
        public void SetName(XmlElement xmlElement, string name);
    }
}
