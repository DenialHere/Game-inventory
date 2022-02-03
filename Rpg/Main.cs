using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rpg.Models;

namespace Rpg
{
    public partial class Main : Form
    {
        GameEntities GameEnt = new GameEntities();
        int woodCounter = 0;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Inventory inv = new Inventory();
            timerWoodCounter.Start();
            try
            {
                var playerItem = GameEnt.Inventories.Single(n => n.PlayerId == 1 && n.ItemId == 5);
                if (playerItem != null)
                {
                    playerItem.Quantity += 1;
                    GameEnt.SaveChanges();
                    labelwood.Text += "+1 wood\n";
                    woodCounter += 1;
                }
                else
                {

                }
            }
            catch
            {
                inv.ItemId = 5;
                inv.PlayerId = 1;
                inv.Quantity = 1;
                GameEnt.Inventories.Add(inv);
                GameEnt.SaveChanges();
                woodCounter += 1;
                labelwood.Text += "+1 wood\n";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 fs = new Form1();
            fs.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Collects logs which sell for 2 gold.");
        }

        private void timerWoodCounter_Tick(object sender, EventArgs e)
        {
            try
            {


                var playerItem = GameEnt.Inventories.Single(n => n.PlayerId == 1 && n.ItemId == 5);
                if (woodCounter >= 12)
                {
                    labelwood.Text = "";
                    playerItem.Quantity += 5;
                    GameEnt.SaveChanges();
                    labelwood.Text = "+5 wood\n";
                }
                else if (woodCounter >= 8)
                {
                    playerItem.Quantity += 2;
                    GameEnt.SaveChanges();
                    labelwood.Text = "+2 wood\n";
                }
                else
                {
                    labelwood.Text = "";
                }

                woodCounter = 0;
            }
            catch
            {

            }
        }
    }
}
