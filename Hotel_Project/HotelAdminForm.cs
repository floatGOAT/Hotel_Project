using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Hotel_Project
{
    public partial class HotelAdminForm : Form
    {
        ExcelSerializer excelSerializer = new ExcelSerializer();
        WordSerializer wordSerializer = new WordSerializer();
        All_Visitors spisok = new All_Visitors();
        XmlSerializer serial = new XmlSerializer(typeof(All_Visitors));

        public void fill(Visitor visitor)
        {
            textBox1.Text = visitor.FIO;
            textBox2.Text = visitor.Phone_Number;
            textBox4.Text = visitor.Room_Number;
            comboBox1.Text = visitor.Room_Rang;
            switch (visitor.Room_Capacity)
            {
                case 1:
                    radioButton1.Checked = true;
                    break;
                case 2:
                    radioButton2.Checked = true;
                    break;
                case 4:
                    radioButton3.Checked = true;
                    break;
            }
            textBox3.Text = Convert.ToString(visitor.Duration);
        }

        public string fio, number, roomnumber, rangroom, capacityroom, howlong; //for textboxs
        public string fname; //adress for button
        public int first = 0; //index for button "next" and "previous"
        public AllUsers listusers = new AllUsers();
        public XmlSerializer serial1 = new XmlSerializer(typeof(AllUsers));

        private static string filename = "slaves.xml";

        public HotelAdminForm()
        {
            InitializeComponent();

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            fio = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            number = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            howlong = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            roomnumber = textBox4.Text;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            capacityroom = radioButton1.Text;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            capacityroom = radioButton2.Text;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            capacityroom = radioButton3.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            rangroom = comboBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Visitor currentVisitor = new Visitor(fio, number, roomnumber, Convert.ToUInt16(capacityroom), rangroom, Convert.ToUInt16(howlong));
            /*
            SaveFileDialog svf = new SaveFileDialog();
            svf.Filter = "XML files(*.xml)|*.xml|All files(*.*)|*.*";
            if (svf.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = svf.FileName;*/
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                spisok = (All_Visitors)serial.Deserialize(fs);
            }
            spisok.addvisitor(currentVisitor);

            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                serial.Serialize(fs, spisok);
            }
            using (StreamWriter wlog = new StreamWriter("log.txt", true))
            {
                wlog.WriteLine("{0} | {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString() + " | " + "В базу добавлен новый клиент - " + fio);
            }
            /*DataSet ds = new DataSet();
            ds.WriteXml(filename);*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "XML files(*.xml)|*.xml|All files(*.*)|*.*";
            if (opf.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = opf.FileName;*/
            fname = filename;
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                spisok = (All_Visitors)serial.Deserialize(fs);
            }

            textBox_Finding.Visible = true;
            delete_button.Visible = true;
            fill(spisok.visitors[0]);

            if (spisok.visitors.Count > 1)
            {
                button3.Visible = true;
            }
            else
                button3.Visible = false;

            if (spisok.visitors.Count > 1 && first != 0)
            {
                button4.Visible = true;
            }
            else
                button4.Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream(fname, FileMode.OpenOrCreate))
            {
                All_Visitors newvisitors = (All_Visitors)serial.Deserialize(fs);
                int n = newvisitors.visitors.Count;
                if (first < (n - 1))
                {
                    first++;
                    button3.Visible = true;
                    button4.Visible = true;
                }
                textBox1.Text = newvisitors.visitors[first].FIO;
                textBox2.Text = newvisitors.visitors[first].Phone_Number;
                textBox4.Text = newvisitors.visitors[first].Room_Number;
                comboBox1.Text = newvisitors.visitors[first].Room_Rang;
                switch (newvisitors.visitors[first].Room_Capacity)
                {
                    case 1:
                        radioButton1.Checked = true;
                        break;
                    case 2:
                        radioButton2.Checked = true;
                        break;
                    case 4:
                        radioButton3.Checked = true;
                        break;
                }
                textBox3.Text = Convert.ToString(newvisitors.visitors[first].Duration);
                if (first < (n - 1) && first > 0)
                {
                    button3.Visible = true;
                    button4.Visible = true;
                }
                else
                {
                    button3.Visible = false;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream(fname, FileMode.OpenOrCreate))
            {
                All_Visitors newvisitors = (All_Visitors)serial.Deserialize(fs);
                int n = newvisitors.visitors.Count;
                if (first > 0)
                {
                    first--;
                    button4.Visible = true;
                    button3.Visible = true;
                }
                textBox1.Text = newvisitors.visitors[first].FIO;
                textBox2.Text = newvisitors.visitors[first].Phone_Number;
                textBox4.Text = newvisitors.visitors[first].Room_Number;
                comboBox1.Text = newvisitors.visitors[first].Room_Rang;
                switch (newvisitors.visitors[first].Room_Capacity)
                {
                    case 1:
                        radioButton1.Checked = true;
                        break;
                    case 2:
                        radioButton2.Checked = true;
                        break;
                    case 4:
                        radioButton3.Checked = true;
                        break;
                }
                textBox3.Text = Convert.ToString(newvisitors.visitors[first].Duration);
                if (first > 0 && first < (n - 1))
                {
                    button3.Visible = true;
                    button4.Visible = true;
                }
                else
                {
                    button4.Visible = false;
                }
            }
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            spisok.visitors.RemoveAt(first);
            if (first > 0)
                first--;
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                serial.Serialize(fs, spisok);
            }
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                spisok = (All_Visitors)serial.Deserialize(fs);
            }
        }

        private void textBox_Finding_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Finding.Text == null)
                return;
            foreach (var visitor in spisok.visitors)
            {
                if (visitor.FIO.StartsWith(textBox_Finding.Text))
                {
                    fill(visitor);
                    first = spisok.visitors.IndexOf(visitor);
                    return;
                }
            }
            MessageBox.Show("Постояльца с такими данными не найдено");
            textBox_Finding.Text = null;
        }

        private void Excel_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Visitor currentVisitor = new Visitor(fio, number, roomnumber, Convert.ToUInt16(capacityroom), rangroom, Convert.ToUInt16(howlong));
            excelSerializer.isExisted();
            excelSerializer.Write(currentVisitor);
            excelSerializer.Close();
        }

        private void Word_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Visitor currentVisitor = new Visitor(fio, number, roomnumber, Convert.ToUInt16(capacityroom), rangroom, Convert.ToUInt16(howlong));
            wordSerializer.isExisted();
            wordSerializer.Write(currentVisitor);
            wordSerializer.Close();
        }

        private void About_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void toolStripExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
    [Serializable]
    public class Visitor
    {
        public List<Visitor> visitors;

        public const UInt16 Standard = 1000;
        public const UInt16 Lux = 5000;
        public const UInt16 SuperLux = 10000;

        public string FIO, Phone_Number;
        public string Room_Number;
        public UInt16 Room_Capacity;
        public String Room_Rang;
        public UInt16 Duration;

        public Visitor(string fio, string phone_number, string room_number, UInt16 capacity, String rang, UInt16 duration)
        {
            FIO = fio;
            Phone_Number = phone_number;
            Room_Number = room_number;
            Room_Capacity = capacity;
            Room_Rang = rang;
            Duration = duration;
        }
        public Visitor()
        {

        }
        public int Counting_Value()
        {
            int a = 0;
            switch (Room_Rang)
            {
                case "Стандартный":
                    a = Room_Capacity * Standard * Duration;
                    break;
                case "Люкс":
                    a = Room_Capacity * Lux * Duration;
                    break;
                case "Суперлюкс":
                    a = Room_Capacity * SuperLux * Duration;
                    break;
            }
            return a;
        }
        public void show()
        {
            Console.WriteLine("Гость: " + FIO + "\n" + "Номер телефона: " + Phone_Number + "\n" + "Номер комнаты: " + Room_Number + "\n" + "Класс номера: " + Room_Rang + "\n" + "Количество мест: " + Room_Capacity + "\n" + "Количество дней: " + Duration + "\n" + "Выручка: " + Counting_Value() + "\n");
        }
    }
    [Serializable]
    public class All_Visitors
    {
        public List<Visitor> visitors = new List<Visitor>();
        public All_Visitors()
        {

        }
        public void addvisitor(string fio, string phone_number, string room_number, UInt16 capacity, String rang, UInt16 duration)
        {
            Visitor visitor = new Visitor(fio, phone_number, room_number, Convert.ToUInt16(capacity), rang, duration);
            visitor.Counting_Value();
            visitor.show();
            visitors.Add(visitor);
        }
        public void addvisitor(Visitor visitor)
        {
            visitor.Counting_Value();
            visitor.show();
            visitors.Add(visitor);
        }
    }
}