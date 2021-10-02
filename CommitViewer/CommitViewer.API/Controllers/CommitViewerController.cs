﻿using CommitViewer.Business.CommitViewer;
using CommitViewer.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace CommitViewer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommitViewerController : ControllerBase
    {
        private readonly ICommitViewerBusiness commitViewerBusiness;

        public CommitViewerController(
            ICommitViewerBusiness commitViewerBusiness)
        {
            this.commitViewerBusiness = commitViewerBusiness;
        }

        [HttpGet]
        [Route("repositories/{owner}/{repository}/commits")]
        [ResponseType(typeof(IEnumerable<CommitModel>))]
        public async Task<IActionResult> GetCommits(string owner, string repository, [FromQuery] int page = 0, int per_page = 10)
            => Ok(commitViewerBusiness.GetCommits(owner, repository, page, per_page));
    }
}
