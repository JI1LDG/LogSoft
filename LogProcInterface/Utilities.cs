using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LogProc.Definitions;

namespace LogProc {
	namespace Utilities {
		public static class Callsign {
			public static bool HasStroke(string callsign) {
				if (callsign[callsign.Length - 2] == '/') return true;
				else return false;
			}

			public static string GetRemovedStroke(string callsign) {
				if (callsign.IndexOf("/") != -1) {
					callsign = callsign.Substring(0, callsign.IndexOf("/"));
				}
				return callsign;
			}

			public static bool IsJACallsign(string callsign) {
				if (callsign[0] == 'J') {
					if ('A' <= callsign[1] && callsign[1] <= 'S') return true;
				} else if (callsign[0] == '7' || callsign[0] == '8') {
					if ('J' <= callsign[1] && callsign[1] <= 'N') return true;
				}

				return false;
			}

			public static string GetRegion(string callsign, bool hasStroke) {
				if (!hasStroke) {
					int region;
					if (int.TryParse(callsign.Substring(2, 1), out region) == false) {
						return "-1";
					}

					if (callsign[0] == '7') {
						if ('K' <= callsign[1] && callsign[1] <= 'N') {
							if ('1' <= callsign[2] && callsign[2] <= '4') {
								return "1";
							}
						}
					}
					return region.ToString();
				}
				return callsign.Substring(callsign.Length - 1);
			}
		}
		public static class Contestno {
			public static void CheckSentCn(LogData log, string contestno, string extraCn, bool hasPower, Dictionary<string, ErrorReason> listErr) {
				string chk = Contestno.GetRstFilteredStr(log)["Sent"];
				var cn = contestno;
				var excn = extraCn;
				if (excn != null) cn = excn;
				if (chk != cn || Contestno.HasPower(log.SentCn) != hasPower) {
					ErrorReason.Set(listErr, Reason.InvalidSentCn.ToString());
				}
			}

			public static bool HasPower(string contestno) {
				return Regex.IsMatch(contestno, @"(\d*)[^\d]");
			}

			public static Dictionary<string, string> GetRstFilteredStr(LogData log) {
				var chk = new Dictionary<string, string>();
				int chars = 2;
				if (log.Mode == ModeStr.CW.ToString()) chars = 3;
				chk.Add("Sent", log.SentCn.Substring(chars));
				chk.Add("Received", log.ReceivedCn.Substring(chars));
				return chk;
			}

			public static string GetAreano(LogData log) {
				return GetAreano(GetRstFilteredStr(log)["Received"]);
			}
			public static string GetAreano(string rstFilteredStr) {
				Match m = Regex.Match(rstFilteredStr, @"(\d*)\w*");
				return m.Groups[1].Value;
			}

			public static string GetPrefno(LogData log, bool isAcagMode = false) {
				return GetPrefno(GetAreano(log), isAcagMode);
			}
			public static string GetPrefno(string areano, bool isAcagMode = false) {
				if (isAcagMode) {
					return areano.Substring(0, 2);//k7zoo america callsign
				} else {
					return areano;
				}
			}
			
			public static string GetRegion(string prefno) {
				int pn = int.Parse(prefno);
				if (101 <= pn && pn <= 114) return "8";
				if (pn == 1) return "8";
				if (2 <= pn && pn <= 7) return "7";
				if (pn == 8 || pn == 9) return "0";
				if (10 <= pn && pn <= 17) return "1";
				if (18 <= pn && pn <= 21) return "2";
				if (22 <= pn && pn <= 27) return "3";
				if (28 <= pn && pn <= 30) return "9";
				if (31 <= pn && pn <= 35) return "4";
				if (36 <= pn && pn <= 39) return "5";
				if (40 <= pn && pn <= 47) return "6";
				if (pn == 48) return "1";
				return "-1";
			}

