using DevExpress.Utils.DPI;
using DevExpress.Xpo.DB.Helpers;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic.Logging;
using StudentManagementSystem.DatabaseCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagementSystem.Classes
{
    public class DiemTongKet
    {
        public string xepLoai;
        public double diemTrungBinh;
        public string hanhKiem;
        public DiemTongKet()
        {
            xepLoai = "";
            diemTrungBinh = -1;
            hanhKiem = "";
        }
    }
    public class Diemtrb
    {
        public double diem;
        public string maDiemMon;
        public Diemtrb(double _diem, string _maDiemMon)
        {
            diem = _diem;
            maDiemMon = _maDiemMon;
        }
    }
    public class DiemtrbHS
    {
        public Student student;
        public HanhKiem hanhKiem;
        public List<Diemtrb> listdiemTrb1 = new List<Diemtrb>();
        public List<Diemtrb> listdiemTrb2 = new List<Diemtrb>();
        public List<Diemtrb> listdiemTrbCN = new List<Diemtrb>();
        public DiemTongKet DiemTongKetHK1 = new DiemTongKet();
        public DiemTongKet DiemTongKetHK2 = new DiemTongKet();
        public DiemTongKet DiemTongKetCN = new DiemTongKet();
        public DiemtrbHS(string maHs = "", string tenHs = "")
        {
            
            student = new Student(maHs);
            student.HoTen = tenHs;
            hanhKiem = new HanhKiem();
        }
        public DiemtrbHS(Student std)
        {
            student = std;
            hanhKiem = new HanhKiem();
        }
    }
    public class Admin_Funcs
    {
        public Admin_Func_Page1 Func_Page1;
        public Admin_Func_Page2 Func_Page2;
        public Admin_Func_Page3 Func_Page3;
        public Admin_Func_page4 Func_Page4;
        public Admin_Func_Page5 Func_Page5;
        public Admin_Func_Page6 Func_Page6;

        public Admin_Funcs(bool init = false)
        {
            if (init)
            {
                Func_Page1 = new Admin_Func_Page1();
                Func_Page2 = new Admin_Func_Page2();
                Func_Page3 = new Admin_Func_Page3();
                Func_Page4 = new Admin_Func_page4();
                Func_Page5 = new Admin_Func_Page5();
                Func_Page6 = new Admin_Func_Page6();
            }
        }

        public List<string> GetNamHoc()
        {
            Dictionary<string, int> dicNH = new Dictionary<string, int>();
            //Get 3 năm học trong niên khóa
            string query = $"SELECT NAMBD, NAMKT FROM NIENKHOA";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);

            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    string bd = "", kt = "";
                    while (rdr.Read())
                    {
                        bd = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                        kt = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                        int namBD = 0, namKT = 0;
                        Int32.TryParse(bd, out namBD);
                        Int32.TryParse(kt, out namKT);
                        if (namBD == 0 || namKT == 0)
                        {
                            continue;
                        }
                        dicNH[namBD.ToString() + "-" + (namBD + 1).ToString()] = 1;
                        dicNH[(namBD + 1).ToString() + "-" + (namBD + 2).ToString()] = 1;
                        dicNH[(namBD + 2).ToString() + "-" + (namBD + 3).ToString()] = 1;
                    }

                }
            }
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, int> kvp in dicNH)
            {
                list.Add(kvp.Key);
            }
            return list;
        }

        public List<Lop> GetMaLop(string maKhoi, string maNamHoc)
        {
            List<Lop> listLop = new List<Lop>();
            //Get mã niên khóa:
            string query = $"SELECT MALOP, MAGVCN, TENLOP, SISO FROM LOP WHERE MAKHOI = '{maKhoi}' AND NAMHOC = '{maNamHoc}'";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);

            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string _maLop = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                        string _maGVCN = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                        string _tenLop = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetString(2).Trim();
                        string _siSo = rdr.IsDBNull(3) ? GlobalProperties.NULLFIELD : rdr.GetInt32(3).ToString();

                        listLop.Add(new Lop(_maLop, maKhoi, _maGVCN, _tenLop, _siSo));
                    }
                }
            }
            return listLop;
        }

        public List<Student> GetInfoHocSinh(string _maKhoi = "", string _namHoc = "", string _tenLop = "", string _maLop = "")
        {
            List<Student> listHS = new List<Student>();
            string addtoQuery = "";
            if (!string.IsNullOrEmpty(_maKhoi))
            {
                addtoQuery = addtoQuery + $" AND LOP.MAKHOI = '{_maKhoi}'";
            }
            if (!string.IsNullOrEmpty(_namHoc))
            {
                addtoQuery = addtoQuery + $" AND LOP.NAMHOC = '{_namHoc}'";
            }
            if (!string.IsNullOrEmpty(_tenLop))
            {
                addtoQuery = addtoQuery + $" AND LOP.TENLOP = '{_tenLop}'";
            }
            if (!string.IsNullOrEmpty(_maLop))
            {
                addtoQuery = addtoQuery + $" AND LOP.MALOP = '{_maLop}'";
            }
            string query = "SELECT MAHS, HotenHS, gioitinh, ngaysinh, LOP.TENLOP, noisinh, diachi, sodt, email, Ghichu FROM HOCSINH, LOP WHERE (LOP.MALOP = HOCSINH.MALOP" + addtoQuery + ") OR EXISTS(SELECT* FROM LOPDAHOC WHERE LOPDAHOC.MAHS = HOCSINH.MAHS AND LOPDAHOC.MALOP = LOP.MALOP " + addtoQuery + ")";
            //  MessageBox.Show(query);
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Student std = new Student(rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim(), false); //Mahs
                        std.HoTen = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                        std.GioiTinh = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetString(2).Trim();
                        std.NgaySinh = rdr.IsDBNull(3) ? GlobalProperties.NULLFIELD : rdr.GetDateTime(3).ToString();
                        std.Lop = rdr.IsDBNull(4) ? GlobalProperties.NULLFIELD : rdr.GetString(4).Trim();
                        std.NoiSinh = rdr.IsDBNull(5) ? GlobalProperties.NULLFIELD : rdr.GetString(5).Trim();
                        std.DiaChi = rdr.IsDBNull(6) ? GlobalProperties.NULLFIELD : rdr.GetString(6).Trim();
                        std.SDT = rdr.IsDBNull(7) ? GlobalProperties.NULLFIELD : rdr.GetString(7).Trim();
                        std.Email = rdr.IsDBNull(8) ? GlobalProperties.NULLFIELD : rdr.GetString(8).Trim();
                        std.GhiChu = rdr.IsDBNull(9) ? GlobalProperties.NULLFIELD : rdr.GetString(9).Trim();
                        listHS.Add(std);
                    }
                }
            }
            return listHS;
        }

        public (string tenLop, int siSo, string tenGV) GetInfoLop(string curMaLop)
        {
            string query = $"SELECT L.TENLOP, L.SISO, GV.TENGV FROM LOP AS L, GIAOVIEN AS GV WHERE L.MALOP = '{curMaLop}' AND L.MAGVCN = GV.MAGV";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    rdr.Read();
                    string tenLop = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                    int siSo = rdr.IsDBNull(1) ? 0 : rdr.GetInt32(1);
                    string tenGV = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetString(2).Trim();
                    return (tenLop, siSo, tenGV);
                }
            }
            return ("N/A", 0, "N/A");
        }

        public string GetKeyTable(string query)
        {
            string key = "";
            bool f = false;
            do
            {
                key = GlobalFunction.RandomString(10);
                string sql = query + $"'{key}'";
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
            return key;
        }

        public List<Diemtrb> TinhDiemTBMonCN(List<Diemtrb>  listdiemTrb1, List<Diemtrb> listdiemTrb2)
        {
            List<Diemtrb> listDiem = new List<Diemtrb>();
            for (int i = 0; i < GlobalProperties.soMonHoc; i++)
            {
                listDiem.Add(new Diemtrb(-1, ""));
                if (listdiemTrb1[i].diem != -1 && listdiemTrb2[i].diem != -1)
                {
                    listDiem[i].diem = Math.Round((listdiemTrb1[i].diem + listdiemTrb2[i].diem * 2) / 3, 1);
                }
            }
            return listDiem;

        }

        public DiemTongKet TinhDiemTongKetHocKy(List<double> diem, int _hanhKiem)
        {
            DiemTongKet hs = new DiemTongKet();

            int khongDuoi9 = 0, khongDuoi8 = 0, khongDuoi65 = 0, khongDuoi5 = 0,khongDuoi35 = 0;
            double tongDiem = 0;

            for (int i = 0; i < diem.Count; i++)
            {
                if (i != 11)
                {
                    khongDuoi9 += diem[i] >= 9 ? 1 : 0;
                    khongDuoi8 += diem[i] >= 8 ? 1 : 0;
                    khongDuoi65 += diem[i] >= 6.5 ? 1 : 0;
                    khongDuoi5 += diem[i] >= 5 ? 1 : 0;
                    khongDuoi35 += diem[i] >= 3.5 ? 1 : 0;
                    tongDiem += diem[i];
                }
            }
            
            if (_hanhKiem >= 0 && _hanhKiem <= 3)
            {
                hs.hanhKiem = GlobalFunction.GetTenHanhKiem(_hanhKiem);
            }
            hs.diemTrungBinh = Math.Round(tongDiem / 12, 1);
            hs.xepLoai = GlobalProperties.NULLFIELD;
            
            if (khongDuoi9 >= 6 && khongDuoi65 == 12 && diem[11] >= 5)
            {
                hs.xepLoai = "Xuất sắc";
                return hs;
            }    
            if (khongDuoi8 >= 6 && khongDuoi65 == 12)
            {
                if (diem[11] >= 5)
                {
                    hs.xepLoai = "Tốt";
                }
                else
                {
                    hs.xepLoai = "Khá"; //Trường hợp rớt 1 môn thể dục
                }
                return hs;
            }
            if (khongDuoi65 >= 6 && khongDuoi5 == 12)
            {
                if (diem[11] >= 5)
                {
                    hs.xepLoai = "Khá";
                }
                else
                {
                    hs.xepLoai = "Đạt"; // trường hợp rớt 1 môn thể dục
                }
                return hs;
            }
                
            if (khongDuoi5 >= 6 && khongDuoi35 == 12)
            {
                if (((khongDuoi8 >= 6 && khongDuoi65 >= 11) || (khongDuoi8 >= 5 && khongDuoi65 == 12)) && diem[11] >= 5) // rớt 1 môn dưới 8 hoặc 1 môn dưới 6,5 so với loại tốt
                {
                    hs.xepLoai = "Khá";
                }
                else
                {
                    hs.xepLoai = "Đạt";
                }    
                return hs;
            }

            //Trường hợp chưa đạt nâng lên đạt
            if (((khongDuoi65 >= 6 && khongDuoi5 >= 11) || (khongDuoi65 >= 5 && khongDuoi5 == 12)) && diem[11] >= 5) // rớt 1 môn dưới 6.5 hoặc 1 môn dưới 5 so với loại tốt
            {
                hs.xepLoai = "Đạt";
            }
            
            hs.xepLoai = "Chưa Đạt";
            return hs;
        }

        public string XetHanhKiemCaNam((int hk1, int hk2) hk)
        {
            string hanhKiem = "";
            if (hk.hk2 == 0 && hk.hk1 <= 1)
            {
                hanhKiem = GlobalFunction.GetTenHanhKiem(0);
            }
            else if ((hk.hk2 == 1 && hk.hk1 <= 2) || (hk.hk2 == 2 && hk.hk1 == 0) || (hk.hk2 == 0 && hk.hk1 >= 2))
            {
                hanhKiem = GlobalFunction.GetTenHanhKiem(1);
            }
            else if ((hk.hk2 == 2 && hk.hk1 >= 1) || (hk.hk2 == 1 && hk.hk1 == 3))
            {
                hanhKiem = GlobalFunction.GetTenHanhKiem(2);
            }
            else
            {
                hanhKiem = GlobalFunction.GetTenHanhKiem(3);
            }
            return hanhKiem;
        }


        public DiemTongKet TinhDiemTongKetCaNam(List<double> diem, (int hk1, int hk2) hk)
        {
            
            DiemTongKet hs = this.TinhDiemTongKetHocKy(diem, hk.hk1);
            hs.hanhKiem = XetHanhKiemCaNam(hk);
            return hs;
        }

        public bool TaoTableDiemMon2HK(string maNamHoc, string maHS)
        {
            string query;
            SqlCommand cmd;
            for (int k = 1; k <= 2; k++)
            {
                string maHK = "HK" + k.ToString();
                for (int j = 0; j < 13; j++)
                {
                    string maMonHoc = GlobalProperties.listMaMH[j];
                    bool coDiemMon = false;
                    query = $"SELECT COUNT(*) FROM DIEMMON WHERE MAMONHOC = '{maMonHoc}' AND NAMHOC = '{maNamHoc}' AND MAHK = '{maHK}' AND MAHOCSINH = '{maHS}'";
                    cmd = new SqlCommand(query, GlobalProperties.conn);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            int count = rdr.GetInt32(0);
                            if (count > 0)
                                coDiemMon = true;
                        }
                    }

                    if (!coDiemMon)
                    {
                        //Tao DIEMMON moi
                        string key = GetKeyTable("SELECT COUNT(*) FROM DIEMMON WHERE MADIEMMON = ");
                        try
                        {
                            // Câu lệnh Insert.
                            query = $"INSERT INTO DIEMMON(MADIEMMON, MAMONHOC, MAHK, NAMHOC, MAHOCSINH) VALUES('{key}', '{maMonHoc}', '{maHK}', '{maNamHoc}', '{maHS}')";

                            cmd = new SqlCommand(query, GlobalProperties.conn);

                            int rowCount = cmd.ExecuteNonQuery();
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
                    }
                }
            }
            return true;
        }
    }

    public class Admin_Func_Page1 : Admin_Funcs
    {
        private string curNamHoc;
        private string curMaLop;
        private string curHK;
        private List<string> listNH;
        private List<Lop> listLop;
        private string curKhoi;
        private List<Student> listHocSinh;
        private string curMon;

        public string CurrentMonHoc
        {
            get { return curMon; }
            set { curMon = value; }
        }
        public List<Student> ListHocSinh
        {
            get { return listHocSinh; }
            set { listHocSinh = value; }
        }
        public string CurrentKhoi
        {
            get { return curKhoi; }
            set { curKhoi = value; }
        }
        public List<Lop> ListLop
        {
            get { return listLop; }
            set { listLop = value; }
        }
        public List<string> ListNamHoc
        {
            get { return listNH; }
            set { listNH = value; }
        }
        public string CurrentHocKi
        {
            get { return curHK; }
            set { curHK = value; }
        }
        public string CurrentMaLop
        {
            get { return curMaLop; }
            set { curMaLop = value; }
        }
        public string CurrentNamHoc
        {
            get { return curNamHoc; }
            set { curNamHoc = value; }
        }

        public Admin_Func_Page1()
        {
            curMaLop = "";
            curNamHoc = "";
            curHK = "";
            listNH = new List<string>();
            listLop = new List<Lop>();
            listHocSinh = new List<Student>();
            curMon = "";
            curKhoi = "";
        }

        public int IndexOfCurrentNamHocInList
        {
            get { return listNH.IndexOf(curNamHoc); }
        }

        public int IndexOfCurrentKhoiInList
        {
            get { return (curKhoi == "K10" ? 0 : (curKhoi == "K11" ? 1 : 2)); }
        }

        public int IndexOfCurrentlopInList
        {
            get
            {
                for (int i = 0; i < listLop.Count; i++)
                {
                    if (listLop[i].MaLop == curMaLop)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }
        public void GetListNamHoc()
        {
            listNH.Clear();
            listNH = this.GetNamHoc();
        }

        public void GetListLop()
        {
            listLop.Clear();
            listLop = this.GetMaLop(curKhoi, curNamHoc);
        }

        public void GetListLop(string _khoi, string _namhoc)
        {
            this.curKhoi = _khoi;
            this.curNamHoc = _namhoc;
            listLop.Clear();
            listLop = this.GetMaLop(curKhoi, curNamHoc);
        }

        public void GetListHocSinh(string _maLop)
        {
            curMaLop = _maLop;

            GetListHocSinh();
        }

        public void GetListHocSinh()
        {
            listHocSinh.Clear();
            listHocSinh = this.GetInfoHocSinh(_maLop: curMaLop);

        }
        public (string tenLop, int siSo, string tenGV) GetThongTinLop()
        {
            return this.GetInfoLop(curMaLop);
        }

        public void GetDiemListHocSinh()
        {       
            int idxMon = Array.IndexOf(GlobalProperties.listTenMH, curMon);
            string _maMon = GlobalProperties.listMaMH[idxMon];
            string query;
            SqlCommand cmd;
            for (int i = 0; i < listHocSinh.Count; i++)
            {
                
                query = $"SELECT DM.MADIEMMON, DM.MAMONHOC, DM.TRUNGBINH FROM DIEMMON AS DM WHERE DM.MAHOCSINH = '{ListHocSinh[i].MaHS}' AND DM.MAHK = '{curHK}' AND DM.NAMHOC = '{curNamHoc}' AND DM.MAMONHOC = '{_maMon}'";
                cmd = new SqlCommand(query, GlobalProperties.conn);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            string maDM = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                            string maMH = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                            double diemtp = rdr.IsDBNull(2) ? -1 : rdr.GetDouble(2);
                            listHocSinh[i].DSDiem[idxMon].MaDiemMon = maDM;
                            listHocSinh[i].DSDiem[idxMon].MaMH = maMH;
                            listHocSinh[i].DSDiem[idxMon].DDGTRB = new DTP(diemtp, maDM);
                            break;
                        }
                    }
                }

                query = @"SELECT CTD.DIEM, CTD.MALOAIKT
                            FROM CHITIETDIEM AS CTD" +
                $" WHERE CTD.MADIEMMON = '{listHocSinh[i].DSDiem[idxMon].MaDiemMon}'";
                cmd = new SqlCommand(query, GlobalProperties.conn);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            string loaiKT = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1);
                            double diemtp = rdr.IsDBNull(0) ? -1 : rdr.GetDouble(0);
                            loaiKT = loaiKT.Trim();
                            if (diemtp != -1)
                            {
                                switch (loaiKT)
                                {
                                    case "DTX1":
                                        ListHocSinh[i].DSDiem[idxMon].DDGTX1 = new DTP(diemtp, listHocSinh[i].DSDiem[idxMon].MaDiemMon);
                                        break;
                                    case "DTX2":
                                        ListHocSinh[i].DSDiem[idxMon].DDGTX2 = new DTP(diemtp, listHocSinh[i].DSDiem[idxMon].MaDiemMon);
                                        break; ;
                                    case "DTX3":
                                        ListHocSinh[i].DSDiem[idxMon].DDGTX3 = new DTP(diemtp, listHocSinh[i].DSDiem[idxMon].MaDiemMon);
                                        break;
                                    case "DTX4":
                                        ListHocSinh[i].DSDiem[idxMon].DDGTX4 = new DTP(diemtp, listHocSinh[i].DSDiem[idxMon].MaDiemMon);
                                        break;
                                    case "DGK":
                                        ListHocSinh[i].DSDiem[idxMon].DDGGK = new DTP(diemtp, listHocSinh[i].DSDiem[idxMon].MaDiemMon);
                                        break;
                                    case "DCK":
                                        ListHocSinh[i].DSDiem[idxMon].DDGCK = new DTP(diemtp, listHocSinh[i].DSDiem[idxMon].MaDiemMon);
                                        break;
                                }
                            }
                        }
                    }
                }
            }

        }

        public string GetThongTinGiangDay()
        {
            string _maMon = GlobalProperties.listMaMH[Array.IndexOf(GlobalProperties.listTenMH, curMon)];
            string query = $"SELECT GV.TENGV FROM GIAOVIEN AS GV, GIANGDAY AS GD WHERE GD.MALOP = '{curMaLop}' AND GD.MAGV = GV.MAGV AND GV.MAMH = '{_maMon}'";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    rdr.Read();
                    string tenGVBM = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                    return tenGVBM;
                }
            }
            return "";
        }
    }

    public class Admin_Func_Page2 : Admin_Funcs
    {
        private List<string> listNamHoc;
        private List<Lop> listLop;
        private List<Student> listHS;
        private string curKhoi;
        private string curNamHoc;
        private string curLop;


        public string CurrentTenLop
        {
            get { return curLop; }
            set { curLop = value; }
        }


        public string CurrentNamHoc
        {
            get { return curNamHoc; }
            set { curNamHoc = value; }
        }


        public string CurrentKhoi
        {
            get { return curKhoi; }
            set { curKhoi = value; }
        }


        public List<Student> ListHS
        {
            get { return listHS; }
            set { listHS = value; }
        }


        public List<Lop> ListLop
        {
            get { return listLop; }
            set { listLop = value; }
        }


        public List<string> ListNamHoc
        {
            get { return listNamHoc; }
            set { listNamHoc = value; }
        }

        public Admin_Func_Page2()
        {
            listNamHoc = new List<string>();
            listLop = new List<Lop>();
            listHS = new List<Student>();
            curKhoi = "";
            curNamHoc = "";
            curLop = "";
        }

        public void GetListNamHoc()
        {
            listNamHoc.Clear();
            listNamHoc = this.GetNamHoc();
        }

        public void GetInfoListHS(string _maKhoi = "", string _namHoc = "", string _tenLop = "", string _maLop = "")
        {
            if (!string.IsNullOrEmpty(_maKhoi))
            {
                curKhoi = _maKhoi;
            }
            if (!string.IsNullOrEmpty(_namHoc))
            {
                curNamHoc = _namHoc;
            }
            if (!string.IsNullOrEmpty(_tenLop))
            {
                CurrentTenLop = _tenLop;
            }
            if (!string.IsNullOrEmpty(_maLop))
            {

            }
            listHS = this.GetInfoHocSinh(_maKhoi, _namHoc, _tenLop, _maLop);
        }

        public void GetListLop()
        {
            listLop.Clear();
            listLop = this.GetMaLop(curKhoi, curNamHoc);
        }

        public void GetListLop(string _khoi, string _namhoc)
        {
            this.curKhoi = _khoi;
            this.curNamHoc = _namhoc;
            listLop.Clear();
            listLop = this.GetMaLop(curKhoi, curNamHoc);
        }

    }

    public class Admin_Func_Page3 : Admin_Funcs
    {

        private List<DiemtrbHS> listHS;
        private string curNamHoc;
        private string curKhoi;
        private string curMaLop;
        private List<Lop> listLop;
        private List<string> listNH;

        public List<string> ListNamHoc
        {
            get { return listNH; }
            set { listNH = value; }
        }
        public List<Lop> ListLop
        {
            get { return listLop; }
            set { listLop = value; }
        }
        public string CurrentMaLop
        {
            get { return curMaLop; }
            set { curMaLop = value; }
        }
        public string CurrentKhoi
        {
            get { return curKhoi; }
            set { curKhoi = value; }
        }
        public string CurrentNamHoc
        {
            get { return curNamHoc; }
            set { curNamHoc = value; }
        }
        public List<DiemtrbHS> ListHocSinh
        {
            get { return listHS; }
            set { listHS = value; }
        }

        public Admin_Func_Page3()
        {
            listHS = new List<DiemtrbHS>();
            curNamHoc = "";
            curKhoi = "";
            curMaLop = "";
            listLop = new List<Lop>();
            listNH = new List<string>();
        }
        public int IndexOfCurrentNamHocInList
        {
            get { return listNH.IndexOf(curNamHoc); }
        }

        public int IndexOfCurrentKhoiInList
        {
            get { return (curKhoi == "K10" ? 0 : (curKhoi == "K11" ? 1 : 2)); }
        }

        public int IndexOfCurrentlopInList
        {
            get
            {
                for (int i = 0; i < listLop.Count; i++)
                {
                    if (listLop[i].MaLop == curMaLop)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }
        public void GetListNamHoc()
        {
            listNH.Clear();
            listNH = this.GetNamHoc();
        }
        public void GetListHocSinh(string _maLop)
        {
            curMaLop = _maLop;

            GetListHocSinh();
        }

        public void GetListHocSinh()
        {
            listHS.Clear();
            var list = this.GetInfoHocSinh(_maLop: curMaLop);
            foreach (var p in list)
            {
                listHS.Add(new DiemtrbHS(p));
            }
        }

        public bool GetDiemTRBListHocSinh()
        {
            if (listHS.Count <= 0)
            {
                return false;
            }
            string query;
            SqlCommand cmd;
            for (int i = 0; i < listHS.Count; i++)
            {
                string _mahs = listHS[i].student.MaHS;
                query = $"SELECT MAMONHOC, MADIEMMON, TRUNGBINH, MAHK FROM DIEMMON WHERE NAMHOC = '{curNamHoc}' AND MAHOCSINH = '{_mahs}'";
                for (int j = 0; j < 13; j++)
                {
                    listHS[i].listdiemTrb1.Add(new Diemtrb(-1, ""));
                    listHS[i].listdiemTrb2.Add(new Diemtrb(-1, ""));
                }
                cmd = new SqlCommand(query, GlobalProperties.conn);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            string maMh = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                            double trb = rdr.IsDBNull(2) ? -1 : rdr.GetDouble(2);
                            string maDm = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                            string mahk = rdr.IsDBNull(3) ? GlobalProperties.NULLFIELD : rdr.GetString(3).Trim();
                            //MessageBox.Show(maMh);
                            for (int j = 0; j < 13; j++)
                            {
                                if (maMh == GlobalProperties.listMaMH[j])
                                {
                                    if (mahk == "HK1")
                                    {
                                        //MessageBox.Show(_mahs + " ->" + trb.ToString() + "--" + maMh);
                                        listHS[i].listdiemTrb1[j] = new Diemtrb(trb, maDm);
                                        break;
                                    }
                                    else
                                    {
                                        listHS[i].listdiemTrb2[j] = new Diemtrb(trb, maDm);
                                        break;
                                    }

                                }
                            }

                        }
                    }
                }

                listHS[i].hanhKiem = new HanhKiem(_mahs, curNamHoc);
            }
            return true;

        }

        public (string tenLop, int siSo, string tenGV) GetThongTinLop()
        {
            return this.GetInfoLop(curMaLop);
        }
        public void GetListLop(string _khoi, string _namhoc)
        {
            this.curKhoi = _khoi;
            this.curNamHoc = _namhoc;
            listLop.Clear();
            listLop = this.GetMaLop(curKhoi, curNamHoc);
        }
        public void GetListLop()
        {
            listLop.Clear();
            listLop = this.GetMaLop(curKhoi, curNamHoc);
        }

        public bool SaveHanhKiemListHocSinh(string[, ] bangHK)
        {
            string xlhk1, xlhk2, xlhkcn, query;
            SqlCommand cmd;
            for (int i = 0; i < listHS.Count; i++)
            {
                string _mahs = listHS[i].student.MaHS;
                xlhk1 = bangHK[i, 0];
                xlhk2 = bangHK[i, 1];
                xlhkcn = bangHK[i, 2];
                if (!string.IsNullOrEmpty(listHS[i].hanhKiem.MaHK))
                {
                    listHS[i].hanhKiem.Save(xlhk1, xlhk2, xlhkcn);
                }
                else
                {
                    //Chưa có bảng hạnh kiểm
                    query = "SELECT COUNT(*) FROM HANHKIEM WHERE MAHK = ";
                    string maHKiem = this.GetKeyTable(query);

                    listHS[i].hanhKiem.Insert(maHKiem, _mahs, xlhk1, xlhk2, xlhkcn, curNamHoc);


                }
                listHS[i].hanhKiem = new HanhKiem(_mahs, curNamHoc);
            }
            return true;
        }

        public void TinhHanhKiem(List<(int hk1, int hk2)> listHK = null)
        {
            for (int i = 0; i < listHS.Count; i++)
            {
                listHS[i].DiemTongKetCN.hanhKiem = XetHanhKiemCaNam(listHK[i]);
            }
        }


        public bool TinhDiemTongKet(List<(int hk1, int hk2)> listHK = null, bool useOtherHK = true)
        {
            if (!useOtherHK)
            {
                listHK = new List<(int hk1, int hk2)>();
                for (int i = 0; i < listHS.Count; i++)
                {
                    listHK.Add((GlobalFunction.GetLoaiHanhKiem(listHS[i].hanhKiem.XepLoaiHK1), GlobalFunction.GetLoaiHanhKiem(listHS[i].hanhKiem.XepLoaiHK2)));
                }
            }
            for (int i = 0; i < listHS.Count; i++)
            {
                bool tkhk1 = false;
                bool tkhk2 = false;
                List<double> diem1 = new List<double>();
                List<double> diem2 = new List<double>();
                List<double> diemCN = new List<double>();
                listHS[i].DiemTongKetHK1 = new DiemTongKet();
                listHS[i].DiemTongKetHK2 = new DiemTongKet();
                listHS[i].DiemTongKetCN = new DiemTongKet();

                listHS[i].listdiemTrbCN = this.TinhDiemTBMonCN(listHS[i].listdiemTrb1, listHS[i].listdiemTrb2);
                for (int j = 0; j < 13; j++)
                {
                    diem1.Add(listHS[i].listdiemTrb1[j].diem);
                    diem2.Add(listHS[i].listdiemTrb2[j].diem);
                    diemCN.Add(listHS[i].listdiemTrbCN[j].diem);
                }

                

                if (diem1.IndexOf(-1) == -1 && listHK[i].hk1 != -1)
                {
                    listHS[i].DiemTongKetHK1 = this.TinhDiemTongKetHocKy(diem1, listHK[i].hk1);
                    //MessageBox.Show(listHS[0].DiemTongKetHK1.xepLoai);
                    tkhk1 = true;
                }

                //Học ki 2
                if (diem2.IndexOf(-1) == -1 && listHK[i].hk2 != -1)
                {
                    listHS[i].DiemTongKetHK2 = this.TinhDiemTongKetHocKy(diem2, listHK[i].hk2);
                    tkhk2 = true;
                }

                if (tkhk1 && tkhk2)
                {
                    listHS[i].DiemTongKetCN = this.TinhDiemTongKetCaNam(diemCN,listHK[i]);
                }
                else
                {
                    return false;
                }
            }
            
            return true;
        }



    }

    public class Admin_Func_page4 : Admin_Funcs
    {
        private List<Lop> listLopCu;
        private List<Lop> listLopMoi;
        private List<string> listNamHoc;
        private List<Student> listHS;

        public List<Student> ListHocSinh
        {
            get { return listHS; }
            set { listHS = value; }
        }


        public List<string> ListNamHoc
        {
            get { return listNamHoc; }
            set { listNamHoc = value; }
        }

        public List<Lop> ListLopMoi
        {
            get { return listLopMoi; }
            set { listLopMoi = value; }
        }

        public List<Lop> ListLopCu
        {
            get { return listLopCu; }
            set { listLopCu = value; }
        }

        public Admin_Func_page4()
        {
            ListLopCu = new List<Lop>();
            listLopMoi = new List<Lop>();
            listNamHoc = new List<string>();
            listHS = new List<Student>();
        }
        public void GetListNamHoc()
        {
            listNamHoc.Clear();
            listNamHoc = this.GetNamHoc();
        }

        public void GetListLopCu(string _khoi, string _namHoc)
        {
            listLopCu.Clear();
            listLopCu = this.GetMaLop(_khoi, _namHoc);
        }

        public void GetListLopMoi(string _khoi, string _namHoc)
        {
            listLopMoi.Clear();
            listLopMoi = this.GetMaLop(_khoi, _namHoc);
        }

        public void GetListHocSinhLopCu(string _maLop, string _namHoc, string _khoi)
        {
            listHS.Clear();
            string query = $"SELECT HS.MAHS, HS.HotenHS, HS.gioitinh, HS.Ghichu FROM HOCSINH AS HS, LOP AS L WHERE HS.MALOP = L.MALOP AND L.MALOP = '{_maLop}' AND L.NAMHOC = '{_namHoc}' AND L.MAKHOI = '{_khoi}'";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string maHs = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                        string hoTen = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                        string gioiTinh = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetString(2).Trim();
                        string ghiChu = rdr.IsDBNull(3) ? GlobalProperties.NULLFIELD : rdr.GetString(3).Trim();

                        Student std = new Student(maHs, false);
                        std.HoTen = hoTen;
                        std.GioiTinh = gioiTinh;
                        std.GhiChu = ghiChu;
                        listHS.Add(std);
                    }
                }
            }
        }

        public bool SaveChuyenLop(List<(string maHS, string tenHS)> listMaHS, string maLopMoi, string maLopCu, string maNamHoc, int loaiChuyen) //Loai chuyen: 0: chuyen lop, 1: Len lop
        {
            string query;
            SqlCommand cmd;
            for (int i = 0; i < listMaHS.Count; i++)
            {
                //Set lại mã lớp cho hs
                query = $"UPDATE HOCSINH SET MALOP = '{maLopMoi}' WHERE MAHS = '{listMaHS[i].maHS}'";
                try
                {
                    cmd = new SqlCommand(query, GlobalProperties.conn);
                    int rowCount = cmd.ExecuteNonQuery();
                    if (rowCount == 0)
                    {
                        MessageBox.Show($"Không thể thêm học sinh {listMaHS[i].tenHS}({listMaHS[i].maHS}) ", "Thông báo");
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
                if (loaiChuyen == 0)
                {
                    //Tạo thêm bảng DIEMMON cho HS nào còn thiếu
                }
                else
                {
                    //Lên lớp: Lưu tại LOPDAHOC, tạo 2 TABLE DIEMMON mới cho 2 học kì
                    //1.Check đã có tồn tại lớp này chưa, nếu có rồi thì ko thêm nữa
                    bool coLopCu = false;
                    query = $"SELECT COUNT(MALOPDAHOC) FROM LOPDAHOC WHERE MALOP = '{maLopCu}' AND MAHS = '{listMaHS[i].maHS}'";
                    cmd = new SqlCommand(query, GlobalProperties.conn);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            int count = rdr.GetInt32(0);
                            if (count > 0)
                                coLopCu = true;
                        }
                    }

                    if (!coLopCu)// thêm vào LOPDAHOC
                    {
                        string key = GetKeyTable("SELECT COUNT(*) FROM LOPDAHOC WHERE MALOPDAHOC = ");
                        try
                        {
                            // Câu lệnh Insert.
                            query = $"INSERT INTO LOPDAHOC(MALOPDAHOC, MALOP, MAHS) VALUES('{key}', '{maLopCu}', '{listMaHS[i].maHS}')";

                            cmd = new SqlCommand(query, GlobalProperties.conn);

                            int rowCount = cmd.ExecuteNonQuery();
                        }
                        catch (Exception ee)
                        {
                            DialogResult dialogResult = MessageBox.Show("Lỗi trong quá trình thêm. Hiển thị lỗi?", "Thông báo", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                MessageBox.Show("Error: " + ee);
                            }
                        }
                    }
                    //đã có rồi thì thôi, ko thêm nữa 

                    //2.tạo 2 bang diem cho 2 hk năm học mới:
                    //TaoTableDiemMon2HK(maNamHoc, _maHS);
                }
                if (!this.TaoTableDiemMon2HK(maNamHoc, listMaHS[i].maHS))
                {
                    MessageBox.Show($"Không thể thêm học sinh {listMaHS[i].tenHS}({listMaHS[i].maHS}) ", "Thông báo");
                }
            }
            return true;
        }

    }

    public class Admin_Func_Page5 : Admin_Funcs
    {
        private List<string> listNamHoc;
        private List<GiaoVien> listGV;
        private string curKhoi;
        private string curNamHoc;
        private List<NienKhoa> listNienKhoa;
        private List<Lop> listLop;

        public List<Lop> ListLop
        {
            get { return listLop; }
            set { listLop = value; }
        }
        public List<NienKhoa> ListNienKhoa
        {
            get { return listNienKhoa; }
            set { listNienKhoa = value; }
        }
        public string CurrentNamHoc
        {
            get { return curNamHoc; }
            set { curNamHoc = value; }
        }
        public string CurrentKhoi
        {
            get { return curKhoi; }
            set { curKhoi = value; }
        }
        public List<GiaoVien> ListGiaoVien
        {
            get { return listGV; }
            set { listGV = value; }
        }
        public List<string> ListNamHoc
        {
            get { return listNamHoc; }
            set { listNamHoc = value; }
        }

        public Admin_Func_Page5()
        {
            listNamHoc = new List<string>();
            listGV = new List<GiaoVien>();
            listLop = new List<Lop>();
            listNienKhoa = new List<NienKhoa>();
            curKhoi = "";
            curNamHoc = "";
        }

        public void GetListNamHoc()
        {
            listNamHoc.Clear();
            listNamHoc = this.GetNamHoc();
        }

        public void GetListNienKhoa()
        {
            listNienKhoa.Clear();
            string query = "SELECT MANK, NAMBD, NAMKT FROM NIENKHOA";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);

            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string mank = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                        string nbd = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                        string nkt = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetString(2).Trim();
                        ListNienKhoa.Add(new NienKhoa(mank, nbd, nkt));
                    }
                }
            }
        }

        public bool ThemNienKhoa(string _maNK, string _namBD, string _namKT)
        {
            string query = $"INSERT INTO NIENKHOA(MANK, NAMBD, NAMKT) VALUES('{_maNK}', '{_namBD}', '{_namKT}')";
            try
            {
                SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
                int rowCount = cmd.ExecuteNonQuery();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Không thể thêm niên khóa!", "Thông báo");
                return false;
            }
            return true;
        }

        public void GetListLop()
        {
            listLop.Clear();
            listLop = this.GetMaLop(curKhoi, curNamHoc);
        }

        public void GetListLop(string _khoi, string _namhoc)
        {
            this.curKhoi = _khoi;
            this.curNamHoc = _namhoc;
            listLop.Clear();
            listLop = this.GetMaLop(curKhoi, curNamHoc);
            string query;
            SqlCommand cmd;
            for (int i = 0; i < listLop.Count; i++)
            {
                query = $"SELECT GV.TENGV FROM GIAOVIEN AS GV WHERE GV.MAGV =  '{listLop[i].MaGVCN}'";
                cmd = new SqlCommand(query, GlobalProperties.conn);

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            listLop[i].TenGVCN = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                           
                        }
                    }
                }
            }
        }

        public void GetListGiaoVien()
        {
            listGV.Clear();
            string query = $"SELECT GV.TENGV, GV.MAGV, GV.MAMH FROM  GIAOVIEN AS GV;";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);

            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string ten = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                        string magv = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                        string mamh = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetString(2).Trim();
                        listGV.Add(new GiaoVien(magv, mamh, ten));
                    }
                }
            }
        }

        public bool ThemLopMoi(int idxGVinList, string _tenLop)
        {
            string key = GetKeyTable("SELECT COUNT(*) FROM LOP WHERE MALOP = ");
            try
            {
                string magv = listGV[idxGVinList].MaGV;
                // Câu lệnh Insert.
                //MessageBox.Show(curKhoi_p5 + " " + magv + " " + TB_TenLopTao.Text.ToString() + " " + curNamHoc_p5);
                string query = $"INSERT INTO LOP(MALOP, MAKHOI, MAGVCN, TENLOP,  NAMHOC) VALUES('{key}', '{curKhoi}', '{magv}', '{_tenLop}', '{curNamHoc}')";

                SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);

                int rowCount = cmd.ExecuteNonQuery();
                MessageBox.Show("Đã lưu", "Thông báo");
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

    public class Admin_Func_Page6 : Admin_Funcs
    {
        private List<Lop> listLop;
        private string curKhoi;
        private string curNamHoc;

        public string CurretnNamHoc
        {
            get { return curNamHoc; }
            set { curNamHoc = value; }
        }


        public string CurrentKhoi
        {
            get { return curKhoi; }
            set { curKhoi = value; }
        }


        public List<Lop> ListLop
        {
            get { return listLop; }
            set { listLop = value; }
        }

        public Admin_Func_Page6()
        {
            listLop = new List<Lop>();
        }

        public bool ThemHocSinh(string _username, string _pass, Student student, string _maNamHoc)
        {
            //Tạo tài khoản
            string _mataikhoan = GetKeyTable("SELECT COUNT(*) FROM TAIKHOAN WHERE MATK = ");
            string query = $"INSERT INTO TAIKHOAN(MATK, USERNAME, PASS) VALUES('{_mataikhoan}', '{_username}', '{_pass}')";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            int rowCount = cmd.ExecuteNonQuery();

            //Tạo học sinh:
            query = $"INSERT INTO HOCSINH(MAHS, MALOP, MATK, HotenHS, ngaysinh, diachi,	gioitinh, nienkhoa, sodt)	VALUES('{student.MaHS}', '{student.Lop}', '{_mataikhoan}', N'{student.HoTen}', '{student.NgaySinh}', N'{student.DiaChi}', N'{student.GioiTinh}', '{student.NienKhoa}', '{student.SDT}')";
            cmd = new SqlCommand(query, GlobalProperties.conn);
            rowCount = cmd.ExecuteNonQuery();
            if (rowCount > 0)
            {
                TaoTableDiemMon2HK(_maNamHoc, student.MaHS);
                return true;
            }
            return false; 
        }

        public void GetListLop()
        {
            listLop.Clear();
            listLop = this.GetMaLop(curKhoi, curNamHoc);
        }

        public void GetListLop(string _khoi, string _namhoc)
        {
            this.curKhoi = _khoi;
            this.curNamHoc = _namhoc;
            listLop.Clear();
            listLop = this.GetMaLop(curKhoi, curNamHoc);
            
        }
    }
}
