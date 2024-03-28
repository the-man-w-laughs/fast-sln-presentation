using System.Text.RegularExpressions;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Models;
using FastSlnPresentation.BLL.Models.SnlInfo;

namespace FastSlnPresentation.BLL.Services
{
    public static partial class SlnParser
    {
        [GeneratedRegex(
            "Project\\(\"(?<typeGuid>.*?)\"\\)\\s*=\\s*\"(?<name>.*?)\".*?\"(?<path>.*?)\".*?\"(?<projectGuid>.*?)\"",
            RegexOptions.Singleline
        )]
        private static partial Regex ProjectInfoRegex();

        public static SlnInfo GetSlnInfo(string sln)
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
