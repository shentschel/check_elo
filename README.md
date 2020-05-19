# check_elo
A ELO 9 Icinga client written in C# without any unit tests.

## Non Nuget Dependencies
* EloIxClientCs.dll Version: 9.0.22.1 (ELO9 ready - Credits to [ELO Digital Office](https://www.elo.com/de-de.html))
* pcmIxClient7.dll Version: 1.0.0.0 (ELO9 ready - Credits to the person whoever wrote it)

## How to use

You need a config file to use check_elo. The config file is needs to have the same name as the check_elo executable. 
Below you find the content of a sample config file:

```json
{
    "Elo": {
        "HostName": "eloserver1",
        "ArchiveName": "archive",
        "Port": "9080",
        "Username": "elouser",
        "Password": "",
        "ArchiveFilePath": "\\",
        "FullTextPath": "D:\\"
    },
    "Database": {
        "HostName": "dbserver1",
        "Username": "user",
        "Password": ""
    },
    "Tomcat": {
        "Username": "TomcatAdmin",
        "Password": ""
    }
}
```


## Logging and Error Handling

There is rudimentary logging and error handling in this project. If you provide a log4net.config file 
the application will logs errors which are thrown by the application. Fun fact: If you provide a log4net.config
file the 3rdParty library pcmIxClient7 also starts logging.

## TODOs

- [ ] Add something something to make VisualStudio able to open the solution (Solution created by JetBrains Rider 2020.1.1)
- [ ] Improve Logging and Error Handling
- [ ] Write Tests for the application...
- [ ] try to upgrade the application for ELO12
