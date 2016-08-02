using System;
using System.IO;
using System.Text;

namespace LogProc {
	public class BinaryStream {
		private Stream Str;
		private BinaryReader BinReader;
		private Encoding Encode;

		public BinaryStream() {
		}
		~BinaryStream() {
			if(BinReader != null) BinReader.Dispose();
			if(Str != null) Str.Dispose();
		}

		public bool ReadOpen(string filePath) {
			try {
				Str = File.OpenRead(filePath);
				BinReader = new BinaryReader(Str);
			} catch(FileNotFoundException) {
				Console.WriteLine("ファイルが存在しない");
				return false;
			} catch(Exception e) {
				Console.WriteLine("不明なエラー: " + e.Message);
				return false;
			}
			return true;
		}
		public bool ChangeEncoding(string changeFor) {
			try {
				Encode = Encoding.GetEncoding(changeFor);
			} catch {
				return false;
			}
			return true;
		}
		public string GETCHAR(int count = 1) {
			string tmp;
			try {
				tmp = Encode.GetString(BinReader.ReadBytes(count));
			} catch(Exception e) {
				throw e;
			}
			for(int i = 0;i < tmp.Length;i++) {
				if(tmp[i] == 0x00 || tmp[i] == 0xff) {
					tmp = tmp.Substring(0, i);
				}
			}

			if(tmp.Length > 1) {
				if(0x52 == tmp[0]) return tmp;
				if(0x20 <= tmp[0] && tmp[0] <= 0x7e) {
				} else if(0xa1 <= tmp[0] && tmp[0] <= 0xdf) {
				} else if(0x3040 <= tmp[0] && tmp[0] <= 0x309f) {
				} else if(0x30a0 <= tmp[0] && tmp[0] <= 0x30ff) {
				} else if(0x4e00 <= tmp[0] && tmp[0] <= 0x9fff) {
				} else return "";
			}
			return tmp;
		}
		public ushort GETUSHORT() {
			ushort tmp;
			try {
				tmp = BinReader.ReadUInt16();
			} catch(Exception e) {
				throw e;
			}
			return tmp;
		}
		public byte GETBYTE() {
			byte tmp;
			try {
				tmp = BinReader.ReadByte();
			} catch(Exception e) {
				throw e;
			}
			return tmp;
		}
		public uint GETUINT() {
			uint tmp;
			try {
				tmp = BinReader.ReadUInt32();
			} catch(Exception e) {
				throw e;
			}
			return tmp;
		}
		public Int64 GETLONG() {
			long tmp;
			try {
				tmp = BinReader.ReadInt64();
			} catch(Exception e) {
				throw e;
			}
			return tmp;
		}
		public void DISP<T>(T gotten) {
			Console.WriteLine(gotten);
		}
	}
}