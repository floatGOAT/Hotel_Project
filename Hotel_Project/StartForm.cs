using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace Hotel_Project
{
    public partial class StartForm : Form
    {
        public string login, password;
        public AllUsers listusers = new AllUsers();
        public XmlSerializer serial = new XmlSerializer(typeof(AllUsers));
        
        public StartForm()
        {
            InitializeComponent();
            using (FileStream fs = new FileStream("LoginList.xml", FileMode.OpenOrCreate))
            {
                listusers = (AllUsers)serial.Deserialize(fs);
            }
            /*List<User> listok = new List<User>();
            listok = listusers.users;
            foreach (var user in listok)
            {
                dataGridView1.Rows.Add(user.Login, user.Password);
            }*/
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            login = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listusers.entry(login, password);
            //this.Hide();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            label4.Visible=false;
            button1.Visible = false;
            button2.Visible = true;
            label5.Visible = true;
            label3.Visible = false;
            linkLabel1.Visible = false;
            textBox1.Text = "";
            textBox2.Text = "";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listusers.registeruser(login, password);
            using (StreamWriter wlog = new StreamWriter("log.txt", true))
            {
                wlog.WriteLine("{0} | {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString() + " | " + "Зарегистрирован новый пользователь - " + login);
            }
            //MessageBox.Show("Пользователь успешно зарегистрирован", "Мои поздравления");
            label4.Visible = true;
            button1.Visible = true;
            button2.Visible = false;
            label5.Visible = false;
            label3.Visible = true;
            linkLabel1.Visible = true;
            textBox1.Text = "";
            textBox2.Text = "";
            using (FileStream fs = new FileStream("LoginList.xml", FileMode.OpenOrCreate))
            {
                serial.Serialize(fs, listusers);
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            BaseAdminForm form = new BaseAdminForm();
            form.Show();
        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            password = textBox2.Text;
        }
        /*public static void example()
        {
          MessageBox.Show(encrypt("puerta1627"));
        }*/
    }
    [Serializable]
    public class User
    {
        public List<Visitor> users;
        public string Login, Password, Hash;
        public User(string login, string password)
        {
            Login = login;
            Password = password;
            Hash = encrypt(password);
        }
        public User()
        {

        }
        public static String encrypt(String input)
        {
            MD5 mdHasher = MD5.Create();
            byte[] data = mdHasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        static bool VerifyMd5Hash(MD5 mdHasher, string input, string hash)
        {
            string hashOfInput = encrypt(input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    [Serializable]
    public class AllUsers
    {
        public List<User> users = new List<User>();
        public AllUsers()
        {

        }
        public void registeruser(string log, string pass)
        {
            bool exist = false;
            foreach (var user in users)
            {
                if (log == user.Login && pass == user.Password)
                {
                    exist = true;
                    MessageBox.Show("Пользователь с таким именем уже зарегистрирован!");
                    using (StreamWriter wlog = new StreamWriter("log.txt", true))
                    {
                        wlog.WriteLine("{0} | {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString() + " | " + "Произведена попытка регистрации пользователя с уже сущестующим именем - " + log);
                    }
                    break;
                }
            }
            if (exist == false)
            {
                User user = new User(log, pass);
                users.Add(user);
                MessageBox.Show("Пользователь успешно зарегистрирован", "Регистрация");
            }
        }
        public void entry(string log, string pass)
        {
            /*using (FileStream fs = new FileStream("LoginList.xml", FileMode.OpenOrCreate))
            {
                listofusers = (AllUsers)serial.Deserialize(fs);
            }*/
            bool exist=true;
            /*if (users.Count == 0)
            {
                exist = false;
                MessageBox.Show("Неверные данные входа");
            }*/
            foreach(var user in users)
            {
                if (log == user.Login && pass == user.Password && log=="Administrator" && pass == "7777777")
                {
                    exist = true;
                    //MessageBox.Show("Вход администратора успешно выполнен");
                    BaseAdminForm form1 = new BaseAdminForm();
                    form1.Show();
                    using (StreamWriter wlog = new StreamWriter("log.txt", true))
                    {
                        wlog.WriteLine("{0} | {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString() + " | " + "Выполнен вход под именем - " + log);
                    }
                    break;
                }
                if (log == user.Login && pass == user.Password)
                {
                    exist = true;
                    //MessageBox.Show("Вход пользователя успешно выполнен");
                    HotelAdminForm form1 = new HotelAdminForm();
                    form1.Show();
                    using (StreamWriter wlog = new StreamWriter("log.txt", true))
                    {
                        wlog.WriteLine("{0} | {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString() + " | " + "Выполнен вход под именем - " + log);
                    }
                    break;
                }
                if (log == user.Login && pass != user.Password)
                {
                    exist = false;
                }
                if (log != user.Login && pass == user.Password)
                {
                    exist = false;
                }
                if (log != user.Login && pass != user.Password)
                {
                    //MessageBox.Show("Неверные данные входа");
                    exist = false;
                }
            }
            if (exist == false || log==null || pass==null || users.Count == 0)
            {
                MessageBox.Show("Неверные данные входа");
                using (StreamWriter wlog = new StreamWriter("log.txt", true))
                {
                    wlog.WriteLine("{0} | {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString() + " | " + "Неудавшаяся попытка войти в систему под именем - " + log);
                }
            }
        }
    }
}
