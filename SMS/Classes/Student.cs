using DevExpress.Data.Utils;
using DevExpress.Utils.Design;
using DevExpress.Utils.DirectXPaint.Svg;
using DevExpress.Utils.Win.Hook;
using DevExpress.Xpo.DB.Helpers;
using StudentManagementSystem.Classes;
using StudentManagementSystem.DatabaseCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagementSystem
{
    public class Student
    {
        private string maHS = "";
        private string hoTen = "";
        private string ngaySinh = "";
        private string diaChi = "";
        private string gioiTinh = "";
        private string nienKhoa = "";
        private string danToc = "";
        private string tonGiao = "";
        private string tenCha = "";
        private string ngheNghiepCha = "";
        private string ngaySinhCha = "";
        private string tenMe = "";
        private string ngheNghiepMe = "";
        private string ngaySinhMe = "";
        private string lop = "";
        private string ghiChu = "";
        private string noiSinh = "";
        private Image anhHS = null;
        private List<(string HK, string namHoc)> ttBangDiem;
        private List<DiemThanhPhan> dsDiem;
        private string sdt = "";
        private string email = "";
        private string maTK = "";
        private List<Diemtrb> listDiemTrBHKI;
        private List<Diemtrb> listDiemTrBHKII;
        private List<Diemtrb> listDiemTrBCN;
        private DiemTongKet diemTKHKI;
        private DiemTongKet diemTKHKII;
        private DiemTongKet diemTKCN;
        private ViPham viPham;
        private Admin_Func_Page3 Func_Page3 = new Admin_Func_Page3();

        public ViPham BangViPham
        {
            get { return viPham; }
            set { viPham = value; }
        }



        public DiemTongKet DiemTKCN
        {
            get { return diemTKCN; }
            set { diemTKCN = value; }
        }

        public DiemTongKet DiemTKHKII
        {
            get { return diemTKHKII; }
            set { diemTKHKII = value; }
        }


        public DiemTongKet DiemTKHKI
        {
            get { return diemTKHKI; }
            set { diemTKHKI = value; }
        }


        public List<Diemtrb> ListDiemTrBCN
        {
            get { return listDiemTrBCN; }
            set { listDiemTrBCN = value; }
        }

        public List<Diemtrb> ListDiemTrBHKII
        {
            get { return listDiemTrBHKII; }
            set { listDiemTrBHKII = value; }
        }

        public List<Diemtrb> ListDiemTrBHKI
        {
            get { return listDiemTrBHKI; }
            set { listDiemTrBHKI = value; }
        }


        public string MaTK
        {
            get { return maTK; }
            set { maTK = value; }
        }


        public string Email
        {
            get { return email; }
            set { email = value; }
        }


        public string SDT
        {
            get { return sdt; }
            set { sdt = value; }
        }

        public List<DiemThanhPhan> DSDiem
        {
            get { return dsDiem; }
            set { dsDiem = value; }
        }
        public List<(string HK, string namHoc)> TTBangDiem
        {
            get { return ttBangDiem; }
            set { TTBangDiem = value; }
        }
        public Image AnhHS
        {
            get { return anhHS; }
            set { anhHS = value; }
        }
        public string NoiSinh
        {
            get { return noiSinh; }
            set { noiSinh = value; }
        }
        public string GhiChu
        {
            get { return ghiChu; }
            set { ghiChu = value; }
        }
        public string Lop
        {
            get { return lop; }
            set { lop = value; }
        }
        public string NgaySinhMe
        {
            get { return ngaySinhMe; }
            set { ngaySinhMe = value; }
        }
        public string NgheNghiepMe
        {
            get { return ngheNghiepMe; }
            set { ngheNghiepMe = value; }
        }
        public string TenMe
        {
            get { return tenMe; }
            set { tenMe = value; }
        }
        public string NgaySinhCha
        {
            get { return ngaySinhCha; }
            set { ngaySinhCha = value; }
        }
        public string NgheNghiepCha
        {
            get { return ngheNghiepCha; }
            set { ngheNghiepCha = value; }
        }
        public string TenCha
        {
            get { return tenCha; }
            set { tenCha = value; }
        }
        public string TonGiao
        {
            get { return tonGiao; }
            set { tonGiao = value; }
        }
        public string DanToc
        {
            get { return danToc; }
            set { danToc = value; }
        }
        public string NienKhoa
        {
            get { return nienKhoa; }
            set { nienKhoa = value; }
        }
        public string GioiTinh
        {
            get { return gioiTinh; }
            set { gioiTinh = value; }
        }
        public string DiaChi
        {
            get { return diaChi; }
            set { diaChi = value; }
        }
        public string NgaySinh
        {
            get { return ngaySinh; }
            set { ngaySinh = value; }
        }
        public string HoTen
        {
            get { return hoTen; }
            set { hoTen = value; }
        }
        public string MaHS
        {
            get { return maHS; }
            set { maHS = value; }
        }

        public Student(string maHS = "", bool autoGet = true)
        {
            MaHS = maHS;
            ttBangDiem = new List<(string HK, string namHoc)>();
            dsDiem = new List<DiemThanhPhan>();
            for (int i = 0; i < 13; i++)
            {
                dsDiem.Add(new DiemThanhPhan(GlobalProperties.listMaMH[i], GlobalProperties.listTenMH[i]));
            }
            if (!string.IsNullOrEmpty(maHS) && autoGet)
            {
                GetDataStudent();
            }
        }

        public void SetDataStudent(string _maHS, string _hoTen, string _ngaySinh, string _diaChi, string _gioiTinh, string _nienKhoa, string _DanToc, string _tonGiao, string _tenCha, string _ngheCha, string _ngaySinhCha, string _tenMe, string _ngheMe, string _ngaySinhMe, string _lop, string _ghiChu, string _noiSinh, string _sdt, string _email, Image _anhHS)
        {
            maHS = _maHS;
            hoTen = _hoTen;
            ngaySinh = _ngaySinh;
            diaChi = _diaChi;
            gioiTinh = _gioiTinh;
            nienKhoa = _nienKhoa;
            danToc = _DanToc;
            tonGiao = _tonGiao;
            tenCha = _tenCha;
            ngheNghiepCha = _ngheCha;
            ngaySinhCha = _ngaySinhCha;
            tenMe = _tenMe;
            ngheNghiepMe = _ngheMe;
            ngaySinhMe = _ngaySinhMe;
            lop = _lop;
            ghiChu = _ghiChu;
            noiSinh = _noiSinh;
            sdt = _sdt;
            email = _email;
            anhHS = _anhHS;
        }

        public bool GetTongKetNamHoc(string namHoc)
        {
            var Func_Page3 = new Admin_Func_Page3();
            Func_Page3.ListHocSinh = new List<DiemtrbHS> { new DiemtrbHS(this) };
            Func_Page3.CurrentNamHoc = namHoc;

            if (!Func_Page3.GetDiemTRBListHocSinh())
            {
                return false;
            }
            Func_Page3.TinhDiemTongKet(useOtherHK: false);
            listDiemTrBHKI = Func_Page3.ListHocSinh[0].listdiemTrb1;
            listDiemTrBHKII = Func_Page3.ListHocSinh[0].listdiemTrb2;
            listDiemTrBCN = Func_Page3.ListHocSinh[0].listdiemTrbCN;
            diemTKHKI = Func_Page3.ListHocSinh[0].DiemTongKetHK1;
            diemTKHKII = Func_Page3.ListHocSinh[0].DiemTongKetHK2;
            diemTKCN = Func_Page3.ListHocSinh[0].DiemTongKetCN;
            return true;
        }

        public bool GetTongKetHocKi(string hocKi, string namHoc)
        {
            
            Func_Page3.ListHocSinh = new List<DiemtrbHS> { new DiemtrbHS(this) };
            Func_Page3.CurrentNamHoc = namHoc;

            if (!Func_Page3.GetDiemTRBListHocSinh())
            {
                return false;
            }
            Func_Page3.TinhDiemTongKet(useOtherHK: false);
            if (hocKi == "HK1")
            {
                diemTKHKI = Func_Page3.ListHocSinh[0].DiemTongKetHK1;
            }
            else
            {
                diemTKHKII = Func_Page3.ListHocSinh[0].DiemTongKetHK2;
            }

            GetViPhamNamHoc(namHoc);
            return true;
        }

        public void GetViPhamNamHoc(string _namHoc)
        {
            viPham = new ViPham(this.maHS, _namHoc);
        }

        public bool SaveViPham(string _HK, int cp, int kp, int vp)
        {
            return viPham.Save(_HK, cp, kp, vp);
        }

        public bool SaveHanhKiem(string _hk, string xl)
        {
            string[,] table = new string[1, 3];
            table[0, 0] = Func_Page3.ListHocSinh[0].hanhKiem.XepLoaiHK1;
            table[0, 1] = Func_Page3.ListHocSinh[0].hanhKiem.XepLoaiHK2;
            table[0, 2] = Func_Page3.ListHocSinh[0].hanhKiem.XepLoaiCN;
            if (_hk == "HK1")
            {
                table[0, 0] = xl;
            }
            else
            {
                table[0, 1] = xl;
            }
            return Func_Page3.SaveHanhKiemListHocSinh(table);
        }


        public bool GetDataStudent()
        {
            //Thông tin học sinh
            string query = @"SELECT HS.HotenHS, HS.NgaySinh, HS.diachi, HS.gioitinh, HS.nienkhoa, HS.dantoc,
                            HS.tongiao, HS.tencha, HS.nghenghiepcha, HS.ngaysinhcha, HS.tenme, HS.nghenghiepme, 
                            HS.ngaysinhme, HS.ghichu, L.TENLOP, HS.noisinh, HS.email, HS.sodt
                            FROM HOCSINH AS HS
                            LEFT JOIN LOP AS L ON L.MALOP = HS.MALOP
                            WHERE HS.MAHS = " + $"'{maHS}'";

            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        hoTen = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                        ngaySinh = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetDateTime(1).ToString("dd/MM/yyyy");
                        diaChi = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetString(2);
                        gioiTinh = rdr.IsDBNull(3) ? GlobalProperties.NULLFIELD : rdr.GetString(3);
                        nienKhoa = rdr.IsDBNull(4) ? GlobalProperties.NULLFIELD : rdr.GetString(4);
                        danToc = rdr.IsDBNull(5) ? GlobalProperties.NULLFIELD : rdr.GetString(5);
                        tonGiao = rdr.IsDBNull(6) ? GlobalProperties.NULLFIELD : rdr.GetString(6);
                        tenCha = rdr.IsDBNull(7) ? GlobalProperties.NULLFIELD : rdr.GetString(7);
                        ngheNghiepCha = rdr.IsDBNull(8) ? GlobalProperties.NULLFIELD : rdr.GetString(8);
                        ngaySinhCha = rdr.IsDBNull(9) ? GlobalProperties.NULLFIELD : rdr.GetDateTime(9).ToString("yyyy");
                        tenMe = rdr.IsDBNull(10) ? GlobalProperties.NULLFIELD : rdr.GetString(10);
                        ngheNghiepMe = rdr.IsDBNull(11) ? GlobalProperties.NULLFIELD : rdr.GetString(11);
                        ngaySinhMe = rdr.IsDBNull(12) ? GlobalProperties.NULLFIELD : rdr.GetDateTime(12).ToString("yyyy");
                        ghiChu = rdr.IsDBNull(13) ? GlobalProperties.NULLFIELD : rdr.GetString(13);
                        lop = rdr.IsDBNull(14) ? GlobalProperties.NULLFIELD : rdr.GetString(14).Trim();
                        noiSinh = rdr.IsDBNull(15) ? GlobalProperties.NULLFIELD : rdr.GetString(15);
                        email = rdr.IsDBNull(16) ? GlobalProperties.NULLFIELD : rdr.GetString(16);
                        sdt = rdr.IsDBNull(17) ? GlobalProperties.NULLFIELD : rdr.GetString(17);
                    }

                }
                else
                {
                    return false;
                }
            }

            string sql = $"SELECT ANHHS FROM HOCSINH WHERE MAHS = '{maHS}'";
            cmd = new SqlCommand(sql, GlobalProperties.conn);

            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["ANHHS"] != null)
                        {
                            byte[] byteBLOBData = (byte[])reader["ANHHS"];
                            anhHS = GlobalFunction.ToImage(byteBLOBData);
                        }


                    }
                }
            }
            catch { }


            ////Thông tin điểm tuyển sinh
            //query = @"SELECT D.SBD, HS.HotenHS, HS.NgaySinh, D.NAMTHI, D.TOAN, D.VAN, D.ANH, D.MONCHUYEN
            //        FROM DIEMDAUVAO AS D
            //        RIGHT JOIN Student AS HS ON HS.MAHS = D.MAHS
            //        WHERE HS.MAHS = " + $"'{MaHS}'";
            //cmd = new SqlCommand(query, GlobalProperties.conn);

            //using (SqlDataReader rdr = cmd.ExecuteReader())
            //{
            //    if (rdr.HasRows)
            //    {
            //        while (rdr.Read())
            //        {

            //            LB_SBD_TS.Text = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
            //            LB_HoTen_TS.Text = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
            //            LB_NS_TS.Text = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetDateTime(2).ToString("dd/MM/yyyy"); //DateTime.ParseExact(rdr.GetString(2), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString();
            //            LB_NamThi_TS.Text = rdr.IsDBNull(3) ? GlobalProperties.NULLFIELD : rdr.GetString(3);
            //            LB_dToan_TS.Text = rdr.IsDBNull(4) ? GlobalProperties.NULLFIELD : rdr.GetDouble(4).ToString();
            //            LB_dVan_TS.Text = rdr.IsDBNull(5) ? GlobalProperties.NULLFIELD : rdr.GetDouble(5).ToString();
            //            LB_dAnh_TS.Text = rdr.IsDBNull(6) ? GlobalProperties.NULLFIELD : rdr.GetDouble(6).ToString();
            //            LB_dChuyen_TS.Text = rdr.IsDBNull(7) ? GlobalProperties.NULLFIELD : rdr.GetDouble(7).ToString();
            //        }

            //    }
            //}

            ////Thông tin điểm tốt nghiệp:
            //query = @"SELECT HS.HotenHS, HS.NgaySinh, D.SBD, D.NAMTHI, D.TOAN, D.VAN, D.ANH, TN.VATLI, TN.HOAHOC, TN.SINHHOC, XH.LICHSU, XH.DIALI, XH.GDCD
            //        FROM DIEMTHITN AS D
            //        LEFT JOIN BANGDIEMTOHOPTUNHIEN AS TN ON TN.MABDTN = D.MABDTN
            //        LEFT JOIN BANGDIEMTOHOPXAHOI AS XH ON XH.MABDTN = D.MABDTN
            //        RIGHT JOIN Student AS HS ON HS.MAHS = D.MAHS
            //        WHERE HS.MAHS = " + $"'{MaHS}'";
            //cmd = new SqlCommand(query, GlobalProperties.conn);

            //using (SqlDataReader rdr = cmd.ExecuteReader())
            //{
            //    if (rdr.HasRows)
            //    {
            //        while (rdr.Read())
            //        {
            //            LB_HoTen_TN.Text = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
            //            Lb_NgaySinh_TN.Text = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetDateTime(1).ToString("dd/MM/yyyy");
            //            LB_SBD_TN.Text = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetString(2).Trim();
            //            Lb_NamThi_TN.Text = rdr.IsDBNull(3) ? GlobalProperties.NULLFIELD : rdr.GetString(3).Trim();
            //            LB_dToan_TN.Text = rdr.IsDBNull(4) ? GlobalProperties.NULLFIELD : rdr.GetDouble(4).ToString();
            //            LB_dVan_TN.Text = rdr.IsDBNull(5) ? GlobalProperties.NULLFIELD : rdr.GetDouble(5).ToString();
            //            LB_dAnh_TN.Text = rdr.IsDBNull(6) ? GlobalProperties.NULLFIELD : rdr.GetDouble(6).ToString();
            //            LB_dVatLi_TN.Text = rdr.IsDBNull(7) ? GlobalProperties.NULLFIELD : rdr.GetDouble(7).ToString();
            //            LB_dHoaHoc_TN.Text = rdr.IsDBNull(8) ? GlobalProperties.NULLFIELD : rdr.GetDouble(8).ToString();
            //            LB_dSinhHoc_TN.Text = rdr.IsDBNull(9) ? GlobalProperties.NULLFIELD : rdr.GetDouble(9).ToString();
            //            LB_dLichSu_TN.Text = rdr.IsDBNull(10) ? GlobalProperties.NULLFIELD : rdr.GetDouble(10).ToString();
            //            LB_dDiaLi_TN.Text = rdr.IsDBNull(11) ? GlobalProperties.NULLFIELD : rdr.GetDouble(11).ToString();
            //            LB_dGDCD_TN.Text = rdr.IsDBNull(12) ? GlobalProperties.NULLFIELD : rdr.GetDouble(12).ToString();
            //        }
            //    }
            //}

            ////Lấy thông tin về học kì và năm học:
            ttBangDiem.Clear();
            query = @"SELECT DISTINCT DM.MAHK, DM.NAMHOC
                    FROM DIEMMON AS DM
                    LEFT JOIN HOCSINH AS HS ON HS.MAHS = DM.MAHOCSINH
                    WHERE HS.MAHS = " + $"'{maHS}'" + @"ORDER BY DM.NAMHOC ASC, DM.MAHK ASC";
            cmd = new SqlCommand(query, GlobalProperties.conn);

            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string _hk = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                        string _namHoc = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                        ttBangDiem.Add(((string, string))(_hk, _namHoc));
                    }
                }
            }
            return true;
        }

        public bool GetThongTinBangDiem(string _HK, string _namHoc)
        {
            dsDiem = new List<DiemThanhPhan>();
            string query = @"SELECT MN.MAMH, MN.TENMH, CTD.MADIEMMON, CTD.DIEM, LKT.TENLOAIKT, DM.TRUNGBINH
                            FROM CHITIETDIEM AS CTD
                            INNER JOIN DIEMMON AS DM ON CTD.MADIEMMON = DM.MADIEMMON 
                            LEFT JOIN LOAIKIEMTRA AS LKT ON LKT.MALOAIKT = CTD.MALOAIKT
                            LEFT JOIN HOCSINH AS HS ON HS.MAHS = DM.MAHOCSINH
                            LEFT JOIN MONHOC AS MN ON MN.MAMH = DM.MAMONHOC " +
                            $"WHERE DM.MAHOCSINH = '{MaHS}' AND DM.MAHK = '{_HK}' AND DM.NAMHOC = '{_namHoc}'";

            for (int i = 0; i < 13; i++)
            {
                dsDiem.Add(new DiemThanhPhan(GlobalProperties.listMaMH[i], GlobalProperties.listTenMH[i]));
            }
            string maDiemMon;
            //13 columns
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            string maMH;
            string tenMH = "N/A";
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {

                        maMH = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                        tenMH = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1);
                        maDiemMon = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetString(2);
                        string loaiKT = rdr.IsDBNull(4) ? GlobalProperties.NULLFIELD : rdr.GetString(4);
                        double diemtp = rdr.IsDBNull(3) ? -1 : rdr.GetDouble(3);
                        double diemtrb = rdr.IsDBNull(5) ? -1 : rdr.GetDouble(5);
                        int f = -1;
                        if (diemtp != -1)
                        {
                            for (int i = 0; i < 13; i++)
                            {
                                if (maMH == GlobalProperties.listMaMH[i])
                                {
                                    f = i;
                                    break;
                                }
                            }
                            //MessageBox.Show(f.ToString());
                            if (f != -1)
                            {
                                switch (loaiKT)
                                {
                                    case "DDGTX1":
                                        dsDiem[f].DDGTX1 = new DTP(diemtp, maDiemMon);
                                        break;
                                    case "DDGTX2":
                                        dsDiem[f].DDGTX2 = new DTP(diemtp, maDiemMon);
                                        break;
                                    case "DDGTX3":
                                        dsDiem[f].DDGTX3 = new DTP(diemtp, maDiemMon);
                                        break;
                                    case "DDGTX4":
                                        dsDiem[f].DDGTX4 = new DTP(diemtp, maDiemMon);
                                        break;
                                    case "DDGGK":
                                        dsDiem[f].DDGGK = new DTP(diemtp, maDiemMon);
                                        break;
                                    case "DDGCK":
                                        dsDiem[f].DDGCK = new DTP(diemtp, maDiemMon);
                                        break;
                                }
                                //dsDiem[f].DDGTRB = new DTP(diemtrb, maDiemMon);

                            }
                        }
                    }
                }
                else
                {
                    //return false;
                }
            }
            query = $"SELECT DM.MADIEMMON, DM.MAMONHOC, DM.TRUNGBINH FROM DIEMMON AS DM WHERE DM.MAHOCSINH = '{MaHS}' AND DM.MAHK = '{_HK}' AND DM.NAMHOC = '{_namHoc}'";
            //MessageBox.Show(query);
            cmd = new SqlCommand(query, GlobalProperties.conn);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string maDM = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                        maMH = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                        double diemtp = rdr.IsDBNull(2) ? -1 : rdr.GetDouble(2);
                        //         MessageBox.Show(maMH);
                        for (int i = 0; i < 13; i++)
                        {
                            if (maMH == GlobalProperties.listMaMH[i])
                            {
                                dsDiem[i].MaDiemMon = maDM;
                                dsDiem[i].MaMH = maMH;
                                dsDiem[i].HaveTableDiemMon = true;
                                dsDiem[i].DDGTRB = new DTP(diemtp, maDM);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool SaveDataStudent(string _maHS, string _hoTen, string _ngaySinh, string _diaChi, string _gioiTinh, string _nienKhoa, string _danToc, string _tonGiao, string _tenCha, string _ngheCha, string _ngaySinhCha, string _tenMe, string _ngheMe, string _ngaySinhMe, string _lop, string _ghiChu, string _noiSinh, string _sdt, string _email, Image _anhHS)
        {

            //Khong thay doi maHS, lop, nienkhoa
            string sql = @"UPDATE HOCSINH
                            SET HotenHS = @hoten, diachi = @diachi, noisinh = @noisinh, sodt = @sodt,
                                                email = @email, gioitinh = @gioitinh, dantoc = @dantoc, tongiao = @tongiao, tencha = @tencha, tenme = @tenme,
                                                nghenghiepcha = @nghecha, nghenghiepme = @ngheme, Ghichu = @ghichu
                            WHERE MAHS = @MAHS";

            string sqlUpdateNgaySinh = @"UPDATE HOCSINH
                            SET ngaysinh = @ngaysinh
                            WHERE MAHS = @MAHS";
            string sqlUpdateNgaySinhCha = @"UPDATE HOCSINH
                            SET ngaysinhcha = @sinhcha
                            WHERE MAHS = @MAHS";
            string sqlUpdateNgaySinhMe = @"UPDATE HOCSINH
                            SET ngaysinhme = @sinhme
                            WHERE MAHS = @MAHS";

            string sqlUpadteANHHS = @"UPDATE HOCSINH
                            SET ANHHS = @anhhs
                            WHERE MAHS = @MAHS";

            try
            {

                SqlCommand cmd = new SqlCommand(sql, GlobalProperties.conn);

                cmd.Parameters.Add("@MAHS", SqlDbType.Char).Value = _maHS.ToString();
                cmd.Parameters.Add("@hoten", SqlDbType.NVarChar).Value = _hoTen;
                cmd.Parameters.Add("@diachi", SqlDbType.NVarChar).Value = _diaChi;
                cmd.Parameters.Add("@noisinh", SqlDbType.NVarChar).Value = _noiSinh;
                cmd.Parameters.Add("@sodt", SqlDbType.VarChar).Value = _sdt;
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = _email;
                cmd.Parameters.Add("@gioitinh", SqlDbType.NVarChar).Value = _gioiTinh;
                cmd.Parameters.Add("@dantoc", SqlDbType.NVarChar).Value = _danToc;
                cmd.Parameters.Add("@tongiao", SqlDbType.NVarChar).Value = _tonGiao;
                cmd.Parameters.Add("@tencha", SqlDbType.NVarChar).Value = _tenCha;
                cmd.Parameters.Add("@tenme", SqlDbType.NVarChar).Value = _tenMe;
                cmd.Parameters.Add("@nghecha", SqlDbType.NVarChar).Value = _ngheCha;
                cmd.Parameters.Add("@ngheme", SqlDbType.NVarChar).Value = _ngheMe;
                cmd.Parameters.Add("@ghichu", SqlDbType.NVarChar).Value = _ghiChu;

                int rowCount = cmd.ExecuteNonQuery();

            }
            catch (Exception w)
            {
                MessageBox.Show(w.ToString());
                return false;
            }
            //MessageBox.Show(Datepicker_cha.Text);
            //   //Update Ngày sinh
            if (!String.IsNullOrWhiteSpace(_ngaySinh.Trim()))
            {
                try
                {
                    //MessageBox.Show(datepicker_hs);
                    SqlCommand cmd = new SqlCommand(sqlUpdateNgaySinh, GlobalProperties.conn);

                    cmd.Parameters.Add("@ngaysinh", SqlDbType.SmallDateTime).Value = _ngaySinh.Trim();
                    cmd.Parameters.Add("@MAHS", SqlDbType.Char).Value = _maHS.ToString();

                    int rowCount = cmd.ExecuteNonQuery();
                }
                catch (Exception w)
                {
                    MessageBox.Show(w.ToString());
                    return false;
                }
            }

            //MessageBox.Show(Datepicker_cha.Text);
            //UpDATE NGÀY SINH MẸ
            if (!String.IsNullOrWhiteSpace(_ngaySinhMe.Trim()))
            {
                if (_ngaySinhMe.Trim().IndexOf("/") < 0)
                {
                    _ngaySinhMe = "01/01/" + _ngaySinhMe;
                }
                try
                {

                    SqlCommand cmd = new SqlCommand(sqlUpdateNgaySinhMe, GlobalProperties.conn);


                    cmd.Parameters.Add("@sinhme", SqlDbType.SmallDateTime).Value = _ngaySinhMe.Trim();
                    cmd.Parameters.Add("@MAHS", SqlDbType.Char).Value = _maHS.ToString();

                    int rowCount = cmd.ExecuteNonQuery();
                }
                catch (Exception w)
                {
                    MessageBox.Show(w.ToString());
                    return false;
                }
            }

            //update Ngày sinh cha
            if (!String.IsNullOrWhiteSpace(_ngaySinhCha.Trim()))
            {
                if (_ngaySinhCha.Trim().IndexOf("/") < 0)
                {
                    _ngaySinhCha = "01/01/" + _ngaySinhCha;
                }
                try
                {

                    SqlCommand cmd = new SqlCommand(sqlUpdateNgaySinhCha, GlobalProperties.conn);

                    cmd.Parameters.Add("@sinhcha", SqlDbType.SmallDateTime).Value = _ngaySinhCha.Trim();
                    cmd.Parameters.Add("@MAHS", SqlDbType.Char).Value = _maHS.ToString();

                    int rowCount = cmd.ExecuteNonQuery();
                }
                catch (Exception w)
                {
                    MessageBox.Show(w.ToString());
                    return false;
                }
            }


            //Upadte ảnh
            if (_anhHS != null)
            {
                try
                {

                    SqlCommand cmd = new SqlCommand(sqlUpadteANHHS, GlobalProperties.conn);

                    cmd.Parameters.Add("@anhhs", SqlDbType.VarBinary).Value = GlobalFunction.ImageToByteArray(_anhHS);
                    cmd.Parameters.Add("@MAHS", SqlDbType.Char).Value = _maHS.ToString();

                    int rowCount = cmd.ExecuteNonQuery();
                }
                catch (Exception w)
                {
                    //MessageBox.Show("Here------" + w.ToString());
                    return false;
                }
            }
            GetDataStudent();
            return true;
        }

        public bool SaveDiemStudent(double[,] BangDiem, string _hK, string _namHoc, int idxMon = -1)
        {
            //Cập nhật các cột điểm, chưa có điểm TRB
            for (int i = 0; i < GlobalProperties.soMonHoc; i++)
            {
                if (idxMon >= 0 && idxMon != i)
                {
                    continue;
                }
                for (int j = 0; j < 6; j++)
                {

                    //MessageBox.Show(diem);
                    double diemthuc = BangDiem[i, j];
                    string maDiem = GetMaDiem(i, j);
                    string maLoaiKT = GetMaLoaiKT(j);
                    if (diemthuc < 0)
                    {
                        if (checkDiemTonTai(i, j))
                        {
                            //Xóa khỏi db;
                            if (!DeleteChiTietDiem(maDiem, maLoaiKT))
                            {
                                return false;
                            }
                        }
                        //MessageBox.Show(diemthuc.ToString());
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(maDiem))
                        {
                            if (!UpdateDiem(maDiem, maLoaiKT, diemthuc))
                            {
                                //MessageBox.Show("here!");
                                return false;
                            }
                        }
                        else
                        {
                            //string keyMaMonHoc = GetMaMonHoc(i);

                            if (!InsertChiTietDiem(dsDiem[i].MaDiemMon, maLoaiKT, diemthuc))
                            {
                                //MessageBox.Show("Here insert!");
                                return false;
                            }
                        }
                    }

                }
            }

            //Cập nhật điểm trung bình

            for (int i = 0; i < GlobalProperties.soMonHoc; i++)
            {
                if (idxMon >= 0 && idxMon != i)
                {
                    continue;
                }
                double diemthuc = BangDiem[i, 6];
                string maDiem = GetMaDiem(i, 6);
                //MessageBox.Show(diemthuc.ToString() + " " + maDiem);
                if (!UpdateDiemTrB(maDiem, diemthuc))
                {
                    return false;
                }

            }
            GetThongTinBangDiem(_hK, _namHoc);
            return true;
        }
        private string GetMaDiem(int x, int y) //Lấy mã điểm tại môn thứ i và cột thứ j
        {
            DiemThanhPhan d = dsDiem[x];
            switch (y)
            {
                case 0:
                    return d.DDGTX1.maDiem;
                case 1:
                    return d.DDGTX2.maDiem;
                case 2:
                    return d.DDGTX3.maDiem;
                case 3:
                    return d.DDGTX4.maDiem;
                case 4:
                    return d.DDGGK.maDiem;
                case 5:
                    return d.DDGCK.maDiem;
                case 6:
                    return d.DDGTRB.maDiem;
                default:
                    return GlobalProperties.NULLFIELD;
            }
        }

        private string GetMaLoaiKT(int y)//Mã loại Kiểm tra của cột y = 0 - > 6
        {
            if (y >= 0 && y <= 6)
            {
                return GlobalProperties.listMaLoaiKT[y];
            }
            return GlobalProperties.NULLFIELD;
        }

        string GetMaMonHoc(int i)
        {
            if (i >= 0 && i <= 12)
            {
                return GlobalProperties.listMaMH[i];
            }
            return GlobalProperties.NULLFIELD;

        }

        bool checkDiemTonTai(int x, int y) //Check có tồn tại điểm môn x, cột y không?
        {
            DiemThanhPhan _diem = dsDiem[x];
            return (y == 0 && _diem.DDGTX1.diem != -1)
                || (y == 1 && _diem.DDGTX2.diem != -1)
                || (y == 2 && _diem.DDGTX3.diem != -1)
                || (y == 3 && _diem.DDGTX4.diem != -1)
                || (y == 4 && _diem.DDGGK.diem != -1)
                || (y == 5 && _diem.DDGCK.diem != -1)
                || (y == 6 && _diem.DDGTRB.diem != -1);
        }

        private bool DeleteChiTietDiem(string maDiem, string maLoaiKT)
        {
            string sqlUpdateDiem = @"DELETE FROM CHITIETDIEM
                                    WHERE MADIEMMON = @madiem AND MALOAIKT = @maloaikt";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlUpdateDiem, GlobalProperties.conn);

                cmd.Parameters.Add("@madiem", SqlDbType.Char).Value = maDiem.ToString();
                cmd.Parameters.Add("@maloaikt", SqlDbType.Char).Value = maLoaiKT.ToString();
                int rowCount = cmd.ExecuteNonQuery();
            }
            catch (Exception w)
            {
                DialogResult dialogResult = MessageBox.Show("Có lỗi trong quá trình lưu. Hiển thị lỗi?", "Lỗi", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.Yes)
                {
                    MessageBox.Show(w.ToString());
                }
                return false;
            }
            return true;
        }

        private bool UpdateDiem(string maDiem, string maLoaiKT, double diemThuc)
        {
            string sqlUpdateDiem = @"UPDATE CHITIETDIEM
                                    SET DIEM = @diem
                                    WHERE MADIEMMON = @madiem AND MALOAIKT = @maloaikt";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlUpdateDiem, GlobalProperties.conn);

                cmd.Parameters.Add("@madiem", SqlDbType.Char).Value = maDiem.ToString();
                cmd.Parameters.Add("@maloaikt", SqlDbType.Char).Value = maLoaiKT.ToString();
                cmd.Parameters.Add("@diem", SqlDbType.Float).Value = diemThuc;
                int rowCount = cmd.ExecuteNonQuery();
            }
            catch (Exception w)
            {
                DialogResult dialogResult = MessageBox.Show("Có lỗi trong quá trình lưu. Hiển thị lỗi?", "Lỗi", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.Yes)
                {
                    MessageBox.Show(w.ToString());
                }
                return false;
            }
            return true;
        }

        private bool InsertChiTietDiem(string maDiemMon, string maLoaiKT, double diemThuc)
        {
            string sqlTaoDiem = $"INSERT INTO CHITIETDIEM(MADIEMMON, MALOAIKT, DIEM) VALUES('{maDiemMon}', '{maLoaiKT}', {diemThuc})";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlTaoDiem, GlobalProperties.conn);
                int rowCount = cmd.ExecuteNonQuery();
            }
            catch (Exception w)
            {
                //MessageBox.Show(sqlTaoDiem);
                DialogResult dialogResult = MessageBox.Show("Có lỗi trong quá trình lưu. Hiển thị lỗi?", "Lỗi", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.Yes)
                {
                    MessageBox.Show(w.ToString());
                }

                return false;
            }
            return true;
        }

        private string GetMaDiemMonMoi()
        {
            string keyMaDiemMon = "";
            bool f = false;
            //Tạo mới.
            do
            {

                keyMaDiemMon = GlobalFunction.RandomString(10);
                //MessageBox.Show(keyMaDiemMon);
                string sql = $"SELECT COUNT(*) FROM CHITIETDIEM WHERE MADIEMMON = '{keyMaDiemMon}'";
                SqlCommand cmd = new SqlCommand(sql, GlobalProperties.conn);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        int count = rdr.GetInt32(0);
                        if (count > 0)
                            f = false;
                        else
                            f = true;

                    }
                }

            } while (!f);
            //MessageBox.Show(keyMaDiemMon);
            return keyMaDiemMon;
        }
        private bool UpdateDiemTrB(string maDiem, double diemThuc)
        {
            string tmp = diemThuc.ToString().Replace(',', '.');
            if (diemThuc == -1)
            {
                tmp = "null";
            }
            string query = $"UPDATE DIEMMON SET TRUNGBINH = {tmp} WHERE MADIEMMON = '{maDiem}'";
            //MessageBox.Show(query);
            try
            {
                SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
                int rowCount = cmd.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    MessageBox.Show($"Không thể thêm điểm trung bình ", "Thông báo");
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

        bool InsertTableDiemMon(string keyMaDiemMon, string keyMaMonHoc, string _hocKi, string _MaHS, string _maNamHoc)
        {
            string sqlTaoTableDiemMon = @"INSERT INTO DIEMMON(MADIEMMON, MAMONHOC, MAHK, NAMHOC, MAHOCSINH)
	                                    VALUES(@madiemmon, @mamonhoc, @mahk, @manamhoc, @mahs)";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlTaoTableDiemMon, GlobalProperties.conn);

                cmd.Parameters.Add("@madiemmon", SqlDbType.Char).Value = keyMaDiemMon.ToString();
                cmd.Parameters.Add("@mamonhoc", SqlDbType.Char).Value = keyMaMonHoc.ToString();
                cmd.Parameters.Add("@manamhoc", SqlDbType.Char).Value = _maNamHoc;
                cmd.Parameters.Add("@mahk", SqlDbType.Char).Value = _hocKi;
                cmd.Parameters.Add("@mahs", SqlDbType.Char).Value = _MaHS;

                int rowCount = cmd.ExecuteNonQuery();
            }
            catch (Exception w)
            {
                DialogResult dialogResult = MessageBox.Show("Có lỗi trong quá trình lưu. Hiển thị lỗi?", "Lỗi", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.Yes)
                {
                    MessageBox.Show(w.ToString());
                }
                return false;
            }
            return true;

        }

        public static void InsertToDB(List<Student> list)
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();

            try
            {
                string sql = "INSERT into HOCSINH VALUES (@maHS, @maLop, @maTK, @hoTen, @ngaySinh, @diaChi, @noiSinh, @sdt, @email, @gioiTinh" +
                    "@nienKhoa, @danToc, @tonGiao, @tenCha, @ngheNghiepCha, @ngaySinhCha, @tenMe, @ngheNghiepMe, @ngaySinhMe, @ghiChu)";

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                for (int i = 0; i < list.Count(); i++)
                {
                    cmd.Parameters.Add("@maHS", SqlDbType.Char).Value = list[i].maHS;
                    cmd.Parameters.Add("@maLop", SqlDbType.Char).Value = list[i].lop;
                    cmd.Parameters.Add("@maTK", SqlDbType.Char).Value = list[i].maTK;
                    cmd.Parameters.Add("@hoTen", SqlDbType.NVarChar).Value = list[i].hoTen;
                    cmd.Parameters.Add("@ngaySinh", SqlDbType.SmallDateTime).Value = list[i].ngaySinh;
                    cmd.Parameters.Add("@diaChi", SqlDbType.NVarChar).Value = list[i].diaChi;
                    cmd.Parameters.Add("@noiSinh", SqlDbType.NVarChar).Value = list[i].noiSinh;
                    cmd.Parameters.Add("@sdt", SqlDbType.VarChar).Value = list[i].sdt;
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = list[i].email;
                    cmd.Parameters.Add("@gioiTinh", SqlDbType.NVarChar).Value = list[i].gioiTinh;
                    cmd.Parameters.Add("@nienKhoa", SqlDbType.VarChar).Value = list[i].nienKhoa;
                    cmd.Parameters.Add("@danToc", SqlDbType.NVarChar).Value = list[i].danToc;
                    cmd.Parameters.Add("@tonGiao", SqlDbType.NVarChar).Value = list[i].tonGiao;
                    cmd.Parameters.Add("@tenCha", SqlDbType.NVarChar).Value = list[i].tenCha;
                    cmd.Parameters.Add("@ngheNghiepCha", SqlDbType.NVarChar).Value = list[i].ngheNghiepCha;
                    cmd.Parameters.Add("@ngaySinhCha", SqlDbType.SmallDateTime).Value = list[i].ngaySinhCha;
                    cmd.Parameters.Add("@tenMe", SqlDbType.NVarChar).Value = list[i].tenMe;
                    cmd.Parameters.Add("@ngheNghiepMe", SqlDbType.NVarChar).Value = list[i].ngheNghiepMe;
                    cmd.Parameters.Add("@ngaySinhMe", SqlDbType.SmallDateTime).Value = list[i].ngaySinhMe;
                    cmd.Parameters.Add("@ghiChu", SqlDbType.NVarChar).Value = list[i].ghiChu;
                }
                int rowCount = cmd.ExecuteNonQuery();
                Console.WriteLine("Row Count affected = " + rowCount);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
            Console.Read();
        }

    }


}
