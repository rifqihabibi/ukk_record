using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing.Imaging;
using System.Globalization;

namespace ZeaMart
{
    public partial class Form1 : Form
    {
        MySqlConnection connection = connections.getConnection();
        DataTable dt = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox4.KeyPress += new KeyPressEventHandler(textBox4_KeyPress);
            textBox5.KeyPress += new KeyPressEventHandler(textBox5_KeyPress);
            textBox6.KeyPress += new KeyPressEventHandler(textBox6_KeyPress);
            textBox7.KeyPress += new KeyPressEventHandler(textBox7_KeyPress);
            filldataTable();
        }

        public DataTable getDataBatik()
        {
            //mambaca datatable di localhost
            dt.Reset();
            dt = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM daftar_ragnarok", connection))
            {
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
            return dt;
        }

        public void resetIncrement()
        {
            MySqlScript script = new MySqlScript(connection, "SET @id := 0; UPDATE daftar_ragnarok SET id = @id := (@id+1); " +
                "ALTER TABLE daftar_ragnarok AUTO_INCREMENT = 1;");
            script.Execute();
        }

        public void filldataTable()
        {
            //menampilkan data

            MySqlCommand command = new MySqlCommand(" SELECT * FROM daftar_ragnarok", connection);
            
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            
            DataTable table = new DataTable();

            adapter.Fill(table);

            dgv_ragnarok.RowTemplate.Height = 60;
            dgv_ragnarok.AllowUserToAddRows = false;
            
            dgv_ragnarok.DataSource = table;

            DataGridViewButtonColumn colEdit = new DataGridViewButtonColumn();
            colEdit.UseColumnTextForButtonValue = true;
            colEdit.Text = "Edit";
            colEdit.Name = "";
            dgv_ragnarok.Columns.Add(colEdit);

            DataGridViewButtonColumn colDelete = new DataGridViewButtonColumn();
            colDelete.UseColumnTextForButtonValue = true;
            colDelete.Text = "Delete";
            colDelete.Name = "";
            dgv_ragnarok.Columns.Add(colDelete);

            DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
            imgCol = (DataGridViewImageColumn)dgv_ragnarok.Columns[1];
            imgCol.ImageLayout = DataGridViewImageCellLayout.Stretch;

            dgv_ragnarok.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            filldataTable();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" | cb_ras.Text == "" | textBox4.Text == "" | textBox5.Text == "" | pictureBox1.Image == null)
            {
                MessageBox.Show("Silakan isi semua kolom dan gambar terlebih dahulu.");
            }
            else
            {

                // Ambil nilai dari textbox dan konversi ke tipe data numerik
                decimal harga = decimal.Parse(textBox4.Text);

                // Konversi harga ke format mata uang rupiah
                string hargaRupiah = harga.ToString("C", new CultureInfo("id-ID"));

                // Set nilai rupiah ke textbox
                textBox4.Text = hargaRupiah;

                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                byte[] img = ms.ToArray();

                resetIncrement();

                MySqlCommand command = new MySqlCommand("INSERT INTO daftar_ragnarok(image, nama, ras, harga, stok) VALUES (@img, @name, @ras, @harga, @stok)", connection);

                command.Parameters.Add("@img", MySqlDbType.Blob).Value = img;
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = textBox1.Text;
                command.Parameters.Add("@ras", MySqlDbType.VarChar).Value = cb_ras.Text;
                command.Parameters.Add("@harga", MySqlDbType.VarChar).Value = textBox4.Text;
                command.Parameters.Add("@stok", MySqlDbType.Int64).Value = textBox5.Text;

                ExecMyQuery(command, "Data Berhasil Ditambahkan");

                textBox1.Clear();
                textBox4.Clear();
                textBox5.Clear();
                pictureBox1.Image = null;
                dgv_ragnarok.Columns.Clear();
                filldataTable();
            }
        }

        private void dgv_ragnarok_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();

            opf.Filter = "Choose Image(*.JPG;*.PNG;*.GIF)|*.jpg;*.png;*.gif";

            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(opf.FileName);
            }
        }

        private void dgv_ragnarok_Click(object sender, EventArgs e)
        {
           /* 

            Byte[] img = (Byte[])dgv_ragnarok.CurrentRow.Cells[1].Value;

            MemoryStream ms = new MemoryStream(img);

            pictureBox2.Image = Image.FromStream(ms);

            textBox9.Text = dgv_ragnarok.CurrentRow.Cells[0].Value.ToString();
            textBox10.Text = dgv_ragnarok.CurrentRow.Cells[2].Value.ToString();
            cb_ras_edit.Text = dgv_ragnarok.CurrentRow.Cells[3].Value.ToString();
            textBox7.Text = dgv_ragnarok.CurrentRow.Cells[4].Value.ToString();
            textBox6.Text = dgv_ragnarok.CurrentRow.Cells[5].Value.ToString();*/

        }

        public void ExecMyQuery(MySqlCommand mcomd, string myMsg)
        {
            connection.Open();
            if (mcomd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show(myMsg);
            }
            else
            {
                MessageBox.Show("Query Not Executed");
            }

            connection.Close();
        }

        private void dgv_ragnarok_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                panel.Enabled = true;

                int id = Convert.ToInt32(dgv_ragnarok.CurrentCell.RowIndex.ToString());
                textBox9.Text = dgv_ragnarok.Rows[id].Cells[0].Value.ToString();
                textBox10.Text = dgv_ragnarok.CurrentRow.Cells[2].Value.ToString();
                cb_ras_edit.Text = dgv_ragnarok.CurrentRow.Cells[3].Value.ToString();
                textBox6.Text = dgv_ragnarok.CurrentRow.Cells[5].Value.ToString();

                string value = dgv_ragnarok.Rows[id].Cells[4].Value.ToString().Replace("Rp", "").Replace(".", "").Replace(",", "");
                value = value.Substring(0, value.Length - 2);
                textBox7.Text = value;

                Byte[] img = (Byte[])dgv_ragnarok.Rows[id].Cells[1].Value;

                MemoryStream ms = new MemoryStream(img);

                pictureBox2.Image = Image.FromStream(ms);
            }

            if (e.ColumnIndex == 7)
            {

                int id = Convert.ToInt32(dgv_ragnarok.CurrentCell.RowIndex.ToString());
                textBox9.Text = dgv_ragnarok.Rows[id].Cells[0].Value.ToString();


                MySqlCommand command = new MySqlCommand("DELETE FROM daftar_ragnarok WHERE id = @id", connection);

                command.Parameters.Add("@id", MySqlDbType.Int64).Value = textBox9.Text;

                ExecMyQuery(command, "Data berhasil dihapus");

                resetIncrement();

                textBox9.Clear();
                textBox10.Clear();
                textBox6.Clear();
                textBox7.Clear();
                pictureBox2.Image = null;
                panel.Enabled = false;
                dgv_ragnarok.Columns.Clear();
                filldataTable();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox10.Text == "" | cb_ras_edit.Text == "" | textBox7.Text == "" | textBox6.Text == "" | pictureBox2.Image == null)
            {
                MessageBox.Show("Silakan isi semua kolom dan gambar terlebih dahulu.");
            }
            else
            {
                // Ambil nilai dari textbox dan konversi ke tipe data numerik
                decimal harga = decimal.Parse(textBox7.Text);

                // Konversi harga ke format mata uang rupiah
                string hargaRupiah = harga.ToString("C", new CultureInfo("id-ID"));

                // Set nilai rupiah ke textbox
                textBox7.Text = hargaRupiah;

                MemoryStream ms = new MemoryStream();
                pictureBox2.Image.Save(ms, pictureBox2.Image.RawFormat);
                byte[] img = ms.ToArray();

                resetIncrement();

                MySqlCommand command = new MySqlCommand("UPDATE daftar_ragnarok SET image = @img, nama = @name, ras = @ras, harga = @harga, stok = @stok WHERE id = @id", connection);

                command.Parameters.Add("@id", MySqlDbType.Int64).Value = textBox9.Text;
                command.Parameters.Add("@img", MySqlDbType.Blob).Value = img;
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = textBox10.Text;
                command.Parameters.Add("@ras", MySqlDbType.VarChar).Value = cb_ras_edit.Text;
                command.Parameters.Add("@harga", MySqlDbType.VarChar).Value = textBox7.Text;
                command.Parameters.Add("@stok", MySqlDbType.Int64).Value = textBox6.Text;

                ExecMyQuery(command, "Data Berhasil DiUpdate");

                panel.Enabled = false;
                dgv_ragnarok.Columns.Clear();
                filldataTable();
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Mengabaikan karakter selain angka dan backspace
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Mengabaikan karakter selain angka dan backspace
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Mengabaikan karakter selain angka dan backspace
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Mengabaikan karakter selain angka dan backspace
            }
        }

        private void tb_search_TextChanged(object sender, EventArgs e)
        {
            string kataKunci = tb_search.Text;
            string query = "SELECT * FROM daftar_ragnarok WHERE CONCAT (id, nama, ras, harga, stok) LIKE '%" + kataKunci + "%'";
            MySqlDataAdapter sda = new MySqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dgv_ragnarok.DataSource = dt;
        }

        private void tb_search_Leave(object sender, EventArgs e)
        {
            lbl_search.Text = "Search";
        }

        private void tb_search_Enter(object sender, EventArgs e)
        {
            lbl_search.Text = "";
        }
    }
}
