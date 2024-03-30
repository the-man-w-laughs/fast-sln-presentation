using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastSlnPresentation.Server.Dtos
{
    public class GitRepoRequestModel
    {
        public string Pat { get; set; }
        public string Owner { get; set; }
        public string RepoName { get; set; }
    }
}
