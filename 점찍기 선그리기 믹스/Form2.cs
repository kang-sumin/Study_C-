using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 점찍기_선그리기_믹스
{
    public partial class Form2 : Form
    {

        public Color DialogPenColor { get; set; }
        private int DialogShape;
        

        public int DialogPenWidth
        {
            get { return Convert.ToInt32(comboBox1.Text); }
            set { comboBox1.Text = value.ToString(); }
        }

        public int Shape
        {
            get
            {
                if (radioButton1.Checked) DialogShape = 0;
                if (radioButton2.Checked) DialogShape = 1;
                if (radioButton3.Checked) DialogShape = 2;
                return DialogShape;
            }
            set
            {
                DialogShape = value;
                if (DialogShape == 0) radioButton1.Checked = true;
                if (DialogShape == 1) radioButton2.Checked = true;
                if (DialogShape == 2) radioButton3.Checked = true;
            }
        }


        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            for(int i = 2; i <= 10; i += 2)
            {
                comboBox1.Items.Add(i);
            }
            //comboBox1.SelectedIndex = iDialogPenWidth / 2 - 1;
            comboBox1.Text = DialogPenWidth.ToString();

            //모양 선택된것으로 체크유지 해주기
            if (DialogShape == 0)
                radioButton1.Checked = true;
            else if (DialogShape == 1)
                radioButton2.Checked = true;
            else if (DialogShape == 2)
                radioButton3.Checked = true;

            //컬러 값 표시
            hScrollBar1.Value = DialogPenColor.R;
            hScrollBar2.Value = DialogPenColor.G;
            hScrollBar3.Value = DialogPenColor.B;
            textBox1.Text = DialogPenColor.R.ToString();
            textBox2.Text = DialogPenColor.G.ToString();
            textBox3.Text = DialogPenColor.B.ToString();

            label8.Invalidate();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //스크롤바 움직임에 따라 컬러 표기해 주기
            DialogPenColor = Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
            button1.BackColor= Color.FromArgb(hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value);
            textBox1.Text = hScrollBar1.Value.ToString();
            textBox2.Text = hScrollBar2.Value.ToString();
            textBox3.Text = hScrollBar3.Value.ToString();
            label8.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogPenWidth = (((ComboBox)sender).SelectedIndex + 1) * 2;
            label8.Invalidate();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            //펜 굵기 사용자가 작성한것으로 지정해줌
            DialogPenWidth = int.Parse(comboBox1.Text);
            label8.Invalidate();
        }

        private void label8_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush brc = new SolidBrush(DialogPenColor);
            Pen p = new Pen(DialogPenColor);
            int x = ((Label)sender).Width / 2 - (DialogPenWidth * 10 / 2);
            int y = ((Label)sender).Height / 2 - (DialogPenWidth * 10 / 2);
            if (DialogShape == 0) //선
            {
                //label8==((Label)sender)
                e.Graphics.DrawLine(new Pen(DialogPenColor, DialogPenWidth), 0, label8.Height/2, ((Label)sender).Width, ((Label)sender).Height/2);
            }
            else if (DialogShape == 1) //원
            {
                e.Graphics.DrawEllipse(p, x, y, DialogPenWidth * 10, DialogPenWidth * 10);
                e.Graphics.FillEllipse(brc, x, y, DialogPenWidth * 10, DialogPenWidth * 10);
            }
            else if (DialogShape == 2) //사각형
            {
                e.Graphics.DrawRectangle(p, x, y, DialogPenWidth * 10, DialogPenWidth * 10);
                e.Graphics.FillRectangle(brc, x, y, DialogPenWidth * 10, DialogPenWidth * 10);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DialogShape = 0;
            label8.Invalidate();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            DialogShape = 1;
            label8.Invalidate();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            DialogShape = 2;
            label8.Invalidate();
        }

       
    }
}
