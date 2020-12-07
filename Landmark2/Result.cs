using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace Landmark2
{
    public partial class Result : Form
    {
        Items class1;
        public Result(Items class1In)
        {
            InitializeComponent();
            this.Activate();
            this.class1 = class1In;
            label2.Text = (class1.meaning.Length-class1.getMissLength())+"";
            label4.Text = class1.getMissLength() + "";
            
            comboBox1.SelectedIndex = class1.hint;
            comboBox2.SelectedIndex = class1.repetitionMax - 1;
            checkBox1.Checked = class1In.random;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Main(makeClass1Out_missedItems()).Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Items class1Ecport = makeClass1Out_missedItems();
            try
            {
                XLWorkbook wb = new XLWorkbook(System.IO.Directory.GetCurrentDirectory() + "LANDMARK2.xlsx");
                String newWorkSheetName = "ミス" + DateTime.Now.ToString("MMddHHmm");
                wb.AddWorksheet(newWorkSheetName);
                var worksheet = wb.Worksheet(newWorkSheetName);
                for (int i = 0; i < class1Ecport.meaning.Length; i++)
                {
                    worksheet.Cell(i + 1, 1).Value = class1Ecport.spelling[i];
                    worksheet.Cell(i + 1, 2).Value = class1Ecport.meaning[i];
                    worksheet.Cell(i + 1, 3).Value = class1Ecport.speak[i];

                }
                wb.Save();
                MessageBox.Show("書き込み完了しました。");
            }
            catch
            {

                MessageBox.Show("エラーが発生しました。");
            }
            
        }
        private Items makeClass1Out_missedItems()
        {

            int[] miss = new int[class1.getMissLength()];
            //missにclass1の間違えた番号を入れる
            int j = 0;
            for (int i = 0; i < miss.Length; i++)
            {
                while (class1.miss[j] == false)
                {
                    j++;
                }
                miss[i] = j;
                j++;
            }
            if (checkBox1.Checked)
            {
                miss = miss.OrderBy(i => Guid.NewGuid()).ToArray();
            }
            String[] meaning = new String[miss.Length];
            String[] spelling = new String[miss.Length];
            String[] speak = new String[miss.Length];
            for (int i = 0; i < miss.Length; i++)
            {
                meaning[i] = class1.meaning[miss[i]];
                spelling[i] = class1.spelling[miss[i]];
                speak[i] = class1.speak[miss[i]];
            }
            Items class1Out= new Items();
            class1Out.meaning = meaning;
            class1Out.spelling = spelling;
            class1Out.speak = speak;
            class1Out.miss = new bool[miss.Length];
            class1Out.random = checkBox1.Checked;
            class1Out.hint = comboBox1.SelectedIndex;
            class1Out.repetitionMax = comboBox2.SelectedIndex + 1;
            return class1Out;
        }
        
    }
}
