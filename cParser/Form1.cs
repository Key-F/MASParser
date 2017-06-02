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

namespace cParser
{
    public partial class Form1 : Form
    {
        string htmlString;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            string htmlString = "<html>blabla</html>";
            document.LoadHtml(htmlString);
            HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//a");
            foreach (HtmlNode link in collection)
            {
                string target = link.Attributes["href"].Value;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List <comment> Com = new List<comment>();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            openFileDialog1.Filter = "html files (*.html)| *.html" ;
     
            openFileDialog1.ShowDialog();
            
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
            //HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//*[@id='id_text_q_1273059']");
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

                    Com.Add(newc);
                    richTextBox1.Text = richTextBox1.Text + newc.author + " " + newc.date;
                    richTextBox1.Text = richTextBox1.Text + "\n";
                    // MessageBox.Show(link.InnerText); //string target = link.Attributes["href"].Value;
                    i--;
                }
                while (i != 0);
            }
        }
    }
}
