﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Protoeuous
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string Protofilepath = "";
        public static string SafeFileName = "";

        private void ProtoLoadButton_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Protofilepath = openFileDialog1.FileName;
            SafeFileName = openFileDialog1.SafeFileName;
            FileStream stream = new FileStream(Protofilepath,FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            var text = reader.ReadToEnd();
            richTextBox1.Text = text;
            File.Copy(Protofilepath, SafeFileName, true);
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory("Output");
            foreach (var file in Directory.EnumerateFiles("Output"))
            {
                File.Delete(file);
            }
            
            string strCmdText;
            strCmdText = "/C protoc.exe --csharp_out Output " + SafeFileName + " --grpc_out Output --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe";
            Process.Start("CMD.exe", strCmdText);
            strCmdText = "/C protoc.exe --go_out=plugins=grpc:Output " + SafeFileName;
            Process.Start("CMD.exe", strCmdText);
            Process.Start(Directory.GetCurrentDirectory() + "/Output/");
        }
    }
}
