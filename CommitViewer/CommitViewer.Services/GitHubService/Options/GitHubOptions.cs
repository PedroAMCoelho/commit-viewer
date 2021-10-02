using CommitViewer.Shared.Options;
using System.ComponentModel.DataAnnotations;

namespace CommitViewer.Services.GitHubService.Options
{
    public class GitHubOptions : AppSettingsOptions
    {
        /// <summary>
        /// Gets or sets key Name.
        /// </summary>
        [Required]
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets key value.
        /// </summary>
        [Required]
        public string APIBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets key value.
        /// </summary>
        [Required]
        public double APITimeout { get; set; }
    }
}
