using DevExpress.Utils.DPI;
using DevExpress.Xpo.DB.Helpers;
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
        public string hanhKiem1;
        public string hanhKiem2;
        public string hanhKiemCN;
        public string maHKiem;

        public List<Diemtrb> listdiemTrb1 = new List<Diemtrb>();
        public List<Diemtrb> listdiemTrb2 = new List<Diemtrb>();
        public DiemTongKet DiemTongKetHK1 = new DiemTongKet();
        public DiemTongKet DiemTongKetHK2 = new DiemTongKet();
        public DiemTongKet DiemTongKetCN = new DiemTongKet();
        public DiemtrbHS(string maHs = "", string tenHs = "")
        {
            
            student = new Student(maHs);
            student.HoTen = tenHs;
            hanhKiem1 = "";
            hanhKiem2 = "";
            hanhKiemCN = "";
            maHKiem = "";
        }
        public DiemtrbHS(Student std)
        {
            student = std;
            hanhKiem1 = "";
            hanhKiem2 = "";
            hanhKiemCN = "";
            maHKiem = "";
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
            int stt = 0;
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

        public DiemTongKet TinhDiemTongKetHocKy(List<double> diem, int _hanhKiem)
        {
            Dictionary<string, int> hocLuc = new Dictionary<string, int>();
            hocLuc["Giỏi"] = 0;
            hocLuc["Khá"] = 1;
            hocLuc["Trung Bình"] = 2;
            hocLuc["Yếu"] = 3;

            Dictionary<string, int> hanhKiem = new Dictionary<string, int>();
            hanhKiem["Tốt"] = 0;
            hanhKiem["Khá"] = 1;
            hanhKiem["Trung bình"] = 2;
            hanhKiem["yếu"] = 3;
            DiemTongKet hs = new DiemTongKet();

            double toan = diem[0];
            double van = diem[1];
            double anhVan = diem[8];
            double theDuc = diem[11];

            diem.RemoveAt(11);

            //code here;
            int soMonHoc = diem.Count();
            double sum = 0;
            for (int i = 0; i < soMonHoc; i++)
            {
                sum += diem[i];
            }
            hs.diemTrungBinh = sum / soMonHoc;

            if (hs.diemTrungBinh >= 8)
            {
                if (diem[0] >= 8 || diem[1] >= 8 || diem[8] >= 8)
                {
                    for (int i = 0; i < soMonHoc; i++)
                    {
                        if (diem[i] >= 6.5)
                        {
                            hs.xepLoai = "Giỏi";
                        }
                        else if (diem[i] < 6.5)
                        {
                            hs.xepLoai = "Khá";
                            break;
                        }
                    }
                }
                else
                {
                    hs.xepLoai = "Khá";
                }
            }
            else if (hs.diemTrungBinh >= 6.5)
            {
                if (diem[0] >= 6.5 || diem[1] >= 6.5 || diem[8] >= 6.5)
                {
                    for (int i = 0; i < soMonHoc; i++)
                    {
                        if (diem[i] >= 5)
                        {
                            hs.xepLoai = "Khá";
                        }
                        else if (diem[i] < 5)
                        {
                            hs.xepLoai = "Trung bình";
                            break;
                        }
                    }
                }
                else
                {
                    hs.xepLoai = "Khá";
                }
            }
            else if (hs.diemTrungBinh >= 5)
            {
                if (diem[0] >= 5 || diem[1] >= 5 || diem[8] >= 5)
                {
                    for (int i = 0; i < soMonHoc; i++)
                    {
                        if (diem[i] >= 3.5)
                        {
                            hs.xepLoai = "Trung bình";
                        }
                        else if (diem[i] < 3.5)
                        {
                            hs.xepLoai = "Yếu";
                            break;
                        }
                    }
                }
                else
                {
                    hs.xepLoai = "Trung bình";
                }
            }
            else
            {
                hs.xepLoai = "Yếu";
            }

            if (hs.xepLoai == "Giỏi")
            {
                if (theDuc < 5)
                {
                    hs.xepLoai = "Khá";
                }
            }
            else if (hs.xepLoai == "Khá")
            {
                if (theDuc < 5)
                {
                    hs.xepLoai = "Trung bình";
                }
            }
            else if (hs.xepLoai == "Trung bình")
            {
                if (theDuc < 5)
                {
                    hs.xepLoai = "Yếu";
                }
            }
            else
            {
                hs.xepLoai = "Yếu";
            }

            for (int i = 0; i < soMonHoc; i++)
            {
                if (hs.diemTrungBinh >= 8)
                {
                    if (diem[i] >= 3.5 && diem[i] < 5)
                    {
                        hs.xepLoai = "Khá";
                    }
                    else if (diem[i] < 3.5)
                    {
                        hs.xepLoai = "Trung bình";
                    }
                }
                else if (hs.diemTrungBinh < 8 && hs.diemTrungBinh >= 6.5)
                {
                    if (diem[i] >= 3.5 && diem[i] < 5)
                    {
                        hs.xepLoai = "Trung Bình";
                    }
                    else if (diem[i] < 3.5)
                    {
                        hs.xepLoai = "Yếu";
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (hs.xepLoai == hocLuc.ElementAt(i).Key)
                {
                    int tmp = hocLuc.ElementAt(i).Value;

                    tmp += _hanhKiem;
                    if (tmp == 0)
                    {
                        hs.xepLoai = "Giỏi";
                    }
                    else if (tmp == 1)
                    {
                        hs.xepLoai = "Khá";
                    }
                    else if (tmp == 2)
                    {
                        hs.xepLoai = "Trung bình";
                    }
                    else
                    {
                        hs.xepLoai = "Yếu";
                    }
                    break;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (_hanhKiem == hanhKiem.ElementAt(i).Value)
                {
                    hs.hanhKiem = hanhKiem.ElementAt(i).Key;
                }
            }

            //hanh kiem: 0: kem, 1: trung binh, 2: kha, 3: tot;
            //Listdiem.count = 13
            //diem[11]: diem the duc
            //listTenMH = { "0-Toán học", "1-Ngữ văn", "Vật lí", "Hóa học", "Sinh học", "Tin học", "Lịch sử", "Địa lí", "8-Ngoại ngữ", "GDCD", "Công nghệ", "Thể dục", "GDQP" };
            hs.diemTrungBinh = Math.Round(hs.diemTrungBinh, 2);
            return hs;
        }

        public DiemTongKet TinhDiemTongKetCaNam(List<double> diem1, int _hanhKiem1, List<double> diem2, int _hanhKiem2)
        {
            Dictionary<string, int> hocLuc = new Dictionary<string, int>();
            hocLuc["Giỏi"] = 0;
            hocLuc["Khá"] = 1;
            hocLuc["Trung Bình"] = 2;
            hocLuc["Yếu"] = 3;

            Dictionary<string, int> hanhKiem = new Dictionary<string, int>();
            hanhKiem["Tốt"] = 0;
            hanhKiem["Khá"] = 1;
            hanhKiem["Trung bình"] = 2;
            hanhKiem["yếu"] = 3;
            DiemTongKet hs = new DiemTongKet();
            List<double> diem = new List<double>();

            int soMonHoc = diem1.Count();

            for (int i = 0; i < soMonHoc; i++)
            {
                diem.Add((diem1[i] + diem2[i] * 2) / 3);
            }

            double theDuc = diem[11];
            diem.RemoveAt(11);
            soMonHoc = diem.Count();

            DiemTongKet hk1 = TinhDiemTongKetHocKy(diem1, _hanhKiem1);
            DiemTongKet hk2 = TinhDiemTongKetHocKy(diem2, _hanhKiem2);

            hs.diemTrungBinh = (hk1.diemTrungBinh + hk2.diemTrungBinh * 2) / 3;

            int _hanhKiemTB = (_hanhKiem1 + _hanhKiem2 * 2) / 3;
            for (int i = 0; i < 4; i++)
            {
                if (_hanhKiemTB == hanhKiem.ElementAt(i).Value)
                {
                    hs.hanhKiem = hanhKiem.ElementAt(i).Key;
                }
            }

            if (hs.diemTrungBinh >= 8)
            {
                if (diem[0] >= 8 || diem[1] >= 8 || diem[8] >= 8)
                {
                    for (int i = 0; i < soMonHoc; i++)
                    {
                        if (diem[i] >= 6.5)
                        {
                            hs.xepLoai = "Giỏi";
                        }
                        else if (diem[i] < 6.5)
                        {
                            hs.xepLoai = "Khá";
                            break;
                        }
                    }
                }
                else
                {
                    hs.xepLoai = "Khá";
                }
            }
            else if (hs.diemTrungBinh >= 6.5)
            {
                if (diem[0] >= 6.5 || diem[1] >= 6.5 || diem[8] >= 6.5)
                {
                    for (int i = 0; i < soMonHoc; i++)
                    {
                        if (diem[i] >= 5)
                        {
                            hs.xepLoai = "Khá";
                        }
                        else if (diem[i] < 5)
                        {
                            hs.xepLoai = "Trung bình";
                            break;
                        }
                    }
                }
                else
                {
                    hs.xepLoai = "Khá";
                }
            }
            else if (hs.diemTrungBinh >= 5)
            {
                if (diem[0] >= 5 || diem[1] >= 5 || diem[8] >= 5)
                {
                    for (int i = 0; i < soMonHoc; i++)
                    {
                        if (diem[i] >= 3.5)
                        {
                            hs.xepLoai = "Trung bình";
                        }
                        else if (diem[i] < 3.5)
                        {
                            hs.xepLoai = "Yếu";
                            break;
                        }
                    }
                }
                else
                {
                    hs.xepLoai = "Trung bình";
                }
            }
            else
            {
                hs.xepLoai = "Yếu";
            }

            if (hs.xepLoai == "Giỏi")
            {
                if (theDuc < 5)
                {
                    hs.xepLoai = "Khá";
                }
            }
            else if (hs.xepLoai == "Khá")
            {
                if (theDuc < 5)
                {
                    hs.xepLoai = "Trung bình";
                }
            }
            else if (hs.xepLoai == "Trung bình")
            {
                if (theDuc < 5)
                {
                    hs.xepLoai = "Yếu";
                }
            }
            else
            {
                hs.xepLoai = "Yếu";
            }

            for (int i = 0; i < soMonHoc; i++)
            {
                if (hs.diemTrungBinh >= 8)
                {
                    if (diem[i] >= 3.5 && diem[i] < 5)
                    {
                        hs.xepLoai = "Khá";
                    }
                    else if (diem[i] < 3.5)
                    {
                        hs.xepLoai = "Trung bình";
                    }
                }
                else if (hs.diemTrungBinh < 8 && hs.diemTrungBinh >= 6.5)
                {
                    if (diem[i] >= 3.5 && diem[i] < 5)
                    {
                        hs.xepLoai = "Trung Bình";
                    }
                    else if (diem[i] < 3.5)
                    {
                        hs.xepLoai = "Yếu";
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (hs.xepLoai == hocLuc.ElementAt(i).Key)
                {
                    int tmp = hocLuc.ElementAt(i).Value;

                    tmp += _hanhKiemTB;
                    if (tmp == 0)
                    {
                        hs.xepLoai = "Giỏi";
                    }
                    else if (tmp == 1)
                    {
                        hs.xepLoai = "Khá";
                    }
                    else if (tmp == 2)
                    {
                        hs.xepLoai = "Trung bình";
                    }
                    else
                    {
                        hs.xepLoai = "Yếu";
                    }
                    break;
                }
            }

            //hanh kiem: 0: kem, 1: trung binh, 2: kha, 3: tot;
            //Listdiem.count = 13
            //diem[11]: diem the duc
            //listTenMH = { "Toán học", "Ngữ văn", "Vật lí", "Hóa học", "Sinh học", "Tin học", "Lịch sử", "Địa lí", "Ngoại ngữ", "GDCD", "Công nghệ", "Thể dục", "GDQP" };
            hs.diemTrungBinh = Math.Round(hs.diemTrungBinh, 2);
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

        public void GetDiemTRBListHocSinh()
        {
            if (listHS.Count <= 0)
            {
                return;
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

                query = $"SELECT XEPLOAIHKI, XEPLOAIHKII, XEPLOAICN, MaHK FROM HANHKIEM WHERE MAHS = '{_mahs}' AND NAMHOC = '{curNamHoc}'";
                cmd = new SqlCommand(query, GlobalProperties.conn);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            string hk1 = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                            string hk2 = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                            string hkcn = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetString(2).Trim();
                            string maHk = rdr.IsDBNull(3) ? GlobalProperties.NULLFIELD : rdr.GetString(3).Trim();
                            listHS[i].hanhKiem1 = hk1;
                            listHS[i].hanhKiem2 = hk2;
                            listHS[i].hanhKiemCN = hkcn;
                            listHS[i].maHKiem = maHk;
                        }
                    }
                }
            }
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
                if (!string.IsNullOrEmpty(listHS[i].maHKiem))
                {
                    //Đã có hạnh kiểm:
                    query = $"UPDATE HANHKIEM SET XEPLOAIHKI = N'{xlhk1}', XEPLOAIHKII = N'{xlhk2}', XEPLOAICN = N'{xlhkcn}' WHERE MAHS = '{_mahs}'";
                    try
                    {
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
                else
                {
                    //Chưa có bảng hạnh kiểm
                    query = "SELECT COUNT(*) FROM HANHKIEM WHERE MAHK = ";
                    string maHKiem = this.GetKeyTable(query);

                    try
                    {
                        // Câu lệnh Insert.
                        query = $"INSERT INTO HANHKIEM(MAHK, MAHS, XEPLOAIHKI, XEPLOAIHKII, XEPLOAICN, NAMHOC) VALUES('{maHKiem}', '{_mahs}', N'{xlhk1}', N'{xlhk2}', N'{xlhkcn}', '{curNamHoc}')";

                        cmd = new SqlCommand(query, GlobalProperties.conn);

                        int rowCount = cmd.ExecuteNonQuery();
                        if (rowCount > 0)
                        {
                            listHS[i].maHKiem = maHKiem;
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

                }
                listHS[i].hanhKiem1 = xlhk1;
                listHS[i].hanhKiem2 = xlhk2;
                listHS[i].hanhKiemCN = xlhkcn;
            }
            return true;
        }

        public void TinhDiemTongKet(List<(int hk1, int hk2)> listHK)
        {
            for (int i = 0; i < listHS.Count; i++)
            {
                bool tkhk1 = false;
                bool tkhk2 = false;
                bool tinh = true;
                List<double> diem1 = new List<double>();
                List<double> diem2 = new List<double>();
                for (int j = 0; j < 13; j++)
                {
                    //MessageBox.Show(listHS[i].listdiemTrb1[j].diem.ToString());
                    if (listHS[i].listdiemTrb1[j].diem == -1)
                    {
                        tinh = false;
                        break;
                    }
                    else
                    {
                        diem1.Add(listHS[i].listdiemTrb1[j].diem);
                    }
                }
                //MessageBox.Show(str);
                if (tinh && listHK[i].hk1 != -1)
                {
                    listHS[i].DiemTongKetHK1 = this.TinhDiemTongKetHocKy(diem1, listHK[i].hk1);
                    tkhk1 = true;
                }
                else
                {
                    listHS[i].DiemTongKetHK1 = new DiemTongKet();
                }

                //Học ki2
                tinh = true;
                for (int j = 0; j < 13; j++)
                {
                    if (listHS[i].listdiemTrb2[j].diem == -1)
                    {
                        tinh = false;
                        break;
                    }
                    else
                    {
                        diem2.Add(listHS[i].listdiemTrb2[j].diem);
                    }
                }
                //MessageBox.Show(str);
                if (tinh == true && listHK[i].hk2 != -1)
                {
                    listHS[i].DiemTongKetHK2 = this.TinhDiemTongKetHocKy(diem2, listHK[i].hk2);
                    tkhk2 = true;
                }
                else
                {
                    listHS[i].DiemTongKetHK2 = new DiemTongKet();
                }

                if (tkhk1 && tkhk2)
                {
                    listHS[i].DiemTongKetCN = this.TinhDiemTongKetCaNam(diem1, listHK[i].hk1, diem2, listHK[i].hk2);
                }
                else
                {
                    listHS[i].DiemTongKetCN = new DiemTongKet();
                }
            }
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
