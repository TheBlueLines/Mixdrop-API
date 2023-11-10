namespace TTMC.Mixdrop
{
	public class Item
	{
		public bool success { get; set; }
		public int? pages { get; set; }
		public object? result { get; set; }
	}
	public class Message
	{
		public string? msg { get; set; }
	}
	public class ID
	{
		public int id { get; set; }
	}
	public class Title
	{
		public string? title { get; set; }
	}
	public class CollectionFolder
	{
		public string? id { get; set; }
		public string? title { get; set; }
	}
	public class Collection
	{
		public List<CollectionFolder>? folders { get; set; }
		public List<CollectionFile>? files { get; set; }
	}
	public class FileStatus
	{
		public int? id { get; set; }
		public string? name { get; set; }
		public string? status { get; set; }
		public string? error { get; set; }
		public string? progress { get; set; }
		public string? added { get; set; }
		public string? completed { get; set; }
		public string? fileref { get; set; }
	}
	public class CollectionFile
	{
		public string? fileref { get; set; }
		public string? title { get; set; }
		public string? size { get; set; }
		public string? duration { get; set; }
		public bool? subtitle { get; set; }
		public bool? isvideo { get; set; }
		public bool? isaudio { get; set; }
		public string? added { get; set; }
		public string? status { get; set; }
		public int? date { get; set; }
		public bool? deleted { get; set; }
		public string? thumb { get; set; }
		public string? url { get; set; }
		public bool? yourfile { get; set; }
	}
	public class Language
	{
		public static string Arabic { get { return "ar"; } }
		public static string Bulgarian { get { return "bg"; } }
		public static string Bosnian { get { return "bs"; } }
		public static string Croatian { get { return "hr"; } }
		public static string Czech { get { return "cz"; } }
		public static string Danish { get { return "dk"; } }
		public static string English { get { return "en"; } }
		public static string Finnish { get { return "fi"; } }
		public static string French { get { return "fr"; } }
		public static string German { get { return "de"; } }
		public static string India { get { return "in"; } }
		public static string Italian { get { return "it"; } }
		public static string Indonesian { get { return "id"; } }
		public static string Japanese { get { return "jp"; } }
		public static string Malay { get { return "my"; } }
		public static string Norwegian { get { return "no"; } }
		public static string Polish { get { return "pl"; } }
		public static string Portuguese { get { return "pt"; } }
		public static string Russian { get { return "ru"; } }
		public static string Romanian { get { return "ro"; } }
		public static string Serbian { get { return "rs"; } }
		public static string Slovak { get { return "sk"; } }
		public static string Spanish { get { return "es"; } }
		public static string Sami { get { return "se"; } }
		public static string Turkish { get { return "tr"; } }
		public static string Mapudungun { get { return "ud"; } }
	}
}