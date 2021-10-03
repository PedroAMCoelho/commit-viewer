namespace CommitViewer.Services.Constants
{
    public static class GitConstants
    {
        // --depth is how far the clone/pull will go in commit hierarchy to retrieve parent commits. 
        // Note: Remember that each commit may have more than one parent.
        public const string GitCloneCommand = "cd {0} > NUL && git.exe clone --quiet {1} --depth={2} . > NUL && git.exe --no-pager log --pretty=format:{3} --date=iso --skip={4} --max-count={5}";

        public const string GitPullCommand = "cd {0} > NUL && git.exe pull --quiet {1} --depth={2} > NUL && git.exe --no-pager log --pretty=format:{3} --date=iso --skip={4} --max-count={5}";

        public const string FindLocalRepo = "cd \\; cd Users; (Get-ChildItem -Attributes 'Directory+Hidden' -ErrorAction 'SilentlyContinue' -Filter '.git' -Recurse).FullName -like '*{0}*'";

        public const string JsonFormatGitLog = "%%n{%%n\\\"sha\\\":\\\"%%H\\\",%%n\\\"authorName\\\":\\\"%%an\\\",%%n\\\"authorEmail\\\":\\\"%%ae\\\",%%n\\\"date\\\":\\\"%%ad\\\",%%n\\\"message\\\":\\\"%%f\\\"%%n},";
    }
}
