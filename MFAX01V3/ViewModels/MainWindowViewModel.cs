using FAXCOMEXLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;


namespace MFAX01V3
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ListViewItem _SelectedMenu;
        public BaseViewModel SelectedViewmodel { get; set; }

        private DispatcherTimer timerStatusLine;

        public bool ModemStatus { get; set; } = true;


        public ICommand CmdFaxMoi
        {
            get {
                return new RelayCommand(
                    () => {
                        (new wndSendFax()).ShowDialog();
                        }
                    );
            }
            set { }
        }

        public ListViewItem SelectedMenu
        {
            get => _SelectedMenu;
            set
            {
                _SelectedMenu = value;
                SwitchViewmodel(_SelectedMenu);
            }
        }

        public string StatusJob { get; private set; } = "";

        public MainWindowViewModel()
        {
         
            SelectedViewmodel = new InBoxViewModel();
            StartTimer();
            App.objFaxServer.OnIncomingJobChanged += new IFaxServerNotify2_OnIncomingJobChangedEventHandler(ObjFaxServer_OnIncomingJobChanged);
            App.objFaxServer.OnOutgoingJobChanged += new IFaxServerNotify2_OnOutgoingJobChangedEventHandler(ObjFaxServer_OnOutgoingJobChanged);
            var eventsToListen =
               FAX_SERVER_EVENTS_TYPE_ENUM.fsetIN_QUEUE | FAX_SERVER_EVENTS_TYPE_ENUM.fsetOUT_QUEUE |
               FAX_SERVER_EVENTS_TYPE_ENUM.fsetOUT_ARCHIVE | FAX_SERVER_EVENTS_TYPE_ENUM.fsetINCOMING_CALL;

            App.objFaxServer.ListenToServerEvents(eventsToListen);
        }

        private void ObjFaxServer_OnOutgoingJobChanged(FaxServer pFaxServer, string bstrJobId, FaxJobStatus pJobStatus)
        {
            switch (pJobStatus.Status)
            {
                case FAX_JOB_STATUS_ENUM.fjsPENDING:
                    StatusJob = "Đang chờ xử lí";
                             break;
                case FAX_JOB_STATUS_ENUM.fjsINPROGRESS:
                    switch (pJobStatus.ExtendedStatusCode)
                {
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesNONE:
                        StatusJob = "NONE";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesDISCONNECTED:
                        StatusJob = "Ngắt Kết Nối";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesINITIALIZING:
                        StatusJob = "Đang khởi tạo";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesDIALING:
                        StatusJob = "Đang quay số";

                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesTRANSMITTING:
                        StatusJob = "Đang Gửi Fax";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesANSWERED:
                        StatusJob = "Đã Trả Lời";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesRECEIVING:
                        StatusJob = "Đang nhận";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesLINE_UNAVAILABLE:
                        StatusJob = "Line không sẵn sàng";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesBUSY:
                        StatusJob = "Line Bận";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesNO_ANSWER:
                        StatusJob = "Không Trả Lời";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesBAD_ADDRESS:
                        StatusJob = "Bad Adrress";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesNO_DIAL_TONE:
                        StatusJob = "No Dial Tone";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesFATAL_ERROR:
                        StatusJob = "Fatal Error";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesCALL_DELAYED:
                        StatusJob = "Call Delayed";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesCALL_BLACKLISTED:
                        StatusJob = "Cuộc gọi bị chặn";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesNOT_FAX_CALL:
                        StatusJob = "Not Fax Call";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesPARTIALLY_RECEIVED:
                        StatusJob = "Nhận 1 phần";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesHANDLED:
                        StatusJob = "Handled";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesCALL_COMPLETED:
                        StatusJob = "Gọi Thành Công";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesCALL_ABORTED:
                        StatusJob = "Gọi Bị Từ Chối";
                        break;
                    case FAX_JOB_EXTENDED_STATUS_ENUM.fjesPROPRIETARY:
                        StatusJob = "PROPRIETARY";
                        break;
                    default:
                        break;
                }
                break;
                case FAX_JOB_STATUS_ENUM.fjsFAILED:
                    StatusJob = "Lỗi gửi fax";
                break;
                case FAX_JOB_STATUS_ENUM.fjsPAUSED:
                    StatusJob = "Tạm dừng";
                break;
                case FAX_JOB_STATUS_ENUM.fjsNOLINE:
                    StatusJob = "Không có Line";
                break;
                case FAX_JOB_STATUS_ENUM.fjsRETRYING:
                    StatusJob = "Đang thử lại";
                break;
                case FAX_JOB_STATUS_ENUM.fjsRETRIES_EXCEEDED:
                    StatusJob = "RETRIES_EXCEEDED";
                break;
                case FAX_JOB_STATUS_ENUM.fjsCOMPLETED:
                    StatusJob = "Gửi thành công";
                //File.Delete(folderLuufax + @"\Queue\" + bstrJobId + ".tif");

                break;
                case FAX_JOB_STATUS_ENUM.fjsCANCELED:
                    StatusJob = "Hủy fax";

                break;
                case FAX_JOB_STATUS_ENUM.fjsCANCELING:
                    StatusJob = "Đang hủy fax";
                break;
                case FAX_JOB_STATUS_ENUM.fjsROUTING:
                    StatusJob = "routing";
                break;
                default:
                    break;

            }
        }

        private void ObjFaxServer_OnIncomingJobChanged(FaxServer pFaxServer, string bstrJobId, FaxJobStatus pJobStatus)
        {
            switch (pJobStatus.Status)
            {
                case FAX_JOB_STATUS_ENUM.fjsPENDING:
                    StatusJob = "Fax Chờ Gửi";
                    break;
                case FAX_JOB_STATUS_ENUM.fjsINPROGRESS:
                    StatusJob = "Đang xử lí";
                    Task.Run(() => {
                        PlaySound("Rings\\ringin.wav", 2);
                        }
                    );
                        
                        
                    break;
                case FAX_JOB_STATUS_ENUM.fjsFAILED:
                    StatusJob = "Fax Bị Lỗi";
                    break;
                case FAX_JOB_STATUS_ENUM.fjsPAUSED:
                    StatusJob = "Fax Tạm Dừng";
                    break;
                case FAX_JOB_STATUS_ENUM.fjsNOLINE:
                    StatusJob = "Không Có Line";
                    break;
                case FAX_JOB_STATUS_ENUM.fjsRETRYING:
                    StatusJob = "Gửi Lại";
                    break;
                case FAX_JOB_STATUS_ENUM.fjsRETRIES_EXCEEDED:
                    break;
                case FAX_JOB_STATUS_ENUM.fjsCOMPLETED:
                    StatusJob = "Nhận Thành Công";
                    break;
                case FAX_JOB_STATUS_ENUM.fjsCANCELED:
                    StatusJob = "Fax Bị Hủy";
                    break;
                case FAX_JOB_STATUS_ENUM.fjsCANCELING:
                    StatusJob = "Fax Đang Hủy";
                    break;
                case FAX_JOB_STATUS_ENUM.fjsROUTING:
                    StatusJob = "Nhận Thành Công";
                    break;
                default:
                    break;
            }
        }
        #region Detect status modem
        private void StartTimer()
        {
            timerStatusLine = new DispatcherTimer();
            timerStatusLine.Interval = new TimeSpan(0, 0, 2);
            timerStatusLine.Tick += TimerStatusLine_Tick;
            timerStatusLine.Start();
        }

        private void TimerStatusLine_Tick(object sender, EventArgs e)
        {
            FaxDevices devices = App.objFaxServer.GetDevices();
            bool result = devices.Count > 0 ? true : false;
            if (result == ModemStatus) return;
            ModemStatus = result;
        }
        #endregion

        private void PlaySound(string path, int time)
        {
            SoundPlayer player = new SoundPlayer
            {
                SoundLocation = path
            };
            player.Load();
            for (int i = 0; i < time; i++)
            {
                player.PlaySync();
                Thread.Sleep(500);
            }
        }

        private void SwitchViewmodel(ListViewItem selectedMenu)
        {
            switch (selectedMenu.Name)
            {
                case "itemInbox":
                    SelectedViewmodel = new InBoxViewModel();
                    break;
                case "itemOutbox":
                    SelectedViewmodel = new OutboxViewmodel();
                    break;
                case "itemSentbox":
                    SelectedViewmodel = new SentBoxViewModel();
                    break;
                default:
                    break;
            }

        }
    }
}
