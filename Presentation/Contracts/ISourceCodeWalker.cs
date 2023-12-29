using System.Xml;
using Microsoft.CodeAnalysis;

namespace Presentation.Contracts
{
    public interface ISourceCodeWalker
    {
        public void Parse();
    }
}
