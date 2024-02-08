using System.Xml;
using Microsoft.CodeAnalysis;

namespace Presentation.Contracts
{
    public interface ISourceCodeToXmlWalker
    {
        public void Parse();
    }
}
