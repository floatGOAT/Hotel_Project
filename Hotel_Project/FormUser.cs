using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_Project
{
    public partial class FormUser : Form
    {
        public string fio, number, roomnumber, rangroom, capacityroom, howlong; //for textboxs
        All_Visitors spisok = new All_Visitors();
        XmlSerializer serial = new XmlSerializer(typeof(All_Visitors));
        public FormUser()
        {
            InitializeComponent();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            rangroom = comboBox1.Text;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            capacityroom = radioButton2.Text;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            capacityroom = radioButton3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            howlong = textBox4.Text;
        }

        private void FormUser_Load(object sender, EventArgs e)
        {

        }

        private void wordButton_Click(object sender, EventArgs e)
        {

        }

        private void excelButton_Click(object sender, EventArgs e)
        {

        }

        private void commitButton_Click(object sender, EventArgs e)
        {
            Visitor currentVisitor = new Visitor(fio, number, roomnumber, Convert.ToUInt16(capacityroom), rangroom,
                Convert.ToUInt16(howlong));

            /*excelSerializer.isExisted();
excelSerializer.Write(currentVisitor);
excelSerializer.Close();

wordSerializer.isExisted();
wordSerializer.Write(currentVisitor);
wordSerializer.Close();*/

            spisok.addvisitor(currentVisitor);

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            /*fbd.SelectedPath = "E:\\Ucheba\\Interfaces\\Hotel_Project\\Hotel_Project\\bin\\Debug\\";
            if (fbd.ShowDialog() == DialogResult.Cancel)
                return;*/
            SaveFileDialog svf = new SaveFileDialog();
            svf.Filter = "XML files(*.xml)|*.xml|All files(*.*)|*.*";
            if (svf.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = svf.FileName;
            using (FileStream fs = new FileStream("Slaves.xml", FileMode.OpenOrCreate))
            {
                spisok = (All_Visitors)serial.Deserialize(fs);
            }
            using (FileStream fs = new FileStream("Slaves.xml", FileMode.OpenOrCreate))
            {
                serial.Serialize(fs, spisok);
            }
            using (StreamWriter wlog = new StreamWriter("log.txt", true))
            {
                wlog.WriteLine("{0} | {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString() + " | " + "В базу добавлен новый клиент - " + fio);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            roomnumber = textBox3.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            number = textBox2.Text;
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            capacityroom = radioButton1.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            fio = textBox1.Text;
        }
    }
}
