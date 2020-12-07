using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace Landmark2
{
    public partial class Menu : Form
    {
        XLWorkbook wb;
        public Menu()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 2;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            try
            {
                wb = new XLWorkbook(System.IO.Directory.GetCurrentDirectory() + "\\LANDMARK2.xlsx");
                //wb = new XLWorkbook(Landmark2-data);

                int worksheetHowMany = 1;
                try
                {
                    IXLWorksheet sheetTmp;
                    while (true)
                    {
                        sheetTmp = wb.Worksheet(worksheetHowMany);
                        worksheetHowMany++;
                    }
                }
                catch { }
                String[] worksheetName = new String[worksheetHowMany];
                for (int i = 1; i < worksheetHowMany; i++)
                {
                    listView1.Items.Add(wb.Worksheet(i).Name);
                }
            }
            catch
            {
                MessageBox.Show("必要なファイルが見つかりませんでした。\n"+System.IO.Directory.GetCurrentDirectory() + "\\LANDMARK2.xlsx", "エラー",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            

        }
        private Items takeItems()
        {
            var worksheet = wb.Worksheet(listView1.FocusedItem.Text);

            int minNo = 0;
            int maxNo = 0;
            int noCorsor = 1;
            //要改善
            for (int i = 0; i < comboBox3.SelectedIndex + 1; i++)
            {
                minNo = noCorsor;
                while (!(worksheet.Cell(noCorsor, 1).GetString().Equals("")))
                {
                    maxNo = noCorsor;
                    noCorsor++;
                }
                noCorsor++;
            }

            int length = maxNo - minNo + 1;
            int[] numberBox = new int[length];
            for (int i = 0; i < length; i++)
            {
                numberBox[i] = minNo + i;
            }
            if (checkBox1.Checked)
            {

                numberBox = numberBox.OrderBy(i => Guid.NewGuid()).ToArray();
            }

            Items items = new Items();
            items.meaning = new String[length];
            items.spelling = new String[length];
            items.speak = new String[length];
            items.miss = new bool[length];

            //元の位置
            for (int i = 0; i < length; i++)
            {
                items.spelling[i] = worksheet.Cell(numberBox[i], 1).GetString();
                items.meaning[i] = worksheet.Cell(numberBox[i], 2).GetString();
                items.speak[i] = worksheet.Cell(numberBox[i], 3).GetString();
            }
            items.hint = comboBox1.SelectedIndex;
            items.repetitionMax = comboBox2.SelectedIndex + 1;
            items.random = checkBox1.Checked;
            return items;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            
            new Main(takeItems()).Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IXLWorksheet worksheet = wb.Worksheet(listView1.FocusedItem.Text);
            new Download(takeItems()).Show();
        }
    }
}
