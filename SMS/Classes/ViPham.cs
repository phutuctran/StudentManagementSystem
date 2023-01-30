using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagementSystem.Classes
{
    public class ViPham
    {
		private string maViPham;
		private string maHocSinh;
		private string namHoc;
		private int nghiCoPhepHKI;
        private int nghiKhongPhepHKI;
		private int viPhamHKI;
        private int nghiCoPhepHKII;
        private int nghiKhongPhepHKII;
        private int viPhamHKII;

        public int ViPhamHKII
        {
            get { return viPhamHKII; }
            set { viPhamHKII = value; }
        }


        public int NghiKhongPhepHKII
        {
            get { return nghiKhongPhepHKII; }
            set { nghiKhongPhepHKII = value; }
        }

        public int NghiCoPhepHKII
        {
            get { return nghiCoPhepHKII; }
            set { nghiCoPhepHKII = value; }
        }

        public int ViPhamHKI
		{
			get { return viPhamHKI; }
			set { viPhamHKI = value; }
		}


		public int NghiKhongPhepHKI
        {
            get { return nghiKhongPhepHKI; }
            set { nghiKhongPhepHKI = value; }
        }

        public int NghiCoPhepHKI
		{
			get { return nghiCoPhepHKI; }
			set { nghiCoPhepHKI = value; }
		}


		public string NamHoc
		{
			get { return namHoc; }
			set { namHoc = value; }
		}


		public string MaHocSinh
		{
			get { return maHocSinh; }
			set { maHocSinh = value; }
		}


		public string MaViPham
		{
			get { return maViPham; }
			set { maViPham = value; }
		}


		public ViPham(string mavp = "", string maHS = "", string namhoc = "")
		{
			maViPham = mavp;
			maHocSinh = maHS;
			namHoc = namhoc;
			nghiCoPhepHKI = 0;
			nghiKhongPhepHKI= 0;
			viPhamHKI = 0;
			nghiKhongPhepHKII = 0;
			NghiCoPhepHKII = 0;
			viPhamHKII = 0;

		}

		public ViPham(string _maHS, string _namHoc)
		{
            maHocSinh = _maHS;
            namHoc = _namHoc;
            string query = @"SELECT COPHEPHKI, KHONGPHEPHKI, VIPHAMHKI, COPHEPHKII, KHONGPHEPHKII, VIPHAMHKII, MAVIPHAM                                      FROM VIPHAM" +
                            $" WHERE VIPHAM.MAHS = '{_maHS}' AND NAMHOC = '{_namHoc}'";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        NghiCoPhepHKI = rdr.IsDBNull(0) ? 0 : rdr.GetInt16(0);
                        NghiKhongPhepHKI = rdr.IsDBNull(1) ? 0 : rdr.GetInt16(1);
                        ViPhamHKI = rdr.IsDBNull(2) ? 0 : rdr.GetInt16(2);
                        NghiCoPhepHKII = rdr.IsDBNull(3) ? 0 : rdr.GetInt16(3);
                        NghiKhongPhepHKII = rdr.IsDBNull(4) ? 0 : rdr.GetInt16(4);
                        ViPhamHKII = rdr.IsDBNull(5) ? 0 : rdr.GetInt16(5);
                        maViPham = rdr.IsDBNull(5) ? "" : rdr.GetString(6);
                    }
                }
                else
                {
                    NghiCoPhepHKI = 0;
                    NghiKhongPhepHKI = 0;
                    ViPhamHKI = 0;
                    NghiCoPhepHKII = 0;
                    NghiKhongPhepHKII = 0;
                    ViPhamHKII = 0;
                    maViPham = "";
                }
            }
        }
        public ViPham(string _maVP)
        {
            string query = @"SELECT COPHEPHKI, KHONGPHEPHKI, VIPHAMHKI, COPHEPHKII, KHONGPHEPHKII, VIPHAMHKII, MAHS, NAMHOC                                      FROM VIPHAM" +
                            $" WHERE VIPHAM.MAVIPHAM = '{_maVP}'";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        NghiCoPhepHKI = rdr.IsDBNull(0) ? 0 : rdr.GetInt16(0);
                        NghiKhongPhepHKI = rdr.IsDBNull(1) ? 0 : rdr.GetInt16(1);
                        ViPhamHKI = rdr.IsDBNull(2) ? 0 : rdr.GetInt16(2);
                        NghiCoPhepHKII = rdr.IsDBNull(3) ? 0 : rdr.GetInt16(3);
                        NghiKhongPhepHKII = rdr.IsDBNull(4) ? 0 : rdr.GetInt16(4);
                        ViPhamHKII = rdr.IsDBNull(5) ? 0 : rdr.GetInt16(5);
                        maHocSinh = rdr.IsDBNull(6) ? "" : rdr.GetString(6);
                        namHoc  = rdr.IsDBNull(7) ? "" : rdr.GetString(7);
                    }
                }
                else
                {
                    NghiCoPhepHKI = 0;
                    NghiKhongPhepHKI = 0;
                    ViPhamHKI = 0;
                    NghiCoPhepHKII = 0;
                    NghiKhongPhepHKII = 0;
                    ViPhamHKII = 0;
                    maHocSinh = "";
                    namHoc = "";
                }
            }
        }

        public bool Save(string _HK, int cp, int kp, int vp)
        {
            string query = "";
            if (_HK == "HK1")
            {
                query = @"UPDATE VIPHAM" +
                                    $" SET COPHEPHKI = '{cp}', KHONGPHEPHKI = '{kp}', VIPHAMHKI = '{vp}'" +
                                    $"WHERE MAVIPHAM = '{maViPham}'";
            }
            else
            {
                query = @"UPDATE VIPHAM" +
                                   $" SET COPHEPHKII = '{cp}', KHONGPHEPHKII = '{kp}', VIPHAMHKII = '{vp}'" +
                                   $"WHERE MAVIPHAM = '{maViPham}'";
            }
            try
            {
                SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
                int rowCount = cmd.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    return false;
                }
            }
            catch (Exception ee)
            {
                DialogResult dialogResult = MessageBox.Show("Lỗi trong quá trình thêm. Hiển thị lỗi?", "Thông báo", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    MessageBox.Show("Error: " + ee);
                }
                return false;
            }
            return true;
        }

    }
}
