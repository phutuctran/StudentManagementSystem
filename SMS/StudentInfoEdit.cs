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
            GetDataStudent();
            ShowDataPage1();
            if (EditDiem == false)
            {
                btn_hoantacpag2.Visible = false;
                btn_Savepage2.Visible = false;
            }

        }

        private void StudentInfoEdit_Load(object sender, EventArgs e)
        {
            bt_ResetPag1.AutoSize = false;
            bt_ResetPag1.Size = new Size(86, 36);
            bt_SavePage1.AutoSize = false;
            bt_SavePage1.Size = new Size(86, 36);
            btn_Savepage2.AutoSize= false;
            btn_Savepage2.Size = new Size(86, 36);
            btn_in.AutoSize = false;
            btn_in.Size = new Size(86, 36);
            btn_Savepage2.AutoSize= false;
            btn_Savepage2.Size = new Size(86, 36);
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
            ShowBangDiemPage2();
        }

        private void btn_Savepage2_Click(object sender, EventArgs e)
        {
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

            TinhDTB();
            //string sqlDiemTB = "";
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
            if (student.SaveDiemStudent(bangDiemNew, hocKi, namHoc))
            {
                ShowBangDiemPage2();
                MessageBox.Show("Đã lưu!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Có lỗi, không thể hoàn tất lưu!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            dataGridView_Diem.Rows[GlobalProperties.soMonHoc].Cells[8].Value = diemTrBHK != -1 ? diemTrBHK.ToString() : "";
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
            ShowBangDiemPage2();

        }//done

        void ShowBangDiemPage2()
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
            DataGridViewRow _row = (DataGridViewRow)dataGridView_Diem.Rows[0].Clone();

            _row.Cells[7].Value = "TrBHK:";
            _row.Cells[8].Value = diemTrBHK != -1 ? diemTrBHK.ToString() : GlobalProperties.NULLFIELD;
            dataGridView_Diem.Rows.Add(_row);
            dataGridView_Diem.Rows[GlobalProperties.soMonHoc].ReadOnly = true;

            dataGridView_Diem.AllowUserToAddRows = false;
            dataGridView_Diem.AllowUserToDeleteRows = false;
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


    }
}
