using Blackjack.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blackjack
{
    public partial class HowToPlay : Form
    {
        private int currentPage = 0;
        private List<Image> screenshots = new List<Image>();  // Add your screenshots to this list

        public HowToPlay()
        {
            InitializeComponent();

            // Load screenshots into the list
            int numberOfScreenshots = 17; // Adjust this based on the actual number of screenshots
            for (int i = 1; i <= numberOfScreenshots; i++)
            {
                screenshots.Add(Properties.Resources.ResourceManager.GetObject($"screenshot{i}") as Image);

            }

            // Display the first screenshot
            if (screenshots.Count > 0)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = screenshots[currentPage];
                UpdatePageLabel();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            // Navigate to the previous page
            currentPage = (currentPage - 1 + screenshots.Count) % screenshots.Count;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = screenshots[currentPage];
            UpdatePageLabel();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Navigate to the next page
            currentPage = (currentPage + 1) % screenshots.Count;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = screenshots[currentPage];
            UpdatePageLabel();
        }

        private void UpdatePageLabel()
        {
            // Update the label to show current page and total pages
            lblPages.Text = $"Page {currentPage + 1} of {screenshots.Count}";
        }
    }
}
