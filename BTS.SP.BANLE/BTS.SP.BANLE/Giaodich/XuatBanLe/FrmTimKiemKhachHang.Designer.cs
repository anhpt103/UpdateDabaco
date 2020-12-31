namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    partial class FrmTimKiemKhachHang
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTimKiemKhachHang));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTimKiemKhachHang = new System.Windows.Forms.Button();
            this.cboDieuKienTimKiem = new System.Windows.Forms.ComboBox();
            this.lblDieuKien = new System.Windows.Forms.Label();
            this.txtDieuKienTimKiem = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvThongTinKhachHang = new System.Windows.Forms.DataGridView();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAKH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TENKH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DIENTHOAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SODIEM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CMTND = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DIACHI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EMAIL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NGAYCAPTHE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NGAYSINH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HANGKHACHHANG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HANGKHACHHANGCU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TONGTIEN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongTinKhachHang)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTimKiemKhachHang);
            this.groupBox1.Controls.Add(this.cboDieuKienTimKiem);
            this.groupBox1.Controls.Add(this.lblDieuKien);
            this.groupBox1.Controls.Add(this.txtDieuKienTimKiem);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1129, 66);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tìm kiếm";
            // 
            // btnTimKiemKhachHang
            // 
            this.btnTimKiemKhachHang.ForeColor = System.Drawing.Color.CadetBlue;
            this.btnTimKiemKhachHang.Location = new System.Drawing.Point(799, 24);
            this.btnTimKiemKhachHang.Name = "btnTimKiemKhachHang";
            this.btnTimKiemKhachHang.Size = new System.Drawing.Size(93, 29);
            this.btnTimKiemKhachHang.TabIndex = 3;
            this.btnTimKiemKhachHang.Text = "Tìm kiếm";
            this.btnTimKiemKhachHang.UseVisualStyleBackColor = true;
            this.btnTimKiemKhachHang.Click += new System.EventHandler(this.btnTimKiemKhachHang_Click);
            // 
            // cboDieuKienTimKiem
            // 
            this.cboDieuKienTimKiem.FormattingEnabled = true;
            this.cboDieuKienTimKiem.Location = new System.Drawing.Point(600, 26);
            this.cboDieuKienTimKiem.Name = "cboDieuKienTimKiem";
            this.cboDieuKienTimKiem.Size = new System.Drawing.Size(182, 27);
            this.cboDieuKienTimKiem.TabIndex = 2;
            // 
            // lblDieuKien
            // 
            this.lblDieuKien.AutoSize = true;
            this.lblDieuKien.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDieuKien.ForeColor = System.Drawing.Color.CadetBlue;
            this.lblDieuKien.Location = new System.Drawing.Point(127, 28);
            this.lblDieuKien.Name = "lblDieuKien";
            this.lblDieuKien.Size = new System.Drawing.Size(83, 23);
            this.lblDieuKien.TabIndex = 1;
            this.lblDieuKien.Text = "Từ khóa";
            // 
            // txtDieuKienTimKiem
            // 
            this.txtDieuKienTimKiem.Location = new System.Drawing.Point(223, 26);
            this.txtDieuKienTimKiem.Name = "txtDieuKienTimKiem";
            this.txtDieuKienTimKiem.Size = new System.Drawing.Size(371, 27);
            this.txtDieuKienTimKiem.TabIndex = 0;
            this.txtDieuKienTimKiem.TextChanged += new System.EventHandler(this.txtDieuKienTimKiem_TextChanged);
            this.txtDieuKienTimKiem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDieuKienTimKiem_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvThongTinKhachHang);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1129, 407);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Chi tiết";
            // 
            // dgvThongTinKhachHang
            // 
            this.dgvThongTinKhachHang.AllowUserToAddRows = false;
            this.dgvThongTinKhachHang.AllowUserToDeleteRows = false;
            this.dgvThongTinKhachHang.AllowUserToResizeColumns = false;
            this.dgvThongTinKhachHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThongTinKhachHang.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.MAKH,
            this.TENKH,
            this.DIENTHOAI,
            this.SODIEM,
            this.CMTND,
            this.DIACHI,
            this.EMAIL,
            this.NGAYCAPTHE,
            this.NGAYSINH,
            this.HANGKHACHHANG,
            this.HANGKHACHHANGCU,
            this.TONGTIEN});
            this.dgvThongTinKhachHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvThongTinKhachHang.Location = new System.Drawing.Point(3, 22);
            this.dgvThongTinKhachHang.MaximumSize = new System.Drawing.Size(1123, 382);
            this.dgvThongTinKhachHang.MinimumSize = new System.Drawing.Size(1123, 382);
            this.dgvThongTinKhachHang.Name = "dgvThongTinKhachHang";
            this.dgvThongTinKhachHang.ReadOnly = true;
            this.dgvThongTinKhachHang.RowHeadersVisible = false;
            this.dgvThongTinKhachHang.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvThongTinKhachHang.Size = new System.Drawing.Size(1123, 382);
            this.dgvThongTinKhachHang.TabIndex = 0;
            this.dgvThongTinKhachHang.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvThongTinKhachHang_CellMouseDoubleClick);
            // 
            // STT
            // 
            this.STT.DataPropertyName = "STT";
            this.STT.HeaderText = "STT";
            this.STT.MinimumWidth = 60;
            this.STT.Name = "STT";
            this.STT.ReadOnly = true;
            this.STT.Width = 60;
            // 
            // MAKH
            // 
            this.MAKH.DataPropertyName = "MAKH";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MAKH.DefaultCellStyle = dataGridViewCellStyle1;
            this.MAKH.HeaderText = "Mã KH";
            this.MAKH.MinimumWidth = 100;
            this.MAKH.Name = "MAKH";
            this.MAKH.ReadOnly = true;
            // 
            // TENKH
            // 
            this.TENKH.DataPropertyName = "TENKH";
            this.TENKH.HeaderText = "Tên KH";
            this.TENKH.MinimumWidth = 200;
            this.TENKH.Name = "TENKH";
            this.TENKH.ReadOnly = true;
            this.TENKH.Width = 200;
            // 
            // DIENTHOAI
            // 
            this.DIENTHOAI.DataPropertyName = "DIENTHOAI";
            this.DIENTHOAI.HeaderText = "Điện thoại";
            this.DIENTHOAI.MinimumWidth = 110;
            this.DIENTHOAI.Name = "DIENTHOAI";
            this.DIENTHOAI.ReadOnly = true;
            this.DIENTHOAI.Width = 110;
            // 
            // SODIEM
            // 
            this.SODIEM.DataPropertyName = "SODIEM";
            this.SODIEM.HeaderText = "Số điểm";
            this.SODIEM.Name = "SODIEM";
            this.SODIEM.ReadOnly = true;
            // 
            // CMTND
            // 
            this.CMTND.DataPropertyName = "CMTND";
            this.CMTND.HeaderText = "CMTND";
            this.CMTND.Name = "CMTND";
            this.CMTND.ReadOnly = true;
            // 
            // DIACHI
            // 
            this.DIACHI.DataPropertyName = "DIACHI";
            this.DIACHI.HeaderText = "Địa chỉ";
            this.DIACHI.Name = "DIACHI";
            this.DIACHI.ReadOnly = true;
            // 
            // EMAIL
            // 
            this.EMAIL.DataPropertyName = "EMAIL";
            this.EMAIL.HeaderText = "Email";
            this.EMAIL.Name = "EMAIL";
            this.EMAIL.ReadOnly = true;
            // 
            // NGAYCAPTHE
            // 
            this.NGAYCAPTHE.DataPropertyName = "NGAYCAPTHE";
            this.NGAYCAPTHE.HeaderText = "Ngày cấp thẻ";
            this.NGAYCAPTHE.MinimumWidth = 130;
            this.NGAYCAPTHE.Name = "NGAYCAPTHE";
            this.NGAYCAPTHE.ReadOnly = true;
            this.NGAYCAPTHE.Width = 130;
            // 
            // NGAYSINH
            // 
            this.NGAYSINH.DataPropertyName = "NGAYSINH";
            this.NGAYSINH.HeaderText = "Ngày sinh";
            this.NGAYSINH.MinimumWidth = 120;
            this.NGAYSINH.Name = "NGAYSINH";
            this.NGAYSINH.ReadOnly = true;
            this.NGAYSINH.Width = 120;
            // 
            // HANGKHACHHANG
            // 
            this.HANGKHACHHANG.DataPropertyName = "HANGKHACHHANG";
            this.HANGKHACHHANG.HeaderText = "Hạng";
            this.HANGKHACHHANG.Name = "HANGKHACHHANG";
            this.HANGKHACHHANG.ReadOnly = true;
            // 
            // HANGKHACHHANGCU
            // 
            this.HANGKHACHHANGCU.DataPropertyName = "HANGKHACHHANGCU";
            this.HANGKHACHHANGCU.HeaderText = "Hạng cũ";
            this.HANGKHACHHANGCU.Name = "HANGKHACHHANGCU";
            this.HANGKHACHHANGCU.ReadOnly = true;
            // 
            // TONGTIEN
            // 
            this.TONGTIEN.DataPropertyName = "TONGTIEN";
            this.TONGTIEN.HeaderText = "Tổng tiền";
            this.TONGTIEN.Name = "TONGTIEN";
            this.TONGTIEN.ReadOnly = true;
            // 
            // FrmTimKiemKhachHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 473);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmTimKiemKhachHang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin khách hàng";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThongTinKhachHang)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblDieuKien;
        private System.Windows.Forms.TextBox txtDieuKienTimKiem;
        private System.Windows.Forms.ComboBox cboDieuKienTimKiem;
        private System.Windows.Forms.DataGridView dgvThongTinKhachHang;
        private System.Windows.Forms.Button btnTimKiemKhachHang;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAKH;
        private System.Windows.Forms.DataGridViewTextBoxColumn TENKH;
        private System.Windows.Forms.DataGridViewTextBoxColumn DIENTHOAI;
        private System.Windows.Forms.DataGridViewTextBoxColumn SODIEM;
        private System.Windows.Forms.DataGridViewTextBoxColumn CMTND;
        private System.Windows.Forms.DataGridViewTextBoxColumn DIACHI;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMAIL;
        private System.Windows.Forms.DataGridViewTextBoxColumn NGAYCAPTHE;
        private System.Windows.Forms.DataGridViewTextBoxColumn NGAYSINH;
        private System.Windows.Forms.DataGridViewTextBoxColumn HANGKHACHHANG;
        private System.Windows.Forms.DataGridViewTextBoxColumn HANGKHACHHANGCU;
        private System.Windows.Forms.DataGridViewTextBoxColumn TONGTIEN;
    }
}