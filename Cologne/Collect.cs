using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Forms;

namespace Cologne
{
    public partial class Collect : MetroForm
    {
        public Collect()
        {
            InitializeComponent();
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            if(Valid())
            {
                Configuration conf = new Configuration();

                conf.data = new List<int>();

                conf.data.Add(Convert.ToInt16(materialSingleLineTextField1.Text));
                conf.data.Add(Convert.ToInt16(materialSingleLineTextField2.Text));
                conf.data.Add(Convert.ToInt16(materialSingleLineTextField3.Text));

                conf.Update();

                MetroMessageBox.Show(this, "Successfully saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Question);
            } else
            {
                MetroMessageBox.Show(this, "Invalid parameters", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool Valid()
        {
            if(materialSingleLineTextField1.Text == null || materialSingleLineTextField2.Text == null || materialSingleLineTextField3.Text == null)
            {
                return false;
            } else
            {
                int a_i, b_i, c_i;

                bool a = int.TryParse(materialSingleLineTextField1.Text, out a_i);
                bool b = int.TryParse(materialSingleLineTextField2.Text, out b_i);
                bool c = int.TryParse(materialSingleLineTextField3.Text, out c_i);

                return (a && b && c);
            }
        }
    }
}
