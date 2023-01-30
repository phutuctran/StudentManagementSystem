using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using StudentManagementSystem.Classes;
using StudentManagementSystem.CustomControls;
using StudentManagementSystem.DatabaseCore;

namespace StudentManagementSystem
{
    public partial class StudentInfo : MaterialForm
    {
        Student student;
        string maHS;

        public bool haveBangDiem_p2 { get; private set; }
        public bool haveBangDiem_p3 { get; private set; }

        public StudentInfo(string _MaHS) //_MaHS phải luôn tồn tại
        {
            InitializeComponent();
            maHS = _MaHS;
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            student = new Student(maHS);
            GetDataStudent();
        }
        private void StudentInfo_Load(object sender, EventArgs e)
        {

            tabPage2.BackColor = Color.Honeydew;

        }


        void GetDataStudent()
        {
            if (!student.GetDataStudent())
            {
                MessageBox.Show("Không thể lấy thông tin học sinh!!!", "Thông báo!");
                this.Close();
                return;
            }
            //thong tin hs
            LB_MaHS.Text = @"Mã học sinh: " + student.MaHS;
            LB_HoTenHS.Text = student.HoTen;
            LB_NgaySinh.Text = student.NgaySinh;
            LB_DiaChi.Text = student.DiaChi;
            LB_GioiTinh.Text = student.GioiTinh;
            LB_NienKhoa.Text = student.NienKhoa;
            LB_DanToc.Text = student.DanToc;
            LB_TonGiao.Text = student.TonGiao;
            LB_TenCha.Text = student.TenCha;
            LB_NgheNghiepCha.Text = student.NgheNghiepCha;
            LB_NamSinhCha.Text = student.NgaySinhCha;
            LB_TenMe.Text = student.TenMe;
            LB_NgheNghiepMe.Text = student.NgheNghiepMe;
            LB_NamSinhMe.Text = student.NgaySinhMe;
            LB_GhiChu.Text = student.GhiChu;
            LB_Lop.Text = @"Lớp: " + student.Lop;
            LB_NoiSinh.Text = student.NoiSinh;

            //anh dai dien
            if (student.AnhHS != null)
            {
                Image img = PB_Avatar.Image;
                PB_Avatar.Image = student.AnhHS;
                if (img != null)
                {
                    img.Dispose();
                }
            }

            //thong tin bang diem
            if (student.TTBangDiem.Count > 0)
            {
                CB_ttHK_NH.Items.Clear();
                CB_ChonNamHoctab3.Items.Clear();
                foreach ((string hk, string namHoc) p in student.TTBangDiem)
                {
                    CB_ttHK_NH.Items.Add(p.hk + ", " + p.namHoc);
                    if (CB_ChonNamHoctab3.Items.IndexOf(p.namHoc) == -1)
                    {
                        CB_ChonNamHoctab3.Items.Add(p.namHoc);
                    }
                }
            }
        }

        private void PB_Avatar_Paint(object sender, PaintEventArgs e)
        {
            PB_Avatar.Location = new Point((panel2.Width - PB_Avatar.Width) / 2, 50);
        }

        private void LB_HoTenHS_Paint(object sender, PaintEventArgs e)
        {
            LB_HoTenHS.Location = new Point(PB_Avatar.Location.X + PB_Avatar.Width / 2 - LB_HoTenHS.Width / 2, 320);
        }

        private void LB_MaHS_Paint(object sender, PaintEventArgs e)
        {
            LB_MaHS.Text = LB_MaHS.Text.Trim();
            LB_MaHS.Location = new Point(PB_Avatar.Location.X + PB_Avatar.Width / 2 - LB_MaHS.Width / 2, 380);
        }

        private void LB_Lop_Paint(object sender, PaintEventArgs e)
        {
            LB_Lop.Location = new Point(PB_Avatar.Location.X + PB_Avatar.Width / 2 - LB_Lop.Width / 2, 440);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            this.PB_Avatar.Dispose();
            //this.pictureBox1.Dispose();
        }

