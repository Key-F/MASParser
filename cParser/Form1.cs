using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;
using System.Globalization;

namespace cParser
{
    
    public partial class Form1 : Form
    {
        string htmlString; // Сюда считыаем из файла html код
        List<comment> Com = new List<comment>(); // Список комментариев
        string PostAuthor; // Автор поста, ключевых коментариев
         
        public Form1()
        {
            InitializeComponent();
            DateTime thisDay = DateTime.Today;
            
            textBox4.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            for(int i = Com.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < Com[i].vlozhennost / 10; j++)
                {
                    richTextBox1.AppendText("---");
                }
                if (Com[i].author == PostAuthor)
                    richTextBox1.AppendText(Com[i].author, Color.DarkViolet);
                else
                    richTextBox1.AppendText(Com[i].author, Color.DimGray);
                richTextBox1.AppendText(" " + Com[i].date + " : ");
                if ((Com[i].vlozhennost == 0) && (Com[i].author == PostAuthor))
                {
                    richTextBox1.AppendText(Com[i].text, Color.Red);
                }
                else
                richTextBox1.AppendText(Com[i].text, Color.Blue);
                richTextBox1.ScrollToCaret();
                richTextBox1.AppendText("\n");
                
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //List <comment> Com = new List<comment>();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            openFileDialog1.Filter = "html files (*.html)| *.html" ;
     
            openFileDialog1.ShowDialog();
            Com.Clear();
            
            StreamReader s = new StreamReader(openFileDialog1.FileName, true);
            //string line;
                
           /* while ((line = s.ReadLine()) != null)
            {

                richTextBox1.Text = richTextBox1.Text + line;
            } */
            htmlString =  new  StreamReader(openFileDialog1.FileName).ReadToEnd();
            document.LoadHtml(htmlString);

            HtmlNodeCollection comments = document.DocumentNode.SelectNodes("//div[@class='text']");
            HtmlNodeCollection names = document.DocumentNode.SelectNodes("//b[@class='user_name']");
            HtmlNodeCollection date = document.DocumentNode.SelectNodes("//div[@class='descr']");
            HtmlNodeCollection relation = document.DocumentNode.SelectNodes("//div[@class='preloadMe comment pr ']");
            HtmlNodeCollection mainguy = document.DocumentNode.SelectNodes("//span[@class='krohi']");
            //HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//*[@id='id_text_q_1273059']");
            string findguy1 = mainguy[0].OuterHtml.Substring(mainguy[0].OuterHtml.IndexOf("m/blog/") + 7);
            string temp = ">";
            //string temp = "</a>";
            string findguy2 = findguy1.Substring(0, findguy1.IndexOf(temp) - 1); // @"/" чет не работает
            PostAuthor = findguy2;
            int i = names.Count;
            if (comments != null)
            {
                //foreach (HtmlNode link in collection)
                do
                {
                    comment newc = new comment();
                    newc.text = comments[i].InnerText;
                    newc.author = names[i - 1].InnerText;
                    newc.date = date[i - 1].InnerText.Replace("\t", "");
                    string[] words = newc.date.Split(new char[] { ' ', ',' });
                    words[words.Length - 5] = newc.deletebukvi(words[words.Length - 5]); // Убрали страну от числа
                    newc.date = words[words.Length - 5] +" " + words[words.Length - 4] + " " + words[words.Length - 3]+ " " + words[words.Length - 1];
                    newc.setDateOfPost(words);
                    string orezrel1 = relation[i - 1].OuterHtml.Substring(relation[i - 1].OuterHtml.IndexOf(':') + 1); // Убрали до двоеточия
                    string orezrel2 = orezrel1.Substring(0, orezrel1.IndexOf("px;")); // Убрали после px;

                    newc.vlozhennost = Convert.ToInt32(orezrel2);

                    Com.Add(newc);
                    //richTextBox1.Text = richTextBox1.Text + newc.author + " " + newc.date;
                    //richTextBox1.Text = richTextBox1.Text + "\n";
                    // MessageBox.Show(link.InnerText); //string target = link.Attributes["href"].Value;
                    i--;
                }
                while (i != 0);
                MessageBox.Show("Done!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime End = DateTime.ParseExact(textBox4.Text, "dd-MM-yyyy hh:mm:ss", new CultureInfo("en-US"));
            //End.Date.ToString("yyyy-M-dd HH:mm:ss.fff");
            for (int i = Com.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < Com[i].vlozhennost / 10; j++)
                {
                    richTextBox1.AppendText("---");
                }
                if (Com[i].author == PostAuthor)
                    richTextBox1.AppendText(Com[i].author, Color.DarkViolet);
                else
                    richTextBox1.AppendText(Com[i].author, Color.DimGray);
                TimeSpan ts = End - Com[i].dateOfPost;
               // if (Convert.ToInt32(ts.Hours) > Convert.ToInt32(textBox1.Text))
                richTextBox1.AppendText(" " + Com[i].date + " : ");
                if ((Com[i].vlozhennost == 0) && (Com[i].author == PostAuthor))
                {
                    richTextBox1.AppendText(Com[i].text, Color.Red);
                }
                else
                    richTextBox1.AppendText(Com[i].text, Color.Blue);
                if (i != 0)
                {

                    if ((Com[i].author != PostAuthor) && (Com[i].vlozhennost == 20) && (Com[i - 1].vlozhennost != 20)&& (Com[i - 1].author == PostAuthor) && (Convert.ToDouble(ts.TotalHours) > Convert.ToInt32(textBox1.Text)))
                        richTextBox1.AppendText(" Possible Winner", Color.Firebrick);
                }
                else if ((Com[i].author != PostAuthor) && (Com[i].vlozhennost != 20) && (Convert.ToDouble(ts.TotalHours) > Convert.ToInt32(textBox1.Text)))
                    richTextBox1.AppendText(" Possible Winner", Color.Firebrick);

                richTextBox1.ScrollToCaret();
                richTextBox1.AppendText("\n");

            }

        }

        
    }
    
}
