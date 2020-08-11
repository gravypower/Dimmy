SharpHostsFile
====================

[![Build status](https://ci.appveyor.com/api/projects/status/6la9stqxod5i96xs?svg=true)](https://ci.appveyor.com/project/NateShoffner/sharphostsfile/branch/master)

SharpHostsFile is a .NET wrapper library for the Windows hosts file.

```cs
var hostsFile = new HostsFile();
hostsFile.Load(HostsFile.GetDefaultHostsFilePath());
hostsFile.Add(new HostsFileMapEntry(IPAddress.Loopback, "github.com"));
hostsFile.Save(HostsFile.GetDefaultHostsFilePath());
```  
            
 ### License ###

    Copyright 2018 Nate Shoffner

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.