        private void btn_xem_Click(object sender, EventArgs e)
        {
            int idx = CB_ttHK_NH.SelectedIndex;
            if (idx < 0)
            {
                MessageBox.Show("Hãy chọn thông tin cần xem", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                haveBangDiem_p2 = false;
                return;
            }
            if (!student.GetThongTinBangDiem(student.TTBangDiem[idx].HK, student.TTBangDiem[idx].namHoc))
            {
                MessageBox.Show("Không thể lấy thông tin bảng điểm hoặc chưa có điểm!!!", "Thông báo!");
                haveBangDiem_p2 = false;
                return;
            }

            
            LV_BangDiem.Items.Clear();
            int stt = 0;
            for (int i = 0; i < student.DSDiem.Count; i++)
            {
                string[] row = new string[9];
                row[0] = (++stt).ToString();//Số thứ tự
                row[1] = student.DSDiem[i].TenMH;
                if (student.DSDiem[i].TenMH == GlobalProperties.listTenMH[11])
                {
                    row[2] = student.DSDiem[i].DDGTX1.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX1.diem < 5 ? "CĐ" : "Đ";
                    row[3] = student.DSDiem[i].DDGTX2.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX2.diem < 5 ? "CĐ" : "Đ";
                    row[4] = student.DSDiem[i].DDGTX3.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX3.diem < 5 ? "CĐ" : "Đ";
                    row[5] = student.DSDiem[i].DDGTX4.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX4.diem < 5 ? "CĐ" : "Đ";
                    row[6] = student.DSDiem[i].DDGGK.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGGK.diem < 5 ? "CĐ" : "Đ";
                    row[7] = student.DSDiem[i].DDGCK.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGCK.diem < 5 ? "CĐ" : "Đ";
                    row[8] = student.DSDiem[i].DDGTRB.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTRB.diem < 5 ? "CĐ" : "Đ";
                }
                else
                {
                    row[2] = student.DSDiem[i].DDGTX1.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX1.diem.ToString();
                    row[3] = student.DSDiem[i].DDGTX2.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX2.diem.ToString();
                    row[4] = student.DSDiem[i].DDGTX3.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX3.diem.ToString();
                    row[5] = student.DSDiem[i].DDGTX4.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX4.diem.ToString();
                    row[6] = student.DSDiem[i].DDGGK.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGGK.diem.ToString();
                    row[7] = student.DSDiem[i].DDGCK.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGCK.diem.ToString();
                    row[8] = student.DSDiem[i].DDGTRB.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTRB.diem.ToString();
                }

                ListViewItem listViewItem = new ListViewItem(row);
                LV_BangDiem.Items.Add(listViewItem);
                haveBangDiem_p2 = true;
            }
            if (!student.GetTongKetHocKi(student.TTBangDiem[idx].HK, student.TTBangDiem[idx].namHoc))
            {
                MessageBox.Show("Không thể xếp loại kết quả", "Thông báo!");
                return;
            }
            if (student.TTBangDiem[idx].HK == "HK1")
            {
                LB_KetQuaHocTap_p2.Text = student.DiemTKHKI.xepLoai.ToString();
                LB_RenLuyen_p2.Text = student.DiemTKHKI.hanhKiem.ToString();
                LB_NghiCoPhep_p2.Text = student.BangViPham.NghiCoPhepHKI.ToString();
                LB_KhongPhep_p2.Text = student.BangViPham.NghiKhongPhepHKI.ToString();
                LB_ViPham_p2.Text = student.BangViPham.ViPhamHKI.ToString();
            }   
            else
            {
                LB_KetQuaHocTap_p2.Text = student.DiemTKHKII.xepLoai.ToString();
                LB_RenLuyen_p2.Text = student.DiemTKHKII.hanhKiem.ToString();
                LB_NghiCoPhep_p2.Text = student.BangViPham.NghiCoPhepHKII.ToString();
                LB_KhongPhep_p2.Text = student.BangViPham.NghiKhongPhepHKII.ToString();
                LB_ViPham_p2.Text = student.BangViPham.ViPhamHKII.ToString();
            }
        }

        private void CB_ChonNamHoctab3_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = CB_ChonNamHoctab3.SelectedIndex;
            if (idx < 0)
            {
                MessageBox.Show("Hãy chọn thông tin cần xem", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                haveBangDiem_p3 = false;
                return;
            }

            if (!student.GetTongKetNamHoc(CB_ChonNamHoctab3.SelectedItem.ToString()))
            {
                MessageBox.Show("Không thể lấy thông tin bảng điểm hoặc chưa có điểm!!!", "Thông báo!");
                haveBangDiem_p3 = false;
                return;
            }
            LV_TongKetDiem.Items.Clear();
            int stt = 0;
            for (int i = 0; i < GlobalProperties.soMonHoc; i++)
            {
                string[] row = new string[9];
                row[0] = (++stt).ToString();//Số thứ tự
                row[1] = student.DSDiem[i].TenMH;
                double hk1 = student.ListDiemTrBHKI[i].diem;
                double hk2 = student.ListDiemTrBHKII[i].diem;
                double cn = student.ListDiemTrBCN[i].diem;
                if (i == 11)
                {
                    row[2] = hk1 == -1 ? GlobalProperties.NULLFIELD : hk1 < 5 ? "CĐ" : "Đ";
                    row[3] = hk2 == -1 ? GlobalProperties.NULLFIELD : hk2 < 5 ? "CĐ" : "Đ";
                    row[4] = cn == -1 ? GlobalProperties.NULLFIELD : cn < 5 ? "CĐ" : "Đ";
                }
                else
                {
                    row[2] = hk1 == -1 ? GlobalProperties.NULLFIELD : hk1.ToString();
                    row[3] = hk2 == -1 ? GlobalProperties.NULLFIELD : hk2.ToString();
                    row[4] = cn  == -1 ? GlobalProperties.NULLFIELD : cn.ToString();
                }
                ListViewItem listViewItem = new ListViewItem(row);
                LV_TongKetDiem.Items.Add(listViewItem);
            }
            hocTapHKI_Page3.Text = student.DiemTKHKI.xepLoai.ToString();
            hanhKiemHKI_page3.Text = student.DiemTKHKI.hanhKiem.ToString();

            hocTapHkII_page3.Text = student.DiemTKHKII.xepLoai.ToString();
            hanhKiemHKII_page3.Text = student.DiemTKHKII.hanhKiem.ToString();

            hocTapCn_page3.Text = student.DiemTKCN.xepLoai.ToString();
            hanhKiemCN_page3.Text = student.DiemTKCN.hanhKiem.ToString();
            haveBangDiem_p3 = true;
        }

        private void PB_In_page1_Click(object sender, EventArgs e)
        {
            if (!haveBangDiem_p2)
            {
                return;
            }
            string[,] saveValue = new string[13, 7];
            for (int i = 0; i < 13; i++)
                for (int j = 0; j < 7; j++)
                {
                    saveValue[i, j] = LV_BangDiem.Items[i].SubItems[j + 2].Text;
                }
            MakeReport rp = new MakeReport(GlobalProperties.BANGDIEMHSTEMPLATEPATH, new Point(13, 3), saveValue);
            int idx = CB_ttHK_NH.SelectedIndex;
            List<(string value, Point location)> otherValue = new List<(string value, Point location)>();
            otherValue.Add((student.HoTen, new Point(6, 3)));
            otherValue.Add((student.MaHS, new Point(6, 8)));
            otherValue.Add((student.Lop, new Point(7, 3)));
            otherValue.Add((student.TTBangDiem[idx].HK, new Point(7, 5)));
            otherValue.Add((student.TTBangDiem[idx].namHoc, new Point(7, 8)));
            otherValue.Add((LB_KetQuaHocTap_p2.Text, new Point(9, 3)));
            otherValue.Add((LB_RenLuyen_p2.Text, new Point(9, 8)));
            otherValue.Add((LB_NghiCoPhep_p2.Text, new Point(10, 3)));
            otherValue.Add((LB_KhongPhep_p2.Text, new Point(10, 6)));
            otherValue.Add((LB_ViPham_p2.Text, new Point(10, 8)));
            rp.OrtherValue = otherValue;
            rp.GetSavePathWithSaveFileDialog();
            rp.OverwritetoExcelFile();
            rp.OpenExcelFile();
        }

        private void pictureBox3_Click(object sender, EventArgs e) //In bảng điểm cả năm
        {
            student.GetViPhamNamHoc(CB_ChonNamHoctab3.SelectedItem.ToString());
            if (!haveBangDiem_p3)
            {
                return;
            }
            string[,] saveValue = new string[13, 3];
            for (int i = 0; i < 13; i++)
                for (int j = 0; j < 3; j++)
                {
                    saveValue[i, j] = LV_TongKetDiem.Items[i].SubItems[j + 2].Text;
                }
            MakeReport rp = new MakeReport(GlobalProperties.BANGDIEMHSTONGKETTEMPLATEPATH, new Point(17, 3), saveValue);
            int idx = CB_ttHK_NH.SelectedIndex;
            List<(string value, Point location)> otherValue = new List<(string value, Point location)>();
            otherValue.Add((@"Họ tên: " + student.HoTen, new Point(6, 2)));
            otherValue.Add((@"Mã học sinh: " + student.MaHS, new Point(6, 4)));
            otherValue.Add((@"Lớp: " + student.Lop, new Point(7, 2)));
            otherValue.Add((@"Năm học: " + CB_ChonNamHoctab3.SelectedItem.ToString(), new Point(7, 4)));
            //HKI
            otherValue.Add((hocTapHKI_Page3.Text, new Point(9, 3)));
            otherValue.Add((hanhKiemHKI_page3.Text, new Point(9, 5)));
            otherValue.Add((student.BangViPham.NghiCoPhepHKI.ToString() + "/" + student.BangViPham.NghiKhongPhepHKI.ToString(), new Point(10, 3)));
            otherValue.Add((student.BangViPham.ViPhamHKI.ToString(), new Point(10, 5)));
            //HKII
            otherValue.Add((hocTapHkII_page3.Text, new Point(12, 3)));
            otherValue.Add((hanhKiemHKII_page3.Text, new Point(12, 5)));
            otherValue.Add((student.BangViPham.NghiCoPhepHKII.ToString() + "/" + student.BangViPham.NghiKhongPhepHKII.ToString(), new Point(13, 3)));
            otherValue.Add((student.BangViPham.ViPhamHKII.ToString(), new Point(13, 5)));
            //Cả năm
            otherValue.Add((hocTapCn_page3.Text, new Point(15, 3)));
            otherValue.Add((hanhKiemCN_page3.Text, new Point(15, 5)));
            rp.OrtherValue = otherValue;
            rp.GetSavePathWithSaveFileDialog();
            rp.OverwritetoExcelFile();
            rp.OpenExcelFile();
        }
    }
}
