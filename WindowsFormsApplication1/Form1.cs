using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        PictureBox[][] cells = new PictureBox[9][];

        Image s1 = Image.FromFile(string.Format(@"{0}\..\..\images\Circle_Blue.png", Environment.CurrentDirectory));
        Image s2 = Image.FromFile(string.Format(@"{0}\..\..\images\Circle_Green.png", Environment.CurrentDirectory));
        Image s3 = Image.FromFile(string.Format(@"{0}\..\..\images\Circle_Orange.png", Environment.CurrentDirectory));
        Image s4 = Image.FromFile(string.Format(@"{0}\..\..\images\Circle_Red.png", Environment.CurrentDirectory));
        Image s5 = Image.FromFile(string.Format(@"{0}\..\..\images\Circle_Yellow.png", Environment.CurrentDirectory));

        bool cellIsSelected = false;
        int x = 0, y = 0; // Позиция ячейки, по которой кликаем
        int x1 = 0, y1 = 0; // Позиция картинки, которую требуется переместить
        int score = 0;
        bool gameIsOver = false;
        bool[,] cellIsNotEmpty = new bool[9, 9];


        public Form1()
        {
            InitializeComponent();
            creatingPlayingArea();
        }


        void checkAreaToMove(int x, int y)
        {
            if ((cells[x][y].BackColor == Color.LightGray) || (cells[x][y].BackColor == Color.Gold))
            {
                if ((x < 8) && !cellIsNotEmpty[x + 1, y] && (cells[x + 1][y].BackColor == Color.AliceBlue))
                { 
                    cells[x + 1][y].BackColor = Color.LightGray;
                    checkAreaToMove(x + 1, y);
                }

                if ((x > 0) && !cellIsNotEmpty[x - 1, y] && (cells[x - 1][y].BackColor == Color.AliceBlue))
                { 
                    cells[x - 1][y].BackColor = Color.LightGray;
                    checkAreaToMove(x - 1, y);
                }

                if ((y < 8) && !cellIsNotEmpty[x, y + 1] && (cells[x][y + 1].BackColor == Color.AliceBlue))
                { 
                    cells[x][y + 1].BackColor = Color.LightGray;
                    checkAreaToMove(x, y + 1);
                }

                if ((y > 0) && !cellIsNotEmpty[x, y - 1] && (cells[x][y - 1].BackColor == Color.AliceBlue))
                { 
                    cells[x][y - 1].BackColor = Color.LightGray;
                    checkAreaToMove(x, y - 1);
                }
            }
        }

        void creatingPlayingArea()
        {
            for (int i = 0; i < 9; i++)
            {
                cells[i] = new PictureBox[9];
                for (int j = 0; j < 9; j++)
                {
                    cells[i][j] = new PictureBox();
                    cells[i][j].Width = 50;
                    cells[i][j].Height = 50;
                    cells[i][j].Location = new Point(i * 51, j * 51);
                    cells[i][j].BackColor = Color.AliceBlue;
                    cells[i][j].MouseDown += Picture_MouseDown;
                    cells[i][j].Parent = this;

                }
            }
            
        }


        void deleteHorizontal()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if ((cells[i][j].Image == cells[i][j + 1].Image) && (cells[i][j + 1].Image == cells[i][j + 2].Image) && (cells[i][j + 2].Image == cells[i][j + 3].Image) &&
                        (cells[i][j + 3].Image == cells[i][j + 4].Image) && (cells[i][j + 4].Image != null))
                    {
                        for (int h = j; h < j + 5; h++)
                        {
                            cells[i][h].Image = null;
                            cellIsNotEmpty[i, h] = false;

                        }
                        score = score + 10;
                    }
                }
            }
        }
        
        void deleteVertical()
        {
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    if ((cells[i][j].Image == cells[i + 1][j].Image) && (cells[i + 1][j].Image == cells[i + 2][j].Image) && (cells[i + 2][j].Image == cells[i + 3][j].Image) && (cells[i + 3][j].Image == cells[i + 4][j].Image) && (cells[i + 4][j].Image != null))
                    {
                        for (int h = i; h < i + 5; h++)
                        {
                            cells[h][j].Image = null;
                            cellIsNotEmpty[h, j] = false;
                        }
                        score = score + 10;
                    }
                }
            }
        }


        void setDefaultBackgroundToCells()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                { cells[i][j].BackColor = Color.AliceBlue; }
            }
        }

        void spawn3NewPics()
        {
            Random rand = new Random();
            int i, j, type;
            int count = 0;

            int amountOfEmptyCells = 0;
            for (int n = 0; n < 9; n++)
            {
                for (int m = 0; m < 9; m++)
                {
                    if (!cellIsNotEmpty[n, m]) amountOfEmptyCells++;
                }
            }
            if (amountOfEmptyCells < 3)
            {
                gameIsOver = true;
                MessageBox.Show("Game Over!");
                return;
            }

            while (count != 3)
            {
                i = rand.Next(0, 9);
                j = rand.Next(0, 9);
                type = rand.Next(1, 6);

                if (cells[i][j].Image == null)
                {
                    switch (type)
                    {
                        case 1:
                            cells[i][j].Image = s1;
                            break;
                        case 2:
                            cells[i][j].Image = s2;
                            break;
                        case 3:
                            cells[i][j].Image = s3;
                            break;
                        case 4:
                            cells[i][j].Image = s4;
                            break;
                        case 5:
                            cells[i][j].Image = s5;
                            break;
                    }
                    
                    cellIsNotEmpty[i, j] = true;
                    count++;
                }

            }

        }



        private void Picture_MouseDown(object sender, MouseEventArgs e)
        {
            if (gameIsOver) return;

            PictureBox a = (PictureBox)sender;

            // Найдем и запомним позицию нажатой ячейки в сетке
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (cells[i][j].Location == a.Location)
                    {
                        x = i;
                        y = j;
                    }
                }
            }

            // Если ячейка содержала картику проверим куда ей можно ходить
            if (cells[x][y].Image != null)
            {
                if (cellIsSelected) return;
                x1 = x;
                y1 = y;
                cellIsSelected = true;

                cells[x][y].BackColor = Color.Gold;

                checkAreaToMove(x, y);

                return;
            }
            else
            {
                // Если нужно переместить картинку и эта ячейка подходит - перемещаем
                if ((cellIsSelected) && (cells[x][y].BackColor == Color.LightGray))
                {
                    cells[x][y].Image = cells[x1][y1].Image;
                    cells[x1][y1].Image = null;
                    cellIsNotEmpty[x1, y1] = false;
                    cellIsNotEmpty[x, y] = true;
                    cellIsSelected = false;

                    spawn3NewPics();
                    deleteHorizontal();
                    deleteVertical();
                    setDefaultBackgroundToCells();
                    textBox1.Text = Convert.ToString(score);
                    return;
                }
            }


        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            gameIsOver = false;
            for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
            {
                
                cells[i][j].Image = null;
                cellIsNotEmpty[i,j] = false;
                cells[i][j].Invalidate();
                
            }
            setDefaultBackgroundToCells();
            spawn3NewPics();
            cellIsSelected = false;
        }

        
       
    }
}
