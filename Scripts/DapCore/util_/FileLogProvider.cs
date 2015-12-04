using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Diagnostics;

namespace angeldnd.dap {
    [DapPriority(1)]
    public class FileLogProvider : LogProvider {
        public const int MAX_STACK_TRACK_NUM = 12;
        public const long FLUSH_DURATION = 10 * 1000 * 60; // flush every minute
        public const string TIMESTAMP_FORMAT = "HH:mm:ss.fff";    //http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx

        private string _LogRoot = "";
        private string _LogDir = "";
        private string _LogName = "";
        private int _RunID = -1;
        private int _LogDayOfYear = -1;
        private string _LogFilePath = "";
        private bool _LogAutoFlush = false;
        private StreamWriter _LogWriter = null;
        private DateTime _LastFlushTime;

        public FileLogProvider() : this(RegistryConsts.DefaultLogDir,
                                               RegistryConsts.DefaultLogName,
                                               -1) {
        }

        public FileLogProvider(string logDir, string logName, int runID) {
            _LogRoot = GetLogRoot();

            Info("FileLogProvider: {0} {1} {2}", logDir, logName, runID);
            _LogDir = logDir;
            _LogName = logName;
            if (runID < 0) {
                _RunID = GetNextRunID(logDir, logName, 0);
            } else {
                _RunID = runID;
            }
            SetupLogWriter();
        }

        public int GetNextRunID(string logDir, string logName, int startRunID) {
            int runID = startRunID;

            while (true) {
                if (!IsLogFileExist(logDir, logName, runID)) {
                    break;
                }
                runID++;
            }
            Info("GetNextRunID: {0} {1} {2} -> {3}", logDir, logName, startRunID, runID);
            return runID;
        }

        public void SetRunID(int runID) {
            Info("SetRunID: {0}", runID);
            _RunID = runID;
            SetupLogWriter();
        }

        public void SetLogAutoFlush(bool autoFlush) {
            _LogAutoFlush = autoFlush;
            SetupLogWriter();
        }

        public override void Flush() {
            if (!_LogAutoFlush && _LogWriter != null) {
                _LogWriter.Flush();
            }
        }

        public void StopLogging() {
            if (_LogWriter != null) {
                _LogWriter.Flush();
                _LogWriter.Close();
                _LogWriter = null;
            }
        }

        private bool IsLogFileExist(string logDir, string logName, int runID) {
            var now = System.DateTime.UtcNow;
            string month = now.ToString("yyyy-MM");
            string dir = string.Format("{0}/{1}/{2}", _LogRoot, month, logDir);

            string date = now.ToString("yyyy-MM-dd");
            string logFilePath = string.Format("{0}/{1}_{2}_{3}.log", dir, date, logName, runID);
 
            try {
                if (!Directory.Exists(dir)) {
                    return false;
                }
                return File.Exists(logFilePath);
            } catch (Exception e) {
                Log.Error("IsLogFileExist: Get Exception: {0}", e);
                return false;
            }
        }

        private void SetupLogWriter() {
            StopLogging();
            var now = System.DateTime.UtcNow;
            _LastFlushTime = now;

            string month = now.ToString("yyyy-MM");
            string dir = string.Format("{0}/{1}/{2}", _LogRoot, month, _LogDir);

            _LogDayOfYear = now.DayOfYear;
            string date = now.ToString("yyyy-MM-dd");
            _LogFilePath = string.Format("{0}/{1}_{2}_{3}.log", dir, date, _LogName, _RunID);
 
            try {
                if (!Directory.Exists(dir)) {
                    Info("CreateDirectory: {0}", dir);
                    Directory.CreateDirectory(dir);
                }
                _LogWriter = File.AppendText(_LogFilePath);
                Info("Start Logging: {0}", _LogFilePath);

                //Probably can't use AutoFlush here due to performance issue
                //need to check whether the system level cache make this workable
                //or need to flush periodically.
                _LogWriter.AutoFlush = _LogAutoFlush;
            } catch (Exception e) {
                _LogWriter = null;
                Error("Failed to create log writer: {0} : {1}", _LogFilePath, e);
            }
        }

        protected void Info(string format, params object[] values) {
            //TODO: Add log prefix and tostring()
            AddLog(this, LoggerConsts.INFO, null, format, values);
        }

        protected void Error(string format, params object[] values) {
            //TODO: Add log prefix and tostring()
            AddLog(this, LoggerConsts.ERROR, null, format, values);
        }

        public override void AddLog(object source, string kind, StackTrace stackTrace, string format, params object[] values) {
            var now = System.DateTime.UtcNow;

            string msg = format;
            if (values != null && values.Length > 0) {
                msg = string.Format(format, values);
            }

            string log = null;
            if (stackTrace != null) {
                log = string.Format("{0} [{1}] {2}\n{3}", now.ToString(TIMESTAMP_FORMAT), kind, msg,
                                    FormatStackTrace(stackTrace, "\t", MAX_STACK_TRACK_NUM));
            } else {
                log = string.Format("{0} [{1}] {2}", now.ToString(TIMESTAMP_FORMAT), kind, msg);
            }

            if (_LogWriter != null) {
                if (now.DayOfYear != _LogDayOfYear) {
                    SetupLogWriter();
                    if (_LogWriter == null) {
                        return;
                    }
                }
                try {
                    _LogWriter.WriteLine(log);
                    if (kind == LoggerConsts.CRITICAL || now.Ticks - _LastFlushTime.Ticks > FLUSH_DURATION) {
                        _LogWriter.Flush();
                        _LastFlushTime = now;
                    }
                } catch (Exception e) {
                    _LogWriter = null;
                    Error("Failed to write log: {0} : {1}", _LogFilePath, e);
                    //TODO: maybe try to resume logging to file later.
                }
            }
            OnLog(source, log, stackTrace);
        }

        public string FormatStackTrace(StackTrace stackTrace, string prefix, int max) {
            StringWriter writer = new StringWriter();
            for (int i = 0; i< stackTrace.FrameCount; i++) {
                if (i >= max) break;
                StackFrame stackFrame = stackTrace.GetFrame(i);
                var method = stackFrame.GetMethod();
                string line = string.Format("{0}{1}.{2}:{3}() ({4}:{5})", prefix,
                                            method.ReflectedType.Namespace,
                                            method.ReflectedType.Name,
                                            method.Name,
                                            stackFrame.GetFileName(),
                                            stackFrame.GetFileLineNumber());
                writer.WriteLine(line);
            }
            return writer.ToString();
        }

        protected virtual string GetLogRoot() {
            return "Logs";
        }

        protected virtual void OnLog(object source, string log, StackTrace stackTrace) {}

    }
}

