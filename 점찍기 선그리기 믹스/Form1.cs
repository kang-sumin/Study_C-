using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections; //ArrayList 만들기 위해서 필요함
using System.IO; //파일 스트림 쓰기위해 추가해야함
using System.Runtime.Serialization.Formatters.Binary; //Serialization 하기위해 꼭 있어야 함


namespace 점찍기_선그리기_믹스
{
    public partial class Form1 : Form
    {
        private LinkedList<CMyData> total_lines; //링크드 리스트 total_lines 생성
        CMyData data; //인스턴스 생성
        private Color iCurrentPenColor; //현재 펜 색깔
        private int iCurrentPenWidth; //현재 펜 굵기
        private int iCurrentShape; //현재 모양

        public Form1()
        {
            //그림판 초기 설정
            total_lines = new LinkedList<CMyData>();
            iCurrentPenColor = Color.Black;
            iCurrentPenWidth = 1;
            iCurrentShape = 0;
            InitializeComponent();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                data = new CMyData(); //객체 생성 : data에 CMyData 내용들을 저장하겠다
                data.pColor = iCurrentPenColor; //현재 색 저장
                data.Size = iCurrentPenWidth; //현재 굵기 저장
                data.Shape = iCurrentShape; //현재 모양 저장
                data.Point = new Point(e.X, e.Y); //현재 x,y좌표 저장 
                data.AR.Add(data.Point); //CMyData의 ArrayList AR에 좌표 저장

                Pen p = new Pen(data.pColor); //펜 객체 생성
                SolidBrush brc = new SolidBrush(data.pColor); //채워주는 브러시 객체 생성
                Graphics G = this.CreateGraphics();
                if (data.Shape == 1)
                {
                    G.DrawEllipse(p, data.Point.X, data.Point.Y, data.Size * 10, data.Size * 10);
                    G.FillEllipse(brc, data.Point.X, data.Point.Y, data.Size * 10, data.Size * 10);
                }
                else if (data.Shape == 2)
                {
                    G.DrawRectangle(p, data.Point.X, data.Point.Y, data.Size * 10, data.Size * 10);
                    G.FillRectangle(brc, data.Point.X, data.Point.Y, data.Size * 10, data.Size * 10);
                }

            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Capture && e.Button == MouseButtons.Left && data.Shape == 0)
            {
                //CreateGraphics() : Paint 이벤트 처리기가 아닌 다른 곳에서 그리기를 하고자 할 때 사용
                Graphics G = CreateGraphics();
                G.DrawLine(new Pen(data.pColor, data.Size), data.Point.X, data.Point.Y, e.X, e.Y);
                data.Point = new Point(e.X, e.Y);
                data.AR.Add(data.Point);
                //사용한 도화지는 닫아줘야 함
                G.Dispose();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            total_lines.AddLast(data);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (CMyData cdata in total_lines)
            {
                Pen p = new Pen(cdata.pColor); //펜 객체 생성
                SolidBrush brc = new SolidBrush(cdata.pColor); //브러시 객체 생성
                if (cdata.Shape == 0) //선
                {
                    for (int i = 1; i < cdata.AR.Count; i++)
                    {
                        e.Graphics.DrawLine(new Pen(cdata.pColor, cdata.Size), (Point)cdata.AR[i - 1], (Point)cdata.AR[i]);
                    }
                }
                else if (cdata.Shape == 1) //원
                {
                    e.Graphics.DrawEllipse(p, cdata.Point.X, cdata.Point.Y, cdata.Size * 10, cdata.Size * 10);
                    e.Graphics.FillEllipse(brc, cdata.Point.X, cdata.Point.Y, cdata.Size * 10, cdata.Size * 10);
                }
                else if (cdata.Shape == 2) //사각형
                {
                    e.Graphics.DrawRectangle(p, cdata.Point.X, cdata.Point.Y, cdata.Size * 10, cdata.Size * 10);
                    e.Graphics.FillRectangle(brc, cdata.Point.X, cdata.Point.Y, cdata.Size * 10, cdata.Size * 10);
                }
            }
        }

        //메뉴창에서 색 바꿀 경우
        private void 빨강ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentPenColor = Color.Red;
        }

        private void 초록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentPenColor = Color.Green;
        }

        private void 파랑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentPenColor = Color.Blue;
        }

        //대화상자클릭시 Form2와 데이터 주고받아야함
        private void 대화상자ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form2와 데이터를 주고받기 위해 객체 생성
            Form2 dlg = new Form2();
            dlg.DialogPenColor = iCurrentPenColor;  //set
            dlg.DialogPenWidth = iCurrentPenWidth;
            dlg.Shape = iCurrentShape;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                iCurrentPenColor = dlg.DialogPenColor; //get
                iCurrentPenWidth = dlg.DialogPenWidth;
                iCurrentShape = dlg.Shape;
            }
            dlg.Dispose(); //대화상자 끝내기
        }

        private void 저장하기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = @"C:\temp";
            saveFileDialog1.Title = "파일 저장하기";
            saveFileDialog1.Filter = "Bin 파일|*.bin|모든 파일|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, total_lines); //fs에 total_lines를 바이트 단위로 포장시킨다
                fs.Close();
            }
        }

        private void 불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\temp"; //안내책자
            openFileDialog1.Title = "파일 불러오기"; //대화창 이름
            openFileDialog1.Filter = "Bin 파일|*.bin|모든 파일|*.*"; //파일형식

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                    BinaryFormatter bf = new BinaryFormatter();
                    total_lines = (LinkedList<CMyData>)bf.Deserialize(fs);
                    fs.Close();
                    Invalidate();
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("지정한 파일이 없습니다.");
                }
            }
        }

        private void 선ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentShape = 0;
        }

        private void 원ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentShape = 1;
        }

        private void 사각형ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iCurrentShape = 2;
        }

        //현재 상태 메뉴에서 체크상태로 유지시키기
        private void menuStrip1_MenuActivate(object sender, EventArgs e)
        {
            빨강ToolStripMenuItem.Checked = (iCurrentPenColor == Color.Red);
            초록ToolStripMenuItem.Checked = (iCurrentPenColor == Color.Green);
            파랑ToolStripMenuItem.Checked = (iCurrentPenColor == Color.Blue);
            선ToolStripMenuItem.Checked = (iCurrentShape == 0);
            원ToolStripMenuItem.Checked = (iCurrentShape == 1);
            사각형ToolStripMenuItem.Checked = (iCurrentShape == 2);
        }
    }

    //CMyData 클래스
    [Serializable]
    public class CMyData
    {
        private Point point;
        private Color penCol;
        private int size, shape;
        private ArrayList Ar;

        public CMyData()
        {
            Ar = new ArrayList();
        }

        public Color pColor
        {
            get { return penCol; }  
            set { penCol = value; } 
        }
        public Point Point
        {
            get { return point; }
            set { point = value; }
        }
        public int Size 
        {
            get { return size; }
            set { size = value; }
        }
        public int Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        public ArrayList AR
        {
            get { return Ar; }
        }

    }
}
