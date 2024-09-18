# .Net Aspire

This project uses the .Net [Aspire Framework](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=linux&pivots=vscode).

## Run in Visual Studio Code

To run the project, go to `Run & Debug` and click the green play button.

Make sure the dropdown says `C#: WeatherCopilot.AppHost [Default Configuration].

## Cert Errors

In case of any certificate errors, try the following:

Try restarting VSCode. If that does not work, run the following command in the terminal:

```bash
dotnet dev-certs https --clean

dotnet dev-certs https --trust
```
