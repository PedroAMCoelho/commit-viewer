# commit-viewer
This project allows the user to retrieve a list of commits based on a GitHub url or simply on a GitHub's owner and repository name information (there are 2 endpoints for achieving the same outcome).

Two services have been designed (with resilient implementation):

- GitHub Service: commit-viewer takes advantage of GitHub's public API service to get a list of commits passing repository name and owner name as parameters. commit-viewer's GitHub Service has a 3 minute timeout define, in case there are any sort of issue on the client side or even GitHub's side (API offline or failing for some other reason).

- GitCli Service: In case GitHub Service fails, Git CLI Service is called. Its timeout has been defined as 5 minutes. This service works based on a couple of steps:
1. Search for an existing local repository for the input project (temporary PowerShell script is created for searching inside "C:/Users/" - not the entire system);
2. If a local repository is found, its path is used; If no local repository is found, a temporary directory is created and its path is used;
3. Next step is cloning or pulling the repository (depending on whether there is or there is not a local repository) with git log included in the script (temp .bat is used);
4. Both commands contain a pretty option to format the command output as JSON (this was very handy to convert to the application's model in business layer)

Both services have pagination feature.

For the scenario where GitHub Service is unavailable or failing, a resilient behave has been implemented, so that Git CLI Service is called as fallback.

## Executing the project

Visual Studio 2019 is recommended for running this project.

Steps:
1. After cloning the repository, right click the project CommitViewer.API and select the option "Set as Startup Project".
2. After that, the project may be started by clicking play on "IIS Express" in the Visual Studio top bar OR by right clicking CommitViewer.API again and selecting Debug -> Start new instance.

A browser window will start and the initial page is Swagger's page, in order to help performing our requests.

See below screenshots of the 2 available APIs:

GitHub Docs provided as example the cURL below and it was interesting to use the mentioned owner and repository for testing purposes:
```
curl \
  -H "Accept: application/vnd.github.v3+json" \
  https://api.github.com/repos/octocat/hello-world/commits
```
Source: https://docs.github.com/en/rest/reference/repos#list-commits--code-samples

![image](https://user-images.githubusercontent.com/28615741/135924521-2bf1597f-6bad-472f-9707-113e8e548ed2.png)

![image](https://user-images.githubusercontent.com/28615741/135924643-4ca7fadf-20af-4942-867e-d3e8d283dd85.png)

## Project Structure

- CommitViewer.API
- CommitViewer.Business: For handling logic such as the mapping of the services responses into the commit model
- CommitViewer.IoC: Used for registering the interfaces and their implementations for DI purposes and also to define settings of the services, such as GitHubService
- CommitViewer.Services: Layer containing all the logic related to the services and also their interfaces
- CommitViewer.Shared: With extension method and other generic logic and abstractions that might be used throughout the application
- CommitViewer.Tests
