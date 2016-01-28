using System.Data.SQLite;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;

namespace LogProc {
	class SearchLog {
		public ObservableCollection<LogData> Log { get; set; }
		public List<StationData> Station { get; private set; }
		public int ExecutedNum { get; private set; }
		public bool? ExecStatus { get; private set; }
		public int FailedNum { get; private set; }
		public int FCheckNum { get; private set; }
		private SearchData SearchFunc;
		public bool Abort { get; private set; }
		private List<Area> AreaData { get; set; }
		private Setting Config;
		private ISearch Plugins;

		public SearchLog(WorkingData wd, ISearch isp) {
			Plugins = isp;
			Config = wd.Config;
			Log = wd.Log;
			Station = new List<StationData>();
			ExecStatus = null;
		}

		public void StartSearch() {
			ExecutedNum = 0;
			FailedNum = 0;
			FCheckNum = 0;
			ExecStatus = false;
			Abort = false;

			string anv = Anv.GetFromFile();
			if(anv == null) anv = Anv.GetFromWeb();

			foreach(LogData ld in Log) {
				if(Abort) break;
				ld.IsSearched = true;
				ld.IsFinded = true;
				ld.FailedStr = "";
				SearchFunc = new SearchData(ld.Callsign);
				if(SearchFunc.Station == null) {
					FailedNum++;
					ld.IsFinded = false;
				} else Station.Add(SearchFunc.Station);

				Plugins.log = ld;
				Plugins.station = SearchFunc.Station;
				Plugins.anvStr = anv;
				Plugins.config = Config;

				Plugins.check();
				if(Plugins.isErrorAvailable) FCheckNum++;
				ExecutedNum++;
			}

			ExecStatus = true;
		}

		private void LoadArea(string ContestAka) {
			System.IO.StreamReader sr = new System.IO.StreamReader(ContestAka + ".area.txt", System.Text.Encoding.Default);
			AreaData = new List<Area>();
			while(sr.Peek() > 0) {
				string buf = sr.ReadLine();
				switch(buf[0]) {
					case 'A':
						var m = Regex.Match(buf, @"A (\d*) (\w*)");
						AreaData.Add(new Area() { No = m.Groups[1].Value, Address = new List<string>() { m.Groups[2].Value } });
						break;
					case 'E':
						var mc = Regex.Matches(buf, @"E (\d*) (\w*)\(\w*\) (\w*)(, (\w*)){0,}");
						List<string> ls = new List<string>();
						ls.Add(mc[0].Groups[2].Captures[0].Value + mc[0].Groups[3].Captures[0].Value);
						foreach(var tn in mc[0].Groups[5].Captures) {
							ls.Add(mc[0].Groups[2].Captures[0].Value + (tn as Capture).Value);
						}
						AreaData.Add(new Area() { No = mc[0].Groups[1].Value, Address = ls });
						break;
					case 'I':
						var mci = Regex.Matches(buf, @"I (\d*) (\w*)\(\w*\) (\w*)(, (\w*)){0,}");
						List<string> lsi = new List<string>();
						lsi.Add(mci[0].Groups[2].Captures[0].Value + mci[0].Groups[3].Captures[0].Value);
						foreach(var tn in mci[0].Groups[5].Captures) {
							lsi.Add(mci[0].Groups[2].Captures[0].Value + (tn as Capture).Value);
						}
						AreaData.Add(new Area() { No = mci[0].Groups[1].Value, Address = lsi });
						break;
					case 'N':
						var mcn = Regex.Matches(buf, @"N (\d*) (\w*) (\w*)(, (\w*)){0,}");
						List<string> lsn = new List<string>();
						lsn.Add(mcn[0].Groups[3].Captures[0].Value);
						foreach(var ti in mcn[0].Groups[5].Captures) {
							lsn.Add((ti as Capture).Value);
						}
						AreaData.Add(new Area() { No = mcn[0].Groups[1].Value, Address = lsn });
						break;
					default: break;
				}
			}
			sr.Close();
		}

		public void DoAbort() {
			Abort = true;
		}

		public string GetCondition() {
			if(SearchFunc == null) return "";
			return SearchFunc.Condition;
		}

		class SearchData {
			public StationData Station { get; private set; }
			private string callsign;
			public string Condition { get; private set; }

