﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Text.RegularExpressions;
using LogProc.Definitions;
using LogProc.Interfaces;
using LogProc.Utilities;

namespace LogProc {
	/// <summary>
	/// OutputSummery.xaml の相互作用ロジック
	/// </summary>
	public partial class OutputSummery : Window {
		public OutputSummery(WorkingData wd, InterSet[] isp) {

			OutputLog ol = new OutputLog(wd, isp[0].Sum);
			if (!ol.Execute()) {
				MessageBox.Show("サマリ作成に失敗しました。", "通知");
				return;
			}
			InitializeComponent();
			tbOutput.Text = ol.Sheet;
		}
	}

	public class OutputLog {
		private ISummery Plugins;
		private WorkingData Work { get; set; }
		private List<Multiply> Multi;
		public string opLog;
		private string Summery;
		public string Sheet { get; set; }
		private int freqNum;
		private int areaMax;

		public OutputLog(WorkingData wd, ISummery isp) {
			Plugins = isp;
			freqNum = 15;
			areaMax = 500000;
			Multi = new List<Multiply>();
			Work = wd;
			if (Plugins != null) {
				Plugins.config = Work.Config;
				Plugins.areaMax = areaMax;
				Plugins.freqNum = freqNum;
				Plugins.listMulti = Multi;
			}
		}

		public bool Execute() {
			CreateLog();
			CreateSummery();

			Sheet = Summery + opLog;
			return true;
		}

		public void CreateSummery() {
			Summery = "<SUMMARYSHEET VERSION=R1.0>\r\n";
			Summery += "<CONTESTNAME>" + Work.Config.ContestName + "</CONTESTNAME>\r\n";
			if(Work.Config.CategoryCode != "") Summery += "<CATEGORYCODE>" + Work.Config.CategoryCode + "</CATEGORYCODE>\r\n";
			Summery += "<CATEGORYNAME>" + Work.Config.CategoryName + "</CATEGORYNAME>\r\n";
			Summery += "<CALLSIGN>" + Work.Config.Callsign + "</CALLSIGN>\r\n";
			Summery += Plugins.isScoreEdited ? Plugins.getScoreStr() : GetScoreStr();
			Summery += "<ADDRESS>〒" + Work.Config.ZipCode + " " + Work.Config.Address + "</ADDRESS>\r\n";
			Summery += "<TEL>" + Work.Config.Phone + "</TEL>\r\n";
			Summery += "<NAME>" + Work.Config.Name + "</NAME>\r\n";
			Summery += "<EMAIL>" + Work.Config.Mail + "</EMAIL>\r\n";
			Summery += "<LICENSECLASS>" + Work.Config.LicenserLicense + "</LICENSECLASS>\r\n";
			Summery += "<POWER>" + Work.Config.PowerVal + "</POWER>\r\n";
			Summery += "<POWERTYPE>" + Work.Config.PowerType + "</POWERTYPE>\r\n";
			if (Work.Config.Place != "") Summery += "<OPPLACE>" + Work.Config.Place + "</OPPLACE>\r\n";
			if (Work.Config.Supply != "") Summery += "<POWERSUPPLY>" + Work.Config.Supply + "</POWERSUPPLY>\r\n";
			Summery += "<EQUIPMENT>" + Work.Config.Equip + "</EQUIPMENT>\r\n";
			Summery += "<COMMENTS>" + Work.Config.Comment + "</COMMENTS>\r\n";
			Summery += "<MULTIOPLIST>" + Utils.GetOpList(Work) + "</MULTIOPLIST>\r\n";
			Summery += "<OATH>" + Work.Config.Oath + "</OATH>\r\n";
			Summery += "<DATE>" + DateTime.Today.ToLongDateString() + "</DATE>\r\n";
			Summery += "<SIGNATURE>" + Work.Config.LicenserName + "</SIGNATURE>\r\n";
			Summery += "</SUMMARYSHEET>\r\n";
		}

