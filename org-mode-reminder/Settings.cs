using System;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace orgmodereminder
{
	public class Settings : Form
	{
		public static string configFile = "config.xml";
		private Label status;

		public Settings ()
		{
			this.Icon = new Icon("app.ico", 40, 40);

			status = new Label();
			status.Location = new Point(75, 0);
			status.Size = new Size(200,40);
			status.Font = new Font(status.Font.FontFamily.Name, 20);
			status.ForeColor = Color.Green;
			this.Controls.Add(status);

			Button selectFilesButton = new Button();
			selectFilesButton.Location = new Point(100,230);
			selectFilesButton.Click += new EventHandler(selectFilesButton_Click);
			selectFilesButton.Text = "Set Org Files";
			this.Controls.Add(selectFilesButton);
		}

		private void selectFilesButton_Click (object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog ();
			fileDialog.Multiselect = true;
			fileDialog.Filter = "All Files|*.*";
			
			if (fileDialog.ShowDialog () == DialogResult.OK) 
			{
				CreateXMLDocument (configFile);
				foreach (string fileName in fileDialog.FileNames)
				{
					AddOrgFileToConfig (fileName);
				}
				status.Text = "Complete!";
			}
		}

		private void CreateXMLDocument(string filePath)
		{
			XmlTextWriter xtw = new XmlTextWriter(filePath, System.Text.Encoding.UTF8);
			xtw.WriteStartDocument();
			xtw.WriteStartElement("files");
			xtw.WriteEndDocument();
			xtw.Close();
		}

		private void AddOrgFileToConfig(string pathToOrgFile)
		{
			XmlDocument xd = new XmlDocument();
			System.IO.FileStream fs = new System.IO.FileStream(configFile, System.IO.FileMode.Open);
			xd.Load(fs);

			XmlElement orgFile = xd.CreateElement("file");
			XmlText path = xd.CreateTextNode(pathToOrgFile);
			orgFile.AppendChild(path);

			xd.DocumentElement.AppendChild(orgFile);

			fs.Close();
			xd.Save(configFile);
		}

		public List<string> getOrgFilesPath ()
		{
			List<string> OrgFilePaths = new List<string> ();

			XmlDocument xd = new XmlDocument ();
			System.IO.FileStream fs = new System.IO.FileStream (configFile, System.IO.FileMode.Open);
			xd.Load (fs);

			XmlNodeList list = xd.GetElementsByTagName ("file");

			for (int i = 0; i < list.Count; i++)
			{
				OrgFilePaths.Add(list.Item(i).InnerText);
			}

			return OrgFilePaths;
		}
	}
}
