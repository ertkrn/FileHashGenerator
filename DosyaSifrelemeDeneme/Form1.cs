using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;


namespace DosyaSifrelemeDeneme
{
    public partial class Form1 : Form
    {
        string anahtar;
        public Form1()
        {
            InitializeComponent();
            anahtar = anahtar_çalıştırma();

        }

        private string anahtar_çalıştırma()
        {
            DESCryptoServiceProvider kripto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(kripto.Key);
        }

    private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ac = new OpenFileDialog();
            ac.ShowDialog();
            textBox1.Text = ac.FileName;//Disk üzerindeki şifrelenecek dosya dizini
            SaveFileDialog kaydet = new SaveFileDialog();
            kaydet.ShowDialog();
            textBox2.Text = kaydet.FileName;//Yeni dosya dizinine farklı kaydet yaptık.
            sifreleme(textBox1.Text, textBox2.Text, anahtar);//DES(anahtar_calisma fonksiyonu) ten gelen gizli anahtar burada kullanılır.
        }
        private void sifreleme(string adres, string yeniadres, string StrKarma)
        {
            TripleDESCryptoServiceProvider TDCS = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            FileStream OkuStream = new FileStream(adres, FileMode.Open, FileAccess.Read);
            FileStream YazmaStream = new FileStream(yeniadres, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] Karma = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(StrKarma));
            //textBox5.Text = System.Text.Encoding.UTF8.GetString(md5);
            textBox5.Text = BitConverter.ToString(Karma).Replace("-", "");
            byte[] Metin_Oku = File.ReadAllBytes(adres);
            md5.Clear();
            TDCS.Key = Karma;
            TDCS.Mode = CipherMode.ECB;
            CryptoStream kriptoStream = new CryptoStream(YazmaStream, TDCS.CreateEncryptor(), CryptoStreamMode.Write);
            int depo;
            long position = 0;
            while (position<OkuStream.Length)
            {
                depo=OkuStream.Read(Metin_Oku,0,Metin_Oku.Length);
                position += depo;
                kriptoStream.Write(Metin_Oku,0,depo);
            }
            OkuStream.Close();
            YazmaStream.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ac = new OpenFileDialog();
            ac.ShowDialog();
            textBox3.Text = ac.FileName;//Disk üzerindeki şifre çözme dosya dizini
            SaveFileDialog kaydet = new SaveFileDialog();
            kaydet.ShowDialog();
            textBox4.Text = kaydet.FileName;//Yeni dosya dizinine farklı kaydet yaptık.
            sifreCozme(textBox3.Text, textBox4.Text, anahtar);
        }

        private void sifreCozme(string adres, string yeniadres, string StrKarma)
        {
            TripleDESCryptoServiceProvider TDCS = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            FileStream OkuStream = new FileStream(adres, FileMode.Open, FileAccess.Read);
            FileStream YazmaStream = new FileStream(yeniadres, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] Karma = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(StrKarma));
            byte[] Metin_Oku = File.ReadAllBytes(adres);
            md5.Clear();
            TDCS.Key = Karma;
            TDCS.Mode = CipherMode.ECB;
            CryptoStream kriptoStream = new CryptoStream(YazmaStream, TDCS.CreateDecryptor(), CryptoStreamMode.Write);
            int depo;
            long position = 0;
            while (position<OkuStream.Length)
            {
                depo = OkuStream.Read(Metin_Oku, 0, Metin_Oku.Length);
                position += depo;
                kriptoStream.Write(Metin_Oku, 0, depo);
            }
            OkuStream.Close();
            YazmaStream.Close();
        }
    }
}
