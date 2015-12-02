using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows;
using LogProc.Definitions;

namespace LogProc {
	class LoadLog {
		private List<LogData> _ContestLog { get; set; }
		public List<LogData> ContestLog {
			get {
				return _ContestLog;
			}
		}

		public LoadLog() {
			_ContestLog = new List<LogData>();
		}

		public bool AddFile(string filePath) {
			List<LogData> tmp = new List<LogData>();
			if((tmp = LogFormat.GetCTESTWINLogList(filePath)) == null)
				if((tmp = LogFormat.GetZLOGTXTLogList(filePath)) == null) return false;

			_ContestLog.AddRange(tmp);

			return true;
		}

		public bool AddFiles() {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "CTESTWINログファイル(*.lg8)|*.lg8|ZLOGテキストログファイル(*.TXT)|*.TXT";
			ofd.Title = "ログファイル選択";
			ofd.FileName = "";
			ofd.Multiselect = true;
			if(ofd.ShowDialog() != true) {
				if(ofd.FileNames != null) return true;
				else return false;
			}
			foreach(string fp in ofd.FileNames) {
				AddFile(fp);
			}
			return true;
		}
	}

	class LogFormat {
		private static bool IsCTESTWINLog(string filePath) {
			BinaryStream bs = new BinaryStream();
			bs.ChangeEncoding("Shift-JIS");
			if(!bs.ReadOpen(filePath)) {
				return false;
			}
			try {
				bs.GETUSHORT();
				bs.GETCHAR(6);
				if(bs.GETCHAR(8) != "CQsoData") return false;
			} catch {
				return false;
			}

			return true;
		}

		private static DateTime CTimeToDate(long CTime) {
			return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(CTime).ToLocalTime();
		}

		public static List<LogData> GetCTESTWINLogList(string filePath) {
			List<LogData> tmp = new List<LogData>();
			if(!IsCTESTWINLog(filePath)) {
				return null;
			}

			BinaryStream bs = new BinaryStream();
			bs.ChangeEncoding("Shift-JIS");
			bs.ReadOpen(filePath);
			try {
				ushort logNum = bs.GETUSHORT();
				bs.GETCHAR(6);
				bs.GETCHAR(8);
				for(ushort i = 0;i < logNum;i++) {
					string call = bs.GETCHAR(defCTESTWIN.CallLen);
					string mycno = bs.GETCHAR(defCTESTWIN.NumLen);
					string opcno = bs.GETCHAR(defCTESTWIN.NumLen);
					string mode = ((defCTESTWIN.ModeStr)bs.GETUSHORT()).ToString();
					string freq = defCTESTWIN.GetFreqString((defCTESTWIN.FreqStr)bs.GETUSHORT());
					bs.GETUSHORT();
					bs.GETUSHORT();
					DateTime date = CTimeToDate(bs.GETLONG());
					string oprtr = bs.GETCHAR(defCTESTWIN.CallLen);
					bs.GETUSHORT();
					string rem = bs.GETCHAR(defCTESTWIN.RemLen + 2);
					tmp.Add(new LogData {
						Date = date, CallSign = call, SendenContestNo = mycno, ReceivenContestNo = opcno,
						Mode = mode, Frequency = freq, Operator = oprtr, Rem = rem, Point = 1,
						Searchen = false, Finden = true,
					});
				}
			} catch(Exception e) {
				MessageBox.Show("ログファイル読み込みエラー\r\n" + e.Message, "通知");
				return null;
			}

			return tmp;
		}

		public static List<LogData> GetZLOGTXTLogList(string filePath) {
			List<LogData> loglist = new List<LogData>();
			System.IO.StreamReader sr = new System.IO.StreamReader(filePath, System.Text.Encoding.Default);
			string[] splits;
			string[] columns = new string[11]{
				"mon", "day", "time", "callsign", "sent",
				"rcvd", "multi", "MHz", "mode", "pts", "memo"
			};

			try {
				for(int i = 0;sr.Peek() >= 0;i++) {
					splits = sr.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
					if(i == 0) {
						if(splits.Count() != 11) return null;
						for(int j = 0;j < 11;j++) {
							if(splits[j] != columns[j]) return null;
						}
					} else {
						if(splits.Count() == 0) break;
						int mon = int.Parse(splits[0]);
						int day = int.Parse(splits[1]);
						int time = int.Parse(splits[2]);
						int hour = time / 100;
						int min = (time - hour * 100) % 100;
						DateTime dt = new DateTime(DateTime.Now.Year, mon, day, hour, min, 0);
						int mhzno = 7;
						if(splits.Count() == 10) {
							mhzno = 6;
						}
						loglist.Add(new LogData() {
							Date = dt, CallSign = splits[3], SendenContestNo = splits[4],
							ReceivenContestNo = splits[5], Frequency = splits[mhzno++] + "MHz",
							Mode = splits[mhzno++], Operator = splits[++mhzno].Replace("%%", ""),
							Rem = "", Finden = false, Searchen = false,
						});
					}
				}
			} catch(Exception e) {
				MessageBox.Show("ログファイル読み込みエラー\r\n" + e.Message, "通知");
				return null;
			}

			sr.Close();

			return loglist;
		}
	}
}