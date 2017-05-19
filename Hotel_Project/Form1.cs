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
    public partial class Form1 : Form
    {
        public string fio, number, roomnumber, rangroom, capacityroom, howlong; //for textboxs
        public string fname; //adress for button
        public int first = 0; //index for button "next" and "previous"
        //public int index;
        All_Visitors spisok = new All_Visitors();
        XmlSerializer serial = new XmlSerializer(typeof(All_Visitors));
        public AllUsers listusers = new AllUsers();
        public XmlSerializer serial1 = new XmlSerializer(typeof(AllUsers));
        public Form1()
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

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            capacityroom = radioButton2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Visitor currentVisitor = new Visitor(fio, number, roomnumber, Convert.ToUInt16(capacityroom), rangroom, Convert.ToUInt16(howlong));

            ExcelSerializer excelSerializer = new ExcelSerializer();
            excelSerializer.isExisted();
            excelSerializer.Write(currentVisitor);
            excelSerializer.Close();

            WordSerializer wordSerializer = new WordSerializer();
            wordSerializer.isExisted();
            wordSerializer.Write(currentVisitor);
            wordSerializer.Close();
            spisok.addvisitor(currentVisitor);

            SaveFileDialog svf = new SaveFileDialog();
            svf.Filter = "XML files(*.xml)|*.xml|All files(*.*)|*.*";
            if (svf.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = svf.FileName;
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
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "XML files(*.xml)|*.xml|All files(*.*)|*.*";
            if (opf.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = opf.FileName;
            fname = filename;
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                spisok = (All_Visitors)serial.Deserialize(fs);

                textBox1.Text = spisok.visitors[0].FIO;
                textBox2.Text = spisok.visitors[0].Phone_Number;
                textBox4.Text = spisok.visitors[0].Room_Number;
                comboBox1.Text = spisok.visitors[0].Room_Rang;
                switch (spisok.visitors[0].Room_Capacity)
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
                textBox3.Text = Convert.ToString(spisok.visitors[0].Duration);

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
            string filetext = File.ReadAllText(filename);
            textBox5.Text = filetext;
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
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream("LoginList.xml", FileMode.OpenOrCreate))
            {
                listusers = (AllUsers)serial1.Deserialize(fs);
            }
            Form2 form = new Form2();
            form.Show();
            /*bool admin = false;
            List<User> listok = new List<User>();
            listok = listusers.users;
            foreach (var user in listok)
            {
                if (user.Login == "Administrator" && user.Password == "7777777")
                    admin = true;
                Form2 form = new Form2();
                form.Show();
                break;
                //MessageBox.Show("Вы не имеете доступа в данный раздел");
            }
            if (admin == false)
            {
                MessageBox.Show("Вы не имеете доступа в данный раздел");
            }*/
        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //using (FileStream log = new FileStream("log.txt", FileMode.Append))
            //{
                using (StreamWriter wlog = new StreamWriter("log.txt", true))
                {
                    wlog.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                }

                /*StreamWriter wlog = new StreamWriter(log);
                wlog.WriteLine(DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                wlog.WriteLine("Мать");
                wlog.Write("Мать");
                wlog.*/
           // }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            capacityroom = radioButton3.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            howlong = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            roomnumber = textBox4.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            rangroom = comboBox1.Text;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            capacityroom = radioButton1.Text;
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
    /*class UserDataBase: Room
    {
        string FIO, Phone_Number;
        public UserDataBase(string fio, string number, ushort room_number, ushort floor, ushort capacity, string rang, ushort duration) : base(room_number, floor, capacity, rang, duration)
        {
            FIO = fio;
            Phone_Number = number;
            Room_Number = room_number;
            Floor = floor;
            Room_Capacity = capacity;
            Room_Rang = rang;
            Duration = duration;
        }
    }

    class ListofRoom
    {
        public Room[] allrooms = { new Room(1, 1, 2, "Стандартный"), new Room(1, 2, 2, "Стандартный"), new Room(1, 3, 2, "Стандартный") };
    }*/

    /*public ListofRoom(Room[] array)
    {
        allrooms = new Room[array.Length];
        for (int i = 0; i < array.Length; i++)
            allrooms[i] = array[i];
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this;
    }
    public RoomEnum GetEnumerator()
    {
        return new RoomEnum(allrooms);
    }
}*/



    /*public class User_Info
   {
       string FIO, Phone_Number;
       public User_Info (string fio, string number)
       {
           FIO = fio;
           Phone_Number = number;
       }
   }
   public class Room_Info
   {
       public const UInt16 Standard = 1000;
       public const UInt16 Lux = 5000;
       public const UInt16 SuperLux = 10000;

       UInt16 Room_Number;
       UInt16 Floor;
       static UInt16 Room_Capacity;
       static String Room_Rang;

       public Room_Info(UInt16 room_number, UInt16 floor, UInt16 capacity, String rang)
       {
           Room_Number = room_number;
           Floor = floor;
           Room_Capacity = capacity;
           Room_Rang = rang;
       }
       public static int Counting_Value()
       {
           int a = 0;
           switch(Room_Rang)
           {
               case "Стандартный":
                   a = Room_Capacity * Standard ;
                   break;
               case "Люкс":
                   a = Room_Capacity * Lux;
                   break;
               case "Суперлюкс":
                   a = Room_Capacity * SuperLux;
                   break;
           }
           return a;
       }
   }
   public class Room_Deal
   {
       UInt16 Duration;
       public int Counting_Cost()
       {
           int Cost;
           Cost = Duration*(Room_Info.Counting_Value());
           return Cost;
       }
   }
   public class DataBase
   {
       public DataBase()
       {

       }
   }*/
}
