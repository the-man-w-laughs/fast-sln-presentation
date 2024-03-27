using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FastSlnPresentation.BLL.Services
{
    public class TypeName
    {
        public static string From(TypeDeclarationSyntax node)
        {
            string typeName = node.Identifier.ValueText;

            if (node.TypeParameterList != null)
            {
                string genericTypeParameters =
                    $"<{string.Join(", ", node.TypeParameterList.Parameters.Select(p => p.Identifier.ValueText))}>";
                typeName += genericTypeParameters;
            }

            return typeName;
        }

        public static string From(BaseTypeSyntax syntax)
        {
            if (syntax.Type is SimpleNameSyntax simpleName)
            {
                string name = simpleName.Identifier.Text;

                if (
                    syntax.Type is GenericNameSyntax genericName
                    && genericName.TypeArgumentList != null
                )
                {
                    string typeArgs =
                        $"<{string.Join(",", genericName.TypeArgumentList.Arguments)}>";
                    return $"{name}{typeArgs}";
                }

                return name;
            }

            throw new InvalidOperationException("Unsupported syntax type");
        }
    }
}
