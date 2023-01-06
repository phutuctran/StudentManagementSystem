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
using StudentManagementSystem.CustomControls;
using StudentManagementSystem.DatabaseCore;

namespace StudentManagementSystem
{
    public partial class StudentInfo : MaterialForm
    {
        Student student;
        string maHS;
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
                foreach ((string hk, string namHoc) p in student.TTBangDiem)
                {
                    CB_ttHK_NH.Items.Add(p.hk + ", " + p.namHoc);
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
                return;
            }
            if (!student.GetThongTinBangDiem(student.TTBangDiem[idx].HK, student.TTBangDiem[idx].namHoc))
            {
                MessageBox.Show("Không thể lấy thông tin bảng điểm hoặc chưa có điểm!!!", "Thông báo!");
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
                    row[3] = student.DSDiem[i].DDGTX2.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX2.diem< 5 ? "CĐ" : "Đ";
                    row[4] = student.DSDiem[i].DDGTX3.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX3.diem< 5 ? "CĐ" : "Đ";
                    row[5] = student.DSDiem[i].DDGTX4.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX4.diem< 5 ? "CĐ" : "Đ";
                    row[6] = student.DSDiem[i].DDGGK.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGGK.diem< 5 ? "CĐ" : "Đ";
                    row[7] = student.DSDiem[i].DDGCK.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGCK.diem< 5 ? "CĐ" : "Đ";
                    row[8] = student.DSDiem[i].DDGTRB.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTRB.diem< 5 ? "CĐ" : "Đ";
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
            }
        }


    }
}
