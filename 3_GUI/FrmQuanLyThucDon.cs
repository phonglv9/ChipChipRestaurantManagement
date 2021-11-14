﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _1_DAL.Models;
using _2_BUS.iBUSServices;
using _2_BUS.BUSServices;
using _2_BUS.Utilities;
using System.Data.SqlClient;



namespace _3_GUI
{
    public partial class FrmQuanLyThucDon : Form
    {
        private iQLMenuService _iQLMenuService;
        private MonAnChiTiet _monCT;
        private DonVi _donVi;
        private DanhMucFood _dmFood;
        private CachCheBien _cachCB;
        private ThucDon _item;
        private Utilities _utilities;
        public FrmQuanLyThucDon()
        {
            //MaximizeBox = false;
            InitializeComponent();
            _iQLMenuService = new QLMenuService();
            FrmQuanLyThucDon_Load();
        }

        private void FrmQuanLyThucDon_Load()
        {
            dgvMonAn.ColumnCount = 7;
            dgvMonAn.Columns[0].Name = "Mã món ăn";
            dgvMonAn.Columns[1].Name = "Tên món";
            dgvMonAn.Columns[2].Name = "Giá";
            dgvMonAn.Columns[3].Name = "Danh mục";
            dgvMonAn.Columns[4].Name = "Cách chế biên";
            dgvMonAn.Columns[5].Name = "Đơn vị tính";
            dgvMonAn.Columns[6].Name = "Trạng thái";
            dgvMonAn.Rows.Clear();
            foreach(var x in _iQLMenuService.GetViewMenus())
            {
                dgvMonAn.Rows.Add(x.details.Id,x.details.Name, x.details.Price, x.cat.Name, x.method.Name, x.unit.Name, x.details.Status == 1 ? "Đang bán" : "Dừng bán");
            }
            dgvCongThuc.ColumnCount = 2;
            dgvCongThuc.Columns[0].Name = "Tên món ăn";
            dgvCongThuc.Columns[1].Name = "Tên nguyên liệu";
            foreach(var x in _iQLMenuService.GetViewMenus())
            {
                dgvCongThuc.Rows.Add(x.details.Name, x.ingre.Name);
            }
            this.dgvCongThuc.ClearSelection();
            this.dgvMonAn.ClearSelection();
            LoadCBox();
        }

