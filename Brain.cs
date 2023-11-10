using System.Text.Json;

namespace TTMC.Mixdrop
{
	public class Mixdrop
	{
		private string email;
		private string key;
		private string domain;
		private HttpClient client;
		public Mixdrop(string email, string key, string domain = "mixdrop.ag")
		{
			this.email = email;
			this.key = key;
			this.domain = domain;
			client = new();
		}
		public string Upload(string fileName, byte[] data, int? folderID = null)
		{
			MultipartFormDataContent content = new();
			ByteArrayContent bytes = new(data);
			content.Add(bytes, "file", Path.GetFileName(fileName));
			content.Add(new StringContent(email), "email");
			content.Add(new StringContent(key), "key");
			if (folderID != null)
			{
				content.Add(new StringContent(folderID.Value.ToString()), "folder");
			}
			HttpResponseMessage response = client.PostAsync($"https://ul.{domain}/api", content).Result;
			string resp = response.Content.ReadAsStringAsync().Result;
			FileStatus fileStatus = ErrorCheck<FileStatus>(resp);
			return fileStatus.fileref ?? string.Empty;
		}
		public string Upload(string fileName, Stream stream, int? folderID = null)
		{
			MultipartFormDataContent content = new();
			StreamContent bytes = new(stream);
			content.Add(bytes, "file", Path.GetFileName(fileName));
			content.Add(new StringContent(email), "email");
			content.Add(new StringContent(key), "key");
			if (folderID != null)
			{
				content.Add(new StringContent(folderID.Value.ToString()), "folder");
			}
			HttpResponseMessage response = client.PostAsync($"https://ul.{domain}/api", content).Result;
			string resp = response.Content.ReadAsStringAsync().Result;
			FileStatus fileStatus = ErrorCheck<FileStatus>(resp);
			return fileStatus.fileref ?? string.Empty;
		}
		public string AddSubtitle(string path, string language)
		{
			MultipartFormDataContent content = new();
			byte[] file = System.IO.File.ReadAllBytes(path);
			ByteArrayContent bytes = new(file);
			content.Add(bytes, "file", Path.GetFileName(path));
			content.Add(new StringContent(language), "lang");
			HttpResponseMessage response = client.PostAsync($"https://api.{domain}/addsubtitle?email=" + email + "&key=" + key, content).Result;
			string tmp = response.Content.ReadAsStringAsync().Result;
			return ErrorCheck<string>(tmp);
		}
		public FileStatus RemoteUpload(string url, string? name = null, int? folderID = null)
		{
			string tmp = client.GetStringAsync($"https://api.{domain}/remoteupload?email=" + email + "&key=" + key + "&url=" + url + (name == null ? string.Empty : "&name=" + name) + (folderID == null ? string.Empty : "&folder=" + folderID.Value)).Result;
			return ErrorCheck<FileStatus>(tmp);
		}
		public FileStatus RemoteStatus(int? ID = null)
		{
			string tmp = client.GetStringAsync($"https://api.{domain}/remotestatus?email=" + email + "&key=" + key + (ID == null ? string.Empty : "&id=" + ID)).Result;
			return ErrorCheck<FileStatus>(tmp);
		}
		public Dictionary<string, CollectionFile> FileInfo(params string[] fileref)
		{
			string url = $"https://api.{domain}/fileinfo2?email=" + email + "&key=" + key;
			foreach (string value in fileref)
			{
				url += "&ref[]=" + value;
			}
			string tmp = client.GetStringAsync(url).Result;
			return ErrorCheck<Dictionary<string, CollectionFile>>(tmp);
		}
		public Collection FolderList(int? folderID = null, int? page = null)
		{
			string tmp = client.GetStringAsync($"https://api.{domain}/folderlist?email=" + email + "&key=" + key + (folderID == null ? string.Empty : "&id=" + folderID.Value) + (page == null ? string.Empty : "&page=" + page.Value)).Result;
			try
			{
				return ErrorCheck<Collection>(tmp);
			}
			catch
			{
				return new Collection() { files = new(), folders = new() };
			}
		}
		public int CreateFolder(string title, int? parent = null)
		{
			string tmp = client.GetStringAsync($"https://api.{domain}/foldercreate?email=" + email + "&key=" + key + "&title=" + title + (parent == null ? string.Empty : "&parent=" + parent.Value)).Result;
			ID id = ErrorCheck<ID>(tmp);
			return id.id;
		}
		public IEnumerable<CollectionFile> RemovedFiles(int? page = null)
		{
			string tmp = client.GetStringAsync($"https://api.{domain}/removed?email=" + email + "&key=" + key + (page == null ? string.Empty : "&page=" + page.Value)).Result;
			return ErrorCheck<IEnumerable<CollectionFile>>(tmp);
		}
		public string FileDuplicate(string fileref, int? folder = null)
		{
			string tmp = client.GetStringAsync($"https://api.{domain}/fileduplicate?email=" + email + "&key=" + key + "&ref=" + fileref + (folder == null ? string.Empty : "&folder=" + folder)).Result;
			FileStatus fileStatus = ErrorCheck<FileStatus>(tmp);
			return fileStatus.fileref ?? string.Empty;
		}
		public string FileRename(string fileref, string title)
		{
			string tmp = client.GetStringAsync($"https://api.{domain}/filerename?email=" + email + "&key=" + key + "&ref=" + fileref + "&title=" + title).Result;
			Title fileStatus = ErrorCheck<Title>(tmp);
			return fileStatus.title ?? string.Empty;
		}
		public string FolderRename(int id, string title)
		{
			string tmp = client.GetStringAsync($"https://api.{domain}/folderrename?email=" + email + "&key=" + key + "&id=" + id + "&title=" + title).Result;
			Title temp = ErrorCheck<Title>(tmp);
			return temp.title ?? string.Empty;
		}
		public T ErrorCheck<T>(string json)
		{
            if (json.StartsWith('{') && json.EndsWith('}'))
			{
				Item? item = JsonSerializer.Deserialize<Item>(json);
				if (item != null && item.result != null)
				{
					JsonElement result = (JsonElement)item.result;
					string raw = result.GetRawText();
					if (item.success)
					{
						T? response = JsonSerializer.Deserialize<T>(raw);
						if (response != null)
						{
							return response;
						}
					}
					else if (result.TryGetProperty("msg", out JsonElement error))
					{
						throw new(error.GetString());
					}
				}
			}
			throw new(json);
		}
	}
}