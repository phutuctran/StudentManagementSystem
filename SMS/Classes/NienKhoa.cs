using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Classes
{
    public class NienKhoa
    {
		private string maNK;
		private string namBD;
		private string namKT;

		public string NamKetThuc
		{
			get { return namKT; }
			set { namKT = value; }
		}


		public string NamBatDau
		{
			get { return namBD; }
			set { namBD = value; }
		}


		public string MaNienKhoa
		{
			get { return maNK; }
			set { maNK = value; }
		}

		public NienKhoa(string _maNK, string _bd, string _kt)
		{
			maNK = _maNK;
			namBD = _bd;
			namKT = _kt;
		}

	}
}
