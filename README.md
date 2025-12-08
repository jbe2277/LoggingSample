# LoggingSample
Sample applications for the article: [Logging with .NET Standard 2.0](https://github.com/jbe2277/waf/wiki/Logging-with-.NET-Standard-2.0)

The solution contains:
Name | Description
--- | ---
SampleLibrary | .NET Standard 2.0 library that shows how TraceSource can be used for logging
SampleLibrary2 | .NET Standard 2.0 library that shows how Extensions.Logging can be used for logging
NLogConfigSample | WPF app that uses NLog for logging via `NLog.config` file
NLogCodeSample | WPF app that uses NLog for logging via code configuration
MicrosoftLoggingSample | WPF app that uses Microsoft.Extensions.Logging for logging. Just shows how to integrate TraceSource from SampleLibrary. But it does not support logging into files.
