using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Classes
{
    internal class Admin_Funcs
    {
        public Admin_Func_Page1 Func_Page1;

        public Admin_Funcs() 
        {
           Func_Page1= new Admin_Func_Page1();
        }

        public List<string> GetNamHoc()
        {
            Dictionary<string, int>  dicNH = new Dictionary<string, int>();
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
            List<Lop>  listLop = new List<Lop>();
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
    }

    internal class Admin_Func_Page1 : Admin_Funcs
    {
        private string curNamHoc;
        private string curMaLop;
        private string curHK;
        private List<string> listNH;
        private List<Lop> listLop;
        private string curKhoi;

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


        //List<DiemHocSinh> listHocSinh_page1 = new List<DiemHocSinh>();
        
        public Admin_Func_Page1()
        {
            curMaLop = "";
            curNamHoc = "";
            curHK = "";
            listNH = new List<string>();
            listLop= new List<Lop>();
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
            get { return (curKhoi == "K10" ? 0 : (curKhoi == "K11" ? 1 : 2)); }
        }
        public void GetListNamHoc()
        {
            listNH = this.GetNamHoc();
        }

        public void GetListLop() 
        {
            listLop = this.GetMaLop(curKhoi, curNamHoc);
        }

        public void GetListLop(string _khoi, string _namhoc)
        {
            this.curKhoi= _khoi;
            this.curNamHoc= _namhoc;
            listLop = this.GetMaLop(curKhoi, curNamHoc);
        }





    }

}
