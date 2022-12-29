using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StudentManagementSystem.Classes;
using StudentManagementSystem.DatabaseCore;
using System.Text.RegularExpressions;
using System.IO;
using DevExpress.Utils.Extensions;
using Microsoft.VisualBasic.Logging;
using static DevExpress.Data.Helpers.FindSearchRichParser;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace StudentManagementSystem
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
        Admin_Funcs Admin = new Admin_Funcs(true);

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            
            mtCu_p8._TextBox.PasswordChar = '\u25CF';
            mkmoi_p8._TextBox.PasswordChar = '\u25CF';
            mtk_p8._TextBox.PasswordChar = '\u25CF';
            tabMain.Appearance = TabAppearance.FlatButtons;
            tabMain.ItemSize = new Size(0, 1);
            tabMain.SizeMode = TabSizeMode.Fixed;
            GetandShowMaNamHoc();
            GetandShowMaNamHocpage2();
            GetandShowMaNamHocpage3();
            GetandShowMaNamHocpage4();
            LoadPage5();
           
            string check = File.ReadAllText("./StudentEdit");
            if (check == "1")
                bunifuCheckBox1.Checked = true;
            else
                bunifuCheckBox1.Checked = false;
        }
        
        //Event menu
        private void bunifuIconButton5_Click(object sender, EventArgs e)
        {
            btn_TabThemHS.PerformClick();
        }

        private void bunifuIconButton7_Click(object sender, EventArgs e)
        {
            btn_tabThemGV_GD.PerformClick();
        }

        private void bunifuIconButton1_Click(object sender, EventArgs e)
        {
            btn_tabBangDiem.PerformClick();
        }

        private void bunifuIconButton2_Click(object sender, EventArgs e)
        {
            btn_tab_Thongtin.PerformClick();
        }

        private void bunifuIconButton3_Click(object sender, EventArgs e)
        {
            btn_tab_tongket.PerformClick();
        }

        private void bunifuIconButton4_Click(object sender, EventArgs e)
        {
            btn_tabChuyenLop.PerformClick();
        }

        private void bunifuIconButton6_Click(object sender, EventArgs e)
        {
            btn_tab_themNK_HK.PerformClick();
        }

        private void bunifuIconButton8_Click(object sender, EventArgs e)
        {
            btn_CaiDat.PerformClick();
        }

        private void btn_tab_Thongtin_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 1;
        }

        private void btn_tab_tongket_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 2;
        }

        private void btn_tabChuyenLop_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 3;
        }

        private void btn_tab_themNK_HK_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 4;
        }

        private void btn_tabBangDiem_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 0;
        }

        private void btn_TabThemHS_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 5;
        }

        private void btn_tabThemGV_GD_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 6;
        }

        private void btn_CaiDat_Click(object sender, EventArgs e)
        {
            tabMain.SelectedIndex = 7;
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (panel_Menu.Width > 75)
            {
                panel_Menu.Width = 75;
                btn_Menu.Location = new Point(16, 10);

            }
            else
            {
                panel_Menu.Width = 200;
                btn_Menu.Location = new Point(141, 10);
            }
        }

        //----------------------Page1----------------------
        void GetandShowMaNamHoc()
        {
            CB_NamHoc.SelectedIndex = -1;
            CB_NamHoc.Items.Clear();
            CB_Lop.SelectedIndex = -1;
            CB_Lop.Items.Clear();
            Admin.Func_Page1.GetListNamHoc();

            foreach (string p in Admin.Func_Page1.ListNamHoc)
            {
                CB_NamHoc.Items.Add(p);
            }
        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e) //search page1
        {
            string text = TB_search_page1.Text;
            if (string.IsNullOrEmpty(text))
            {
                for (int i = 0; i < dataGridView_BangDiem.RowCount; i++)
                {
                    dataGridView_BangDiem.Rows[i].Visible = true;
                }
            }
            for (int i = 0; i < dataGridView_BangDiem.RowCount; i++)
            {
                string mahs = dataGridView_BangDiem.Rows[i].Cells[1].Value.ToString();
                string tenhs = dataGridView_BangDiem.Rows[i].Cells[2].Value.ToString();
                if (!mahs.Contains(text) && !tenhs.Contains(text))
                {
                    dataGridView_BangDiem.Rows[i].Visible = false;
                }
                else
                {
                    dataGridView_BangDiem.Rows[i].Visible = true;
                }
            }
        }

        private void CB_NamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Admin.Func_Page1.CurrentNamHoc))
            {
                if (!CheckDataGridView())
                {
                    CB_NamHoc.SelectedIndex = Admin.Func_Page1.IndexOfCurrentNamHocInList;
                    return;
                }
            }
            if (CB_NamHoc.SelectedIndex == -1)
            {
                Admin.Func_Page1.CurrentNamHoc = GlobalProperties.NULLFIELD;
                return;
            }
            Admin.Func_Page1.CurrentNamHoc = CB_NamHoc.SelectedItem.ToString();
            CB_Lop.SelectedIndex = -1;
            CB_Lop.Items.Clear();
            //Admin.Func_Page1.CurrentMaLop = GlobalProperties.NULLFIELD;
            if (CB_Khoi.SelectedIndex != -1)
            {
                Admin.Func_Page1.GetListLop(CB_Khoi.SelectedItem.ToString(), CB_NamHoc.SelectedItem.ToString());
                foreach (Lop p in Admin.Func_Page1.ListLop)
                    CB_Lop.Items.Add(p.TenLop);
            }
        }

        private void CB_Khoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Admin.Func_Page1.CurrentKhoi))
            {
                if (!CheckDataGridView())
                {
                    CB_Khoi.SelectedIndex = Admin.Func_Page1.IndexOfCurrentKhoiInList;
                    return;
                }
            }
            if (CB_Khoi.SelectedIndex == -1)
            {
                Admin.Func_Page1.CurrentKhoi = GlobalProperties.NULLFIELD;
                return;
            }
            Admin.Func_Page1.CurrentKhoi = CB_Khoi.SelectedItem.ToString();

            CB_Lop.SelectedIndex = -1;
            CB_Lop.Items.Clear();
            //Admin.Func_Page1.CurrentMaLop = GlobalProperties.NULLFIELD;
            if (CB_NamHoc.SelectedIndex != -1)
            {
                Admin.Func_Page1.GetListLop(CB_Khoi.SelectedItem.ToString(), CB_NamHoc.SelectedItem.ToString());
                foreach (Lop p in Admin.Func_Page1.ListLop)
                    CB_Lop.Items.Add(p.TenLop);
            }
        }

        private void CB_Lop_Click(object sender, EventArgs e)
        {
            if (CB_NamHoc.SelectedIndex == -1 || CB_Khoi.SelectedIndex == -1)
            {
                MessageBox.Show("Chọn năm học và khối trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CB_Lop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Admin.Func_Page1.CurrentMaLop))
            {
                if (!CheckDataGridView())
                {
                    CB_Lop.SelectedIndex = Admin.Func_Page1.IndexOfCurrentlopInList;
                    return;
                }
            }
            if (CB_Lop.SelectedIndex == -1)
            {
                Admin.Func_Page1.CurrentMaLop = GlobalProperties.NULLFIELD;
                return;
            }
            Admin.Func_Page1.CurrentMaLop = Admin.Func_Page1.ListLop[CB_Lop.SelectedIndex].MaLop;

            CB_HocKi.SelectedIndex = -1;
            CB_MonHoc.SelectedIndex = -1;

            //Admin.Func_Page1.ListHocSinh.Clear();
            int stt = 0;
            //lấy thông tin học sinh
            Admin.Func_Page1.GetListHocSinh(Admin.Func_Page1.ListLop[CB_Lop.SelectedIndex].MaLop);
            dataGridView_BangDiem.Rows.Clear();
            foreach (var p in Admin.Func_Page1.ListHocSinh)
            {
                var index = dataGridView_BangDiem.Rows.Add();

                dataGridView_BangDiem.Rows[index].Cells[0].Value = (++stt).ToString();//Số thứ tự
                dataGridView_BangDiem.Rows[index].Cells[1].Value = p.MaHS;
                dataGridView_BangDiem.Rows[index].Cells[2].Value = p.HoTen;
            }

            //Get thong tin lop hoc
            var tmp = Admin.Func_Page1.GetThongTinLop();
            lb_SiSo_page1.Text = "Sĩ số: " + tmp.siSo;
            lb_TenLop_page1.Text = "Lớp: " + tmp.tenLop;
            lb_GVCN_page1.Text = "GVCN: " + tmp.tenGV;

        }

        private void CB_HocKi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Admin.Func_Page1.CurrentHocKi))
            {
                if (!CheckDataGridView(false))
                {
                    CB_HocKi.SelectedIndex = (Admin.Func_Page1.CurrentHocKi == "HK1" ? 0 : 1);
                    return;
                }
            }
            
            if (CB_HocKi.SelectedIndex == -1)
            {
                return;
            }
            Admin.Func_Page1.CurrentHocKi = CB_HocKi.SelectedItem.ToString();
            if (CB_MonHoc.SelectedIndex == -1)
            {
                return;
            }
            GetDiemHocSinh();

        }

        private void CB_MonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Admin.Func_Page1.CurrentMonHoc))
            {
                if (!CheckDataGridView(false))
                {
                    CB_MonHoc.SelectedIndex = Array.IndexOf(GlobalProperties.listTenMH, Admin.Func_Page1.CurrentMonHoc.Trim()); ;
                    return;
                }
            }
            //MessageBox.Show(Admin.Func_Page1.CurrentMonHoc);
            if (CB_MonHoc.SelectedIndex == -1)
            {
                return;
            }
            Admin.Func_Page1.CurrentMonHoc = CB_MonHoc.SelectedItem.ToString().Trim();
            if (CB_HocKi.SelectedIndex == -1)
            {
                MessageBox.Show("Chọn thêm học kì!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            GetDiemHocSinh();
        }

        void GetDiemHocSinh()
        {
            Admin.Func_Page1.GetDiemListHocSinh();
            ShowBangDiem();

            //Thông tin giáo viên giảng dạy
            lb_GVBM_page1.Text = "GVBM: " + Admin.Func_Page1.GetThongTinGiangDay();
            lb_HK_page1.Text = "Học kì: " + CB_HocKi.SelectedItem.ToString();
            lb_MonHoc_page1.Text = "Môn học: " + CB_MonHoc.SelectedItem.ToString();
        }

        void ShowBangDiem()
        {
            if (Admin.Func_Page1.ListHocSinh.Count < 1)
            {
                return;
            }
            int idxMon = Array.IndexOf(GlobalProperties.listTenMH, Admin.Func_Page1.CurrentMonHoc);
            for (int i = 0; i < Admin.Func_Page1.ListHocSinh.Count; i++)
            {
                Student p = Admin.Func_Page1.ListHocSinh[i];
                dataGridView_BangDiem.Rows[i].Cells[3].Value = p.DSDiem[idxMon].DDGTX1.diem == -1 ? GlobalProperties.NULLFIELD : p.DSDiem[idxMon].DDGTX1.diem.ToString();
                dataGridView_BangDiem.Rows[i].Cells[4].Value = p.DSDiem[idxMon].DDGTX2.diem == -1 ? GlobalProperties.NULLFIELD : p.DSDiem[idxMon].DDGTX2.diem.ToString();
                dataGridView_BangDiem.Rows[i].Cells[5].Value = p.DSDiem[idxMon].DDGTX3.diem == -1 ? GlobalProperties.NULLFIELD : p.DSDiem[idxMon].DDGTX3.diem.ToString();
                dataGridView_BangDiem.Rows[i].Cells[6].Value = p.DSDiem[idxMon].DDGTX4.diem == -1 ? GlobalProperties.NULLFIELD : p.DSDiem[idxMon].DDGTX4.diem.ToString();
                dataGridView_BangDiem.Rows[i].Cells[7].Value = p.DSDiem[idxMon].DDGGK.diem == -1 ? GlobalProperties.NULLFIELD : p.DSDiem[idxMon].DDGGK.diem.ToString();
                dataGridView_BangDiem.Rows[i].Cells[8].Value = p.DSDiem[idxMon].DDGCK.diem == -1 ? GlobalProperties.NULLFIELD : p.DSDiem[idxMon].DDGCK.diem.ToString();
                dataGridView_BangDiem.Rows[i].Cells[9].Value = p.DSDiem[idxMon].DDGTRB.diem == -1 ? GlobalProperties.NULLFIELD : p.DSDiem[idxMon].DDGTRB.diem.ToString();
            }
        }

        private void btn_tinhDTB_pag1_Click(object sender, EventArgs e)
        {
            int[] heSo = { 1, 1, 1, 1, 2, 3 };
            for (int i = 0; i < Admin.Func_Page1.ListHocSinh.Count; i++)
            {
                int tongHeSo = 0;
                double tongDiem = 0;
                int tongCotDiem = 0;
                for (int j = 3; j <= 8; j++)
                {
                    string diem = dataGridView_BangDiem.Rows[i].Cells[j].Value == null ? string.Empty : dataGridView_BangDiem.Rows[i].Cells[j].Value.ToString();
                    //MessageBox.Show(diem);
                    if (string.IsNullOrEmpty(diem.Trim()))
                    {
                        continue;
                    }
                    double diemthuc = GlobalFunction.CheckDiem(diem.Trim());
                    if (diemthuc == -1)
                    {
                        MessageBox.Show($"Điểm nhập không hợp lệ ở STT {i + 1}", "Thông Báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    tongCotDiem = (j >= 3 && j <= 6) ? 1 : tongCotDiem;
                    tongCotDiem += (j > 6) ? heSo[j - 3] : 0;
                    tongDiem += diemthuc * heSo[j - 3];
                    tongHeSo += heSo[j - 3];
                }

                if (tongCotDiem == 6)
                {
                    double tmp = Math.Round(tongDiem / tongHeSo, 1);
                    dataGridView_BangDiem.Rows[i].Cells[9].Value = tmp.ToString();
                }
                else
                {
                    dataGridView_BangDiem.Rows[i].Cells[9].Value = "";
                }
            }

        }

        private void btn_HoanTac_page1_Click(object sender, EventArgs e)
        {
            ShowBangDiem();
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e) // Lưu điểm hs
        {
            if (CB_MonHoc.SelectedIndex < 0)
            {
                MessageBox.Show("Chưa chọn môn học!", "Thông báo");
                return;
            }
            for (int i = 0; i < Admin.Func_Page1.ListHocSinh.Count; i++)
            {
                for (int j = 3; j <= 9; j++)
                {
                    string diem = dataGridView_BangDiem.Rows[i].Cells[j].Value == null ? string.Empty : dataGridView_BangDiem.Rows[i].Cells[j].Value.ToString();
                    //MessageBox.Show(diem);
                    if (string.IsNullOrEmpty(diem.Trim()))
                    {
                        continue;
                    }
                    double diemthuc = GlobalFunction.CheckDiem(diem.Trim());
                    if (diemthuc == -1)
                    {
                        MessageBox.Show($"Điểm nhập không hợp lệ ở {i}", "Thông Báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            //btn_tinhDTB_pag1.PerformClick();
            for (int i = 0; i < Admin.Func_Page1.ListHocSinh.Count; i++)
            {
                double[,] bangDiemNew = new double[13, 7];
                int idxMon = Array.IndexOf(GlobalProperties.listTenMH, Admin.Func_Page1.CurrentMonHoc);
                //MessageBox.Show(idxMon.ToString());
                for (int j = 3; j <= 9; j++)
                {
                    string _diem = dataGridView_BangDiem.Rows[i].Cells[j].Value == null ? string.Empty : dataGridView_BangDiem.Rows[i].Cells[j].Value.ToString();
                    bangDiemNew[idxMon, j - 3] = GlobalFunction.CheckDiem(_diem.Trim());
                }
                Admin.Func_Page1.ListHocSinh[i].SaveDiemStudent(bangDiemNew, Admin.Func_Page1.CurrentHocKi, Admin.Func_Page1.CurrentNamHoc, idxMon);
            }

            GetDiemHocSinh();
            MessageBox.Show("Đã lưu", "Thông báo");
        }

        private void tbn_reset_Click(object sender, EventArgs e)
        {
            Admin.Func_Page1 = new Admin_Func_Page1();
            CB_NamHoc.Items.Clear();
            CB_NamHoc.Text = "";
            CB_Lop.Items.Clear();
            CB_Lop.Text = "";
            CB_Khoi.SelectedIndex = -1;
            CB_HocKi.SelectedIndex = -1;
            CB_MonHoc.SelectedIndex = -1;
            GetandShowMaNamHoc();
            dataGridView_BangDiem.Rows.Clear();
        }

        bool CheckDataGridView(bool del = true)
        {
            //MessageBox.Show(dataGridView_BangDiem.Rows.Count.ToString());
            if (dataGridView_BangDiem.RowCount > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Thay đổi sẽ làm thay đổi dữ liệu đang hiển thị", "Cảnh báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    if (del)
                        dataGridView_BangDiem.Rows.Clear();
                    return true;
                }
                else if (dialogResult == DialogResult.No)
                {
                    return false;
                }
            }
            return true;
        }

        //----------------------tabPage2---------------------------
        void GetandShowMaNamHocpage2()
        {
            CB_NamHoc_page2.Items.Clear();
            CB_Lop_page2.Items.Clear();
            CB_NamHoc_page2.Items.Add("*");
            Admin.Func_Page2 = new Admin_Func_Page2();
            Admin.Func_Page2.GetListNamHoc();
            foreach (string p in Admin.Func_Page2.ListNamHoc)
            {
                CB_NamHoc_page2.Items.Add(p);
            }
        }

        private void CB_Khoi_page2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_Lop_page2.SelectedIndex = -1;
            CB_Lop_page2.Items.Clear();
            if (CB_NamHoc_page2.SelectedIndex <= 0)
                if (CB_Khoi_page2.SelectedIndex <= 0)//
                    GetInfoHocSinh(); //get toàn bộ học sinh
                else
                    GetInfoHocSinh(_maKhoi: CB_Khoi_page2.SelectedItem.ToString());//get học sinh theo khối.
            else
                if (CB_Khoi_page2.SelectedIndex <= 0)//
                GetInfoHocSinh(_namHoc: CB_NamHoc_page2.SelectedItem.ToString()); //get học sinh theo năm học
            else
            {
                GetInfoHocSinh(_namHoc: CB_NamHoc_page2.SelectedItem.ToString(), _maKhoi: CB_Khoi_page2.SelectedItem.ToString());//get học sinh theo khối và theo năm học
                Admin.Func_Page2.GetListLop(CB_Khoi_page2.SelectedItem.ToString(), CB_NamHoc_page2.SelectedItem.ToString());
                CB_Lop_page2.Items.Clear();
                CB_Lop_page2.Items.Add("*");
                foreach (Lop p in Admin.Func_Page2.ListLop)
                    CB_Lop_page2.Items.Add(p.TenLop);
            }
        }

        private void CB_Lop_page2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CB_NamHoc_page2.SelectedIndex < 1 || CB_Khoi_page2.SelectedIndex < 1)
            {
                //MessageBox.Show("Hãy chọn năm học và khối trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (CB_Lop_page2.SelectedIndex < 1)
            {
                GetInfoHocSinh(_namHoc: CB_NamHoc_page2.SelectedItem.ToString(), _maKhoi: CB_Khoi_page2.SelectedItem.ToString()); ;//get học sinh theo khối và theo năm học
            }
            else
            {
                GetInfoHocSinh(_namHoc: CB_NamHoc_page2.SelectedItem.ToString(), _maKhoi: CB_Khoi_page2.SelectedItem.ToString(), _tenLop: CB_Lop_page2.SelectedItem.ToString());//get học sinh theo khối và theo năm học và lớp
            }

        }

        private void CB_Lop_page2_Click(object sender, EventArgs e)
        {
            if (CB_NamHoc_page2.SelectedIndex < 1 || CB_Khoi_page2.SelectedIndex < 1)
            {
                MessageBox.Show("Hãy chọn năm học và khối trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void dataGridView_ThongTinHocSinh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            string maHS = dataGridView_ThongTinHocSinh.Rows[e.RowIndex].Cells[1].Value.ToString();
            if (!string.IsNullOrEmpty(maHS))
            {
                using (Form frm = new StudentInfoEdit(maHS))
                {
                    frm.ShowDialog();
                    GC.Collect();
                }
            }
        }

        private void btn_reset_page2_Click(object sender, EventArgs e)
        {
            CB_NamHoc_page2.Text = "";
            GetandShowMaNamHocpage2();
            CB_Khoi.Text = "";
            CB_Khoi_page2.SelectedIndex = -1;
            CB_Lop_page2.Text = "";
            CB_Lop_page2.SelectedIndex = -1;
            CB_Lop_page2.Items.Clear();
            dataGridView_ThongTinHocSinh.Rows.Clear();

        }

        private void TB_Search_page2_TextChanged(object sender, EventArgs e)
        {

            string text = TB_Search_page2.Text;
            if (string.IsNullOrEmpty(text))
            {
                for (int i = 0; i < dataGridView_ThongTinHocSinh.RowCount; i++)
                {
                    dataGridView_ThongTinHocSinh.Rows[i].Visible = true;
                }
            }
            for (int i = 0; i < dataGridView_ThongTinHocSinh.RowCount; i++)
            {
                string mahs = dataGridView_ThongTinHocSinh.Rows[i].Cells[1].Value.ToString();
                string tenhs = dataGridView_ThongTinHocSinh.Rows[i].Cells[2].Value.ToString();
                if (!mahs.Contains(text) && !tenhs.Contains(text))
                {
                    dataGridView_ThongTinHocSinh.Rows[i].Visible = false;
                }
                else
                {
                    dataGridView_ThongTinHocSinh.Rows[i].Visible = true;
                }
            }
        }

        private void CB_NamHoc_page2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_Lop_page2.SelectedIndex = -1;
            CB_Lop_page2.Items.Clear();
            if (CB_NamHoc_page2.SelectedIndex <= 0)
            {
                if (CB_Khoi_page2.SelectedIndex <= 0)//
                {
                    GetInfoHocSinh(); //get toàn bộ học sinh
                }
                else
                {
                    GetInfoHocSinh(_maKhoi: CB_Khoi_page2.SelectedItem.ToString());//get học sinh theo khối.
                }
            }
            else
            {
                if (CB_Khoi_page2.SelectedIndex <= 0)
                {
                    GetInfoHocSinh(_namHoc: CB_NamHoc_page2.SelectedItem.ToString()); //get học sinh theo năm học
                }
                else
                {
                    GetInfoHocSinh(_namHoc: CB_NamHoc_page2.SelectedItem.ToString(), _maKhoi: CB_Khoi_page2.SelectedItem.ToString());//get học sinh theo khối và theo năm học
                    Admin.Func_Page2.GetListLop(CB_Khoi_page2.SelectedItem.ToString(), CB_NamHoc_page2.SelectedItem.ToString());
                    CB_Lop_page2.Items.Clear();
                    CB_Lop_page2.Items.Add("*");
                    foreach (Lop p in Admin.Func_Page2.ListLop)
                        CB_Lop_page2.Items.Add(p.TenLop);
                }
            }
        }

        void GetInfoHocSinh(string _maKhoi = "", string _namHoc = "", string _tenLop = "", string _maLop = "")
        {
            int stt = 0;
            dataGridView_ThongTinHocSinh.Rows.Clear();
            Admin.Func_Page2.GetInfoListHS(_maKhoi, _namHoc, _tenLop, _maLop);
            foreach (var p in Admin.Func_Page2.ListHS)
            {
                var index = dataGridView_ThongTinHocSinh.Rows.Add();
                dataGridView_ThongTinHocSinh.Rows[index].Cells[0].Value = (++stt).ToString();//Số thứ tự
                dataGridView_ThongTinHocSinh.Rows[index].Cells[1].Value = p.MaHS;
                dataGridView_ThongTinHocSinh.Rows[index].Cells[2].Value = p.HoTen;
                dataGridView_ThongTinHocSinh.Rows[index].Cells[3].Value = p.GioiTinh == "Nam" ? false : true;
                dataGridView_ThongTinHocSinh.Rows[index].Cells[4].Value = p.NgaySinh;
                dataGridView_ThongTinHocSinh.Rows[index].Cells[5].Value = p.Lop;
                dataGridView_ThongTinHocSinh.Rows[index].Cells[6].Value = p.NoiSinh;
                dataGridView_ThongTinHocSinh.Rows[index].Cells[7].Value = p.DiaChi;
                dataGridView_ThongTinHocSinh.Rows[index].Cells[8].Value = p.SDT;
                dataGridView_ThongTinHocSinh.Rows[index].Cells[9].Value = p.Email;
                dataGridView_ThongTinHocSinh.Rows[index].Cells[10].Value = p.GhiChu;
            }
        }

        //-----------page3------------
        private void CB_Lop_page3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Admin.Func_Page3.CurrentMaLop))
            {
                if (!CheckDataGridView_page3())
                {
                    CB_Lop_page3.SelectedIndex = Admin.Func_Page3.IndexOfCurrentlopInList;
                    return;
                }
            }
            Admin.Func_Page3.CurrentMaLop = Admin.Func_Page3.ListLop[CB_Lop_page3.SelectedIndex].MaLop;
            GetAndShowDataHS_p3();
            btn_tinhtongket_p3.PerformClick();

        }

        private void CB_Khoi_page3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Admin.Func_Page3.CurrentKhoi))
            {
                if (!CheckDataGridView_page3())
                {
                    CB_Khoi_page3.SelectedIndex = Admin.Func_Page3.IndexOfCurrentKhoiInList;
                    return;
                }
            }
            if (CB_Khoi_page3.SelectedIndex == -1)
            {
                return;
            }
            Admin.Func_Page3.CurrentKhoi = CB_Khoi_page3.SelectedItem.ToString();
            CB_Lop_page3.Text = "";
            CB_Lop_page3.Items.Clear();
            if (CB_NamHoc_page3.SelectedIndex != -1)
            {
                Admin.Func_Page3.GetListLop(CB_Khoi_page3.SelectedItem.ToString(), CB_NamHoc_page3.SelectedItem.ToString());
                foreach (Lop p in Admin.Func_Page3.ListLop)
                    CB_Lop_page3.Items.Add(p.TenLop);
            }
        }

        private void CB_NamHoc_page3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Admin.Func_Page3.CurrentNamHoc))
            {
                if (!CheckDataGridView_page3())
                {
                    CB_NamHoc_page3.SelectedIndex = Admin.Func_Page3.IndexOfCurrentNamHocInList;
                    return;
                }
            }
            if (CB_NamHoc_page3.SelectedIndex == -1)
            {
                return;
            }
            Admin.Func_Page3.CurrentNamHoc = CB_NamHoc_page3.SelectedItem.ToString();
            CB_Lop_page3.Text = "";
            CB_Lop_page3.Items.Clear();
            if (CB_Khoi_page3.SelectedIndex != -1)
            {
                Admin.Func_Page3.GetListLop(CB_Khoi_page3.SelectedItem.ToString(), CB_NamHoc_page3.SelectedItem.ToString());
                foreach (Lop p in Admin.Func_Page3.ListLop)
                    CB_Lop_page3.Items.Add(p.TenLop);
            }
        }

        void GetAndShowDataHS_p3()
        {
            Admin.Func_Page3.GetListHocSinh(Admin.Func_Page3.ListLop[CB_Lop_page3.SelectedIndex].MaLop);
            Admin.Func_Page3.GetDiemTRBListHocSinh();
            ShowDataHS();
        }

        void ShowDataHS()
        {
            int stt = 0;
            dataGridView_Tongket.Rows.Clear();
            foreach (var p in Admin.Func_Page3.ListHocSinh)
            {
                var index = dataGridView_Tongket.Rows.Add();
                dataGridView_Tongket.Rows[index].Cells[0].Value = (++stt).ToString();//Số thứ tự
                dataGridView_Tongket.Rows[index].Cells[1].Value = p.student.MaHS;
                dataGridView_Tongket.Rows[index].Cells[2].Value = p.student.HoTen;
            }

            for (int i = 0; i < Admin.Func_Page3.ListHocSinh.Count; i++)
            {
                var _hanhKiem1 = Admin.Func_Page3.ListHocSinh[i].hanhKiem1;
                var _hanhKiem2 = Admin.Func_Page3.ListHocSinh[i].hanhKiem2;
                var _hanhKiemCN = Admin.Func_Page3.ListHocSinh[i].hanhKiemCN;
                if (GetLoaiHanhKiem(_hanhKiem1) != -1)
                {
                    (dataGridView_Tongket.Rows[i].Cells[4] as DataGridViewComboBoxCell).Value = (dataGridView_Tongket.Rows[i].Cells[4] as DataGridViewComboBoxCell).Items[GetLoaiHanhKiem(_hanhKiem1)];
                }
                if (GetLoaiHanhKiem(_hanhKiem2) != -1)
                {
                    (dataGridView_Tongket.Rows[i].Cells[7] as DataGridViewComboBoxCell).Value = (dataGridView_Tongket.Rows[i].Cells[7] as DataGridViewComboBoxCell).Items[GetLoaiHanhKiem(_hanhKiem2)];
                }
                dataGridView_Tongket.Rows[i].Cells[10].Value = _hanhKiemCN;
            }

            var tmp = Admin.Func_Page3.GetThongTinLop();
            lb_siso_p3.Text = "Sĩ số: " + tmp.siSo.ToString();
            lb_tenlop_p3.Text = "Lớp: " + tmp.tenLop;
            lb_tengvcn_p3.Text = "GVCN: " + tmp.tenGV;
        }

        private void materialRaisedButton3_Click(object sender, EventArgs e) ///Lưu thông tin học sinh
        {
            string query;
            SqlCommand cmd;
            string[,] bangHanhKiem = new string[Admin.Func_Page3.ListHocSinh.Count, 3];
            for (int i = 0; i < Admin.Func_Page3.ListHocSinh.Count; i++)
            {
                bangHanhKiem[i, 0] = dataGridView_Tongket.Rows[i].Cells[4].Value == null ? GlobalProperties.NULLFIELD : dataGridView_Tongket.Rows[i].Cells[4].Value.ToString();
                bangHanhKiem[i, 1] = dataGridView_Tongket.Rows[i].Cells[7].Value == null ? GlobalProperties.NULLFIELD : dataGridView_Tongket.Rows[i].Cells[7].Value.ToString();
                bangHanhKiem[i, 2] = dataGridView_Tongket.Rows[i].Cells[10].Value == null ? GlobalProperties.NULLFIELD : dataGridView_Tongket.Rows[i].Cells[10].Value.ToString();
            }
            if (Admin.Func_Page3.SaveHanhKiemListHocSinh(bangHanhKiem))
            {
                MessageBox.Show("Đã lưu!", "Thông báo");
            }
        }

        int GetLoaiHanhKiem(string hk)// 0: Tôt, 1: Khá, 2: Trung bình, 3: yếu
        {
            switch (hk)
            {
                case "Tốt":
                    return 0;
                case "Khá":
                    return 1;
                case "Trung bình":
                    return 2;
                case "Yếu":
                    return 3;
                default:
                    return -1;
            }
        }

        bool CheckDataGridView_page3(bool del = true)
        {
            //MessageBox.Show(dataGridView_BangDiem.Rows.Count.ToString());
            if (dataGridView_Tongket.RowCount > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Thay đổi sẽ làm thay đổi dữ liệu đang hiển thị", "Cảnh báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    if (del)
                        dataGridView_Tongket.Rows.Clear();
                    return true;
                }
                else if (dialogResult == DialogResult.No)
                {
                    return false;
                }
            }
            return true;
        }

        private void CB_Lop_page3_Click(object sender, EventArgs e)
        {
            if (CB_Khoi_page3.SelectedIndex < 0 || CB_NamHoc_page3.SelectedIndex < 0)
            {
                MessageBox.Show("Chọn năm học và khối trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TB_search_page3_TextChanged(object sender, EventArgs e)
        {
            string text = TB_search_page3.Text;
            if (string.IsNullOrEmpty(text))
            {
                for (int i = 0; i < dataGridView_Tongket.RowCount; i++)
                {
                    dataGridView_Tongket.Rows[i].Visible = true;
                }
            }
            for (int i = 0; i < dataGridView_Tongket.RowCount; i++)
            {
                string mahs = dataGridView_Tongket.Rows[i].Cells[1].Value.ToString();
                string tenhs = dataGridView_Tongket.Rows[i].Cells[2].Value.ToString();
                if (!mahs.Contains(text) && !tenhs.Contains(text))
                {
                    dataGridView_Tongket.Rows[i].Visible = false;
                }
                else
                {
                    dataGridView_Tongket.Rows[i].Visible = true;
                }
            }
        }

        private void btn_Reset_p3_Click(object sender, EventArgs e)
        {
            Admin.Func_Page3 = new Admin_Func_Page3();

            CB_NamHoc_page3.Items.Clear();
            CB_NamHoc_page3.Text = "";
            CB_Lop_page3.Items.Clear();
            CB_Lop_page3.Text = "";
            CB_Khoi_page3.SelectedIndex = -1;
            GetandShowMaNamHocpage3();
            dataGridView_Tongket.Rows.Clear();
        }

        void GetandShowMaNamHocpage3()
        {
            CB_NamHoc_page3.Items.Clear();
            CB_Lop_page3.Items.Clear();

            Admin.Func_Page3.GetListNamHoc();
            foreach (var p in Admin.Func_Page3.ListNamHoc)
            {
                CB_NamHoc_page3.Items.Add(p);
            }
        }

        private void btn_tinhtongket_p3_Click(object sender, EventArgs e)
        {
            List<(int hk1, int hk2)> listHK = new List<(int hk1, int hk2)>(); 
            for (int i = 0; i < Admin.Func_Page3.ListHocSinh.Count; i++)
            {
                int hanhkiem1 = GetLoaiHanhKiem(dataGridView_Tongket.Rows[i].Cells[4].Value == null? GlobalProperties.NULLFIELD : dataGridView_Tongket.Rows[i].Cells[4].Value.ToString());
                int hanhkiem2 = GetLoaiHanhKiem(dataGridView_Tongket.Rows[i].Cells[7].Value == null ? GlobalProperties.NULLFIELD : dataGridView_Tongket.Rows[i].Cells[7].Value.ToString());
                listHK.Add((hanhkiem1, hanhkiem2));
            }    
                
            Admin.Func_Page3.TinhDiemTongKet(listHK);
            for (int i = 0; i < Admin.Func_Page3.ListHocSinh.Count; i++)
            {
                var tmp = Admin.Func_Page3.ListHocSinh[i].DiemTongKetHK1;
                if (tmp.diemTrungBinh != -1)
                {
                    dataGridView_Tongket.Rows[i].Cells[3].Value = tmp.diemTrungBinh.ToString();
                    dataGridView_Tongket.Rows[i].Cells[5].Value = tmp.xepLoai;
                }
                tmp = Admin.Func_Page3.ListHocSinh[i].DiemTongKetHK2;
                if (tmp.diemTrungBinh != -1)
                {
                    dataGridView_Tongket.Rows[i].Cells[6].Value = tmp.diemTrungBinh.ToString();
                    dataGridView_Tongket.Rows[i].Cells[8].Value = tmp.xepLoai;
                }
                tmp = Admin.Func_Page3.ListHocSinh[i].DiemTongKetCN;
                if (tmp.diemTrungBinh != -1)
                {
                    dataGridView_Tongket.Rows[i].Cells[9].Value = tmp.diemTrungBinh.ToString();
                    dataGridView_Tongket.Rows[i].Cells[10].Value = tmp.hanhKiem;
                    dataGridView_Tongket.Rows[i].Cells[11].Value = tmp.xepLoai;
                }
            }
        }

        //------------------page4-------------------
        Dictionary<int, int> listChuyen = new Dictionary<int, int>();//key = stt page mới, value = stt page cũ

        private void CB_NamHocCu_p4_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_LopCu_p4.Text = "";
            CB_LopCu_p4.Items.Clear();
            dataGridView_page4_lopcu.Rows.Clear();
            dataGridView_page4_lopmoi.Rows.Clear();
            if (Cb_KhoiCu_p4.SelectedIndex != -1)
            {
                GetLopCu_page4();
            }
        }

        private void Cb_KhoiCu_p4_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_LopCu_p4.Text = "";
            CB_LopCu_p4.Items.Clear();
            dataGridView_page4_lopcu.Rows.Clear();
            dataGridView_page4_lopmoi.Rows.Clear();
            if (CB_NamHocCu_p4.SelectedIndex != -1)
            {
                GetLopCu_page4();
            }
        }

        private void CB_LopCu_p4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Admin.Func_Page4.GetListHocSinhLopCu(Admin.Func_Page4.ListLopCu[CB_LopCu_p4.SelectedIndex].MaLop, CB_NamHocCu_p4.SelectedItem.ToString(), Cb_KhoiCu_p4.SelectedItem.ToString());
            dataGridView_page4_lopcu.Rows.Clear();
            int stt = 0;
            for (int i = 0; i < Admin.Func_Page4.ListHocSinh.Count; i++)
            {
                var tmp = Admin.Func_Page4.ListHocSinh[i];
                var index = dataGridView_page4_lopcu.Rows.Add();
                dataGridView_page4_lopcu.Rows[index].Cells[0].Value = (++stt).ToString();//Số thứ tự
                dataGridView_page4_lopcu.Rows[index].Cells[1].Value = tmp.MaHS;
                dataGridView_page4_lopcu.Rows[index].Cells[2].Value = tmp.HoTen;
                dataGridView_page4_lopcu.Rows[index].Cells[3].Value = tmp.GioiTinh;
                dataGridView_page4_lopcu.Rows[index].Cells[4].Value = tmp.GhiChu;
            }
        }

        void GetandShowMaNamHocpage4()
        {
            CB_NamHocCu_p4.Items.Clear();
            CB_LopCu_p4.Items.Clear();

            Admin.Func_Page4.GetListNamHoc();
            foreach (var p in Admin.Func_Page4.ListNamHoc)
            {
                CB_NamHocCu_p4.Items.Add(p);
                CB_NamHocMoi_p4.Items.Add(p);

            }
        }

        private void CB_LopCu_p4_Click(object sender, EventArgs e)
        {
            if (CB_NamHocCu_p4.SelectedIndex < 0 || Cb_KhoiCu_p4.SelectedIndex < 0)
            {
                MessageBox.Show("Chọn năm học và khối trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView_page4_lopcu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            int sttCu;
            Int32.TryParse(dataGridView_page4_lopcu.Rows[e.RowIndex].Cells[0].Value.ToString(), out sttCu);
            var index = dataGridView_page4_lopmoi.Rows.Add();
            for (int i = 1; i <= 4; i++)
            {
                dataGridView_page4_lopmoi.Rows[index].Cells[i].Value = dataGridView_page4_lopcu.Rows[e.RowIndex].Cells[i].Value;
            }

            dataGridView_page4_lopmoi.Rows[index].Cells[0].Value = (index + 1).ToString();

            listChuyen[index + 1] = sttCu;
            dataGridView_page4_lopcu.Rows[e.RowIndex].Visible = false;

        }

        private void dataGridView_page4_lopmoi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            if (idx < 0)
            {
                return;
            }
            int sttCu = listChuyen[idx + 1];
            dataGridView_page4_lopmoi.Rows.RemoveAt(idx);
            dataGridView_page4_lopcu.Rows[sttCu - 1].Visible = true;
            listChuyen.Remove(idx + 1);
            for (int i = idx; i < dataGridView_page4_lopmoi.RowCount; i++)
            {
                int stt;
                Int32.TryParse(dataGridView_page4_lopmoi.Rows[i].Cells[0].Value.ToString(), out stt);
                dataGridView_page4_lopmoi.Rows[i].Cells[0].Value = (stt - 1).ToString();
                listChuyen[stt - 1] = listChuyen[stt];
                listChuyen.Remove(stt);
            }
        }

        private void CB_KhoiMoi_p4_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_LopMoi_p4.Text = "";
            CB_LopMoi_p4.Items.Clear();
            if (CB_NamHocMoi_p4.SelectedIndex != -1)
            {
                GetLopMoi_page4();
            }
        }

        private void CB_NamHocMoi_p4_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_LopMoi_p4.Text = "";
            CB_LopMoi_p4.Items.Clear();
            if (CB_KhoiMoi_p4.SelectedIndex != -1)
            {
                GetLopMoi_page4();
            }
        }

        private void CB_LopMoi_p4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void GetLopCu_page4()
        {
            CB_LopCu_p4.Items.Clear();
            Admin.Func_Page4.GetListLopCu(Cb_KhoiCu_p4.SelectedItem.ToString(), CB_NamHocCu_p4.SelectedItem.ToString());
            foreach (Lop p in Admin.Func_Page4.ListLopCu)
                CB_LopCu_p4.Items.Add(p.TenLop);
        }

        private void btn_ChuyenLop_Click(object sender, EventArgs e)
        {
            if (CB_LopMoi_p4.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn lớp mới!", "Thông báo");
                return;
            }
            if (CB_LoaiChuyen.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Loại chuyển lớp!", "Thông báo");
                return;
            }
            if (CB_LoaiChuyen.SelectedIndex == 1) //Lên lớp
            {
                if (CB_NamHocCu_p4.SelectedItem.ToString() == CB_NamHocMoi_p4.SelectedItem.ToString())
                {
                    MessageBox.Show("Lên lớp không chọn cùng năm học!", "Thông báo");
                    return;
                }
            }
            if (CB_LoaiChuyen.SelectedIndex == 0) //Chuyển lớp
            {
                if (CB_NamHocCu_p4.SelectedItem.ToString() != CB_NamHocMoi_p4.SelectedItem.ToString() ||
                    Cb_KhoiCu_p4.SelectedItem.ToString() != CB_KhoiMoi_p4.SelectedItem.ToString())
                {
                    MessageBox.Show("Chuyển lớp vui chọn cùng năm học và cùng khối!", "Thông báo");
                    return;
                }
            }
            if (CB_NamHocCu_p4.SelectedItem.ToString() == CB_NamHocMoi_p4.SelectedItem.ToString() &&
                Cb_KhoiCu_p4.SelectedItem.ToString() == CB_KhoiMoi_p4.SelectedItem.ToString() &&
                CB_LopCu_p4.SelectedItem.ToString() == CB_LopMoi_p4.SelectedItem.ToString())
            {
                MessageBox.Show("Vui lòng không chọn trùng lớp cũ!", "Thông báo");
                return;
            }

            string maLopMoi = Admin.Func_Page4.ListLopMoi[CB_LopMoi_p4.SelectedIndex].MaLop;
            string maLopCu = Admin.Func_Page4.ListLopCu[CB_LopCu_p4.SelectedIndex].MaLop;
            string maNamHoc = CB_NamHocMoi_p4.SelectedItem.ToString();
            List<(string maHS, string tenHS)> listMaHS = new List<(string maHS, string tenHS)>();
            for (int i = 0; i < dataGridView_page4_lopmoi.Rows.Count; i++)
            {
                listMaHS.Add((dataGridView_page4_lopmoi.Rows[i].Cells[1].Value.ToString(), dataGridView_page4_lopmoi.Rows[i].Cells[2].Value.ToString()));
            }
            Admin.Func_Page4.SaveChuyenLop(listMaHS, maLopMoi, maLopCu, maNamHoc, CB_LoaiChuyen.SelectedIndex);
            
            MessageBox.Show("Đã lưu", "Thông báo!");
            CB_LopCu_p4_SelectedIndexChanged(sender, e);
            dataGridView_page4_lopmoi.Rows.Clear();

        }

        void GetLopMoi_page4()
        {
            CB_LopMoi_p4.Items.Clear();
            Admin.Func_Page4.GetListLopMoi(CB_KhoiMoi_p4.SelectedItem.ToString(), CB_NamHocMoi_p4.SelectedItem.ToString());
            foreach (Lop p in Admin.Func_Page4.ListLopMoi)
                CB_LopMoi_p4.Items.Add(p.TenLop);
        }

        private void g_TextChanged(object sender, EventArgs e) //Search_page4
        {
            string text = TB_search_page4.Text;
            if (string.IsNullOrEmpty(text))
            {
                for (int i = 0; i < dataGridView_page4_lopcu.RowCount; i++)
                {
                    dataGridView_page4_lopcu.Rows[i].Visible = true;
                }
            }
            for (int i = 0; i < dataGridView_page4_lopcu.RowCount; i++)
            {
                string mahs = dataGridView_page4_lopcu.Rows[i].Cells[1].Value.ToString();
                string tenhs = dataGridView_page4_lopcu.Rows[i].Cells[2].Value.ToString();
                if (!mahs.Contains(text) && !tenhs.Contains(text))
                {
                    dataGridView_page4_lopcu.Rows[i].Visible = false;
                }
                else
                {
                    dataGridView_page4_lopcu.Rows[i].Visible = true;
                }
            }
            foreach (KeyValuePair<int, int> kvp in listChuyen)
            {
                dataGridView_page4_lopcu.Rows[kvp.Value - 1].Visible = false;
            }
        }
        private void btn_reset_p4_Click(object sender, EventArgs e)
        {
            Admin.Func_Page4 = new Admin_Func_page4();
            CB_NamHocCu_p4.Text = "";
            CB_LopCu_p4.Text = "";
            CB_NamHocMoi_p4.Text = "";
            CB_LopMoi_p4.Text = "";

            GetandShowMaNamHocpage4();
            dataGridView_page4_lopcu.Rows.Clear();
            dataGridView_page4_lopmoi.Rows.Clear();
        }

        //---------page5------------------
        void LoadPage5()
        {
            datetimepicker_nienkhoa_p5.Format = DateTimePickerFormat.Custom;
            datetimepicker_nienkhoa_p5.CustomFormat = "yyyy";
            datetimepicker_nienkhoa_p5.ShowUpDown = true;
            GetandShowMaNamHocpage5();
            Admin.Func_Page5.GetListNienKhoa();
        }

        void GetandShowMaNamHocpage5()
        {
            CB_NamHoc_p5.Text = "";
            CB_NamHoc_p5.Items.Clear();
            Admin.Func_Page5.GetListNamHoc();
            foreach (string p in Admin.Func_Page5.ListNamHoc)
            {
                CB_NamHoc_p5.Items.Add(p);
            }
        }

        private void btn_hienthinienkhoap5_Click(object sender, EventArgs e)
        {
            dataGridView_nienkhoa_p5.Rows.Clear();
            foreach (var p in Admin.Func_Page5.ListNienKhoa)
            {
                var index = dataGridView_nienkhoa_p5.Rows.Add();
                dataGridView_nienkhoa_p5.Rows[index].Cells[0].Value = p.MaNienKhoa;
                dataGridView_nienkhoa_p5.Rows[index].Cells[1].Value = p.NamBatDau;
                dataGridView_nienkhoa_p5.Rows[index].Cells[2].Value = p.NamKetThuc;
            }
        }

        private void datetimepicker_nienkhoa_p5_ValueChanged(object sender, EventArgs e)
        {
            TB_NamKT_p5.Text = (datetimepicker_nienkhoa_p5.Value.Year + 3).ToString();
            TB_MaNK_p5.Text = datetimepicker_nienkhoa_p5.Value.Year.ToString() + "-" + TB_NamKT_p5.Text;

        }

        private void btn_ThemNK_p5_Click(object sender, EventArgs e)
        {
            if (!Admin.Func_Page5.ThemNienKhoa(TB_MaNK_p5.Text, datetimepicker_nienkhoa_p5.Value.Year.ToString(), TB_NamKT_p5.Text))
            {
                return;
            }
            btn_hienthinienkhoap5.PerformClick();

        }

        private void btn_hienthi_Lop_p5_Click(object sender, EventArgs e)
        {

            if (CB_NamHoc_p5.SelectedIndex != -1 && CB_Khoi_p5.SelectedIndex != -1)
            {
                dataGridView_Lop_p5.Rows.Clear();
                Admin.Func_Page5.GetListLop(CB_Khoi_p5.SelectedItem.ToString(), CB_NamHoc_p5.SelectedItem.ToString());
                foreach (var p in Admin.Func_Page5.ListLop)
                {
                    var index = dataGridView_Lop_p5.Rows.Add();
                    dataGridView_Lop_p5.Rows[index].Cells[0].Value = p.MaLop;
                    dataGridView_Lop_p5.Rows[index].Cells[1].Value = p.TenLop;
                    dataGridView_Lop_p5.Rows[index].Cells[2].Value = p.TenGVCN;
                }
            }
            else
            {
                MessageBox.Show("Chọn năm học và khối trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void comboBox1_Click(object sender, EventArgs e) //get thong tin giaos vieen
        {
            CB_gv_p5.Items.Clear();
            Admin.Func_Page5.GetListGiaoVien();
            foreach (var p in Admin.Func_Page5.ListGiaoVien)
            {
                CB_gv_p5.Items.Add(p.TenGV + " - " + p.MaGV);
            }
        }

        private void CB_NamHoc_p5_Click(object sender, EventArgs e)
        {
            GetandShowMaNamHocpage5();
        }

        private void CB_gv_p5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //notthing
        }

        private void materialRaisedButton5_Click(object sender, EventArgs e) //Thêm lớp mới
        {
            if (string.IsNullOrEmpty(TB_TenLopTao.Text) || CB_gv_p5.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập tên lớp và chọn giáo viên chủ nhiệm!", "Thông báo");
                return;
            }
            for (int i = 0; i < dataGridView_Lop_p5.Rows.Count; i++)
            {
                if (dataGridView_Lop_p5.Rows[i].Cells[1].Value.ToString().ToLower() == TB_TenLopTao.Text.ToString().Trim().ToLower())
                {
                    MessageBox.Show("Tên lớp đã tồn tại!", "Thông báo");
                    return;
                }
            }
            if (Admin.Func_Page5.ThemLopMoi(CB_gv_p5.SelectedIndex, TB_TenLopTao.Text.ToString().Trim()))
            {
                //comboBox1_Click(sender, e);
                btn_hienthi_Lop_p5.PerformClick();
                CB_gv_p5.SelectedIndex = -1;
                CB_gv_p5.Text = "";
            }
        }

        //-------------page6------------------------------
        private void TB_MaHS_p6_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(TB_MaHS_p6.Text, @"^\d+$"))
            {
                Checkbox_Mahs.Checked = false;
                return;
            }
            if (!string.IsNullOrEmpty(TB_MaHS_p6.Text) && CheckMaHS(TB_MaHS_p6.Text))
            {
                Checkbox_Mahs.Checked = true;
                tb_Username_p6.Text = "HS" + TB_MaHS_p6.Text;
            }
            else
            {
                Checkbox_Mahs.Checked = false;
            }
        }

        private void btn_random_mahs_mk_Click(object sender, EventArgs e)
        {
            TB_matkhau_p6.Text = GlobalFunction.RandomString(6);
            string _maHS;
            do
            {
                _maHS = GlobalFunction.RandomStringInt(8);
                if (CheckMaHS(_maHS))
                {
                    TB_MaHS_p6.Text = _maHS;
                    tb_Username_p6.Text = "HS" + _maHS;
                    break;
                }
            } while (true);

        }

        private void btn_Luu_p6_Click(object sender, EventArgs e)
        {
            // Checkbox_Mah
            if (!Checkbox_Mahs.Checked)
            {
                MessageBox.Show("Mã học sinh không hợp lệ hoặc bị trùng. \n Mã học sinh chỉ bao gồm số", "Thông báo");
                return;
            }
            if (string.IsNullOrEmpty(TB_HoTen_p6.Text) ||
                string.IsNullOrEmpty(dateEdit_NgaySinh_p6.Text) ||
                CB_Gioitinh_p6.SelectedIndex < 0)
            {
                MessageBox.Show("Học sinh phải đảm bảo có tối thiểu 3 thông tin: Họ tên, Ngày sinh, Giới tính", "Thông báo");
                return;
            }
            if (string.IsNullOrEmpty(TB_SDT_p6.Text) ||
                Check_sdt.Checked == true)
            {

            }
            else
            {
                MessageBox.Show("Số điện thoại sai định dạng", "Thông báo");
                return;
            }
            if (CB_Lop_p6.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Lớp học", "Thông báo");
                return;
            }
            if (string.IsNullOrEmpty(TB_matkhau_p6.Text))
            {
                TB_matkhau_p6.Text = GlobalFunction.RandomString(6);
            }

            //Tạo tài khoản
            Student student = new Student(TB_MaHS_p6.Text);
            student.Lop = Admin.Func_Page6.ListLop[CB_Lop_p6.SelectedIndex].MaLop;
            student.HoTen = TB_HoTen_p6.Text; ;
            student.NgaySinh = dateEdit_NgaySinh_p6.Text;
            student.DiaChi = TB_DiaChi_p6.Text;
            student.GioiTinh = CB_Gioitinh_p6.SelectedItem.ToString();
            student.NienKhoa = CB_NienKhoa_p6.SelectedItem.ToString();
            student.SDT = TB_SDT_p6.Text;
            if (Admin.Func_Page6.ThemHocSinh(tb_Username_p6.Text, TB_matkhau_p6.Text, student, CB_NamHoc_p6.SelectedItem.ToString()))
            {
                MessageBox.Show("Đã lưu", "Thông báo!");

                var index = dataGridView_HSThem_p6.Rows.Add();
                dataGridView_HSThem_p6.Rows[index].Cells[0].Value = student.HoTen;
                dataGridView_HSThem_p6.Rows[index].Cells[1].Value = student.NgaySinh;
                dataGridView_HSThem_p6.Rows[index].Cells[2].Value = student.GioiTinh;
                dataGridView_HSThem_p6.Rows[index].Cells[3].Value = student.SDT;
                dataGridView_HSThem_p6.Rows[index].Cells[4].Value = student.DiaChi;
                dataGridView_HSThem_p6.Rows[index].Cells[5].Value = student.MaHS;
                dataGridView_HSThem_p6.Rows[index].Cells[6].Value = tb_Username_p6.Text;
                dataGridView_HSThem_p6.Rows[index].Cells[7].Value = TB_matkhau_p6.Text;


                CB_Gioitinh_p6.SelectedIndex = -1;
                TB_MaHS_p6.Text = "";
                TB_HoTen_p6.Text = "";
                dateEdit_NgaySinh_p6.Text = "";
                TB_DiaChi_p6.Text = "";
                TB_matkhau_p6.Text = "";
                tb_Username_p6.Text = "";
            }
            
            

        }

        private void CB_NamHoc_p6_Click(object sender, EventArgs e)
        {
            
            if (CB_NienKhoa_p6.SelectedIndex < 0)
            {
                MessageBox.Show("Chọn niên khóa trước", "Thông báo");
            }
            string nK = CB_NienKhoa_p6.SelectedItem.ToString();
            CB_NamHoc_p6.Items.Clear();
            string[] nam = nK.Split('-');
            int namBD = 0, namKT = 0;
            Int32.TryParse(nam[0], out namBD);
            Int32.TryParse(nam[1], out namKT);
            CB_NamHoc_p6.Items.Add(namBD.ToString() + "-" + (namBD + 1).ToString());
            CB_NamHoc_p6.Items.Add((namBD + 1).ToString() + "-" + (namBD + 2).ToString());
            CB_NamHoc_p6.Items.Add((namBD + 2).ToString() + "-" + (namBD + 3).ToString());
        }

        private void CB_Lop_p6_Click(object sender, EventArgs e)
        {
            CB_Lop_p6.Items.Clear();
            if (CB_NamHoc_p6.SelectedIndex < 0 && CB_Khoi_p6.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn năm học và khối!", "Thông báo");
            }
            Admin.Func_Page6.GetListLop(CB_Khoi_p6.Text, CB_NamHoc_p6.Text);
            
            foreach (var p in Admin.Func_Page6.ListLop)
            {
                CB_Lop_p6.Items.Add(p.TenLop);
            }
        }

        private void btn_delete_p6_Click(object sender, EventArgs e)
        {
            TB_HoTen_p6.Clear();
            dateEdit_NgaySinh_p6.Text = "";
            TB_DiaChi_p6.Clear();
            CB_Gioitinh_p6.Text = "";
            TB_SDT_p6.Clear();
            TB_MaHS_p6.Text = "";
            tb_Username_p6.Text = "";
            TB_matkhau_p6.Text = "";
        }

        private void CB_NienKhoa_p6_Click(object sender, EventArgs e)
        {
            CB_NienKhoa_p6.Items.Clear();
            string query = "SELECT MANK FROM NIENKHOA";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);

            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        CB_NienKhoa_p6.Items.Add(rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim());
                    }

                }
            }

        }

        bool CheckMaHS(string mahs)
        {
            string query = $"SELECT COUNT(*) FROM HOCSINH WHERE MAHS = '{mahs}'";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);

            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    rdr.Read();
                    int count = rdr.GetInt32(0);
                    if (count > 0)
                        return false;
                    else
                        return true;
                }
            }
            return false;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CB_NienKhoa_p6_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_NamHoc_p6.SelectedIndex = -1;
            CB_NamHoc_p6.Text = "";
            CB_NamHoc_p6.Items.Clear();
            CB_Lop_p6.SelectedIndex = -1;
            CB_Lop_p6.Text = "";
            CB_Lop_p6.Items.Clear();
        }

        private void CB_Khoi_p6_SelectedIndexChanged(object sender, EventArgs e)
        {

            CB_Lop_p6.SelectedIndex = -1;
            CB_Lop_p6.Text = "";
            CB_Lop_p6.Items.Clear();
        }

        private void TB_SDT_p6_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(TB_SDT_p6.Text, @"^\d+$"))
            {
                Check_sdt.Checked = false;
            }
            else
            {
                if (TB_SDT_p6.Text.Length == 10)
                {
                    Check_sdt.Checked = true;
                }
            }
        }


        //----------------page8-----------------------

        List<TaiKhoan> listTaiKhoan = new List<TaiKhoan>();
        private void btn_Hienthi_p8_Click(object sender, EventArgs e)
        {
            dataGridView_taikhoan_p8.Rows.Clear();
            listTaiKhoan.Clear();
            string query = "select HS.MAHS, HS.HotenHS,	HS.nienkhoa, L.TENLOP, TK.USERNAME, TK.PASS, HS.MATK from HOCSINH AS HS, LOP AS L, TAIKHOAN AS TK WHERE HS.MALOP = L.MALOP AND HS.MATK = TK.MATK";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);

            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        listTaiKhoan.Add(new TaiKhoan());
                        var idx = dataGridView_taikhoan_p8.Rows.Add();
                        dataGridView_taikhoan_p8.Rows[idx].Cells[0].Value = rdr.IsDBNull(0) ? GlobalProperties.NULLFIELD : rdr.GetString(0).Trim();
                        dataGridView_taikhoan_p8.Rows[idx].Cells[1].Value = rdr.IsDBNull(1) ? GlobalProperties.NULLFIELD : rdr.GetString(1).Trim();
                        dataGridView_taikhoan_p8.Rows[idx].Cells[2].Value = rdr.IsDBNull(2) ? GlobalProperties.NULLFIELD : rdr.GetString(2).Trim();
                        dataGridView_taikhoan_p8.Rows[idx].Cells[3].Value = rdr.IsDBNull(3) ? GlobalProperties.NULLFIELD : rdr.GetString(3).Trim();
                        dataGridView_taikhoan_p8.Rows[idx].Cells[4].Value = rdr.IsDBNull(4) ? GlobalProperties.NULLFIELD : rdr.GetString(4).Trim();
                        dataGridView_taikhoan_p8.Rows[idx].Cells[5].Value = rdr.IsDBNull(5) ? GlobalProperties.NULLFIELD : rdr.GetString(5).Trim();
                        listTaiKhoan[listTaiKhoan.Count - 1].MaTK = rdr.IsDBNull(6) ? GlobalProperties.NULLFIELD : rdr.GetString(6).Trim();
                        listTaiKhoan[listTaiKhoan.Count - 1].Password = rdr.IsDBNull(5) ? GlobalProperties.NULLFIELD : rdr.GetString(5).Trim();
                    }

                }
            }
        }

        private void materialRaisedButton5_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < listTaiKhoan.Count; i++)
            {
                dataGridView_taikhoan_p8.Rows[i].Cells[5].Value = listTaiKhoan[i].Password;
            }
        }

        private void materialRaisedButton6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listTaiKhoan.Count; i++)
            {
                string query = $"UPDATE TAIKHOAN SET PASS = '{dataGridView_taikhoan_p8.Rows[i].Cells[5].Value.ToString()}' WHERE MATK = '{listTaiKhoan[i].MaTK}'";
                listTaiKhoan[i].Password = dataGridView_taikhoan_p8.Rows[i].Cells[5].Value.ToString();
                SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
                int rowCount = cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Đã lưu");
        }


        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tính năng đang phát triển!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_thaymtAdmin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mtCu_p8.text))
            {
                MessageBox.Show("Nhập mật khẩu cũ!");
                return;
            }
            if (string.IsNullOrEmpty(mkmoi_p8.text))
            {
                MessageBox.Show("Nhập mật khẩu mới!");
                return;
            }
            if (mtk_p8.text.Trim() != mkmoi_p8.text.Trim())
            {
                MessageBox.Show("Xác nhận mật khẩu mới không trùng khớp");
                return;
            }
            bool check = false;
            string query = $"SELECT PASS FROM TAIKHOAN WHERE USERNAME = 'admin'";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    rdr.Read();
                    string pass = rdr.GetString(0);
                    if (pass.Trim() != mtCu_p8.text.Trim())
                    {
                        MessageBox.Show("Mật khẩu cũ không đúng!");
                        return;
                    }
                    MessageBox.Show("ok");
                    check = true;
                }
            }
            if (check)
            {
                query = $"UPDATE TAIKHOAN SET PASS = '{mkmoi_p8.text.Trim()}' WHERE USERNAME = 'admin'";
                cmd = new SqlCommand(query, GlobalProperties.conn);
                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                {
                    MessageBox.Show("Đã lưu");
                    return;
                }

            }
        }

        private void bunifuCheckBox1_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if (bunifuCheckBox1.Checked == true)
            {
                File.WriteAllText("./StudentEdit", "1");
            }
            else
            {
                File.WriteAllText("./StudentEdit", "0");
            }
        }

        private void bunifuTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            string text = bunifuTextBox1.Text;
            if (string.IsNullOrEmpty(text))
            {
                for (int i = 0; i < dataGridView_taikhoan_p8.RowCount; i++)
                {
                    dataGridView_taikhoan_p8.Rows[i].Visible = true;
                }
            }
            for (int i = 0; i < dataGridView_taikhoan_p8.RowCount; i++)
            {
                string mahs = dataGridView_taikhoan_p8.Rows[i].Cells[0].Value.ToString();
                string tenhs = dataGridView_taikhoan_p8.Rows[i].Cells[1].Value.ToString();
                if (!mahs.Contains(text) && !tenhs.Contains(text))
                {
                    dataGridView_taikhoan_p8.Rows[i].Visible = false;
                }
                else
                {
                    dataGridView_taikhoan_p8.Rows[i].Visible = true;
                }
            }
        }
    }
}
