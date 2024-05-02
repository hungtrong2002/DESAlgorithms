using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Remoting.Contexts;
namespace DESAlgrorithms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OpenFileDialog open;
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] keyBytes = new byte[8];

            // Sử dụng RNGCryptoServiceProvider để sinh số ngẫu nhiên
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(keyBytes);
            }

            // Biến đổi mảng byte thành chuỗi hex
            string hexKey = BitConverter.ToString(keyBytes).Replace("-", "");
            khoa_K.Text = hexKey;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cipher_text=textBox1.Text.Trim();
            string key = khoa_K.Text.Trim();
            if (Is64BitHex(cipher_text))
            {
                string result = DesEncryption.Encrypt(cipher_text, key);
                textBox3.Text = result;
                List<PlainText> plainTexts = DesEncryption.PrintLeftRight(cipher_text, key);
                dataGridView2.DataSource = plainTexts;
                List<Key> keys = DesEncryption.printSubKey(key);
                dataGridView1.DataSource = keys;
            }
            else
            {
                MessageBox.Show("Chuỗi đầu vào không hợp lệ","Vui lòng nhập lại", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        public bool Is64BitHex(string input)
        {
            // Loại bỏ khoảng trắng và chuyển đổi sang chữ hoa hoặc chữ thường
            string hexString = input.Trim().ToUpper();

            // Kiểm tra độ dài của chuỗi hex
            if (hexString.Length != 16) // 16 ký tự hex tương ứng với 64 bit
                return false;

            // Kiểm tra xem mỗi ký tự có phải là ký tự hex hợp lệ không
            foreach (char c in hexString)
            {
                if (!IsHexCharacter(c))
                    return false;
            }

            return true;
        }

        private bool IsHexCharacter(char c)
        {
            // Kiểm tra xem ký tự có phải là ký tự hex hợp lệ không
            return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F');
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string cipher_text = textBox1.Text.Trim();
            string key = khoa_K.Text.Trim();
            if (Is64BitHex(cipher_text))
            {
                string result = DesEncryption.Decrypt(cipher_text, key);
                textBox3.Text = result;
                List<PlainText> plainTexts = DesEncryption.PrintLeftRight(cipher_text, key);
                dataGridView2.DataSource = plainTexts;
                List<Key> keys = DesEncryption.printSubKey(key);
                dataGridView1.DataSource = keys;
            }
            else
            {
                MessageBox.Show("Chuỗi đầu vào không hợp lệ", "Vui lòng nhập lại", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
