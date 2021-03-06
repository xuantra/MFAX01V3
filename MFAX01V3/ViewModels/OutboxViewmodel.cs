﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using FAXCOMEXLib;
using MFAX01V3.Controls;

namespace MFAX01V3
{
   public class OutboxViewmodel:BaseViewModel
    {
        private FaxServer objFaxServer;
        private string outboxFolder;
        private FaxAccountOutgoingQueue objFaxOutbox;
        public ObservableCollection<IFaxOutgoingJob> DanhSachFaxGui { get; set; }
        public ICommand TimKiemThu { get;  set; }
        public ICommand CmdReset { get;  set; }

        public OutboxViewmodel()
        {
            objFaxServer = new FaxServer();
            objFaxServer.Connect("");
            outboxFolder = objFaxServer.Configuration.ArchiveLocation + @"\Queue";
            objFaxOutbox = objFaxServer.CurrentAccount.Folders.OutgoingQueue;
            DanhSachFaxGui = LoadOutBox();
            TimKiemThu = new RelayCommand<UcTimKiemThu>((p) => true, (p) => { TimKiem(p); });
            CmdReset = new RelayCommand(() => { DanhSachFaxGui = LoadOutBox(); });
        }

        private void TimKiem(UcTimKiemThu p)
        {
            DanhSachFaxGui = (DanhSachFaxGui.Where(x => x.TransmissionEnd.AddDays(-1) < p.NgayKetThuc && x.TransmissionEnd >= p.NgayBatDau)).ToObservableCollection();
        }

        public ObservableCollection<IFaxOutgoingJob> LoadOutBox()
        {
            ObservableCollection<IFaxOutgoingJob> LstOutgoingJob = new ObservableCollection<IFaxOutgoingJob>();
            FaxOutgoingJobs objFaxOutgoingJobs = objFaxOutbox.GetJobs();
            List<string> LstFaxGuiInFolder = LoadFaxGuiFromFolder(outboxFolder);           
            IEnumerator objEnumerator = objFaxOutgoingJobs.GetEnumerator();
            objEnumerator.Reset();
            while (objEnumerator.MoveNext())
            {
                IFaxOutgoingJob objFaxOutgoingJob = (IFaxOutgoingJob)objEnumerator.Current;
                if (LstFaxGuiInFolder.Exists(x => x.Contains(objFaxOutgoingJob.Id)))
                    LstOutgoingJob.Add(objFaxOutgoingJob);
            }
            return LstOutgoingJob;
        }

        private List<string> LoadFaxGuiFromFolder(string outboxFolder)
        {
            List<string> Result = new List<string>();
            string[] items = Directory.GetFiles(outboxFolder);
            foreach (var item in items)
            {
                if (System.IO.Path.GetExtension(item) == ".tif")
                    Result.Add(item);
            }
            return Result;
        }
    }
}
