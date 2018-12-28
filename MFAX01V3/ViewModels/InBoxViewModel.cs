using FAXCOMEXLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using MFAX01V3.Controls;

namespace MFAX01V3
{
    public class InBoxViewModel : BaseViewModel
    {
        private FaxAccountIncomingArchive objFaxInbox;
        public ObservableCollection<IFaxIncomingMessage2> DanhSachFaxDen { get; set; }
        public ICommand TimKiemThu { get; set; }
        public ICommand CmdReset { get; set; }
        public InBoxViewModel()
        {
            objFaxInbox = App.objFaxServer.CurrentAccount.Folders.IncomingArchive;
            DanhSachFaxDen = LoadFaxicomingFromFolder();
            TimKiemThu = new RelayCommand<UcTimKiemThu>((e) => true, (p) => { Tim(p); });
            CmdReset = new RelayCommand(() => { DanhSachFaxDen = LoadFaxicomingFromFolder(); });
        }

        private void Tim(UcTimKiemThu p)
        {
            DanhSachFaxDen = (DanhSachFaxDen.Where(x => x.TransmissionEnd.AddDays(-1) < p.NgayKetThuc && x.TransmissionEnd >= p.NgayBatDau)).ToObservableCollection();

        }

        private ObservableCollection<IFaxIncomingMessage2> LoadFaxicomingFromFolder()
        {
            ObservableCollection<IFaxIncomingMessage2> LstIcomingMess = new ObservableCollection<IFaxIncomingMessage2>();
            IFaxIncomingMessageIterator objIncomingMsgIterator;
            objIncomingMsgIterator = objFaxInbox.GetMessages(100);
            objIncomingMsgIterator.MoveFirst();
            while (objIncomingMsgIterator.AtEOF == false)
            {
                IFaxIncomingMessage2 objFaxIncomingMsg = (IFaxIncomingMessage2)objIncomingMsgIterator.Message;
                LstIcomingMess.Add(objFaxIncomingMsg);
                objIncomingMsgIterator.MoveNext();
            }
            return LstIcomingMess;
        }
    }
}
