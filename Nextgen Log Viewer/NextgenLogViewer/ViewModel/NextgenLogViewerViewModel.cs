using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.IO;
using System.Windows;
using NextgenLogViewer.Resource;
using NextgenLogViewer.Models;
using System.Threading.Tasks;
using System.Diagnostics;
//abc - 13
namespace NextgenLogViewer.ViewModel
{
    public class NextgenLogViewerViewModel : INotifyPropertyChanged
    {        
        List<LogLine> lstGridValues;
        List<LogLine> _lstGridValues_grid;

        public NextgenLogViewerViewModel()
        {
            lstGridValues = new List<LogLine>();
            _lstGridValues_grid = new List<LogLine>();
        }
        #region Properties
        private string strTimeSpent = "";

        public string TimeSpent
        {
            get
            {
                return strTimeSpent;
            }
            set
            {
                strTimeSpent = value;
                NotifyPropertyChanged("TimeSpent");
            }
        }

        public double _timeElapsed = 0;
        public double TimeElapsed
        {
            get
            {
                return _timeElapsed;
            }
            set
            {
                _timeElapsed = value;
                NotifyPropertyChanged("TimeElapsed");
            }
        }


        private Visibility _processIndicator = Visibility.Collapsed;
        public Visibility ProcessIndicator
        {
            get { return _processIndicator; }
            set
            {
                _processIndicator = value;
                NotifyPropertyChanged("ProcessIndicator");
            }
        }

        private Visibility _Timeelapsedvisibility = Visibility.Collapsed;
        public Visibility Timeelapsedvisibility
        {
            get { return _Timeelapsedvisibility; }
            set
            {
                _Timeelapsedvisibility = value;
                NotifyPropertyChanged("Timeelapsedvisibility");
            }
        }

        private string strFilePath = "";

