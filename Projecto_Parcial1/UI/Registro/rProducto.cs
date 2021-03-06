﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Projecto_Parcial1.BLL;
using Projecto_Parcial1.Entidades;


namespace Projecto_Parcial1
{
    public partial class rProducto : Form
    {
        public rProducto()
        {
          
            InitializeComponent();
            LlenarCombo();
        }

        private void LlenarCombo()
        {
            var listado = new List<Ubicaciones>();

            listado = UbicacionesBLL.GetList(p => true);
            UbicacionesComboBox.DataSource = listado;
            UbicacionesComboBox.DisplayMember = "Descripcion";
            UbicacionesComboBox.ValueMember = "UbicacionId";
        }

        private void NuevoButton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            ProductoIdNumericUpDown.Value = 0;
            DescripcionTextBox.Text = string.Empty;  
            ExitenTextBox.Text = string.Empty;
            CostoTextBox.Text = string.Empty;
            ValorInventarioTextBox.Text = string.Empty;

        }

        private void GuardarButton_Click(object sender, EventArgs e)
        {
            Producto producto;
            bool paso;
    


            if (!Validar())
                return;
            producto = LlenarClase();
          

            if (ProductoIdNumericUpDown.Value == 0)
            {
                paso = ProductosBLL.Guardar(producto);
             
            }
            else
            {
                if (!ExisteEnLaBaseDeDatos())
                {
                    MessageBox.Show("No se puede modificar una persona que no existe", "fallo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                paso = ProductosBLL.Modificar(producto);
            }

            if (paso)
                MessageBox.Show("Guardado!!", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("No fue Posible guardar!!", "Fallo", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

      

        private Producto LlenarClase()
        {
           
            Producto producto = new Producto();
            producto.ProductoId = Convert.ToInt32(ProductoIdNumericUpDown.Value);
            producto.Descripcion = DescripcionTextBox.Text;
            producto.Existen = Convert.ToInt32(ExitenTextBox.Text);
            producto.Costo = Convert.ToSingle(CostoTextBox.Text);
            producto.Valor_Inventario = Convert.ToSingle(CostoTextBox.Text) * Convert.ToInt32(ExitenTextBox.Text);
           



            return producto;
        }

        private void LlenarCampo(Producto producto)
        {
            ValorInventario valor = new ValorInventario();

            ProductoIdNumericUpDown.Value = producto.ProductoId;
            DescripcionTextBox.Text = producto.Descripcion;
            ExitenTextBox.Text = producto.Existen.ToString();
            CostoTextBox.Text = producto.Costo.ToString();
         
        }

        private bool Validar()
        {
            bool paso = true;
            MyErrorProvider.Clear();

            if (string.IsNullOrWhiteSpace(DescripcionTextBox.Text))
            {
                MyErrorProvider.SetError(DescripcionTextBox, "El campo Descripcion no puede estar vacio");
                DescripcionTextBox.Focus();
                paso = false;
            }
            if(ExitenTextBox.Text == string.Empty)
            {

                MyErrorProvider.SetError(ExitenTextBox, "El campo Existen no puede estar vacio");
               ExitenTextBox.Focus();
                paso = false;
            }
            if(CostoTextBox.Text == string.Empty)
            {

                MyErrorProvider.SetError(CostoTextBox, "El campo Costo no puede estar vacio");
                CostoTextBox.Focus();
                paso = false;
            }
            return paso;

        }

        private bool ExisteEnLaBaseDeDatos()
        {
            Producto producto = ProductosBLL.Buscar((int)ProductoIdNumericUpDown.Value);
            return (producto != null);
        }

        private void BuscarButton_Click(object sender, EventArgs e)
        {
            int id;
            Producto producto = new Producto();
            int.TryParse(ProductoIdNumericUpDown.Text, out id);

            Limpiar();

            producto = ProductosBLL.Buscar(id);

            if(producto != null)
            {
                MessageBox.Show("Persona Encontrada");
                LlenarCampo(producto);
            }
            else
            {
                MessageBox.Show("Persona no Encontrada");
            }

        }

        private bool ValidarEliminar()
        {
            bool paso = true;
            MyErrorProvider.Clear();

            if(ProductoIdNumericUpDown.Value == 0)
            {
                MyErrorProvider.SetError(ProductoIdNumericUpDown, "Debe de introducir un ProductoId");
                ProductoIdNumericUpDown.Focus();
                paso = false;
            }
            return paso;
        }

        private void EliminarButton_Click(object sender, EventArgs e)
        {
            MyErrorProvider.Clear();
            int id;
            int.TryParse(ProductoIdNumericUpDown.Text, out id);

            if (!ValidarEliminar())
                return;

            Limpiar();
            if (ProductosBLL.Eliminar(id))
                MessageBox.Show("Eliminado", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MyErrorProvider.SetError(ProductoIdNumericUpDown, "No se puede eliminar una persona que no existe");

            Limpiar();
        }



        private void ExitenTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (e.KeyChar == '.')
            {
                if (ExitenTextBox.TextLength < 1)
                    e.Handled = true;
            }

            if (ch == 46 && ExitenTextBox.Text.IndexOf('.') != -1)
                e.Handled = true;

            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
            return;
        }

        private void CostoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            char ch = e.KeyChar;

            if (e.KeyChar == '.')
            {
                if (ExitenTextBox.TextLength < 1)
                    e.Handled = true;
            }

            if (ch == 46 && ExitenTextBox.Text.IndexOf('.') != -1)
                e.Handled = true;

            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
            return;

        }

        private void ExitenTextBox_TextChanged(object sender, EventArgs e)
        {

            if (CostoTextBox.Text.Length > 0 && ExitenTextBox.Text.Length > 0)
                ValorInventarioTextBox.Text = Convert.ToString(Convert.ToSingle(CostoTextBox.Text) * Convert.ToInt32(ExitenTextBox.Text));

            if (CostoTextBox.Text.Length > 0 && ExitenTextBox.Text.Length == 0)
                ValorInventarioTextBox.Text = "0.0";

            if (CostoTextBox.Text.Length == 0 && ExitenTextBox.Text.Length > 0)
                ValorInventarioTextBox.Text = "0.0";

            if (CostoTextBox.Text.Length == 0 && ExitenTextBox.Text.Length == 0)
                ValorInventarioTextBox.Text = "0.0";
        }

        private void CostoTextBox_TextChanged(object sender, EventArgs e)
        {
            if (CostoTextBox.Text.Length > 0 && ExitenTextBox.Text.Length > 0)
                ValorInventarioTextBox.Text = Convert.ToString(Convert.ToSingle(CostoTextBox.Text) * Convert.ToInt32(ExitenTextBox.Text));

            if (CostoTextBox.Text.Length > 0 && ExitenTextBox.Text.Length == 0)
                ValorInventarioTextBox.Text = "0.0";

            if (CostoTextBox.Text.Length == 0 && ExitenTextBox.Text.Length > 0)
                ValorInventarioTextBox.Text = "0.0";

            if (CostoTextBox.Text.Length == 0 && ExitenTextBox.Text.Length == 0)
                ValorInventarioTextBox.Text = "0.0";

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            rUbicaciones ru = new rUbicaciones();
            ru.Show();

        }

        
    }
}
