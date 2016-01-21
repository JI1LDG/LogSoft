using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LogProc.Definitions;

namespace LogProc {
	namespace Utilities {
		public class SearchUtil {
			public static bool CallsignHasStroke(string Callsign) {
				if (Callsign[Callsign.Length - 2] == '/') return true;
				else return false;
			}

			public static bool CnHasPower(string Cn) {
				return Regex.IsMatch(Cn, @"(\d*)[^\d]");
			}

			public static string[] GetRSTVal(LogData Log) {
				var chk = new List<string>();
				int chars = 2;
				if (Log.Mode == "CW") chars = 3;
				chk.Add(Log.SendenContestNo.Substring(chars));
				chk.Add(Log.ReceivenContestNo.Substring(chars));
				return chk.ToArray();
			}

			public static bool FrequencyChk(string freq) {
				if (defCTESTWIN.GetFreqNum(freq) == -1) {
					return false;
				} else return true;
			}

			public static bool IsJPCallSign(string CallSign) {
				if(CallSign[0] == 'J') {
					if('A' <= CallSign[1] && CallSign[1] <= 'S') return true;
				} else if(CallSign[0] == '7' || CallSign[0] == '8') {
					if('J' <= CallSign[1] && CallSign[1] <= 'N') return true;
				}

				return false;
			}
			
			public static void AnvstaChk(string Callsign, string AnvStation, List<ErrorReason> _er, StationData Station) {
				if (Callsign[0] != '8') return;
				string cs = defSearch.GetCallSignBesideStroke(Callsign);
				if (!AnvStation.Contains(cs)) {
					ErrorReason.SetError(_er, "CannotConfirmAnvsta");
				}
				if(Station == null && Callsign[0] != '8') {
					ErrorReason.SetError(_er, "FailedToGetData");
				}
			}

			public static int GetRegionFromCallSign(string CallSign, bool HasStroke) {
				if (!HasStroke) {
					int region;
					if(int.TryParse(CallSign.Substring(2, 1), out region) == false) {
						return -1;
					}

					if(CallSign[0] == '7') {
						if('K' <= CallSign[1] && CallSign[1] <= 'N') {
							if('1' <= CallSign[2] && CallSign[2] <= '4') {
								return 1;
							}
						}
					}
					return region;
				}
				return int.Parse(CallSign.Substring(CallSign.Length - 1));
			}

			//59(9)####(*)
			public static string GetPrefno(LogData Log, bool AcagMode = false) {
				string res = GetAreano(Log);
				if (AcagMode) {
					return res.Substring(0, 2);//k7zoo america callsign
				} else {
					return res;
				}
			}

			public static string GetAreano(LogData Log) {
				Match m;
				if (Log.Mode != "CW") m = Regex.Match(Log.ReceivenContestNo, @"(\d\d)(\d*)\w*");
				else m = Regex.Match(Log.ReceivenContestNo, @"(\d\d\d)(\d*)\w*");
				return m.Groups[2].Value;
			}

			public static int GetRegionFromCn(string Prefno) {
				int prefno = int.Parse(Prefno);
				if (101 <= prefno && prefno <= 114) return 8;
				if (prefno == 1) return 8;
				if (2 <= prefno && prefno <= 7) return 7;
				if (prefno == 8 || prefno == 9) return 0;
				if (10 <= prefno && prefno <= 17) return 1;
				if (18 <= prefno && prefno <= 21) return 2;
				if (22 <= prefno && prefno <= 27) return 3;
				if (28 <= prefno && prefno <= 30) return 9;
				if (31 <= prefno && prefno <= 35) return 4;
				if (36 <= prefno && prefno <= 39) return 5;
				if (40 <= prefno && prefno <= 47) return 6;
				if (prefno == 48) return 1;
				return -1;
			}

			public static bool AreanoExists(List<Area> AreaList, string areano) {
				foreach(var a in AreaList) {
					if(a.No == areano) {
						return true;
					}
				}
				return false;
			}

			public static List<string> GetAreanoFromStation(StationData Station, List<Area> AreaData) {
				var gsal = GetStationAddressList(Station);
				if (gsal == null) return null;
				var res = new List<string>();
				foreach(var st in gsal) {
					foreach(var ad in AreaData) {
						foreach(var ar in ad.Address) {
							if(st.Contains(ar) == true) {
								res.Add(ad.No); //ato de chofuku jokyo
							}
						}
					}
				}
				return res;
			}

			public static List<string> GetAreanoFromAddressList(List<string> AddressList, List<Area> AreaData) {
				if (AddressList == null) return null;
				var tmp = new List<string>();
				foreach(var al in AddressList) {
					foreach(var ad in AreaData) {
						foreach(var a in ad.Address) {
							if (al.Contains(a)) {
								tmp.Add(al + "(" + ad.No + ")");
							}
						}
					}
				}
				return tmp;
			}

			public static List<string> GetStationAddressList(StationData Station) {
				if (Station == null) return null;
				var tmp = new List<string>();
				var spl = Station.Address.Split(new char[] { ',' });
				return spl.ToList<string>();
			}

			public static bool AreanoMatches(string cnAreano, List<string> stationAreano) {
				if (stationAreano == null) return false;
				foreach(var sa in stationAreano) {
					if(cnAreano == sa) {
						return true;
					}
				}
				return false;
			}

			public static string ConvToStrFromList(List<string> lstr) {
				if (lstr == null) return "";
				return string.Join(", ", lstr.ToArray());
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
				int nowyr = System.DateTime.Now.Year - 1;
				string source = "8jsta";
				try {
					source = wc.DownloadString("http://www.motobayashi.net/8j-station/" + nowyr + ".html");
					source += wc.DownloadString("http://www.motobayashi.net/8j-station/" + nowyr + 1 + ".html");
				} catch(System.Net.WebException e) {
					System.Console.WriteLine("GetError: " + e.Message);
				}

				return source;
			}

			public static string GetOpList(WorkingData Work) {
				if(!Work.Config.AutoOperatorEdit) {
					return Work.Config.Operator;
				}
				List<string> op = new List<string>();
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
				return string.Join(", ", op.ToArray());
			}
		}
	}
}