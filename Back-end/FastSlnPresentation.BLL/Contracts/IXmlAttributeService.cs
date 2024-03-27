using System.Xml;

namespace FastSlnPresentation.BLL.Contracts
{
    public interface IXmlService
    {
        public void SetName(XmlElement xmlElement, string name);
    }
}
