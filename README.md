# Mixdrop API
This library is used to communicate with the mixdrop server in C#.

## Login to your account
```cs
Mixdrop mixdrop = new("email", "key");
```
## List folders and files
```cs
Collection collection = mixdrop.FolderList();
```
## Upload File
```cs
string fileName = "hello_world.nbt";
Stream stream = File.Open(fileName, FileMode.Open);
string fileref = mixdrop.Upload(fileName, stream);
```
## Remote Upload
```cs
FileStatus fileStatus = mixdrop.RemoteUpload("http://127.0.0.1:8080/FTL.mp4");
```