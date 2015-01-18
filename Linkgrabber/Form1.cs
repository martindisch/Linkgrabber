using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace Linkgrabber
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();

            string urlAddress = textBox1.Text;

            WebClient client = new WebClient();
            String htmlCode = client.DownloadString(urlAddress);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlCode);

            String hostUrl = new Uri(urlAddress).Host;

            // Get episode count
            htmlCode = client.DownloadString("http://" + hostUrl + "/" + doc.DocumentNode.SelectNodes("//a[@href]")[9].GetAttributeValue("href", "nothing"));
            doc.LoadHtml(htmlCode);
            HtmlNodeCollection coll = doc.DocumentNode.SelectNodes("//td[@class='epnum']");
            HtmlNode last = coll[coll.Count - 1];
            int episodeCount = Convert.ToInt32(last.InnerHtml.Replace(" ", ""));

            progressBar1.Maximum = episodeCount + 1;
            progressBar1.Step = 1;
            progressBar1.Value = 1;

            String nextAddress = urlAddress.Replace("http://", "").Replace(hostUrl, "");
            for (int i = 0; i < episodeCount; i++)
            {
                doc.LoadHtml(client.DownloadString("http://" + hostUrl + "/" + nextAddress));
                textBox2.Text += doc.DocumentNode.SelectNodes("//iframe[@src]")[0].GetAttributeValue("src", "nothing") + "\r\n";
                nextAddress = doc.DocumentNode.SelectNodes("//a[@href]")[10].GetAttributeValue("href", "nothing");
                progressBar1.PerformStep();
            }

            MessageBox.Show("All links grabbed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Get next episode
            //textBox2.Text = doc.DocumentNode.SelectNodes("//a[@href]")[10].GetAttributeValue("href", "nothing");
            
            // Get episode count
            /*HtmlNodeCollection coll = doc.DocumentNode.SelectNodes("//td[@class='epnum']");
            HtmlNode last = coll[coll.Count - 1];
            textBox2.Text = last.InnerHtml.Replace(" ", "");*/

            // Get host url
            //textBox2.Text = new Uri(urlAddress).Host;

            // Get download link
            //textBox2.Text = doc.DocumentNode.SelectNodes("//iframe[@src]")[0].GetAttributeValue("src", "nothing");
        }
    }
}
