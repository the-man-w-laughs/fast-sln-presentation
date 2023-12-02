using System.Text.RegularExpressions;
using Presentation.Contracts;
using Presentation.Models;

namespace Presentation.Services
{
    public class SlnParser : ISlnParser
    {
        public IEnumerable<SlnProjectInfo> GetSlnProjectInfos(string sln)
        {
            var projectRegex = new Regex(
                @"Project\(""(?<typeGuid>.*?)""\)\s*=\s*""(?<name>.*?)"".*?""(?<path>.*?)"".*?""(?<projectGuid>.*?)""",
                RegexOptions.Singleline
            );

            var matches = projectRegex.Matches(sln);

            var projects = new SlnProjectInfo[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var projectInfo = new SlnProjectInfo
                {
                    TypeGuid = match.Groups["typeGuid"].Value,
                    Name = match.Groups["name"].Value,
                    ProjectGuid = match.Groups["projectGuid"].Value,
                    Path = match.Groups["path"].Value
                };
                projects[i] = projectInfo;
            }

            return projects;
        }
    }
}
