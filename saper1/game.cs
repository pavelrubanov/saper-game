using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace saper1
{
    public class my_buttons : Button
    {
        public int x;
        public int y;
        public bool is_bomb;
        public int near_bombs = 0;
        public bool is_flag=false;
        public void make_flag()
        {
            if (is_flag == false && this.Enabled == true)
            {
                this.Text = "!";
                is_flag = true;
            }
            else
            {
                if (is_flag == true && this.Enabled == true)
                {
                    this.Text = "";
                    is_flag = false;
                }
            }
  
        }
        public my_buttons()
        {
            Size = new Size(game.cellsize, game.cellsize);
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
        }
    }
    public partial class game : Form
    {
        public static int cellsize=40;
        public static int field_size;
        public static int bombs;
        public my_buttons[,] buttons;
        private bool first_click=true;
        private int open_cells = 0;
        private void InitBombs(my_buttons button)
        {
            Random random = new Random();
            List <my_buttons> arr = new List<my_buttons> ();
            for (int i = 0; i < field_size; i++)
            {
                for (int j=0;j<field_size; j++)
                {
                    arr.Add(buttons[i, j]);
                }
            }
            arr.Remove(button);
            for (int i=0;i<bombs;i++)
            {
                int k=random.Next()%arr.Count;
                arr[k].is_bomb = true;
                //arr[k].Text = "*";
                arr.Remove(arr[k]);
            }
            
            for (int i=0;i<field_size;i++)
            {
                for (int j=0;j<field_size;j++)
                {
                    if (buttons[i,j].is_bomb==false)
                    {
                        int near_bombs = 0;
                        if (i > 0 && j > 0) near_bombs += Convert.ToInt32(buttons[i - 1, j - 1].is_bomb);
                        if (i > 0 ) near_bombs += Convert.ToInt32(buttons[i - 1, j].is_bomb);
                        if (i > 0 && j < field_size - 1) near_bombs += Convert.ToInt32(buttons[i - 1, j + 1].is_bomb);
                        if (j < field_size - 1) near_bombs += Convert.ToInt32(buttons[i, j + 1].is_bomb);
                        if (j > 0) near_bombs += Convert.ToInt32(buttons[i, j - 1].is_bomb);
                        if (i < field_size - 1) near_bombs += Convert.ToInt32(buttons[i+1, j].is_bomb);
                        if (i < field_size - 1 && j > 0) near_bombs += Convert.ToInt32(buttons[i + 1, j - 1].is_bomb);
                        if (i < field_size - 1 && j < field_size - 1) near_bombs += Convert.ToInt32(buttons[i + 1, j + 1].is_bomb);
                        buttons[i, j].near_bombs = near_bombs;
                    }
                }
            }
        }
        private void Show_field()
        {
            for (int i=0;i<field_size;i++)
            {
                for (int j=0;j<field_size;j++)
                {
                    if (buttons[i, j].is_bomb) buttons[i, j].Text = "*";
                    else buttons[i, j].Text = buttons[i, j].near_bombs.ToString();

                    buttons[i, j].Enabled = false;
                }
            }
        }
        private void Init()
        {
            buttons = new my_buttons[field_size, field_size];
            for (int i = 0; i < field_size; i++)
            {
                for (int j = 0; j < field_size; j++)
                {
                    my_buttons button = new my_buttons();
                    button.x = i;
                    button.y = j;
                    button.Location = new Point(i * cellsize, j * cellsize);
                    button.MouseUp += new MouseEventHandler(press);
                    buttons[i,j] = button;
                    Controls.Add(button);
                }
            }
            this.Width = cellsize * field_size + 20;
            this.Height = cellsize * field_size + 50;
        }
        public void press (object sender, MouseEventArgs e)
        {

        }
        public void OnButtonPressedMouse(object sender, MouseEventArgs e)
        {
            Button pressedButton = sender as Button;
            my_buttons button=new my_buttons();
            for (int i=0;i<field_size;i++)
            {
                for (int j=0;j<field_size;j++)
                {
                    if (buttons[i,j]==pressedButton)  button=buttons[i,j];
                }
            }
                

             if (e.Button.ToString()=="Left")
             {
                if (first_click == true)
                 {
                     first_click = false;
                     InitBombs(button);
                 }
                 Open_cell(button);

             }
             if (e.Button.ToString()=="Right")
             {
                button.make_flag();
             }
        }
        internal void Open_cell (my_buttons button)
        {
            if (button.Enabled==true)
            {
                button.Enabled = false;
                if (button.is_bomb)
                {
                    Show_field();
                    MessageBox.Show("ПОРАЖЕНИЕ");
                }
                else
                {
                    open_cells++;
                    if (open_cells == field_size * field_size - bombs)
                    {
                        Show_field();
                        MessageBox.Show("ПОБЕДА");
                    }
                    else
                    {
                        button.Text = button.near_bombs.ToString();
                        if (button.near_bombs == 0)
                        {
                            int i = button.x;
                            int j = button.y;
                            if (i > 0 && j > 0) Open_cell(buttons[i - 1, j - 1]);
                            if (i > 0) Open_cell(buttons[i - 1, j]);
                            if (i > 0 && j < field_size - 1) Open_cell(buttons[i - 1, j + 1]);
                            if (j < field_size - 1) Open_cell(buttons[i, j + 1]);
                            if (j > 0) Open_cell(buttons[i, j - 1]);
                            if (i < field_size - 1) Open_cell(buttons[i + 1, j]);
                            if (i < field_size - 1 && j > 0) Open_cell(buttons[i + 1, j - 1]);
                            if (i < field_size - 1 && j < field_size - 1) Open_cell(buttons[i + 1, j + 1]);
                        }

                    }
                }
            }
            
        }
        public game(int n, int k)
        {
            field_size = n;
            bombs = k;
            InitializeComponent();
            Init();

        }
    }
}
