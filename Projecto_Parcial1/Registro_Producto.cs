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
    public partial class Registro_Producto : Form
    {
        public Registro_Producto()
        {
            
 
            InitializeComponent();
        }

        private void NuevoButton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            ProductoIdNumericUpDown.Value = 0;
            DescripcionTextBox.Text = string.Empty;
            ExistenumericUpDown.Value = 0;
            CostoNumericUpDown.Value = 0;
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
                paso = ProductoBLL.Guardar(producto);
            else
            {
                if(!ExisteEnLaBaseDeDatos())
                {
                    MessageBox.Show("No se puede modificar una persona que no existe", "fallo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                paso = ProductoBLL.Modificar(producto);
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
            producto.Existen = Convert.ToInt32(ExistenumericUpDown.Value);
            producto.Costo = Convert.ToInt32(CostoNumericUpDown.Value);
           // producto.Valor_Inventario = Convert.ToInt32(ValorInventarioTextBox.Text);
            


            return producto;
        }

        private void LlenarCampo(Producto producto)
        {
            ProductoIdNumericUpDown.Value = producto.ProductoId;
            DescripcionTextBox.Text = producto.Descripcion;
            ExistenumericUpDown.Value = producto.Existen;
            CostoNumericUpDown.Value = producto.Costo;
            ValorInventarioTextBox.Text = Convert.ToString(producto.Existen * producto.Costo);
           
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
            if(ExistenumericUpDown.Value <= 0)
            {

                MyErrorProvider.SetError(ExistenumericUpDown, "El campo Existen no puede ser negativo o igual a 0");
                DescripcionTextBox.Focus();
                paso = false;
            }
            if(CostoNumericUpDown.Value <= 0)
            {

                MyErrorProvider.SetError(CostoNumericUpDown, "El campo Costo no puede ser negativo o igual a 0");
                DescripcionTextBox.Focus();
                paso = false;
            }
            return paso;

        }

        private bool ExisteEnLaBaseDeDatos()
        {
            Producto producto = ProductoBLL.Buscar((int)ProductoIdNumericUpDown.Value);
            return (producto != null);
        }

        private void BuscarButton_Click(object sender, EventArgs e)
        {
            int id;
            Producto producto = new Producto();
            int.TryParse(ProductoIdNumericUpDown.Text, out id);

            Limpiar();

            producto = ProductoBLL.Buscar(id);

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

        private void EliminarButton_Click(object sender, EventArgs e)
        {
            MyErrorProvider.Clear();
            int id;
            int.TryParse(ProductoIdNumericUpDown.Text, out id);

            Limpiar();
            if (ProductoBLL.Eliminar(id))
                MessageBox.Show("Eliminado", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MyErrorProvider.SetError(ProductoIdNumericUpDown, "No se puede eliminar una persona que no existe");
        }

        private void CostoNumericUpDown_Leave(object sender, EventArgs e)
        {
            int n1, n2, r;
            
            n1 = Convert.ToInt32(ExistenumericUpDown.Text);
            n2 = Convert.ToInt32(CostoNumericUpDown.Text);
            r = n1 * n2;
            ValorInventarioTextBox.Text = r.ToString(); 
        }
    }
}
