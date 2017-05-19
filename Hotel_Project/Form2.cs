using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace Hotel_Project
{
    public partial class Form2 : Form
    {
        public AllUsers listusers = new AllUsers();
        public XmlSerializer serial = new XmlSerializer(typeof(AllUsers));
        public Form2()
        {
            InitializeComponent();
            using (FileStream fs = new FileStream("LoginList.xml", FileMode.OpenOrCreate))
            {
                listusers = (AllUsers)serial.Deserialize(fs);
            }
            List<User> listok = new List<User>();
            listok = listusers.users;
            foreach (var user in listok)
            {
                dataGridView1.Rows.Add(user.Login, user.Hash);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentRow.Index;
            //dataGridView1.Rows.Add();
            User nu = new User(((string)dataGridView1.Rows[rowindex].Cells[0].Value), ((string)dataGridView1.Rows[rowindex].Cells[1].Value));
            listusers.users.Add(nu);
            dataGridView1.Rows[rowindex].Cells[1].Value = nu.Hash;
            using (FileStream fs = new FileStream("LoginList.xml", FileMode.Create))
            {
                serial.Serialize(fs, listusers);
            }
            using (StreamWriter wlog = new StreamWriter("log.txt", true))
            {
                wlog.WriteLine("{0} | {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString() + " | " + "Администратор добавил нового пользователя - " + nu.Login);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            listusers.users[rowindex].Hash=((string)dataGridView1.Rows[rowindex].Cells[1].Value);
            using (FileStream fs = new FileStream("LoginList.xml", FileMode.OpenOrCreate))
            {
                serial.Serialize(fs, listusers);
            }
            using (StreamWriter wlog = new StreamWriter("log.txt", true))
            {
                wlog.WriteLine("{0} | {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString() + " | " + "Администратор сменил пароль у пользователя - " + listusers.users[rowindex].Login);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            using(StreamWriter wlog = new StreamWriter("log.txt", true))
            {
                wlog.WriteLine("{0} | {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString() + " | " + "Администратор удалил пользователя - " + listusers.users[rowindex].Login);
            }
            dataGridView1.Rows.RemoveAt(rowindex);
            /*for(int i=0; i<dataGridView1.Rows.Count; i++)
            {
                listusers.users[i].Login = (string)dataGridView1.Rows[i].Cells[0].Value;
                listusers.users[i].Password = (string)dataGridView1.Rows[i].Cells[1].Value;
            }*/
            listusers.users.RemoveAt(rowindex);
            using (FileStream fs = new FileStream("LoginList.xml", FileMode.Create))
            {
                serial.Serialize(fs, listusers);
            }
        }
    }
}
