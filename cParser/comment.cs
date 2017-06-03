using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace cParser
{
    class comment
    {
        public string author;
        public string text {get; set;}
        public string date;
        public DateTime dateOfPost;
        public int vlozhennost;

        public void setDateOfPost(string[] words)
        {
            int month = 0;
            switch (words[words.Length - 4])
            {
                case "июн":
                case "Jun":
                    month = 6;
                    break;
                case "июл":
                case "Jul":
                    month = 7;
                    break;
                case "авг":
                case "Aug":
                    month = 8;
                    break;
                case "сен":
                case "Sep":
                    month = 9;
                    break;
                case "окт":
                case "Oct":
                    month = 10;
                    break;
                case "ноя":
                case "Nov":
                    month = 11;
                    break;
                case "дек":
                case "Dec":
                    month = 12;
                    break;
                case "янв":
                case "Jan":
                    month = 1;
                    break;
                case "фев":
                case "Feb":
                    month = 2;
                    break;
                case "мар":
                case "Mar":
                    month = 3;
                    break;
                case "апр":
                case "Apr":
                    month = 4;
                    break;
                case "мая":
                case "May":
                    month = 5;
                    break;
                default:
                    month = 1;
                    MessageBox.Show("Error");
                    break;
            }
            int year = Convert.ToInt32(words[words.Length - 3]);
            int day = Convert.ToInt32(words[words.Length - 5]);
            string[] time = words[words.Length - 1].Split(new char[] { ':' });
            int hour = Convert.ToInt32(time[0]);
            int min = Convert.ToInt32(time[1]);
            int sec = Convert.ToInt32(time[2]);

            dateOfPost = new DateTime(year, month, day, hour, min, sec);

        }

        public string deletebukvi (string str )
        {
             string str2 = "";
             char[] mas= str.ToCharArray();
             foreach (char h in mas)
               {
                  if (char.IsDigit(h) == true)
                  {
                      str2 += h;
                  }
               }
             return str2;     
        }
            
    }
}