			public static List<string> GetPhoneRegion(string areano, List<Area> listArea) {
				var tmp = new int[10];
				foreach(var la in listArea) {
					if(la.No == areano) {
						foreach(var ladd in la.Address) {
							for(int i = 0;i < 10; i++) {
								if (tmp[i] > 0) continue;
								foreach(var p in defs.PrefList()[i]) {
									if (ladd.Contains(p)) {
										tmp[i]++;
										break;
									}
								}
							}
						}
					}
				}
				var ret = new List<string>();
				for(int j = 0;j < 10; j++) {
					if (tmp[j] > 0) ret.Add(j.ToString());
				}
				if (ret.Count > 0) return ret;
				return null;
			}

			public static Dictionary<CnStr, string> GetHoge(CnStr cnStr, LogData log, bool isAcagMode) {
				var dicss = new Dictionary<CnStr, string>();

				if (!cnStr.HasFlag(CnStr.RstFltred)) {
					dicss.Add(CnStr.RstFltred, GetRstFilteredStr(log)["Received"]);
					if (!cnStr.HasFlag(CnStr.Area)) {
						dicss.Add(CnStr.Area, GetAreano(dicss[CnStr.RstFltred]));
						if (!cnStr.HasFlag(CnStr.Pref)) {
							dicss.Add(CnStr.Pref, GetPrefno(dicss[CnStr.Area], isAcagMode));
							if (!cnStr.HasFlag(CnStr.Region)) {
								dicss.Add(CnStr.Region, GetRegion(dicss[CnStr.Pref]));
							}
						}
					}
				}

				return dicss;
			}
		}
		public static class Freq {
			public static string CnvTostr(this FreqStr freqStr) {
				string[] s = new string[] { "1.9MHz", "3.5MHz", "7MHz", "10MHz", "14MHz", "18MHz", "21MHz", "24MHz", "28MHz", "50MHz", "144MHz", "430MHz", "1200MHz", "2400MHz", "5600MHz" };
				return s[(int)freqStr];
			}

			public static int CnvTofrqnum(string freq) {
				string[] s = new string[] { "1.9MHz", "3.5MHz", "7MHz", "10MHz", "14MHz", "18MHz", "21MHz", "24MHz", "28MHz", "50MHz", "144MHz", "430MHz", "1200MHz", "2400MHz", "5600MHz" };
				int i = 0;
				for (; i < s.Length; i++) {
					if (s[i] == freq) break;
				}
				if (i == s.Length) return -1;
				else return i;
			}

			public static bool IsBeen(string freq) {
				if (Freq.CnvTofrqnum(freq) == -1) {
					return false;
				} else return true;
			}
		}
		public static class Anv {
			public static void Check(string callsign, string anvStr, Dictionary<string, ErrorReason> listError, StationData station) {
				if (callsign[0] != '8' || callsign.Substring(0, 2) != "8J") return;
				string cs = Callsign.GetRemovedStroke(callsign);
				if (!anvStr.Contains(cs)) {
					ErrorReason.Set(listError, Reason.AnvUnchecked.ToString());
				}
				if(station == null && callsign[0] != '8') {
					ErrorReason.Set(listError, Reason.GetDataFailed.ToString());
				}
			}

			public static string GetFromFile() {
				System.IO.StreamReader sr;
				try {
					sr = new System.IO.StreamReader("data/8JStation.txt", System.Text.Encoding.Default);
				} catch (System.IO.FileNotFoundException e) {
					System.Console.WriteLine(e.Message);
					return null;
				}
				string ret = sr.ReadToEnd();
				sr.Close();
				return ret;
			}

			public static string GetFromWeb() {
				System.Net.WebClient wc = new System.Net.WebClient();
				int nowyr = System.DateTime.Now.Year - 1;
				string source = "8jsta";
				try {
					source = wc.DownloadString("http://www.motobayashi.net/8j-station/" + nowyr + ".html");
					source += wc.DownloadString("http://www.motobayashi.net/8j-station/" + nowyr + 1 + ".html");
				} catch (System.Net.WebException e) {
					System.Console.WriteLine("GetError: " + e.Message);
				}

				System.IO.StreamWriter sw = new System.IO.StreamWriter(@"data/8JStation.txt", false, System.Text.Encoding.Default);
				sw.Write(source);
				sw.Close();

				return source;
			}
		}
		public static class Areano {
			public static bool IsBeen(List<Area> listArea, string areano) {
				foreach(var a in listArea) {
					if(a.No == areano) {
						return true;
					}
				}
				return false;
			}

