using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Landmark2
{
    public partial class Download : Form
    {
        private Items items;
        static String cantFindWebPage = "ウェブページが見つからなかった：";
        static String cantDownloadMp3 = "mp3ファイルダウンロードに失敗した：";
        static String notFound = "適切なmp3を発見できなかった：";

        private void writeLog(String logText)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.SelectionLength = 0;
            textBox1.SelectedText =  logText + "\r\n";
            
        }
        private async Task<String> downloadSource(String word)
        {
            Uri webUri = new Uri("https://ejje.weblio.jp/content/" + word);
            //Uri webUri = new Uri("https://en.hatsuon.info/word/" + word); 
            WebClient client = new WebClient();
            try
            {
                // URIからダウンロードしたデータのストリームを開く
                Stream st = client.OpenRead(webUri);

                Encoding enc = Encoding.GetEncoding("UTF-8");
                // 文字コードを指定して、StreamReaderクラスのインスタンスを作る
                StreamReader sr = new StreamReader(st, enc);
                // ストリームから文字列を末尾まで読み込むのがsr.readtoend()
                return await pullMp3URL(sr.ReadToEnd(), word);
                //pullMp3URL2(sr.ReadToEnd(), word);
            }
            catch (Exception ex)
            {
                cantFindWebPage += word + "/";
                return word + "ウェブページが見つからなかった：" + ex.Message;
            }
        }
        private async Task<string> pullMp3URL(String sauce, String word)
        {

            //mp3該当URLを抽出
            Match matchedObject = null;
            String[] targetURL = new String[] { "https://weblio.hs.llnwd.net/e7/img/dict/kenej/audio/.*?mp3",
                "http://www.weblio.jp/img/dict/kenej/audio/.*?mp3","https://weblio.hs.llnwd.net/e8/audio/.*?mp3" };
            for (int i = 0; i < targetURL.Length; i++)
            {
                if (i == 0)
                {
                    matchedObject = Regex.Match(sauce, targetURL[i]);
                }
                else if (matchedObject.Success == false)
                {
                    matchedObject = Regex.Match(sauce, targetURL[i]);
                    if (System.IO.Path.GetFileNameWithoutExtension(matchedObject.Value).Length == "6b929c2bf76d6781740a62a7624b0051".Length) { matchedObject = Regex.Match("", "わけのわからないmp3がダウンロードされるのを防ぎます"); }
                }
            }
            if (matchedObject.Success == false)
            {
                notFound += word + "/";
                return word + "適切なmp3を発見できなかった";
            }
            //*?できるだけ短く
            //*できるだけながく
            try
            {
                //mp3ファイルのDL
                WebClient wc = new WebClient();
                writeLog(matchedObject.Value);//?????????????????????????????????????????????????????????????非同期にしたけどこのままでもいいの？
                await Task.Run(() =>
                {
                    wc.DownloadFile(matchedObject.Value, path + "\\" + word + ".mp3");
                });
                wc.DownloadFile(matchedObject.Value, path + "\\" + word + ".mp3");
                wc.Dispose();
                return word + "をゲットした!";
            }
            catch (Exception ex)
            {
                cantDownloadMp3 += word + "/";
                return word + "mp3ファイルダウンロードに失敗した：" + ex.Message;
            }
        }
        /*private void pullMp3URL2(String sauce, String word)
        {

            //mp3該当URLを抽出
            Match matchedObject = null;
            String[] targetURL = new String[] { "sound/*?/.*?mp3" };
            for (int i = 0; i < targetURL.Length; i++)
            {
                if (i == 0)
                {
                    matchedObject = Regex.Match(sauce, targetURL[i]);
                }
                else if (matchedObject.Success == false)
                {
                    matchedObject = Regex.Match(sauce, targetURL[i]);
                    if (System.IO.Path.GetFileNameWithoutExtension(matchedObject.Value).Length == "6b929c2bf76d6781740a62a7624b0051".Length) { matchedObject = Regex.Match("", "わけのわからないmp3がダウンロードされるのを防ぎます"); }
                }
            }
            if (matchedObject.Success == false)
            {
                writeLog(word + "適切なmp3を発見できなかった");
                notFound += word + "/";
                return;
            }
            //*?できるだけ短く
            //*できるだけながく
            try
            {
                //mp3ファイルのDL
                WebClient wc = new WebClient();
                writeLog("http://en.hatsuon.info/" + matchedObject.Value);

                wc.DownloadFile("http://en.hatsuon.info/" + matchedObject.Value, path + "\\" + word + ".mp3");
                wc.Dispose();
                writeLog(word + "をゲットした!");
            }
            catch (Exception ex)
            {
                cantDownloadMp3 += word + "/";
                writeLog(word + "mp3ファイルダウンロードに失敗した：" + ex.Message);
            }
        }*/
        static String path = "";
        public Download(Items itemsIn)
        {
            InitializeComponent();
            Download2(itemsIn);
        }
        public async void Download2(Items itemsIn)
        {

            //フォルダ作成
            path = Path.Combine(Directory.GetCurrentDirectory(), "landmark2-data", "tmp");
            Directory.CreateDirectory(path);
            //Directory.GetCurrentDirectory()
            //var path2 = path1.Substring( 0, path1.LastIndexOf( @"\" ) + 1 );

            this.items = itemsIn;
            for (int i = 0; i < items.speak.Length; i++)
            {
                writeLog(await downloadSource(items.speak[i]));
            }


            writeLog(notFound);
            writeLog(cantFindWebPage);
            writeLog(cantDownloadMp3);
            

            

            //Console.ReadKey();
            String pathBat = Path.Combine(Directory.GetCurrentDirectory(), "landmark2-data", "mp3ToWav.bat");
            String pathOut = Path.Combine(Directory.GetCurrentDirectory(), "landmark2-data", "wav");
            if (!Directory.Exists(pathOut))
            {
                Directory.CreateDirectory(pathOut);
            }
            writeLog("ファイル形式の変換を開始します。");
            Process.Start(pathBat, pathOut);
            writeLog("完了しました。");

            //Console.ReadKey();


            //コンソールをオン
            //AllocConsole();
            //コンソールoff
            //FreeConsole();
        }
        
        /*[DllImport("kernel32.dll", SetLastError = true)]
[return: MarshalAs(UnmanagedType.Bool)]
static extern bool FreeConsole();
[DllImport("kernel32.dll", SetLastError = true)]
[return: MarshalAs(UnmanagedType.Bool)]
static extern bool AllocConsole();*/

    }
}
