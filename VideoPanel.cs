﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.DirectX.AudioVideoPlayback;

namespace HomeSystem_CSharp
{
    public partial class VideoPanel : Form
    {
        public VideoPanel()
        {
            InitializeComponent();
        }

        public void addBrush(DrawingBrush db)
        {
            //this.button1.Image = db;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
