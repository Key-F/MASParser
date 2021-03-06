﻿using System;
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
            DateTime thisDay = DateTime.Now; // Today -> Now
            
            textBox4.Text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"); // hh -> HH для корректного отображния часов
            
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
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // OpenFileDialog openFileDialog1 = new OpenFileDialog();
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                openFileDialog1.Filter = "html files (*.html)| *.html";

                //openFileDialog1.ShowDialog();
                Com.Clear();

                StreamReader s = new StreamReader(openFileDialog1.FileName, true);
                //string line;

                /* while ((line = s.ReadLine()) != null)
                 {

                     richTextBox1.Text = richTextBox1.Text + line;
                 } */
                htmlString = new StreamReader(openFileDialog1.FileName).ReadToEnd();
                document.LoadHtml(htmlString);

                HtmlNodeCollection comments = document.DocumentNode.SelectNodes("//div[@class='text']");
                HtmlNodeCollection names = document.DocumentNode.SelectNodes("//b[@class='user_name']");
                HtmlNodeCollection date = document.DocumentNode.SelectNodes("//div[@class='descr']");
                HtmlNodeCollection relation = document.DocumentNode.SelectNodes("//div[@class='preloadMe comment pr ']"); // не все прогружаются
                HtmlNodeCollection mainguy = document.DocumentNode.SelectNodes("//span[@class='krohi']");
                //HtmlNodeCollection collection = document.DocumentNode.SelectNodes("//*[@id='id_text_q_1273059']");
                string findguy1 = mainguy[0].OuterHtml.Substring(mainguy[0].OuterHtml.IndexOf("/blog/") + 6);
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
                        newc.date = words[words.Length - 5] + " " + words[words.Length - 4] + " " + words[words.Length - 3] + " " + words[words.Length - 1];
                        newc.setDateOfPost(words);
                        try
                        {
                            string orezrel1 = relation[i - 1].OuterHtml.Substring(relation[i - 1].OuterHtml.IndexOf(':') + 1); // Убрали до двоеточия
                            string orezrel2 = orezrel1.Substring(0, orezrel1.IndexOf("px;")); // Убрали после px;

                            newc.vlozhennost = Convert.ToInt32(orezrel2);
                        }
                        catch
                        {
                            MessageBox.Show("При сохранении страницы нужно выбрать [только html]");
                            return;

                        }
                        //string orezrel2 = orezrel1.Substring(0, orezrel1.IndexOf("px;")); // Убрали после px;

                        // newc.vlozhennost = Convert.ToInt32(orezrel2);

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
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) // WinCheck
        {
            DateTime End;
            try
            {
                End = DateTime.ParseExact(textBox4.Text, "dd-MM-yyyy HH:mm:ss", new CultureInfo("en-US"));
            }
            catch 
            {
                MessageBox.Show("Неверный формат времени окончания");
                return;
            }
            //End.Date.ToString("yyyy-M-dd HH:mm:ss.fff");
            richTextBox1.Clear();
             //(checkBox1.CheckState == CheckState.Checked)
            
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
                TimeSpan tsnow = DateTime.Today - Com[i].dateOfPost;

               // DateTime ts2 = new DateTime((End - Com[i].dateOfPost).Ticks);
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

                    if ((Com[i].author != PostAuthor) && (Com[i].vlozhennost >= 20) && (Com[i - 1].vlozhennost != 20) && (Com[i - 1].author == PostAuthor) && (Convert.ToDouble(tsnow.TotalHours) > Convert.ToInt32(textBox1.Text)) && (checkBox1.CheckState == CheckState.Checked))
                    {

                        richTextBox1.AppendText(" С момента ставки прошло: ", Color.Black);
                        richTextBox1.AppendText(Convert.ToString((int)tsnow.TotalHours), Color.Fuchsia);
                        richTextBox1.AppendText(" часов", Color.Fuchsia);
                        richTextBox1.AppendText(" Possible Winner", Color.Firebrick);
                    }
                    if ((Com[i].author != PostAuthor) && (Com[i].vlozhennost >= 20) && /*(Com[i - 1].vlozhennost != 20) && */(Com[i - 1].dateOfPost > End) && (Convert.ToDouble(ts.TotalHours) > 0) && (checkBox2.CheckState == CheckState.Checked))
                    {

                        richTextBox1.AppendText(" До окончания: ", Color.Black);
                        richTextBox1.AppendText(Convert.ToString((int)ts.TotalHours), Color.Fuchsia);
                        richTextBox1.AppendText(" часов", Color.Fuchsia);
                        richTextBox1.AppendText(" Possible Winner", Color.Firebrick);
                    }
                }
                else // Обработка последнего коммента
                {
                    if ((Com[i].author != PostAuthor) && (Com[i].vlozhennost >= 20) && (Convert.ToDouble(tsnow.TotalHours) > Convert.ToInt32(textBox1.Text)) && (checkBox1.CheckState == CheckState.Checked))
                    {
                        richTextBox1.AppendText(" С момента ставки прошло: ", Color.Black);
                        richTextBox1.AppendText(Convert.ToString((int)tsnow.TotalHours), Color.Fuchsia);
                        richTextBox1.AppendText(" часов", Color.Fuchsia);
                        richTextBox1.AppendText(" Possible Winner", Color.Firebrick);
                    }
                    if ((Com[i].author != PostAuthor) && (Com[i].vlozhennost >= 20) && (Convert.ToDouble(tsnow.TotalHours) > 0) && (checkBox2.CheckState == CheckState.Checked))
                    {

                        richTextBox1.AppendText(" До окончания: ", Color.Black);
                        richTextBox1.AppendText(Convert.ToString((int)ts.TotalHours), Color.Fuchsia);
                        richTextBox1.AppendText(" часов", Color.Fuchsia);
                        richTextBox1.AppendText(" Possible Winner", Color.Firebrick);
                    }
                }
                richTextBox1.ScrollToCaret();
                richTextBox1.AppendText("\n");

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("https://github.com/Key-F/MASParser", "Made By Key_F", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        
    }
    
}