			public SearchData(string CallSign) {
				callsign = Callsign.GetRemovedStroke(CallSign);

				Condition = "GetFromDB";
				Station = SearchDetailFromDB();
				if(Station != null) return;

				Condition = "Failed to Get from DB, Try to Get from Web";
				Station = GetTableData();
				if(Station == null) return;
				Condition = "Existed at Web, Try to Get Details";
				System.Threading.Thread.Sleep(1000);

				Condition = "Succeeded!";
				InsertStationData(Station);
			}

			private StationData GetTableData() {
				StationData sd = new StationData();
				System.Net.WebClient wc = new System.Net.WebClient();
				string source = wc.DownloadString("http://www.tele.soumu.go.jp/musen/SearchServlet?SC=1&pageID=3&SelectID=1&CONFIRM=0&SelectOW=01&IT=&HC=&HV=&FF=&TF=&HZ=3&NA=&MA=" + callsign + "&DFY=&DFM=&DFD=&DTY=&DTM=&DTD=&SK=2&DC=100&as_fid=2I6vX7ugLE0ekrPjPfMD#result");
				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(source);
				var head = doc.DocumentNode.SelectNodes(@"//div[@id=""temp1""]/table[@class=""borderstyle""]");
				if(head.Count == 2) return null;
				var m = Regex.Matches(head[2].InnerText, @"([ 　\n\t]*([\S　]*))");
				var l = Regex.Matches(head[2].InnerHtml, @"<a\s+[^>]*href\s*=\s*(?:(?<quot>[""'])(?<url>.*?)\k<quot>|" +
				@"(?<url>[^\s>]+))[^>]*>(?<text>.*?)</a>");
				//04-07, 08-0b, last2 space
				int nameint = 4;
				string nm = "";
				string url = "";
				sd.Address = "";
				while(nameint < m.Count) {
					if(Regex.IsMatch(m[nameint].Groups[2].Value, @"[\S　]*（" + callsign + @"）")) {
						nm = m[nameint].Groups[2].Value;
						url = l[3 + nameint / 4].Value;
						sd.Address += m[nameint + 1].Groups[2].Value + ", ";

					}
					nameint += 4;
				}
				if(nm == "") return null;
				var mt = Regex.Matches(nm, @"(.*)（" + callsign + @"）");
				var ml = Regex.Match(url, @"<a\s+[^>]*href\s*=\s*(?:(?<quot>[""'])(?<url>.*?)\k<quot>|" +
				@"(?<url>[^\s>]+))[^>]*>(?<text>.*?)</a>");
				sd.Url = ml.Groups["url"].Value;
				sd.Url = "http://www.tele.soumu.go.jp/musen/" + sd.Url.Substring(2);
				sd.Callsign = callsign;
				sd.Name = mt[0].Groups[1].Value;
				return sd;
			}

			private string GetFrequency(string Freq) {
				switch(Freq) {
					case "3537.5kHz":
						return "3.5MHz";
					case "7100kHz":
						return "7MHz";
					case "21225kHz":
						return "21MHz";
					case "52MHz":
						return "50MHz";
					case "145MHz":
						return "144MHz";
					case "435MHz":
						return "430MHz";
					case "1280MHz":
						return "1200MHz";
					default:
						return null;
				}
			}

			private StationData SearchDetailFromDB() {
				StationData sd = new StationData();
				using(var connect = new SQLiteConnection("Data Source=data/RadioStation.db")) {
					connect.Open();
					using(SQLiteCommand command = connect.CreateCommand()) {
						command.CommandText = "select * from Stations where callsign = '" + callsign + "'";
						using(var reader = command.ExecuteReader()) {
							if(!reader.HasRows) {
								connect.Close();
								return null;
							}
							reader.Read();
							sd.Callsign = reader["callsign"].ToString();
							sd.Address = reader["address"].ToString();
							sd.Name = reader["name"].ToString();
						}
					}
					connect.Close();
				}

				return sd;
			}

			private void InsertStationData(StationData sd) {
				using(var connect = new SQLiteConnection("Data Source=data/RadioStation.db")) {
					connect.Open();
					using(SQLiteTransaction sqlt = connect.BeginTransaction()) {
						using(SQLiteCommand command = connect.CreateCommand()) {
							command.CommandText = "insert into Stations (callsign, name, address) values('" + Callsign.GetRemovedStroke(sd.Callsign) + "', '" + sd.Name + "', '" + sd.Address + "')";
							command.ExecuteNonQuery();
						}
						sqlt.Commit();
					}
					connect.Close();
				}
			}
		}
	}
}