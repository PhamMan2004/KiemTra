using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using De01.Model;

namespace De01
{
    public partial class frmSinhvien : Form
    {
        public frmSinhvien()
        {
            InitializeComponent();
        }

        private void frmSinhvien_Load(object sender, EventArgs e)
        {
            LoadData();
        }


        private void LoadData()
        {
            LoaddataGridView1();
            LoadComboBox();
        }

     

        private void LoaddataGridView1()
        {
            var sinhVienList = GetSinhVienList();
            dataGridView1.DataSource = sinhVienList;
        }


        private List<Sinhvien> GetSinhVienList()
        {
            using (var context = new SinhvienContextDB())
            {
                return context.Sinhvien.Include(s => s.Lop1).ToList();
            }
        }
        private void LoadComboBox()
        {
            var lopList = GetLopList();
            cboLop.DataSource = lopList;
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
        }

        private List<Lop> GetLopList()
        {
            using (var context = new SinhvienContextDB())
            {
                return context.Lop.ToList();
            }
        }

        private void UpdateButtonState(bool isEditing)
        {
            btThem.Enabled = !isEditing;
            btXoa.Enabled = isEditing;
            btSua.Enabled = isEditing;
            btLuu.Enabled = isEditing;
            btKhong.Enabled = isEditing;
            btThoat.Enabled = true;
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text) ||
        string.IsNullOrWhiteSpace(txtHoTenSV.Text) ||
        cboLop.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btLuu.Visible = true; 
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            using (var context = new SinhvienContextDB())
            {
                var maSV = txtMaSV.Text.Trim();
                var sinhVien = context.Sinhvien.Find(maSV);
                if (sinhVien == null)
                {
                    MessageBox.Show("Sinh viên không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                context.Sinhvien.Remove(sinhVien);
                context.SaveChanges();
                LoaddataGridView1();

                txtMaSV.Clear();
                txtHoTenSV.Clear();
                dtNgaySinh.Value = DateTime.Now;
                cboLop.SelectedIndex = -1;
            }
        }
        private void btSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var context = new SinhvienContextDB())
            {
                var maSV = txtMaSV.Text.Trim();
                var sinhVien = context.Sinhvien.Find(maSV);

                if (sinhVien == null)
                {
                    MessageBox.Show("Sinh viên không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboLop.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn lớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                sinhVien.Hoten = txtHoTenSV.Text.Trim();
                sinhVien.Ngaysinh = dtNgaySinh.Value;
                sinhVien.Lop = cboLop.SelectedValue.ToString();

                context.SaveChanges();
                LoaddataGridView1();

                MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text) ||
        string.IsNullOrWhiteSpace(txtHoTenSV.Text) ||
        cboLop.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn lưu thông tin này không?", "Xác nhận lưu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            using (var context = new SinhvienContextDB())
            {
                var maSV = txtMaSV.Text.Trim();
                var sinhVien = context.Sinhvien.Find(maSV);

                if (sinhVien != null)
                {
                    sinhVien.Hoten = txtHoTenSV.Text.Trim();
                    sinhVien.Ngaysinh = dtNgaySinh.Value;
                    sinhVien.Lop = cboLop.SelectedValue.ToString();
                }
                else
                {
                    var newSinhVien = new Sinhvien
                    {
                        MaSV = maSV,
                        Hoten = txtHoTenSV.Text.Trim(),
                        Ngaysinh = dtNgaySinh.Value,
                        Lop = cboLop.SelectedValue.ToString()
                    };
                    context.Sinhvien.Add(newSinhVien);
                }

                context.SaveChanges();
                LoaddataGridView1();

                txtMaSV.Clear();
                txtHoTenSV.Clear();
                dtNgaySinh.Value = DateTime.Now;
                cboLop.SelectedIndex = -1;
                btLuu.Visible = false; 
            }
        }

        private void btKhong_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thông tin không được lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btLuu.Visible = false;
            txtMaSV.Clear();
            txtHoTenSV.Clear();
            dtNgaySinh.Value = DateTime.Now;
            cboLop.SelectedIndex = -1;
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTim_TextChanged(object sender, EventArgs e)
        {

            string searchTerm = txtTim.Text.Trim();

            using (var context = new SinhvienContextDB())
            {
                var results = context.Sinhvien
                    .Where(sv => sv.Hoten.Contains(searchTerm))
                    .ToList();

   
                dataGridView1.DataSource = results;
            }
        }

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboLop.SelectedValue == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                txtMaSV.Text = row.Cells["MaSV"].Value.ToString();
                txtHoTenSV.Text = row.Cells["Hoten"].Value.ToString();
                dtNgaySinh.Value = row.Cells["Ngaysinh"].Value != DBNull.Value ? Convert.ToDateTime(row.Cells["Ngaysinh"].Value) : DateTime.Now;
                cboLop.SelectedValue = row.Cells["Lop"].Value;
                UpdateButtonState(true);
            }
            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];

                if (row.Cells["MaSV"].Value != null)
                    txtMaSV.Text = row.Cells["MaSV"].Value.ToString();
                else
                    txtMaSV.Text = string.Empty;

                if (row.Cells["Hoten"].Value != null)
                    txtHoTenSV.Text = row.Cells["Hoten"].Value.ToString();
                else
                    txtHoTenSV.Text = string.Empty;

                if (row.Cells["Ngaysinh"].Value != DBNull.Value)
                    dtNgaySinh.Value = Convert.ToDateTime(row.Cells["Ngaysinh"].Value);
                else
                    dtNgaySinh.Value = DateTime.Now;

                if (row.Cells["Lop"].Value != null)
                    cboLop.SelectedValue = row.Cells["Lop"].Value.ToString();
                else
                    cboLop.SelectedIndex = -1;

                UpdateButtonState(true);
            }
        }
    }

}


