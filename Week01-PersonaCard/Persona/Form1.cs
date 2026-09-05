using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Persona
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().First(n => n.EndsWith("avatar.png"));
            using var stream = assembly.GetManifestResourceStream(resourceName);
            pictureBox1.Image = Image.FromStream(stream);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/zaochih",
                UseShellExecute = true
            });
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://blog.licaoz.com/",
                UseShellExecute = true
            });
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/zaochih/univ-dotnet-assignments/tree/main/Week01-PersonaCard",
                UseShellExecute = true
            });
        }
    }
}
