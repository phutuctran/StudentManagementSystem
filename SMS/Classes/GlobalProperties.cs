using StudentManagementSystem.DatabaseCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace StudentManagementSystem
{
    public static class GlobalProperties
    {
        public static string curUsername;
        public static string curUserPassword;
        public static bool isConnectDatabase = false;
        public static string serverBDNamepath = "./curServerDBName";
        public static string DATEFORMAT = "dd/MM/yyyy";
        public static SqlConnection conn;
        public static string NULLFIELD = "";
        public static string[] listMaLoaiKT = { "DTX1", "DTX2", "DTX3", "DTX4", "DGK", "DCK"};
        public static string[] listMaMH = { "MHT", "MHV", "MHVL", "MHHH", "MHSH", "MHTH", "MHLS", "MHDL", "MHNN", "MHCD", "MHCN", "MHTD", "MHQP" };
        public static string[] listTenMH = { "Toán học", "Ngữ văn", "Vật lí", "Hóa học", "Sinh học", "Tin học", "Lịch sử", "Địa lí", "Ngoại ngữ", "GDCD", "Công nghệ", "Thể dục", "GDQP" };
        public static int soMonHoc = 13;
        public static string[] listDat = { "D", "DAT" };
        public static string[] listChuaDat = { "C", "CD", "C D", "CHUA", "CHUADAT", "CHUADAT" };
        public static Point LOCATIONWRITEBANGDIEMHS = new Point(11, 3);
        public static string BANGDIEMHSTEMPLATEPATH = System.IO.Directory.GetCurrentDirectory() +  @"\TemplatesExcels\BangDiemHocSinh.xlsx";
        public static string BANGDIEMHSTONGKETTEMPLATEPATH = System.IO.Directory.GetCurrentDirectory() + @"\TemplatesExcels\BangDiemHocSinh_TongKet.xlsx";
    }

    public static class GlobalFunction
    {
        public static int GetLoaiHanhKiem(string hk)// 0: Tôt, 1: Khá, 2: Trung bình, 3: yếu
        {
            switch (hk)
            {
                case "Tốt":
                    return 0;
                case "Khá":
                    return 1;
                case "Đạt":
                    return 2;
                case "Chưa đạt":
                    return 3;
                default:
                    return -1;
            }
        }

        public static string GetTenHanhKiem(int hk)// 0: Tôt, 1: Khá, 2: Trung bình, 3: yếu
        {
            switch (hk)
            {
                case 0:
                    return "Tốt";
                case 1:
                    return "Khá";
                case 2:
                    return "Đạt";
                case 3:
                    return "Chưa đạt";
                default:
                    return "";
            }
        }
        public static byte[] ImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }


        public static Image ToImage(byte[] data) 
        {
            if (data == null)
            {
                return null;
            }
            Image img;
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (Image temp = Image.FromStream(stream))
                {
                    img = new Bitmap(temp);
                }
            }
            return img;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqstuvwyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomStringInt(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //Check diem 0 --> 10đ, không hợp lí trả về -1
        public static double CheckDiem(string negativeString)
        {
            double number;
            if (double.TryParse(negativeString, out number))
            {
                if (number >= 0 && number <= 10)
                {
                    return number;
                }
            }
            return -1;
        }
        public static int ConvertStringToNeInt(this string negativeString)
        {
            int number;
            if (int.TryParse(negativeString, out number))
            {
                return number;
            }
            return -1;
        }
    }
}
