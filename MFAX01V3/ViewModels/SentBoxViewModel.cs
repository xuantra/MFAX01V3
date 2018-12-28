using FAXCOMEXLib;
using MFAX01V3.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MFAX01V3
{
    public class SentBoxViewModel : BaseViewModel
    {
        private string outBoxFolder;
        private List<string> lstFaxGui;

        public ObservableCollection<IFaxOutgoingMessage> LstOutFaxs { get; set; }
        public ICommand TimKiemThu { get;  set; }
        public ICommand CmdReset { get;  set; }

        private FaxAccountOutgoingArchive objFaxOutbox;

        public SentBoxViewModel()
        {
            outBoxFolder = App.folderLuufax + @"\SentItems";
            lstFaxGui = LoadFaxGuiInFolder();
            objFaxOutbox = App.objFaxAccount.Folders.OutgoingArchive;
            LstOutFaxs = LoadSendItem();
            TimKiemThu = new RelayCommand<UcTimKiemThu>((e) => true, (p) => { TimKiem(p); });
            CmdReset = new RelayCommand(() => { lstFaxGui = LoadFaxGuiInFolder(); });
        }

        private void TimKiem(UcTimKiemThu p)
        {
            LstOutFaxs = (LstOutFaxs.Where(x => x.TransmissionEnd.AddDays(-1) < p.NgayKetThuc && x.TransmissionEnd >= p.NgayBatDau)).ToObservableCollection();
        }

        /// <summary>
        /// Load fax gui trong Modem Fax
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<IFaxOutgoingMessage> LoadSendItem()
        {
            if (!Directory.Exists(outBoxFolder)) return new ObservableCollection<IFaxOutgoingMessage>();
            ObservableCollection<IFaxOutgoingMessage> LstOutFaxs = new ObservableCollection<IFaxOutgoingMessage>();

            IFaxOutgoingMessageIterator objOutgoingMsgIterator = objFaxOutbox.GetMessages(100);
            objOutgoingMsgIterator.MoveFirst();
            while (objOutgoingMsgIterator.AtEOF == false)
            {
                IFaxOutgoingMessage objFaxOutgoingMsg = objOutgoingMsgIterator.Message;
                LstOutFaxs.Add(objFaxOutgoingMsg);
                objOutgoingMsgIterator.MoveNext();
            }
            return LstOutFaxs;
        }
        /// <summary>
        /// load fax gui trong folder SentItems
        /// </summary>
        /// <returns></returns>
        private List<string> LoadFaxGuiInFolder()
        {
            List<string> lstFaxGui = new List<string>();
            string[] items = Directory.GetFiles(outBoxFolder);
            foreach (var item in items)
            {
                lstFaxGui.Add(item);
            }
            return lstFaxGui;
        }
    }
}
