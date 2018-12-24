using System;
using System.Collections.ObjectModel;
using FAXCOMEXLib;


namespace MFAX01V3
{
    public class InBoxViewModel:BaseViewModel
    {
        private FaxServer objFaxServer;
        private FaxAccountIncomingArchive objFaxInbox;
        public ObservableCollection<IFaxIncomingMessage2> DanhSachFaxDen { get; set; }

        public InBoxViewModel()
            {
                objFaxServer = new FaxServer();
                objFaxServer.Connect("");
                objFaxInbox = objFaxServer.CurrentAccount.Folders.IncomingArchive;
            DanhSachFaxDen = LoadFaxicomingFromFolder();
           // LoadInbox();
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
