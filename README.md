# FSCC TCP Server
I can't make api calls through our office VPN so this will be running on the server that can do that and I'll access this service and make the FSCC api 

## Install as Windows Service
### Publish
```sc.exe create "fscc_proxy" binpath="C:\Path\To\dotnet.exe C:\Path\To\App.WindowsService.dll"```

### Create the Windows Service
```sc.exe create "fscc_proxy" binpath="C:\Path\To\App.WindowsService.exe"```