			public static List<Area> GetList(string areaFileName) {
				System.IO.StreamReader sr = new System.IO.StreamReader("data/" + areaFileName + ".area.txt", System.Text.Encoding.Default);
				var AreaData = new List<Area>();
				while (sr.Peek() > 0) {
					string buf = sr.ReadLine();
					ExecuteFromStr(AreaData, buf);
				}
				sr.Close();
				return AreaData;
			}

			public static void ExecuteFromStr(List<Area> AreaData, string buf) {
				switch (buf[0]) {
					case 'A':
						var m = Regex.Match(buf, @"A (\d*) (\w*)");
						AreaData.Add(new Area() { No = m.Groups[1].Value, Address = new List<string>() { m.Groups[2].Value } });
						break;
					case 'E':
						var mc = Regex.Matches(buf, @"E (\d*) (\w*)\(\w*\) (\w*)(, (\w*)){0,}");
						List<string> ls = new List<string>();
						ls.Add(mc[0].Groups[2].Captures[0].Value + mc[0].Groups[3].Captures[0].Value);
						foreach (var tn in mc[0].Groups[5].Captures) {
							ls.Add(mc[0].Groups[2].Captures[0].Value + (tn as Capture).Value);
						}
						AreaData.Add(new Area() { No = mc[0].Groups[1].Value, Address = ls });
						break;
					case 'I':
						var mci = Regex.Matches(buf, @"I (\d*) (\w*)\(\w*\) (\w*)(, (\w*)){0,}");
						List<string> lsi = new List<string>();
						lsi.Add(mci[0].Groups[2].Captures[0].Value + mci[0].Groups[3].Captures[0].Value);
						foreach (var tn in mci[0].Groups[5].Captures) {
							lsi.Add(mci[0].Groups[2].Captures[0].Value + (tn as Capture).Value);
						}
						AreaData.Add(new Area() { No = mci[0].Groups[1].Value, Address = lsi });
						break;
					case 'N':
						var mcn = Regex.Matches(buf, @"N (\d*) (\w*) (\w*)(, (\w*)){0,}");
						List<string> lsn = new List<string>();
						lsn.Add(mcn[0].Groups[3].Captures[0].Value);
						foreach (var ti in mcn[0].Groups[5].Captures) {
							lsn.Add((ti as Capture).Value);
						}
						AreaData.Add(new Area() { No = mcn[0].Groups[1].Value, Address = lsn });
						break;
					default: break;
				}
			}

			public static List<string> GetFromList(List<string> listAddress, List<Area> listArea) {
				if (listAddress == null) return null;
				var tmp = new List<string>();
				foreach (var al in listAddress) {
					foreach (var ad in listArea) {
						foreach (var a in ad.Address) {
							if (al.Contains(a)) {
								tmp.Add(al + "(" + ad.No + ")");
							}
						}
					}
				}
				return tmp;
			}

			public static List<string> GetFromStation(StationData station, List<Area> listArea) {
				var gsal = Station.GetList(station);
				if (gsal == null) return null;
				var res = new List<string>();
				foreach(var st in gsal) {
					foreach(var ad in listArea) {
						foreach(var ar in ad.Address) {
							if(st.Contains(ar) == true) {
								res.Add(ad.No); //ato de chofuku jokyo
							}
						}
					}
				}
				return res;
			}

