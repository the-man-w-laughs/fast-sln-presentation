using System.Text.RegularExpressions;
using Presentation.Contracts;
using Presentation.Models;
using Presentation.Models.SnlInfo;

namespace Presentation.Services
{
    public partial class SlnParser : ISlnParser
    {
        [GeneratedRegex(
            "Project\\(\"(?<typeGuid>.*?)\"\\)\\s*=\\s*\"(?<name>.*?)\".*?\"(?<path>.*?)\".*?\"(?<projectGuid>.*?)\"",
            RegexOptions.Singleline
        )]
        private static partial Regex ProjectInfoRegex();

        public SlnInfo GetSlnProjectInfos(string sln)
        {
            var projectRegex = ProjectInfoRegex();

            var projects = projectRegex
                .Matches(sln)
                .Select(
                    match =>
                        new SlnProjectInfo
                        {
                            TypeGuid = match.Groups["typeGuid"].Value,
                            Name = match.Groups["name"].Value,
                            ProjectGuid = match.Groups["projectGuid"].Value,
                            Path = match.Groups["path"].Value
                        }
                )
                .ToList();

            return new SlnInfo { SlnProjectInfos = projects };
        }
    }
}
