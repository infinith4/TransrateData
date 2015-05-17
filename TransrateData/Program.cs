using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
namespace TransrateData
{
    public class Program
    {
        
        static void Main(string[] args)
        {

          List<TransrateData> trasratedataList = ReadCsv();  //変換するデータ
          TransrateNewApplyToAppliedData(trasratedataList);
            
        }
        public static void TransrateNewApplyToAppliedData(List<TransrateData> trasratedataList)
        {
            OfficeManageEntities _db = new OfficeManageEntities();

            var applyManage = _db.ApplyManage;
            foreach(var apply in applyManage)
            {
                if (apply.ApplyStatusID == int.Parse(ConfigurationManager.AppSettings["ApplyStatsID"].ToString()))
                {

                    var transratedata = trasratedataList.Find(c => c.ApplyID == apply.ApplyID);
                    switch (apply.ApplyTypeID)
                    {
                        case 0:  //mailinglist
                            var account = _db.ApplyAccount.Single(c => c.ApplyID == apply.ApplyID);
                            account.StaffID = transratedata.StaffID;
                        
                            break;
                        case 1:  //mailinglist
                            var mailing = _db.ApplyMailingList.Single(c => c.ApplyID == apply.ApplyID);
                            mailing.StaffID = transratedata.StaffID;

                            break;
                        case 2:  //mailinglist
                            var server = _db.ApplyServer.Single(c => c.ApplyID == apply.ApplyID);
                            server.StaffID = transratedata.StaffID;

                            break;
                        case 3:  //mailinglist
                            var backup = _db.ApplyBackup.Single(c => c.ApplyID == apply.ApplyID);
                            backup.StaffID = transratedata.StaffID;

                            break;
                        case 4:  //mailinglist
                            var wireless = _db.ApplyWireless.Single(c => c.ApplyID == apply.ApplyID);
                            wireless.StaffID = transratedata.StaffID;

                            break;
                        case 5:  //mailinglist
                            var vpn = _db.ApplyVPN.Single(c => c.ApplyID == apply.ApplyID);
                            vpn.StaffID = transratedata.StaffID;

                            break;
                        case 6:  //mailinglist
                            var exception = _db.ApplyException.Single(c => c.ApplyID == apply.ApplyID);
                            exception.StaffID = transratedata.StaffID;

                            break;
                        default:
                            break;

                    }
                    apply.Boss_StaffID = transratedata.Boss_StaffID;

                    _db.SaveChanges();
                    /*
                    事前にサーバにアクセス出来ないようにする。
                    ApplyIDをApplyManageとApplyMailingListとで一致させる。
                    ApplyCodeを変更する。

                    新規申請したApplyMailingListのStaffIDを変更する

                    ApplyManageのStaffIDをApplyMailingListのStaffIDと一致させる。
                    ApplyManageのAdminとBoss変更したい人のStaffIDに変更する。
                    */
                }
            }
        }

        public static List<TransrateData> ReadCsv()
        {
            List<TransrateData> transratedataList = new List<TransrateData>();
            try
            {
                // Get the AppSettings section.
                // csvファイルを開く
                // ファイルストリームを開く
                
                using (FileStream fs = File.OpenRead(ConfigurationManager.AppSettings["TransrateDataSettingFile"].ToString()))
                {
                    // 先ほどのファイルストリームを元に、Shift-JISのテキストを読み込むストリームを作成
                    StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("UTF-8"));
                    // ストリームの末尾まで繰り返す
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        // ファイルから一行読み込む
                        var line = sr.ReadLine();
                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        var values = line.Split(',');
                        // 出力する
                        TransrateData transratedata = new TransrateData();
                        
                        transratedata.StaffID = values[0];
                        transratedata.ApplyID = int.Parse(values[1]);
                        transratedata.ApplyTypeID = int.Parse(values[2]);
                        transratedata.Admin_StaffID = values[3];
                        transratedata.Boss_StaffID = values[4];
                        transratedataList.Add(transratedata);
                        //System.Console.WriteLine();
                    }
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したとき
                System.Console.WriteLine(e.Message);
            }

            return transratedataList;
        }

    }
}
