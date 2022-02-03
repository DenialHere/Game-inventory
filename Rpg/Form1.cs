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
    public partial class Form1 : Form
    {
        GameEntities GameEnt = new GameEntities();
        int saleId;
        double saleamount = 1;
        int quantityToSell = 1; 
        int price;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timerUpdate.Start();
            timerSale.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6 };
            Label[] amounts = { labelAmount1, labelAmount2, labelAmount3, labelAmount4, labelAmount5 };
            int items = 0;


            //Reseting picturebox Inventory
            foreach (PictureBox pb in boxes)
            {
                pb.Image = null;
            }


            var listInventory = (from invent in GameEnt.Inventories
                                 join item in GameEnt.Items on invent.ItemId equals item.ItemId
                                 where invent.PlayerId == 1
                                 select invent).ToList<Inventory>();

            foreach (Inventory inv in listInventory)
            {
                if (inv.Quantity == 0)
                {
                    GameEnt.Inventories.Remove(inv);
                    GameEnt.SaveChanges();
                }
                else
                {
                    boxes[items].Image = Image.FromFile("../Images/" + inv.Item.Image);
                    boxes[items].Tag = inv.ItemId + "," + inv.Quantity;
                    amounts[items].Text = inv.Quantity.ToString();
                    items += 1;
                }


            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {


                string[] tag = pictureBox1.Tag.ToString().Split(',');
                int itemid = Convert.ToInt32(tag[0]);
                int quantity = Convert.ToInt32(tag[1]);
                labelAmountSell.Text = quantity.ToString();

                var item = GameEnt.Items.Single(n => n.ItemId == itemid);
                labelSellName.Text = item.ItemName;
                richTextBox1.Text = item.Description;
                labelPrice.Text = item.Price.ToString();
                pictureBox100.Image = Image.FromFile("../Images/" + item.Image);
                buttonSell.Tag = item.ItemId + "," + quantity;
            }
            catch
            {

            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {


                string[] tag = pictureBox2.Tag.ToString().Split(',');
                int itemid = Convert.ToInt32(tag[0]);
                int quantity = Convert.ToInt32(tag[1]);
                labelAmountSell.Text = quantity.ToString();

                var item = GameEnt.Items.Single(n => n.ItemId == itemid);
                labelSellName.Text = item.ItemName;
                richTextBox1.Text = item.Description;
                labelPrice.Text = item.Price.ToString();
                pictureBox100.Image = Image.FromFile("../Images/" + item.Image);
                buttonSell.Tag = item.ItemId + "," + quantity;
            }
            catch
            {

            }
        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {
            try 
        { 

            string[] tag = pictureBox3.Tag.ToString().Split(',');
            int itemid = Convert.ToInt32(tag[0]);
            int quantity = Convert.ToInt32(tag[1]);
            labelAmountSell.Text = quantity.ToString();

            var item = GameEnt.Items.Single(n => n.ItemId == itemid);
            labelSellName.Text = item.ItemName;
            richTextBox1.Text = item.Description;
            labelPrice.Text = item.Price.ToString();
            pictureBox100.Image = Image.FromFile("../Images/" + item.Image);
            buttonSell.Tag = item.ItemId + "," + quantity;
        }
            catch
            {

            }
        }

        private void buttonSell_Click(object sender, EventArgs e)
        {
            try
            {
                //Getting playerid
                int playerid = Convert.ToInt32(textBoxPlayerid.Text);
                //Seperating the item id and quantity from the tag
                string[] tag = buttonSell.Tag.ToString().Split(',');
                int itemid = Convert.ToInt32(tag[0]);
                int quantity = Convert.ToInt32(tag[1]);
                int price =0;
                var playerItem = GameEnt.Inventories.Single(n => n.ItemId == itemid && n.PlayerId == playerid);

                //Choosing the item that has been put up for sale to find the price and if its tradeable
                var item = GameEnt.Items.Single(n => n.ItemId == itemid);

                if (playerItem.Quantity == 0)
                {
                    MessageBox.Show("You have no items");
                }
                else
                {
                    if (item.Tradeable == 1)
                    {
                        var player = GameEnt.Players.Single(n => n.PlayerId == playerid);
                        //Quantity check
                        if (radioButton1.Checked == true && playerItem.Quantity >= 1)
                        {
                            price = Convert.ToInt32(item.Price);
                            playerItem.Quantity -= 1;
                        }
                        else if (radioButton5.Checked == true && playerItem.Quantity >= 5)
                        {
                            price = Convert.ToInt32(item.Price * 5);
                            playerItem.Quantity -= 5;
                        }
                        else if (radioButton10.Checked == true && playerItem.Quantity >= 10)
                        {
                            price = Convert.ToInt32(item.Price * 10);
                            playerItem.Quantity -= 10;
                        }
                        else if (radioButton50.Checked == true && playerItem.Quantity >= 50)
                        {
                            price = Convert.ToInt32(item.Price * 50);
                            playerItem.Quantity -= 50;
                        }
                        else if (radioButtonAll.Checked == true && playerItem.Quantity >=1)
                        {
                            int totalquantity = Convert.ToInt32(playerItem.Quantity);
                            price = Convert.ToInt32(item.Price * totalquantity);
                            playerItem.Quantity -= totalquantity;
                        }
                        if (item.ItemId == saleId)
                        {
                            price =  price * Convert.ToInt32(saleamount);
                            player.Money += price;
                        }
                        else
                        {
                            player.Money += price;
                        }

                        labelAmountSell.Text = playerItem.Quantity.ToString();

                        GameEnt.SaveChanges();
                        if (playerItem.Quantity <= 0)
                        {
                            richTextBox1.Text = "";
                            labelSellName.Text = "";
                            labelPrice.Text = "0";
                            buttonSell.Tag = null;
                            pictureBox100.Image = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("You can't sell this item!");
                    }
                    

                }
            }
            catch
            {

            }
            


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int playerid = Convert.ToInt32(textBoxPlayerid.Text);

            //




            //Updating money
            var player = GameEnt.Players.Single(n => n.PlayerId == playerid);
            labelMoney.Text = player.Money.ToString();

            //If there is nothing in sell tab then disable option to sell
            if (buttonSell.Tag == null)
            {
                buttonSell.Enabled = false;
            }
            else
            {
                buttonSell.Enabled = true;
            }


            PictureBox[] boxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6 };
            Label[] amounts = { labelAmount1, labelAmount2, labelAmount3, labelAmount4, labelAmount5 };
            int items = 0;
            var listInventory = (from invent in GameEnt.Inventories
                                 join item in GameEnt.Items on invent.ItemId equals item.ItemId
                                 where invent.PlayerId == playerid
                                 select invent).ToList<Inventory>();


            //Sale calculation
            if (buttonSell.Tag != null)
            {
                string[] tag = buttonSell.Tag.ToString().Split(',');
                int itemid = Convert.ToInt32(tag[0]);
                int quantity = Convert.ToInt32(tag[1]);
                var item = GameEnt.Items.Single(n => n.ItemId == itemid);
                if (itemid == saleId)
                {
                    labelPrice.Text = (item.Price * saleamount).ToString();
                }
                else
                {
                    if (radioButton1.Checked == true)
                    {
                        labelPrice.Text = (item.Price * 1).ToString();
                    }
                    if (radioButton5.Checked == true)
                    {
                        labelPrice.Text = (item.Price * 5).ToString();
                    }
                    if (radioButton10.Checked == true)
                    {
                        labelPrice.Text = (item.Price * 10).ToString();
                    }
                    if (radioButton50.Checked == true)
                    {
                        labelPrice.Text = (item.Price * 100).ToString();
                    }
                    if (radioButtonAll.Checked == true)
                    {
                        labelPrice.Text = (item.Price * quantity).ToString();
                        string name = "danial";
                        Console.WriteLine(name);
                    }
                }
            }






            foreach (Inventory inv in listInventory)
            {
                if (inv.Quantity == 0)
                {
                    amounts[items].Text = inv.Quantity.ToString();

                    //Reseting picturebox Inventory to remove deleted item
                    foreach (PictureBox pb in boxes)
                    {
                        pb.Image = null;
                        pb.Tag = null;
                    }

                    //Clearing previous slot when item is deleted
                    foreach (Label lab in amounts)
                    {
                        lab.Text = "0";
                    }


                    GameEnt.Inventories.Remove(inv);
                    GameEnt.SaveChanges();
                }
                else
                {
                    boxes[items].Image = Image.FromFile("../Images/" + inv.Item.Image);
                    boxes[items].Tag = inv.ItemId + "," + inv.Quantity;
                    amounts[items].Text = inv.Quantity.ToString();
                    items += 1;
                }


                //If theres an item in an inventory slot enable it else disable it
                foreach (PictureBox pb in boxes)
                {
                    if (pb.Tag == null)
                    {
                        pb.Enabled = false;
                    }
                    else
                    {
                        pb.Enabled = true;
                    }
                }


            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string[] tag = pictureBox4.Tag.ToString().Split(',');
            int itemid = Convert.ToInt32(tag[0]);
            int quantity = Convert.ToInt32(tag[1]);
            labelAmountSell.Text = quantity.ToString();

            var item = GameEnt.Items.Single(n => n.ItemId == itemid);
            labelSellName.Text = item.ItemName;
            richTextBox1.Text = item.Description;
            pictureBox100.Image = Image.FromFile("../Images/" + item.Image);
            buttonSell.Tag = item.ItemId + "," + quantity;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void timerSale_Tick(object sender, EventArgs e)
        {
            var listItem = (from item in GameEnt.Items
                            select item).ToArray<Item>();
            int totalItems = listItem.Count();


            Random rnd = new Random();
            int ChooseSale = rnd.Next(0, totalItems);

            if (ChooseSale == 0 || listItem[ChooseSale].Tradeable == 0)
            {
                labelSale.Text = null;
                saleId = 0;
                saleamount = 1;
            }
            else
            {
                saleamount = 1;
                labelSale.Text = "I'm looking for " + listItem[ChooseSale].ItemName + ".\nIm willing to pay a good price!";
                saleId = listItem[ChooseSale].ItemId;
                saleamount += Math.Round(rnd.NextDouble(), 1);
                if (buttonSell.Tag != null)
                {
                    string[] tag = buttonSell.Tag.ToString().Split(',');
                    int itemid = Convert.ToInt32(tag[0]);
                    int quantity = Convert.ToInt32(tag[1]);
                    if (itemid == listItem[ChooseSale].ItemId)
                    {
                        labelPrice.Text = "SALE";
                    }
                }

            }




        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Main main = new Main ();
            main.Show();
            this.Hide();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton50_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