		public void CreateLog(bool sheetType = true, bool newFormat = true) {
			opLog = "";
			if (sheetType) opLog += "<LOGSHEET TYPE=" + Work.Config.UseType + ">\r\n";
			if(newFormat) opLog += "DATA(JST) TIME BAND MODE CALLSIGN SENTNo RCVNo Multi PTS\r\n";
			else opLog += "mon day time callsign        sent       rcvd       multi  MHz  mode pts memo\r\n";
			foreach (var l in Work.Log) {
				if (l.Point == 0) continue;
				bool mul = false;
				bool found = false;
				string areano = Plugins.getAreano(l);
				int freq = Freq.CnvTofrqnum(l.Freq);
				if (areano == "" || freq == -1) continue;
				foreach (var m in Multi) {
					if (m.Freq == freq && m.Areano == areano) {
						m.Num++;
						m.Point += l.Point;
						found = true;
						break;
					}
				}
				if (!found) {
					Multi.Add(new Multiply() {
						Freq = freq, Areano = areano, Num = 1, Point = l.Point,
					});
					mul = true;
				}
				if (newFormat) {
					opLog += l.Date.Year.ToString() + "-" + l.Date.Month.ToString() + "-" + l.Date.Day.ToString() + l.Date.ToString(" HH:mm ");
					opLog += DivFreqNum(l.Freq) + " " + l.Mode + " " + l.Callsign + " ";
					Match mm, mn;
					if (l.Mode == "CW") {
						mm = Regex.Match(l.SentCn, @"(\d\d\d)(.*)");
						mn = Regex.Match(l.ReceivedCn, @"(\d\d\d)(.*)");
					} else {
						mm = Regex.Match(l.SentCn, @"(\d\d)(.*)");
						mn = Regex.Match(l.ReceivedCn, @"(\d\d)(.*)");
					}
					opLog += mm.Groups[1].Value + " " + mm.Groups[2].Value + " ";
					opLog += mn.Groups[1].Value + " " + mn.Groups[2].Value + " ";
					if (mul) {
						opLog += areano.ToString() + " ";
					} else {
						opLog += "- ";
					}
					opLog += l.Point + "\r\n";
				} else {
					opLog += l.Date.Month.ToString().PadRight(4) + l.Date.Day.ToString().PadRight(4) + l.Date.ToString("HHmm ");

					opLog += l.Callsign.PadRight(15 + 1) + l.SentCn.PadRight(10 + 1) + l.ReceivedCn.PadRight(10 + 1);
					if (mul) opLog += areano.ToString().PadRight(6 + 1);
					else opLog += "".PadRight(6 + 1);
					opLog += DivFreqNum(l.Freq).PadRight(5) + l.Mode.PadRight(5);
					opLog += l.Point.ToString().PadRight(4);
					if (sheetType == false) {
						opLog += "%%" + l.Operator + "%%" + "\r\n";
					} else {
						opLog += l.Operator + "\r\n";
					}
				}
			}
			if (sheetType) opLog += "</LOGSHEET>\r\n";
		}

		private string GetScoreStr() {
			string output = "";
			int totalLog = 0;
			int totalPts = 0;
			int totalMulti = 0;
			for (int i = 0; i < freqNum; i++) {
				int multiNum = 0;
				int logNum = 0;
				int ptsNum = 0;
				foreach (var m in Multi) {
					if (m.Freq != i) continue;
					multiNum++;
					logNum += m.Num;
					ptsNum += m.Point;
				}
				if (multiNum == 0) continue;
				output += "<SCORE BAND=" + Freq.CnvTostr((FreqStr)i) + ">";
				output += logNum + "," + ptsNum + "," + multiNum + "</SCORE>\r\n";
				totalLog += logNum;
				totalPts += ptsNum;
				totalMulti += multiNum;
			}
			if (totalLog == 0) return "";
			output += "<SCORE BAND=TOTAL>" + totalLog + "," + totalPts + "," + totalMulti + "</SCORE>\r\n";
			output += "<TOTALSCORE>" + totalPts * totalMulti + "</TOTALSCORE>\r\n";
			return output;
		}

		private string DivFreqNum(string freq) {
			var m = Regex.Match(freq, @"([\d\w\.]*)MHz");
			return m.Groups[1].Value;
		}
	}
}
