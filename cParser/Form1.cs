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

            HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//div[@class='text']");
            if (collection != null)
            {
                foreach (HtmlNode link in collection)
                {
                    comment newc = new comment();
                    newc.text = link.InnerText;
                    Com.Add(newc);
                    richTextBox1.Text = richTextBox1.Text + link.InnerText;
                   // MessageBox.Show(link.InnerText); //string target = link.Attributes["href"].Value;
                }
            }
        }
    }
}
