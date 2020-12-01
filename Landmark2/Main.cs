using System;
using System.Windows.Forms;

namespace Landmark2
{
    public partial class Main : Form
    {

        private System.Media.SoundPlayer player = null;
        int turn = -1;
        int repetition = 0;
        Items items;
        //public SpVoice voiceSpeech = null;
        public Main(Items items)
        {
            //ファンクションキーの設定
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Function_KeyDown);


            this.items = items;
            this.repetition = items.repetitionMax;
            //voiceSpeech = new SpVoice();
            InitializeComponent();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (turn == -1) return;
            if (textBox1.Text.Equals(items.spelling[turn]))
            {
                textBox1.Text = "";
                speak();
                //voiceSpeech.Speak(items.spelling[turn],SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFIsXML);
                change();
            }
        }
        private void speak()
        {
            if (player != null)
            {
                player.Stop();
                player.Dispose();
                player = null;
            }
            player = new System.Media.SoundPlayer(System.IO.Directory.GetCurrentDirectory() + "\\landmark2-data\\wav\\" + items.speak[turn] + ".wav");
            //非同期再生する
            try
            {
                player.Play();
            }
            catch { }

        }
        private Boolean change()
        {
            if (repetition == items.repetitionMax)
            {
                turn++;
                repetition = 0;
                if (items.meaning.Length == turn)
                {
                    this.Close();
                    new Result(items).Show();
                    return true;
                }
            }
            label1.Text = items.meaning[turn];
            String spelling = items.spelling[turn];
            switch (items.hint)
            {
                case 0:
                    label2.Text = spelling;
                    break;
                case 1:
                    String textHint = spelling.Substring(0, 1);
                    for (int i = 1; i < spelling.Length; i++)
                    {
                        switch (spelling.Substring(i, 1))
                        {
                            case " ":
                                textHint += " ";
                                break;
                            case "~":
                                textHint += "~";
                                break;
                            default:
                                textHint += "?";
                                break;
                        }
                    }
                    label2.Text = textHint;
                    break;
                case 2:
                    label2.Text = "";
                    break;
            }
            label3.Text = "あと" + (items.spelling.Length - turn);
            repetition++;
            return false;
        }

        private void Function_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1 || e.KeyCode == Keys.F2 || e.KeyCode == Keys.F3) { 

                e.Handled = true;
                if (turn == -1)
                {
                    //ENTERを押してスタート　の時
                    textBox1.Text = "";
                    change();
                }
                else if (e.KeyCode == Keys.F1 && turn < 1)
                {
                    //一問目に戻る（F1）が押されたとき
                }
                else
                {
                    label2.Text = items.spelling[turn];
                    items.miss[turn] = true;
                    //textBox1.Text = "";
                    if (e.KeyCode == Keys.F1)
                    {
                        repetition = items.repetitionMax;
                        items.miss[turn] = false;
                        items.miss[turn - 1] = false;
                        turn = turn - 2;
                        change();
                    }
                    else if (e.KeyCode == Keys.F3)
                    {
                        items.miss[turn] = false;
                        change();
                    }
                    else
                    {
                        speak();
                    }
                }
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;
                if (turn == -1)
                {
                    //ENTERを押してスタート　の時
                    textBox1.Text = "";
                    change();
                }
                else
                {
                    label2.Text = items.spelling[turn];
                    items.miss[turn] = true;
                    //textBox1.Text = "";
                    if (e.KeyChar == (Char)Keys.Escape)
                    {
                        change();
                    }
                    else
                    {
                        speak();
                    }


                }

            }

        }
    }
}