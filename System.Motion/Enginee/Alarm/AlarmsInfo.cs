using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Motion.Interfaces;

namespace Motion.Enginee
{
    public partial class AlarmsInfo : UserControl, IAlarmsResult
    {
        private CancellationTokenSource _cancelTokenSource;
        private List<Alarm> Alarms;
        public AlarmsInfo()
        {
            InitializeComponent();
            ContinueEvent = new ManualResetEvent(true);
            StopEvent = new ManualResetEvent(false);
            Alarms = new List<Alarm>();
        }
        public void Add(IList<Alarm> alarms)
        {
            if (alarms != null)
            {
                foreach (Alarm alarm in alarms)
                {
                    Alarms.Add(alarm);
                }
            }
        }
        /// <summary>
        ///     继续信号量。
        /// </summary>
        protected ManualResetEvent ContinueEvent { get; set; }

        /// <summary>
        ///     停止信号量。
        /// </summary>
        protected ManualResetEvent StopEvent { get; set; }

        public bool IsAlarms { get; set; }
        public bool IsPrompt { get; set; }
        public bool IsWarning { get; set; }
        private void Clean()
        {
            var ini = this as INeedClean;
            if (ini != null)
                ini.Clean();
            _cancelTokenSource = null;
        }

        /// <summary>
        ///     运行部件驱动线程。
        /// </summary>
        /// <param name="runningMode">运行模式。</param>
        public virtual void Run()
        {
            StopEvent.Reset();

            if (_cancelTokenSource == null)
            {
                _cancelTokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Running();
                        //Log.Debug("{0}部件启动。", Name);
                    }
                    catch (OperationCanceledException)
                    {
                        //ignorl
                    }
                    catch (Exception ex)
                    {
                        //Log.Fatal("设备驱动程序异常", ex);
                    }
                }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning)
                .ContinueWith(task => Clean());
            }
        }

        /// <summary>
        ///     暂停任务线程
        /// </summary>
        public virtual void Pause()
        {
            if (_cancelTokenSource == null) return;
            ContinueEvent.Reset();
            //Log.Debug("{0}部件暂停。", Name);
        }

        /// <summary>
        ///     唤醒任务线程
        /// </summary>
        public virtual void Resume()
        {
            if (_cancelTokenSource == null) return;
            ContinueEvent.Set();
            //Log.Debug("{0}部件唤醒。", Name);
        }

        /// <summary>
        ///     停止任务线程
        /// </summary>
        public virtual void Stop()
        {
            if (_cancelTokenSource == null) return;
            if (_cancelTokenSource.Token.CanBeCanceled)
            {
                StopEvent.Set();
                _cancelTokenSource.Cancel();
                //Log.Debug("{0}部件停止。", Name);
            }
        }

        /// <summary>
        ///     驱动部件运行。
        /// </summary>
        /// <param name="runningMode">运行模式。</param>
        private void Running()
        {
            while (true)
            {
                Thread.Sleep(100);
                var _alarmsStatus = false;
                var _promptStatus = false;
                var _warningStatus = false;
                foreach (Alarm alarm in Alarms)
                {
                    var btemp = alarm.IsAlarm;
                    if (alarm.AlarmLevel == AlarmLevels.Error)
                    {   
                        _alarmsStatus |= btemp;
                        this.Invoke(new Action(() =>
                        {
                            Msg(string.Format("{0},{1}", alarm.AlarmLevel.ToString(), alarm.Name), btemp);
                        }));
                    }
                    else if (alarm.AlarmLevel == AlarmLevels.None)
                    {
                        _promptStatus |= btemp;
                        this.Invoke(new Action(() =>
                        {
                            Msg(string.Format("{0},{1}", alarm.AlarmLevel.ToString(), alarm.Name), btemp);
                        }));
                    }
                    else
                    {
                        _warningStatus |= btemp;
                        this.Invoke(new Action(() =>
                        {
                            Msg(string.Format("{0},{1}", alarm.AlarmLevel.ToString(), alarm.Name), btemp);
                        }));
                    }
                }
                IsAlarms = _alarmsStatus;
                IsPrompt = _promptStatus;
                IsWarning = _warningStatus;
            }
        }
        public void Add(string str)
        {
            if(InvokeRequired)
            {
                Invoke(new Action<string>(Add), str);
            }
            else
            {
                listBox1.Items.Add(str);
            }
            //this.Invoke(new Action(() =>
            //{
            //    listBox1.Items.Add(str);
            //}));
        }
        public void Insert(string str)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(Insert), str);
            }
            else
            {
                listBox1.Items.Insert(0, str);
            }
            //this.Invoke(new Action(() =>
            //{
            //    listBox1.Items.Insert(0, str);
            //}));
        }
        private void Msg(string str, bool value)
        {
            string tempstr = null;
            bool sign = false;
            try
            {
                var arrRight = new List<object>();
                foreach (var tmpist in listBox1.Items) arrRight.Add(tmpist);
                if (value)
                {
                    foreach (string tmplist in arrRight)
                    {
                        if (tmplist.IndexOf("-") > -1)
                            tempstr = tmplist.Substring(tmplist.IndexOf("-") + 1, tmplist.Length - tmplist.IndexOf("-") - 1);
                        if (tempstr == str)
                        {
                            sign = true;
                            break;
                        }
                    }
                    if (!sign) listBox1.Items.Insert(0, (string.Format("{0}-{1}" + Environment.NewLine, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), str)));
                }
                else
                {
                    foreach (string tmplist in arrRight)
                    {
                        if (tmplist.IndexOf("-") > -1)
                        {
                            tempstr = tmplist.Substring(tmplist.IndexOf("-") + 1, tmplist.Length - tmplist.IndexOf("-") - 1);
                            if (tempstr == str) listBox1.Items.Remove(tmplist);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
    public struct AlarmType
    {
        public bool IsAlarm;
        public bool IsPrompt;
        public bool IsWarning;
    }
}
