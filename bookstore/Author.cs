using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bookstore
{
    public partial class Author : Form
    {
        public Author()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            var books = new Books();
            books.Show();
            this.Hide();
        }

    }
}
