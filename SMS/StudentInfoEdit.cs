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
using StudentManagementSystem.Classes;
using CSharpVitamins;
using System.IO;
using System.Drawing.Imaging;
using StudentManagementSystem.Properties;
namespace StudentManagementSystem
{
    public partial class StudentInfoEdit : MaterialForm
    {

        string maHS;
        string namHoc = "";
        string hocKi = "";
        double diemTrBHK;
        Student student;
        bool editDiem;
        private bool haveDatainGird;

        public bool haveBangDiem_p3 { get; private set; }

        public StudentInfoEdit(string _MaHS, bool EditDiem = true) //_MaHS phải luôn tồn tại
        {
            InitializeComponent();
            //PB_Avatar.Image = Resources.rm422_047;
            maHS = _MaHS;
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            editDiem = EditDiem;
            student = new Student(maHS);
            haveDatainGird = false;
            GetDataStudent();
            ShowDataPage1();
            if (EditDiem == false)
            {
                btn_hoantacpag2.Visible = false;
                btn_Savepage2.Visible = false;
                lB_HoanTac_page2.Visible = false;
                LB_Save_page2.Visible = false;
                dataGridView_Diem.ReadOnly = true; 
            }

        }

        private void StudentInfoEdit_Load(object sender, EventArgs e)
        {
           
            tabPage2.BackColor = Color.Honeydew;
            dataGridView_Diem.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
            dataGridView_Diem.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
            

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

        }//done

