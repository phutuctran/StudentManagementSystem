using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using StudentManagementSystem.DatabaseCore;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Spreadsheet;

namespace StudentManagementSystem.Classes
{
    public class HanhKiem
    {
        private string maHK;
        private string maHS;
        private string xepLoaiHK1;
        private string xepLoaiHK2;
        private string xepLoaiCN;
        private string namHoc;

        public string NamHoc
        {
            get { return namHoc; }
            set { namHoc = value; }
        }

        public string MaHK { get => maHK; set => maHK = value; }
        public string MaHS { get => maHS; set => maHS = value; }
        public string XepLoaiHK1 { get => xepLoaiHK1; set => xepLoaiHK1 = value; }
        public string XepLoaiHK2 { get => xepLoaiHK2; set => xepLoaiHK2 = value; }
        public string XepLoaiCN { get => xepLoaiCN; set => xepLoaiCN = value; }
        public HanhKiem() { }
        public HanhKiem(string maHK, string maHS, string namHoc, string xepLoaiHK1, string xepLoaiHK2, string xepLoaiCN)
        {
            this.maHK = maHK;
            this.maHS = maHS;
            this.namHoc = namHoc;
            this.xepLoaiHK1 = xepLoaiHK1;
            this.xepLoaiHK2 = xepLoaiHK2;
            this.xepLoaiCN = xepLoaiCN;
        }

        public HanhKiem(string _maHK)
        {
            string query = @"SELECT MAHS, NAMHOC, XEPLOAIHKI, XEPLOAIHKII, XEPLOAICN
                                FROM HANHKIEM" +
                            $" WHERE HANHKIEM.MAHK = '{_maHK}'";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        this.maHS = rdr.IsDBNull(0) ? "" : rdr.GetString(0);
                        this.namHoc = rdr.IsDBNull(1) ? "" : rdr.GetString(1);
                        this.xepLoaiHK1 = rdr.IsDBNull(2) ? "" : rdr.GetString(2);
                        this.xepLoaiHK2 = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                        this.XepLoaiCN = rdr.IsDBNull(4) ? "" : rdr.GetString(4);
                    }
                }
                else
                {
                    this.maHS = "";
                    this.namHoc = "";
                    this.xepLoaiHK1 = "";
                    this.xepLoaiHK2 = "";
                    this.XepLoaiCN = "";
                }
            }
        }

        public HanhKiem(string _maHS, string _namHoc)
        {
            string query = @"SELECT MAHS, NAMHOC, XEPLOAIHKI, XEPLOAIHKII, XEPLOAICN, MAHK
                                FROM HANHKIEM" +
                            $" WHERE HANHKIEM.MAHS = '{_maHS}' AND HANHKIEM.NAMHOC = '{_namHoc}'";
            SqlCommand cmd = new SqlCommand(query, GlobalProperties.conn);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        this.maHS = rdr.IsDBNull(0) ? "" : rdr.GetString(0);
                        this.namHoc = rdr.IsDBNull(1) ? "" : rdr.GetString(1);
                        this.xepLoaiHK1 = rdr.IsDBNull(2) ? "" : rdr.GetString(2);
                        this.xepLoaiHK2 = rdr.IsDBNull(3) ? "" : rdr.GetString(3);
                        this.XepLoaiCN = rdr.IsDBNull(4) ? "" : rdr.GetString(4);
                        this.maHK = rdr.IsDBNull(5) ? "" : rdr.GetString(5);
                    }
                }
                else
                {
                    this.maHK = "";
                    this.maHS = "";
                    this.namHoc = "";
                    this.xepLoaiHK1 = "";
                    this.xepLoaiHK2 = "";
                    this.XepLoaiCN = "";
                }
            }
        }

        public bool Insert(string maHKiem, string _mahs, string xlhk1, string xlhk2, string xlhkcn, string curNamHoc)
        {


            try
            {
                // Câu lệnh Insert.
                string query = $"INSERT INTO HANHKIEM(MAHK, MAHS, XEPLOAIHKI, XEPLOAIHKII, XEPLOAICN, NAMHOC) VALUES('{maHKiem}', '{_mahs}', N'{xlhk1}', N'{xlhk2}', N'{xlhkcn}', '{curNamHoc}')";

                var cmd = new SqlCommand(query, GlobalProperties.conn);

                int rowCount = cmd.ExecuteNonQuery();
                if (rowCount > 0)
                {
                    return true;
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
            return false;
        }

        public bool SaveHKII(string xepLoaiHK2)
        {
            string query = @"UPDATE HANHKIEM" +
                                    $" SET XEPLOAIHKII = N'{xepLoaiHK2}'" +
                                    $"WHERE MAHK = '{this.maHK}'";

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
        public bool SaveHKI(string xepLoaiHK1)
        {
            string query = @"UPDATE HANHKIEM" +
                                    $" SET XEPLOAIHKI = N'{xepLoaiHK1}'" +
                                    $"WHERE MAHK = '{this.maHK}'";

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

        public bool Save(string xepLoaiHK1, string xepLoaiHK2, string xepLoaiCN)
        {
            string query = @"UPDATE HANHKIEM" +
                                    $" SET XEPLOAIHKI = N'{xepLoaiHK1}', XEPLOAIHKII = N'{XepLoaiHK2}', XEPLOAICN = N'{XepLoaiCN}'" +
                                    $"WHERE MAHK = '{this.maHK}'";
      
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

        public bool SaveHanhKiemStatic(string maHS, string xepLoaiHK1, string xepLoaiHK2, string xepLoaiCN)
        {
            string query = @"UPDATE HANHKIEM" +
                                    $" SET XEPLOAIHKI = N'{xepLoaiHK1}', XEPLOAIHKII = N'{XepLoaiHK2}', XEPLOAICN = N'{XepLoaiCN}'" +
                                    $"WHERE MAHK = '{this.maHK}'";

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

        public static List<HanhKiem> ReadCSVFile(string fileName)
        {
            List<HanhKiem> hkList = new List<HanhKiem>();
            string[] lines = System.IO.File.ReadAllLines(fileName);
            int linesLength = lines.Length;
            for (int i = 0; i < linesLength; i++)
            {
                string[] columns = lines[i].Split(',');
                HanhKiem hk = new HanhKiem(columns[0], columns[1], columns[2], columns[3], columns[4], columns[5]);
                hkList.Add(hk);
            }
            return hkList;
        }

        public static void InsertToDB(List<HanhKiem> list)
        {
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();

            try
            {
                string sql = "INSERT into HANHKIEM VALUES (@maHK, @maHS, @xepLoaiHK1, @xepLoaiHK2, @xepLoaiCN)";

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                for (int i = 0; i < list.Count(); i++)
                {
                    cmd.Parameters.Add("@maHK", SqlDbType.Char).Value = list[i].maHK;
                    cmd.Parameters.Add("@maHS", SqlDbType.Char).Value = list[i].maHS;
                    cmd.Parameters.Add("@xepLoaiHK1", SqlDbType.NVarChar).Value = list[i].xepLoaiHK1;
                    cmd.Parameters.Add("@xepLoaiHK2", SqlDbType.NVarChar).Value = list[i].xepLoaiHK2;
                    cmd.Parameters.Add("@xepLoaiCN", SqlDbType.NVarChar).Value = list[i].xepLoaiCN;
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
