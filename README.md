# FSCC TCP Server
## Install as Windows Service
### Publish
```sc.exe create "fscc_proxy" binpath="C:\Path\To\dotnet.exe C:\Path\To\App.WindowsService.dll"```

### Create the Windows Service
```sc.exe create "fscc_proxy" binpath="C:\Path\To\App.WindowsService.exe"```