        public string FilePath {
            get {
                return strFilePath;
            }
            set {
                strFilePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }
        private bool _isCheckAll;

        public bool IsCheckAll
        {
            get { return _isCheckAll; }
            set {
                _isCheckAll = value;                
                NotifyPropertyChanged("IsCheckAll");
            }
        }

        private bool _isChkTrace;

        public bool IsChkTrace
        {
            get { return _isChkTrace; }
            set
            {
                if(!value) Timeelapsedvisibility = Visibility.Collapsed;
                _isChkTrace = value;
                NotifyPropertyChanged("IsChkTrace");
            }
        }

        private bool _isChkWarning;

        public bool IsChkWarning
        {
            get { return _isChkWarning; }
            set
            {
                _isChkWarning = value;
                NotifyPropertyChanged("IsChkWarning");
            }
        }

        private bool _isChkInfo;

        public bool IsChkInfo
        {
            get { return _isChkInfo; }
            set
            {
                _isChkInfo = value;
                NotifyPropertyChanged("IsChkInfo");
            }
        }

        private bool _isCheckSQL;

        public bool IsCheckSQL
        {
            get { return _isCheckSQL; }
            set
            {
                _isCheckSQL = value;
                NotifyPropertyChanged("IsCheckSQL");
            }
        }

        private bool _isChkDebug;

        public bool IsChkDebug
        {
            get { return _isChkDebug; }
            set
            {
                _isChkDebug = value;
                NotifyPropertyChanged("IsChkDebug");
            }
        }
        private bool _isChkFatal;

        public bool IsChkFatal
        {
            get { return _isChkFatal; }
            set
            {
                _isChkFatal = value;
                NotifyPropertyChanged("IsChkFatal");
            }
        }
        private bool _isChkError;

        public bool IsChkError
        {
            get { return _isChkError; }
            set
            {
                _isChkError = value;
                NotifyPropertyChanged("IsChkError");
            }
        }
        
        
        private ObservableCollection<LogLine> _lstGrid;

        public ObservableCollection<LogLine> lstGrid
        {
            get { return _lstGrid; }
            set
            {
                _lstGrid = value;
                NotifyPropertyChanged("lstGrid");
            }
        }
        private string _showStatus;

        public string ShowStatus
        {
            get { return _showStatus; }
            set
            {
                _showStatus = value;
                NotifyPropertyChanged("ShowStatus");
            }
        }
        
        #endregion

        #region Private Methods

        private void LoadGrid()
        {
            try
            {
                List<string> textvalues = File.ReadAllLines(strFilePath).ToList();
                lstGridValues.Clear();
                _lstGridValues_grid.Clear();
                ShowStatus = "";
                if (string.IsNullOrEmpty(strFilePath))
                {
                    //MessageBox.Show("Select Log file");
                    return;
                }
                IsCheckAll = true;
                int Count = 0;
                foreach (var line in textvalues)
                {
                    Count++;
                    if (string.IsNullOrEmpty(line)) continue;

                    if (line.Split(' ').Count() < 3) continue;
                    LogLine l = new LogLine();
                    l.logdate = line.Split(' ')[0];
                    l.logtime = line.Split(' ')[1];
                    switch (line.Split(' ')[2])
                    {
                        case "Trace":
                            l.logType = LogType.Trace;
                            break;
                        case "Info":
                            l.logType = LogType.Info;
                            break;
                        case "Fatal":
                            l.logType = LogType.Fatal;
                            break;
                        case "Warning":
                            l.logType = LogType.Warning;
                            break;
                        case "Debug":
                            l.logType = LogType.Debug;
                            break;
                        case "Error":
                            l.logType = LogType.Error;
                            break;
                        default:
                            l.logType = LogType.Other;
                            break;
                    }
                    for (int i = 3; i <= line.Split(' ').Count() - 1; i++)
                    {
                        if (l.logType == LogType.Trace)
                        {
                            if (line.Contains("Time elapsed:"))
                            {
                                int startindex = line.IndexOf("Time elapsed:") + 13;
                                int endindex = line.IndexOf("ms");

                                l.TraceTime = Convert.ToDouble(line.Substring(startindex).Trim().Split(' ')[0]);
                                l.logmessage = l.logmessage + " " + line.Substring(endindex + 4).Trim();
                            }
                            else if (line.Contains("SQL:"))
                            {
                                l.logType = LogType.SQL;
                                int startindex = line.IndexOf("SQL:") + 4;
                                int endindex = line.IndexOf("ms");

                                l.TraceTime = Convert.ToDouble(line.Substring(startindex).Trim().Split(' ')[0]);
                                l.logmessage = l.logmessage + " " + line.Substring(endindex + 4).Trim();
                            }
                            break;
                        }
                        else
                        {
                            l.logmessage = l.logmessage + " " + line.Split(' ')[i];
                        }

                    }
                    lstGridValues.Add(l);
                    if(Count == 500)
                        lstGrid = new ObservableCollection<LogLine>(lstGridValues);
                }
                BuildSerachCritiria();
                UpdateStatusInfo();
                
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show(Ex.Message.ToString());
            }
        }

        private void BuildSerachCritiria()
        {
            _lstGridValues_grid.Clear();

            if (IsCheckAll)
            {
                IsChkTrace = false;
                IsChkWarning = false;
                IsChkInfo = false;
                IsChkDebug = false;
                IsChkFatal = false;
                IsChkError = false;
                IsCheckSQL = false;
                _lstGridValues_grid.AddRange(lstGridValues.FindAll(x => x.logType != LogType.Other));
            }
            else
            {
                if (IsChkError) _lstGridValues_grid.AddRange(lstGridValues.FindAll(x => x.logType == LogType.Error));
                if (IsChkFatal) _lstGridValues_grid.AddRange(lstGridValues.FindAll(x => x.logType == LogType.Fatal));
                if (IsChkDebug) _lstGridValues_grid.AddRange(lstGridValues.FindAll(x => x.logType == LogType.Debug));
                if (IsChkInfo) _lstGridValues_grid.AddRange(lstGridValues.FindAll(x => x.logType == LogType.Info));
                if (IsChkWarning) _lstGridValues_grid.AddRange(lstGridValues.FindAll(x => x.logType == LogType.Warning));
                if (IsChkTrace)
                {
                    Timeelapsedvisibility = Visibility.Visible;
                    _lstGridValues_grid.AddRange(lstGridValues.FindAll(x => x.logType == LogType.Trace && x.TraceTime >= TimeElapsed));
                }
                if (IsCheckSQL)
                {
                    Timeelapsedvisibility = Visibility.Visible;
                    _lstGridValues_grid.AddRange(lstGridValues.FindAll(x => x.logType == LogType.SQL && x.TraceTime >= TimeElapsed));
                }
            }
            lstGrid = new ObservableCollection<LogLine>(_lstGridValues_grid);
        }

        private void UpdateStatusInfo()
        {
            string strStatus = "";
            strStatus = "Total Log Count : " + lstGridValues.Count.ToString();
            strStatus = strStatus + " | Error Count : " + lstGridValues.Count(x => x.logType == LogType.Error);
            strStatus = strStatus + " | Fatal Count : " + lstGridValues.Count(x => x.logType == LogType.Fatal);
            strStatus = strStatus + " | Warning Count : " + lstGridValues.Count(x => x.logType == LogType.Warning);
            strStatus = strStatus + " | Debug Count : " + lstGridValues.Count(x => x.logType == LogType.Debug);
            strStatus = strStatus + " | Trace Count : " + lstGridValues.Count(x => x.logType == LogType.Trace);
            strStatus = strStatus + " | Info Count : " + lstGridValues.Count(x => x.logType == LogType.Info);
            strStatus = strStatus + " | SQL Count : " + lstGridValues.Count(x => x.logType == LogType.SQL);
            ShowStatus = strStatus;
        }

        private async void ProcessLoadLogFile()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ProcessIndicator = Visibility.Visible;
            await Task.Run(() => LoadGrid());
            Task.WaitAll();
            ProcessIndicator = Visibility.Collapsed;
            sw.Stop();
            string ExecutionTimeTaken = string.Format("Minutes :{0}\nSeconds :{1}\n Mili seconds :{2}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds);
            TimeSpent = ExecutionTimeTaken;
        }
        private void ProcesssTimeSpent()
        {
            MessageBox.Show(TimeSpent.ToString(), "Time Spent");
        }
        private void ProcessCheckAll()
        {
            BuildSerachCritiria();
        }

        private void ProcessCheckErrors()
        {
            IsCheckAll = false; ;
            BuildSerachCritiria();
        }
        private void ProcessCheckFatal()
        {
            IsCheckAll = false;
            BuildSerachCritiria();
        }
        private void ProcessCheckWarning()
        {
            IsCheckAll = false;
            BuildSerachCritiria();
        }
        private void ProcessCheckDebug()
        {
            IsCheckAll = false;
            BuildSerachCritiria();
        }
        private void ProcessCheckTrace()
        {
            IsCheckAll = false;
            BuildSerachCritiria();
        }
        private void ProcessCheckInfo()
        {
            IsCheckAll = false;
            BuildSerachCritiria();
        }

        private void ProcessCheckSQL()
        {
            IsCheckAll = false;
            BuildSerachCritiria();
        }
        
        #endregion

        #region Relay Command




        private RelayCommand _sTimeSpent;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand sTimeSpent
        {
            get
            {
                return _sTimeSpent ?? (_sTimeSpent = new RelayCommand(param => ProcesssTimeSpent(), param => true));
            }
        }

        private RelayCommand _loadLogFile;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand LoadLogFile
        {
            get
            {
                return _loadLogFile ?? (_loadLogFile = new RelayCommand(param => ProcessLoadLogFile(), param => true));
            }
        }

        private RelayCommand _checkAll;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand CheckAll
        {
            get
            {
                return _checkAll ?? (_checkAll = new RelayCommand(param => ProcessCheckAll(), param => true));
            }
        }


        private RelayCommand _checkErrors;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand CheckErrors
        {
            get
            {
                return _checkErrors ?? (_checkErrors = new RelayCommand(param => ProcessCheckErrors(), param => true));
            }
        }


        private RelayCommand _checkFatal;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand CheckFatal
        {
            get
            {
                return _checkFatal ?? (_checkFatal = new RelayCommand(param => ProcessCheckFatal(), param => true));
            }
        }


        private RelayCommand _checkWarning;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand CheckWarning
        {
            get
            {
                return _checkWarning ?? (_checkWarning = new RelayCommand(param => ProcessCheckWarning(), param => true));
            }
        }

        private RelayCommand _checkDebug;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand CheckDebug
        {
            get
            {
                return _checkDebug ?? (_checkDebug = new RelayCommand(param => ProcessCheckDebug(), param => true));
            }
        }

        private RelayCommand _checkTrace;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand CheckTrace
        {
            get
            {
                return _checkTrace ?? (_checkTrace = new RelayCommand(param => ProcessCheckTrace(), param => true));
            }
        }

        private RelayCommand _checkInfo;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand CheckInfo
        {
            get
            {
                return _checkInfo ?? (_checkInfo = new RelayCommand(param => ProcessCheckInfo(), param => true));
            }
        }

        private RelayCommand _checkSQL;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand CheckSQL
        {
            get
            {
                return _checkSQL ?? (_checkSQL = new RelayCommand(param => ProcessCheckSQL(), param => true));
            }
        }

        private RelayCommand _TimeElapsed;

        /// <summary>
        /// Convertion starts here
        /// </summary>
        public RelayCommand TimeElapsedSearch
        {
            get
            {
                return _TimeElapsed ?? (_TimeElapsed = new RelayCommand(param => ProcessTimeElapsed(), param => true));
            }
        }

        private void ProcessTimeElapsed()
        {
            BuildSerachCritiria();
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged(String info)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
            }
        #endregion

    }
}