        private void LoadCBox()
        {
            foreach(var x in _iQLMenuService.GetCachCheBiens())
            {
                if(x.Status == true)
                cbx_Meth.Items.Add(x.Name);
            }
            foreach (var x in _iQLMenuService.GetDanhMucFoods())
            {
                if(x.Status == true)
                cbx_Cat.Items.Add(x.Name);
            }
            foreach (var x in _iQLMenuService.GetDonVis())
            {
                if (x.Status == true)
                    cbx_Unit.Items.Add(x.Name);
            }
        }

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Xác nhận thêm?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                if (checknullMonAn())
                {

                    _monCT = new MonAnChiTiet();
                    _item = new ThucDon();
                    _monCT.Id = _iQLMenuService.GetMonAnChiTiets().Count;
                    _monCT.Name = txtTenMonAn.Text;
                    _monCT.Idunit = cbx_Unit.SelectedIndex;
                    _monCT.Idcategory = cbx_Cat.SelectedIndex;
                    _monCT.Idmethod = cbx_Cat.SelectedIndex;
                    _monCT.Price = txt_Price.Value;
                    _item.Id = _monCT.Id;
                    _item.Name = _monCT.Name;
                    _item.Price = _monCT.Price;
                    if (rbtn_HDthucdon.Checked)
                    {
                        _monCT.Status = 1;
                    }
                    else
                    {
                        _monCT.Status = 0;
                    }
                    _item.Status = _monCT.Status == 1 ? true : false;
                    _iQLMenuService.AddDetail(_monCT);
                    _iQLMenuService.AddItem(_item);
                    FrmQuanLyThucDon_Load();
                }
                else
                {
                    MessageBox.Show("Nhập đủ thông tin");
                } 
            } else
            {
                return;
            }
            
            
        }
        private bool checknullMonAn()
        {
            if(txtTenMonAn.Text is null || cbx_Unit.Text is null || cbx_Cat.Text is null || cbx_Meth.Text is null || txt_Price.Value < 1000 || (!rbtn_HDthucdon.Checked && !rbtn_KHDthucdon.Checked))
            {
                return false;
            }
            return true;
        }

        private void btnSuaMon_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Xác nhận sửa?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                if (_monCT is null)
                {
                    MessageBox.Show("Chọn món muốn sửa");
                }
                else if (!checknullMonAn())
                {
                    MessageBox.Show("Nhập đủ thông tin");
                }
                else
                {
                    _monCT.Id = _iQLMenuService.GetMonAnChiTiets().Count;
                    _monCT.Name = txtTenMonAn.Text;
                    _monCT.Idunit = _utilities.GetDonViID(cbx_Unit.Text);
                    _monCT.Idcategory = _utilities.GetCategoryID(cbx_Cat.Text);
                    _monCT.Idmethod = _utilities.GetMethodID(cbx_Meth.Text);
                    _monCT.Price = txt_Price.Value;
                    _item.Id = _monCT.Id;
                    _item.Name = _monCT.Name;
                    _item.Price = _monCT.Price;
                    _iQLMenuService.UpdateDetail(_monCT);
                    _iQLMenuService.UpdateItem(_item);
                    FrmQuanLyThucDon_Load();
                } 
            }
            else
            {
                return;
            }
        }

        private void dgvMonAn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var index = e.RowIndex;
            if(index == _iQLMenuService.GetMonAnChiTiets().Count || index == -1)
            {
                txtTenMonAn.Text = null;
                cbx_Unit.Text = null;
                cbx_Cat.Text = null;
                cbx_Meth.Text = null;
                txt_Price.Value = 0;
                rbtn_HDthucdon.Checked = false;
                rbtn_KHDthucdon.Checked = false;
                return;
            }
            else
            {
                int ID = (int)dgvMonAn.Rows[index].Cells[0].Value;
                _monCT = _iQLMenuService.GetMonAnChiTiets().Where(c => c.Id == ID).FirstOrDefault();
                _item = _iQLMenuService.GetThucDons().Where(c => c.Id == _monCT.Id).FirstOrDefault();
                txtTenMonAn.Text = _monCT.Name;
                cbx_Unit.SelectedIndex = _monCT.Idunit;
                cbx_Cat.SelectedIndex = _monCT.Idcategory;
                cbx_Meth.SelectedIndex = _monCT.Idmethod;
                txt_Price.Value = 0;
                if (_monCT.Status == 1)
                {
                    rbtn_HDthucdon.Checked = true; 
                }
                else
                {
                    rbtn_KHDthucdon.Checked = true;
                }
            }
            

        }

        private void btnXoaMon_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Xác nhận xóa?", "Xác nhận", MessageBoxButtons.YesNo);
            if(dialog == DialogResult.Yes)
            {
                if (_monCT != null)
                {
                    _iQLMenuService.DeleteItem(_item);
                    _iQLMenuService.DeleteDetail(_monCT); 
                }
                else
                {
                    MessageBox.Show("Chọn món để xóa");
                }
            }
        }

        private void txt_TimKiemThucDon_TextChanged(object sender, EventArgs e)
        {
            dgvMonAn.ColumnCount = 7;
            dgvMonAn.Columns[0].Name = "Mã món ăn";
            dgvMonAn.Columns[1].Name = "Tên món";
            dgvMonAn.Columns[2].Name = "Giá";
            dgvMonAn.Columns[3].Name = "Danh mục";
            dgvMonAn.Columns[4].Name = "Cách chế biên";
            dgvMonAn.Columns[5].Name = "Đơn vị tính";
            dgvMonAn.Columns[6].Name = "Trạng thái";
            dgvMonAn.Rows.Clear();
            foreach (var x in _iQLMenuService.TimKiem(txt_TimKiemThucDon.Text))
            {
                dgvMonAn.Rows.Add(x.details.Id, x.details.Name, x.details.Price, x.cat.Name, x.method.Name, x.unit.Name, x.details.Status == 1 ? "Đang bán" : "Dừng bán");
            }
        }

    }

}
