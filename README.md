# arsenite.xyz discord token logger

Reverse engineered source for the arsenite discord token logger

Logs tokens and disables 2fa 

someone attempted to send this to a server I'm in. I downloaded it and saw that it was written in C#

I reverse engineered it. Here's the source.

# directory tree

```
arsenite/
├── arsenite
│   ├── Config.cs
│   ├── Json
│   ├── Program.cs
│   ├── Properties
│   │   └── AssemblyInfo.cs
│   ├── TokenDiscovery.cs
│   ├── Update
│   ├── app.manifest
│   └── arsenite.csproj
├── arsenite.sln
└── binaries
    ├── Update
    └── json
```
