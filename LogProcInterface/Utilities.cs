using System.Collections.Generic;
using System.Text.RegularExpressions;
using LogProc.Definitions;

namespace LogProc {
	namespace Utilities {
		public class SearchUtil {
			//MMなどは考えない
			public static bool CallSignIsStroke(string CallSign) {
				if(CallSign[CallSign.Length - 2] == '/') return true;
				else return false;
			}

			public static bool ContestNoIsWithPower(string Cn) {
				return Regex.IsMatch(Cn, @"(\d*)[^\d]");
			}

			public static bool IsJPCallSign(string CallSign) {
				if(CallSign[0] == 'J') {
					if('A' <= CallSign[1] && CallSign[1] <= 'S') return true;
				} else if(CallSign[0] == '7' || CallSign[0] == '8') {
					if('J' <= CallSign[1] && CallSign[1] <= 'N') return true;
				}

				return false;
			}

			public static int GetAreaNoFromCallSign(string CallSign) {
				if(!CallSignIsStroke(CallSign)) return int.Parse(CallSign.Substring(2, 1));
				return int.Parse(CallSign.Substring(CallSign.Length - 1));
			}

			public static string GetAreaNoFromAddress(StationData Station, List<Area> AreaData) {
				if(Station == null) return null;
				foreach(var a in AreaData) {
					foreach(var ad in a.Address) {
						foreach(var sa in GetAddressListFromStationData(Station)) {
							if(sa.Contains(ad)) return a.No;
						}
					}
				}
				return null;
			}

			//ALLJA 6mAndDown FieldDay AllKanagawa
			public static string GetPrefNoFromRcn(LogData Log) {
				Match m;
				if(Log.Mode != "CW") m = Regex.Match(Log.ReceivenContestNo, @"(\d\d)(\d*)\w*");
				else m = Regex.Match(Log.ReceivenContestNo, @"(\d\d\d)(\d*)\w*");
				int an = int.Parse(m.Groups[2].Value);
				if(101 <= an && an <= 114) return an.ToString();
				return m.Groups[2].Value;
			}

			//
			public static int GetAreaNoFromRcn(LogData Log) {
				int prefno = int.Parse(GetPrefNoFromRcn(Log));
				if(101 <= prefno && prefno <= 114) return 8;
				if(2 <= prefno && prefno <= 7) return 7;
				if(prefno == 8 || prefno == 9) return 0;
				if(10 <= prefno && prefno <= 17) return 1;
				if(18 <= prefno && prefno <= 21) return 2;
				if(22 <= prefno && prefno <= 27) return 3;
				if(28 <= prefno && prefno <= 30) return 9;
				if(31 <= prefno && prefno <= 35) return 4;
				if(36 <= prefno && prefno <= 39) return 5;
				if(40 <= prefno && prefno <= 47) return 6;
				if(prefno == 48) return 1;
				return -1;
			}

			//ACAG
			public static string GetPrefNoFromRcnTwoDigits(LogData Log) {
				Match m;
				if(Log.Mode != "CW") m = Regex.Match(Log.ReceivenContestNo, @"(\d\d)(\d\d)\d*\w*");
				else m = Regex.Match(Log.ReceivenContestNo, @"(\d\d\d)(\d\d)\d*\w*");
				return m.Groups[2].Value;
			}

			//ACAG
			public static int GetAreaNoFromRcnTwoDigits(LogData Log) {
				int prefno = int.Parse(GetPrefNoFromRcnTwoDigits(Log));
				if(prefno == 1) return 8;
				if(2 <= prefno && prefno <= 7) return 7;
				if(prefno == 8 || prefno == 9) return 0;
				if(10 <= prefno && prefno <= 17) return 1;
				if(18 <= prefno && prefno <= 21) return 2;
				if(22 <= prefno && prefno <= 27) return 3;
				if(28 <= prefno && prefno <= 30) return 9;
				if(31 <= prefno && prefno <= 35) return 4;
				if(36 <= prefno && prefno <= 39) return 5;
				if(40 <= prefno && prefno <= 47) return 6;
				return -1;
			}

			public static string GetContestAreaNoFromRcnWithPower(LogData Log) {
				Match m;
				if(Log.Mode != "CW") m = Regex.Match(Log.ReceivenContestNo, @"(\d\d)(\d*)\w");
				else m = Regex.Match(Log.ReceivenContestNo, @"(\d\d\d)(\d*)\w");
				return m.Groups[2].Value;
			}

			public static string GetContestAreaNoFromRcnDisPower(LogData Log) {
				Match m;
				if(Log.Mode != "CW") m = Regex.Match(Log.ReceivenContestNo, @"(\d\d)(\d*)");
				else m = Regex.Match(Log.ReceivenContestNo, @"(\d\d\d)(\d*)");
				return m.Groups[2].Value;
			}

			public static List<Area> GetAreaListFromFile(string AreaFileName) {
				System.IO.StreamReader sr = new System.IO.StreamReader("data/" + AreaFileName + ".area.txt", System.Text.Encoding.Default);
				var AreaData = new List<Area>();
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
				return AreaData;
			}

			public static object GetAddressListOrSuggestFromContestAreaNo(List<Area> AreaData, StationData Station, LogData Log, string Areano) {
				foreach(Area a in AreaData) {
					if(a.No == Areano) return a.Address;
				}
				return GetContestAreaNoFromStationData(Station, AreaData);
			}

			public static string GetContestAreaNoFromStationData(StationData Station, List<Area> AreaData) {
				if(Station == null) return "";
				foreach(Area a in AreaData) {
					foreach(var ad in a.Address) {
						foreach(var sa in GetAddressListFromStationData(Station)) {
							if(sa.Contains(ad)) return a.No;
						}
					}
				}
				return "";
			}

			public static List<string> GetAddressListFromStationData(StationData Station) {
				List<string> adr = new List<string>();

				var m = Regex.Matches(Station.Address, @"((\w*), )*");
				foreach(var c in m[0].Groups[2].Captures) {
					adr.Add((c as Capture).Value);
				}

				return adr;
			}

			public static string Get8JStationDataFromFile() {
				System.IO.StreamReader sr;
				try {
					sr = new System.IO.StreamReader("data/8JStation.txt", System.Text.Encoding.Default);
				}catch(System.IO.FileNotFoundException e) {
					System.Console.WriteLine(e.Message);
					return null;
				}
				string ret = sr.ReadToEnd();
				sr.Close();
				return ret;
			}

			public static string Get8JStationDataFromWeb() {
				System.Net.WebClient wc = new System.Net.WebClient();
				string source = wc.DownloadString("http://www.motobayashi.net/8j-station/2014.html");
				source += wc.DownloadString("http://www.motobayashi.net/8j-station/2015.html");

				return source;
			}

			public static string GetOpList(WorkingData Work) {
				if(!Work.Config.AutoOperatorEdit) {
					return Work.Config.Operator;
				}
				List<string> op = new List<string>();
				string ans = "";
				foreach(var l in Work.Log) {
					bool avail = false;
					foreach(var o in op) {
						if(o == l.Operator) {
							avail = true;
							break;
						}
					}
					if(avail) continue;
					//空白無視
					if(l.Operator == "") continue;
					op.Add(l.Operator);
				}
				op.Sort();
				for(int i = 0;i < op.Count;i++) {
					if(i == 0) {
						ans += op[i];
					} else {
						ans += ", " + op[i];
					}
				}
				return ans;
			}
		}
	}
}