			public static List<Area> GetMixedListFromPref(string[] deletePref, string addDataFileName) {
				string mixedData = "";
				bool isHokkaido = false; int hkdCnt = 0;

				System.IO.StreamReader sr = new System.IO.StreamReader("data/Prefectures.area.txt", System.Text.Encoding.Default);
				if(deletePref.Where(dp => dp == "01").Count() > 0) {
					isHokkaido = true;
				}
				while (sr.Peek() > 0) {
					string buf = sr.ReadLine();
					var tmp = buf.Split(' ');
					if(isHokkaido && hkdCnt++ < 14) {
						continue;
					}
					bool chk = false;
					foreach(var dp in deletePref) {
						if(tmp[1] == dp) {
							chk = true;
						}
					}
					if (!chk) {
						mixedData += buf + "\r\n";
					}
				}
				sr.Close();

				if (addDataFileName == "__hkd") {
					mixedData += "A 01 北海道\r\n";
				} else {
					sr = new System.IO.StreamReader("data/" + addDataFileName + ".area.txt", System.Text.Encoding.Default);
					while (sr.Peek() > 0) {
						string buf = sr.ReadLine();
						mixedData += buf + "\r\n";
					}
				}

				var AreaData = new List<Area>();
				foreach(var d in mixedData.Split(new string[] { "\r\n", }, System.StringSplitOptions.RemoveEmptyEntries)) {
					ExecuteFromStr(AreaData, d);
				}
				return AreaData;
			}

			public static List<Area> GetMixedListFromPref(string[] deletePref, string[] addFromACAGPref) {
				string mixedData = "";
				bool isHokkaido = false; int hkdCnt = 0;

				System.IO.StreamReader sr = new System.IO.StreamReader("data/Prefectures.area.txt", System.Text.Encoding.Default);
				if (deletePref.Where(dp => dp == "01").Count() > 0) {
					isHokkaido = true;
				}
				while (sr.Peek() > 0) {
					string buf = sr.ReadLine();
					var tmp = buf.Split(' ');
					if (isHokkaido && hkdCnt++ < 14) {
						continue;
					}
					bool chk = false;
					foreach (var dp in deletePref) {
						if (tmp[1] == dp) {
							chk = true;
						}
					}
					if (!chk) {
						mixedData += buf + "\r\n";
					}
				}
				sr.Close();

				sr = new System.IO.StreamReader("data/ACAG.area.txt", System.Text.Encoding.Default);
				while (sr.Peek() > 0) {
					string buf = sr.ReadLine();
					var tmp = buf.Split(' ');
					bool chk = false;
					foreach (var ap in addFromACAGPref) {
						if (ap == "__hkd") continue;
						if (tmp[1].Substring(0, 2) == ap) {
							chk = true;
						}
					}
					if (!chk) continue;
					mixedData += buf + "\r\n";
				}
				if(addFromACAGPref.Where(fp => fp == "__hkd").Count() > 0) {
					mixedData += "A 01 北海道\r\n";
				}

				var AreaData = new List<Area>();
				foreach (var d in mixedData.Split(new string[] { "\r\n", }, System.StringSplitOptions.RemoveEmptyEntries)) {
					ExecuteFromStr(AreaData, d);
				}
				return AreaData;
			}
		}
		public static class Station {
			public static List<string> GetList(StationData station) {
				if (station == null) return null;
				var tmp = new List<string>();
				var spl = station.Address.Split(new char[] { ',' });
				return spl.ToList<string>();
			}

			public static bool IsMatched(string areano, List<string> listAreano) {
				if (listAreano == null) return false;
				foreach (var sa in listAreano) {
					if (areano == sa) {
						return true;
					}
				}
				return false;
			}
		}
		public static class Utils {
			public static string ConvTostrarrFromList(List<string> listStr) {
				if (listStr == null) return "";
				return string.Join(", ", listStr.Distinct<string>().ToArray());
			}			

			public static string GetOpList(WorkingData workData) {
				if(!workData.Config.IsAutoOperatorEditEnabled) {
					return workData.Config.Operator;
				}
				List<string> op = new List<string>();
				foreach(var l in workData.Log) {
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
				return string.Join(", ", op.ToArray());
			}
		}
	}
}