        //Xem thông tin bảng điểm
        private void btn_xem_Click(object sender, EventArgs e)
        {
            diemTrBHK = 0;
            int idx = CB_ttHK_NH.SelectedIndex;
            if (idx < 0)
            {
                MessageBox.Show("Hãy chọn thông tin cần xem", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                haveDatainGird = false;
                return;
            }
            namHoc = student.TTBangDiem[idx].namHoc;
            hocKi = student.TTBangDiem[idx].HK;
            //MessageBox.Show(hocKi + "---" + namHoc);
            GetandShowBangDiem(hocKi, namHoc);
        }//done

        private void bt_ResetPag1_Click(object sender, EventArgs e)
        {
            ShowDataPage1();
        }//done

        private void bt_SavePage1_Click(object sender, EventArgs e)
        {
            if (student.SaveDataStudent(student.MaHS, TB_HoTen.Text, datepicker_hs.Text, TB_DiaChi.Text, CB_Sex.Text, student.NienKhoa, TB_DanToc.Text, TB_TonGiao.Text, TB_TenCha.Text, TB_NgheCha.Text, Datepicker_cha.Text, TB_TenMe.Text, TB_NgheMe.Text, Datepicker_me.Text, student.Lop, TB_GhiChu.Text, TB_NoiSinh.Text, TB_Sodt.Text, TB_Email.Text, PB_Avatar.Image))
            {
                MessageBox.Show("Đã lưu!", "Thông báo!");
                ShowDataPage1();
            }
        }//done

        //Hiển thị chi tiết thông tin bảng điểm
        private void btn_hoantacpag2_Click(object sender, EventArgs e)//done
        {
            ShowBangDiemPage2(student.TTBangDiem[CB_ttHK_NH.SelectedIndex].HK);
        }

        private void btn_Savepage2_Click(object sender, EventArgs e)
        {
            if (!haveDatainGird)
            {
                MessageBox.Show("Chưa có thông tin!", "Thông báo!");
                return;
            }
            for (int i = 0; i < GlobalProperties.soMonHoc; i++)
            {
                for (int j = 2; j <= 8; j++)
                {

                    string diem = dataGridView_Diem.Rows[i].Cells[j].Value == null ? string.Empty : dataGridView_Diem.Rows[i].Cells[j].Value.ToString();
                    //MessageBox.Show(diem);
                    if (string.IsNullOrEmpty(diem.Trim()))
                    {
                        continue;
                    }
                    double diemthuc = GlobalFunction.CheckDiem(diem.Trim());
                    if (diemthuc == -1 && i != 11)
                    {
                        MessageBox.Show($"Điểm nhập không hợp lệ ở môn {GlobalProperties.listTenMH[i]}", "Thông Báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (i == 11)
                    {
                        diem = diem.convertToUnSign().ToUpper();
                        if (Array.IndexOf(GlobalProperties.listDat, diem) != -1)
                        {
                            continue;
                        }
                        if (Array.IndexOf(GlobalProperties.listChuaDat, diem) != -1)
                        {
                            continue;
                        }
                        MessageBox.Show($"Điểm nhập không hợp lệ ở môn {GlobalProperties.listTenMH[i]}", "Thông Báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                        
                }
            }
            int CP = TB_cophep_p2.Text.ConvertStringToNeInt();
            int KP = TB_KhongPhep_p2.Text.ConvertStringToNeInt();
            int VP = TB_ViPham_p2.Text.ConvertStringToNeInt();
            if (CP == -1)
            {
                MessageBox.Show($"Ngày nghỉ có phép không hợp lệ", "Thông Báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (KP == -1)
            {
                MessageBox.Show($"Ngày nghỉ không phép không hợp lệ", "Thông Báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (VP == -1)
            {
                MessageBox.Show($"Số vi phạm không hợp lệ", "Thông Báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double[,] bangDiemNew = new double[13, 7];
            for (int i = 0; i < GlobalProperties.soMonHoc; i++)
            {
                for (int j = 2; j <= 8; j++)
                {
                    string _diem = dataGridView_Diem.Rows[i].Cells[j].Value == null ? string.Empty : dataGridView_Diem.Rows[i].Cells[j].Value.ToString().convertToUnSign().ToUpper();
                    if (i == 11)
                    {
                        if (Array.IndexOf(GlobalProperties.listDat, _diem) != -1)
                        {
                            _diem = "10";
                        }
                        if (Array.IndexOf(GlobalProperties.listChuaDat, _diem) != -1)
                        {
                            _diem = "0";
                        }
                    }
                    bangDiemNew[i, j - 2] = GlobalFunction.CheckDiem(_diem.Trim());
                }
            }
            bool saveDone = true;
            if (!student.SaveDiemStudent(bangDiemNew, hocKi, namHoc))
            {
                MessageBox.Show("Không thể hoàn tất toàn bộ quá trình lưu!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                saveDone = false;
            }
            if (CB_RenLuyen_p2.SelectedIndex != -1)
            {
                if (!student.SaveHanhKiem(hocKi, CB_RenLuyen_p2.SelectedItem.ToString()))
                {
                    MessageBox.Show("Không thể hoàn tất toàn bộ quá trình lưu!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    saveDone = false;
                }
            }
            if (!student.SaveViPham(hocKi, CP, KP, VP))
            {
                MessageBox.Show("Không thể hoàn tất toàn bộ quá trình lưu!-", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                saveDone = false;
            }
            if (saveDone)
            {
                MessageBox.Show("Đã lưu!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowBangDiemPage2(hocKi);
            }

        }

        private void TinhDTB()
        {
            if (dataGridView_Diem.RowCount < 1)
            {
                //MessageBox.Show("Vui lòng chọn bảng điểm!", "Thông báo");
                return;
            }
            for (int i = 0; i < GlobalProperties.soMonHoc; i++)
            {
                for (int j = 2; j <= 7; j++)
                {
                    string diem = dataGridView_Diem.Rows[i].Cells[j].Value == null ? string.Empty : dataGridView_Diem.Rows[i].Cells[j].Value.ToString();
                    if (string.IsNullOrEmpty(diem.Trim()))
                    {
                        continue;
                    }
                    double diemthuc = GlobalFunction.CheckDiem(diem.Trim());
                    if (diemthuc == -1 && i != 11)
                    {
                        //MessageBox.Show($"Điểm nhập không hợp lệ ở môn {GlobalProperties.listTenMH[i]}", "Thông Báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (i == 11)
                    {
                        diem = diem.convertToUnSign().ToUpper();
                        if (Array.IndexOf(GlobalProperties.listDat, diem) == -1 && Array.IndexOf(GlobalProperties.listChuaDat, diem) == -1)
                        {
                            //MessageBox.Show($"Điểm nhập không hợp lệ ở môn {GlobalProperties.listTenMH[i]}", "Thông Báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            double TrbHK = 0;
            int soMonHienTai = 0;
            int tongCotDiem = 0;
            int[] heSo = { 1, 1, 1, 1, 2, 3 };
            for (int i = 0; i < GlobalProperties.soMonHoc; i++)
            {
                if (i == 11)
                {
                    continue;
                }
                int tongHeSo = 0;
                double tongDiem = 0;
                tongCotDiem = 0;
                for (int j = 2; j <= 7; j++)
                {
                    string diem = dataGridView_Diem.Rows[i].Cells[j].Value == null ? string.Empty : dataGridView_Diem.Rows[i].Cells[j].Value.ToString();
                    if (string.IsNullOrEmpty(diem.Trim()))
                    {
                        continue;
                    }
                    double diemthuc = GlobalFunction.CheckDiem(diem.Trim());
                    tongCotDiem = (j >= 2 && j <= 5) ? 1 : tongCotDiem;
                    tongCotDiem += (j > 5) ? heSo[j - 2] : 0;
                    tongDiem += diemthuc * heSo[j - 2];
                    tongHeSo += heSo[j - 2];
                }
                if (tongCotDiem == 6)
                {
                    double tmp = Math.Round(tongDiem / tongHeSo, 1);
                    dataGridView_Diem.Rows[i].Cells[8].Value = tmp.ToString();
                    if (i != 11)
                    {
                        TrbHK += tmp;
                        soMonHienTai++;
                    }
                }
                else
                {
                    dataGridView_Diem.Rows[i].Cells[8].Value = "";
                }
            }
            diemTrBHK = Math.Round(TrbHK / soMonHienTai, 1);
            //dataGridView_Diem.Rows[GlobalProperties.soMonHoc].Cells[8].Value = diemTrBHK != -1 ? diemTrBHK.ToString() : "";
            /////////////////////
            tongCotDiem = 0;
            for (int j = 2; j <= 7; j++)
            {

                string diem = dataGridView_Diem.Rows[11].Cells[j].Value == null ? string.Empty : dataGridView_Diem.Rows[11].Cells[j].Value.ToString().convertToUnSign().ToUpper();
                if (string.IsNullOrEmpty(diem.Trim()))
                {
                    continue;
                }
                if (Array.IndexOf(GlobalProperties.listDat, diem) != -1)
                {
                    tongCotDiem = (j >= 2 && j <= 5) ? 1 : tongCotDiem;
                    tongCotDiem += (j > 5) ? heSo[j - 2] : 0;
                    continue;
                }
                if (Array.IndexOf(GlobalProperties.listChuaDat, diem) != -1)
                {
                    dataGridView_Diem.Rows[11].Cells[8].Value = "CĐ";
                    return;
                }
            }    
            if (tongCotDiem == 6)
            {
                dataGridView_Diem.Rows[11].Cells[8].Value = "Đ";
            }
            else
            {
                dataGridView_Diem.Rows[11].Cells[8].Value = "CĐ";
            }
        }

        void GetandShowBangDiem(string HK, string NamHoc)
        {
            if (!student.GetThongTinBangDiem(HK, namHoc) && !editDiem)
            {
                MessageBox.Show("Không thể lấy thông tin bảng điểm hoặc chưa có điểm!!!", "Thông báo!");
                return;
            }
            if (!student.GetTongKetHocKi(HK, NamHoc))
            {
                return;
            }
           
            ShowBangDiemPage2(HK);
           
            haveDatainGird = true;

        }//done

        void ShowBangDiemPage2(string HK)
        {
            dataGridView_Diem.Rows.Clear();
            int stt = 0;
            dataGridView_Diem.AllowUserToAddRows = true;
            dataGridView_Diem.AllowUserToDeleteRows = true;
            dataGridView_Diem.Rows.Clear();
            for (int i = 0; i < GlobalProperties.soMonHoc; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView_Diem.Rows[0].Clone();
                row.Cells[0].Value = (++stt).ToString();//Số thứ tự
                if (i == 11)
                {
                    
                    row.Cells[1].Value = student.DSDiem[i].TenMH;
                    row.Cells[2].Value = student.DSDiem[i].DDGTX1.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX1.diem < 5 ? "CĐ" : "Đ";
                    row.Cells[3].Value = student.DSDiem[i].DDGTX2.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX2.diem < 5 ? "CĐ" : "Đ";
                    row.Cells[4].Value = student.DSDiem[i].DDGTX3.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX3.diem < 5 ? "CĐ" : "Đ";
                    row.Cells[5].Value = student.DSDiem[i].DDGTX4.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX4.diem < 5 ? "CĐ" : "Đ";
                    row.Cells[6].Value = student.DSDiem[i].DDGGK.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGGK.diem < 5 ? "CĐ" : "Đ";
                    row.Cells[7].Value = student.DSDiem[i].DDGCK.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGCK.diem < 5 ? "CĐ" : "Đ";
                    row.Cells[8].Value = student.DSDiem[i].DDGTRB.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTRB.diem < 5 ? "CĐ" : "Đ";
                    dataGridView_Diem.Rows.Add(row);
                    continue;

                }
                row.Cells[1].Value = student.DSDiem[i].TenMH;
                row.Cells[2].Value = student.DSDiem[i].DDGTX1.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX1.diem.ToString();
                row.Cells[3].Value = student.DSDiem[i].DDGTX2.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX2.diem.ToString();
                row.Cells[4].Value = student.DSDiem[i].DDGTX3.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX3.diem.ToString();
                row.Cells[5].Value = student.DSDiem[i].DDGTX4.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTX4.diem.ToString();
                row.Cells[6].Value = student.DSDiem[i].DDGGK.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGGK.diem.ToString();
                row.Cells[7].Value = student.DSDiem[i].DDGCK.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGCK.diem.ToString();
                row.Cells[8].Value = student.DSDiem[i].DDGTRB.diem == -1 ? GlobalProperties.NULLFIELD : student.DSDiem[i].DDGTRB.diem.ToString();
                dataGridView_Diem.Rows.Add(row);
            }
            // Show diem TrB
           // DataGridViewRow _row = (DataGridViewRow)dataGridView_Diem.Rows[0].Clone();

            //_row.Cells[7].Value = "TrBHK:";
            //_row.Cells[8].Value = diemTrBHK != -1 ? diemTrBHK.ToString() : GlobalProperties.NULLFIELD;
            //dataGridView_Diem.Rows.Add(_row);
            //dataGridView_Diem.Rows[GlobalProperties.soMonHoc].ReadOnly = true;

            dataGridView_Diem.AllowUserToAddRows = false;
            dataGridView_Diem.AllowUserToDeleteRows = false;

            if (HK == "HK1")
            {
                LB_HocTap_p2.Text = student.DiemTKHKI.xepLoai.ToString();
                CB_RenLuyen_p2.SelectedIndex = GlobalFunction.GetLoaiHanhKiem(student.DiemTKHKI.hanhKiem.ToString());
                TB_cophep_p2.Text = student.BangViPham.NghiCoPhepHKI.ToString();
                TB_KhongPhep_p2.Text = student.BangViPham.NghiKhongPhepHKI.ToString();
                TB_ViPham_p2.Text = student.BangViPham.ViPhamHKI.ToString();
            }
            else
            {
                LB_HocTap_p2.Text = student.DiemTKHKII.xepLoai.ToString();
                CB_RenLuyen_p2.SelectedIndex = GlobalFunction.GetLoaiHanhKiem(student.DiemTKHKII.hanhKiem.ToString());
                TB_cophep_p2.Text = student.BangViPham.NghiCoPhepHKII.ToString();
                TB_KhongPhep_p2.Text = student.BangViPham.NghiKhongPhepHKII.ToString();
                TB_ViPham_p2.Text = student.BangViPham.ViPhamHKII.ToString();
            }
        }//done

        void ShowDataPage1()
        {
            TB_MaHS.Text = student.MaHS;
            TB_HoTen.Text = student.HoTen;
            datepicker_hs.Text = student.NgaySinh;
            TB_DiaChi.Text = student.DiaChi;
            CB_Sex.Text = student.GioiTinh;
            CB_NienKhoa.Text = student.NienKhoa;
            TB_DanToc.Text = student.DanToc;
            TB_TonGiao.Text = student.TonGiao;
            TB_TenCha.Text = student.TenCha;
            TB_NgheCha.Text = student.NgheNghiepCha;
            Datepicker_cha.Text = student.NgaySinhCha;
            TB_TenMe.Text = student.TenMe;
            TB_NgheMe.Text = student.NgheNghiepMe;
            Datepicker_me.Text = student.NgaySinhMe;
            TB_GhiChu.Text = student.GhiChu;
            CB_Lop.Text = student.Lop;
            TB_NoiSinh.Text = student.NoiSinh;
            TB_Sodt.Text = student.SDT;
            TB_Email.Text = student.Email;
            
            if (student.AnhHS != null)
            {
                Image img = PB_Avatar.Image;
                PB_Avatar.Image = new Bitmap(student.AnhHS);
                if (img != null)
                {
                    img.Dispose();
                }
            }
        }//done

        void GetDataStudent()
        {
            if (!student.GetDataStudent())
            {
                MessageBox.Show("Không thể lấy thông tin học sinh!!!", "Thông báo!");
                this.Close();
                return;
            }

            if (student.TTBangDiem.Count > 0)
            {
                CB_ttHK_NH.Items.Clear();
                foreach ((string hk, string namHoc) p in student.TTBangDiem)
                {
                    CB_ttHK_NH.Items.Add(p.hk + ", " + p.namHoc);
                    if (CB_ChonNamHoctab3.Items.IndexOf(p.namHoc) == -1)
                    {
                        CB_ChonNamHoctab3.Items.Add(p.namHoc);
                    }
                }
            }
        }//done

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Filter = "PNG, JPG|*.png;*.jpg";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                Image img = PB_Avatar.Image;
                PB_Avatar.Image = new Bitmap(selectedFileName);
                if (img != null)
                {
                    img.Dispose();
                }
            }
        }

        private void dataGridView_Diem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TinhDTB();
        }

        private void materialTabSelector1_Click(object sender, EventArgs e)
        {

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
                    row[4] = cn == -1 ? GlobalProperties.NULLFIELD : cn.ToString();
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
            if (!haveDatainGird)
            {
                return;
            }
            string[,] saveValue = new string[13, 7];
            for (int i = 0; i < 13; i++)
                for (int j = 0; j < 7; j++)
                {
                    saveValue[i, j] = dataGridView_Diem.Rows[i].Cells[j + 2].Value.ToString();
                }
            MakeReport rp = new MakeReport(GlobalProperties.BANGDIEMHSTEMPLATEPATH, new Point(13, 3), saveValue);
            int idx = CB_ttHK_NH.SelectedIndex;
            List<(string value, Point location)> otherValue = new List<(string value, Point location)>();
            otherValue.Add((student.HoTen, new Point(6, 3)));
            otherValue.Add((student.MaHS, new Point(6, 8)));
            otherValue.Add((student.Lop, new Point(7, 3)));
            otherValue.Add((student.TTBangDiem[idx].HK, new Point(7, 5)));
            otherValue.Add((student.TTBangDiem[idx].namHoc, new Point(7, 8)));
            otherValue.Add((LB_HocTap_p2.Text, new Point(9, 3)));
            otherValue.Add((CB_RenLuyen_p2.Text, new Point(9, 8)));
            otherValue.Add((TB_cophep_p2.Text, new Point(10, 3)));
            otherValue.Add((TB_KhongPhep_p2.Text, new Point(10, 6)));
            otherValue.Add((TB_ViPham_p2.Text, new Point(10, 8)));
            rp.OrtherValue = otherValue;
            rp.GetSavePathWithSaveFileDialog();
            rp.OverwritetoExcelFile();
            rp.OpenExcelFile();
        }

        private void pictureBox1_Click(object sender, EventArgs e) //Xuaatr bảng điểm cả năm
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
