//SILP: USING_COMMON()
using System;                                                         //__SILP__
using System.Collections;                                             //__SILP__
using System.Collections.Generic;                                     //__SILP__
                                                                      //__SILP__
using angeldnd.dap;                                                   //__SILP__

namespace angeldnd.dap {
    public static class ResponseConsts {
        [DapParam(typeof(string))]
        public const string KeyUri = "uri";
        [DapParam(typeof(Data))]
        public const string KeyReq = "req";
        [DapParam(typeof(int))]
        public const string KeyStatus = "status";
        [DapParam(typeof(string))]
        public const string KeyResult = "result";
        [DapParam(typeof(string))]
        public const string KeyError = "error";
        [DapParam(typeof(Data))]
        public const string KeyData = "data";

        public const int INVALID = -1;
        /* The status codes are borrowed form the HTTP protocol
         *
         * http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
         */
        public const int OK = 200;
        public const int ACCEPTED = 202;

        public const int BAD_REQ = 400;
        public const int UNAUTHORIZED = 401;
        public const int NOT_FOUND = 404;
        public const int INTERNAL_ERROR = 500;
        public const int NOT_IMPLEMENTED = 501;
    }

    public static class ResponseHelper {
        private static Data Result(Handler handler, Data req, int status, string result, Data data) {
            Data res = new Data();
            res.SetString(ResponseConsts.KeyUri, handler.Uri);
            if (req != null) {
                res.SetData(ResponseConsts.KeyReq, req);
            }
            res.SetInt(ResponseConsts.KeyStatus, status);
            if (!string.IsNullOrEmpty(result)) {
                res.SetString(ResponseConsts.KeyResult, result);
            }
            if (data != null) {
                res.SetData(ResponseConsts.KeyData, data);
            }
            if (handler.LogDebug) {
                string logStr = null;
                if (data != null) {
                    logStr = string.Format("Response: {0} {1} -> [{2}] {3}\n{4}",
                                handler.Key, req.ToFullString(), status,
                                result, data.ToFullString());
                } else {
                    logStr = string.Format("Response: {0} {1} -> [{2}] {3}",
                                handler.Key, req.ToFullString(), status,
                                result);
                }
                handler.Debug(logStr);
            }
            return res;
        }

        private static Data Error(Handler handler, Data req, int status, string error, Data data, bool isDebug) {
            Data res = new Data();
            if (req != null) {
                res.SetData(ResponseConsts.KeyReq, req);
            }
            res.SetInt(ResponseConsts.KeyStatus, status);
            if (error == null) {
                error = string.Empty;
            }
            if (!string.IsNullOrEmpty(error)) {
                res.SetString(ResponseConsts.KeyError, error);
            }
            if (data != null) {
                res.SetData(ResponseConsts.KeyData, data);
            }
            string logStr = null;
            if (data != null) {
                logStr = string.Format("Error Response: {0} {1} -> [{2}] {3}\n{4}",
                            handler.Key, req.ToFullString(), status, error, data.ToFullString());
            } else {
                logStr = string.Format("Error Response: {0} {1} -> [{2}] {3}",
                            handler.Key, req.ToFullString(), status, error);
            }
            handler.ErrorOrDebug(isDebug, "{0}", logStr);
            return res;
        }

        public static Data Ok(Handler handler, Data req, string result = "OK", Data data = null) {
            return Result(handler, req, ResponseConsts.OK, result, data);
        }

        public static Data Accepted(Handler handler, Data req, string result, Data data = null) {
            return Result(handler, req, ResponseConsts.ACCEPTED, result, data);
        }

        public static Data BadRequest(Handler handler, Data req, string error, Data data = null) {
            return Error(handler, req, ResponseConsts.BAD_REQ, error, data, false);
        }

        public static Data InvalidParams(Handler handler, Data req, Data paramsHint) {
            return BadRequest(handler, req, "Invalid Params", paramsHint);
        }

        public static Data NotFound(Handler handler, Data req, string error, Data data = null) {
            return Error(handler, req, ResponseConsts.NOT_FOUND, error, data, false);
        }

        public static Data InternalError(Handler handler, Data req, string error, Data data = null) {
            return Error(handler, req, ResponseConsts.INTERNAL_ERROR, error, data, false);
        }

        public static Data NotImplemented(Handler handler, Data req, string error = null, Data data = null) {
            return Error(handler, req, ResponseConsts.NOT_IMPLEMENTED, error, data, false);
        }

        public static Data GetReq(ILogger logger, Data response) {
            if (logger == null) logger = Env.Instance;
            if (response == null) return null;

            return response.GetData(ResponseConsts.KeyReq, null);
        }

        public static int GetResStatus(Data res) {
            if (res == null) return ResponseConsts.INVALID;

            return res.GetInt(ResponseConsts.KeyStatus, ResponseConsts.INVALID);
        }

        public static bool IsResOk(Data res) {
            return GetResStatus(res) == ResponseConsts.OK;
        }

        public static bool IsResAccepted(Data res) {
            return GetResStatus(res) == ResponseConsts.ACCEPTED;
        }

        public static bool IsResFailed(Data res) {
            int status = GetResStatus(res);
            return (status != ResponseConsts.OK) && (status != ResponseConsts.ACCEPTED);
        }

        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Bool, bool)
        public static bool GetReqBool(ILogger logger, Data response, string key) {                     //__SILP__
            Data req = GetReq(logger, response);                                                      //__SILP__
            if (req == null) {                                                                        //__SILP__
                return default(bool);                                                                 //__SILP__
            }                                                                                         //__SILP__
            return req.GetBool(key);                                                                  //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public static bool GetReqBool(ILogger logger, Data response, string key, bool defaultValue) {  //__SILP__
            Data req = GetReq(logger, response);                                                      //__SILP__
            if (req == null) {                                                                        //__SILP__
                return defaultValue;                                                                  //__SILP__
            }                                                                                         //__SILP__
            return req.GetBool(key, defaultValue);                                                    //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Int, int)
        public static int GetReqInt(ILogger logger, Data response, string key) {                    //__SILP__
            Data req = GetReq(logger, response);                                                   //__SILP__
            if (req == null) {                                                                     //__SILP__
                return default(int);                                                               //__SILP__
            }                                                                                      //__SILP__
            return req.GetInt(key);                                                                //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        public static int GetReqInt(ILogger logger, Data response, string key, int defaultValue) {  //__SILP__
            Data req = GetReq(logger, response);                                                   //__SILP__
            if (req == null) {                                                                     //__SILP__
                return defaultValue;                                                               //__SILP__
            }                                                                                      //__SILP__
            return req.GetInt(key, defaultValue);                                                  //__SILP__
        }                                                                                          //__SILP__
                                                                                                   //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Long, long)
        public static long GetReqLong(ILogger logger, Data response, string key) {                     //__SILP__
            Data req = GetReq(logger, response);                                                      //__SILP__
            if (req == null) {                                                                        //__SILP__
                return default(long);                                                                 //__SILP__
            }                                                                                         //__SILP__
            return req.GetLong(key);                                                                  //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public static long GetReqLong(ILogger logger, Data response, string key, long defaultValue) {  //__SILP__
            Data req = GetReq(logger, response);                                                      //__SILP__
            if (req == null) {                                                                        //__SILP__
                return defaultValue;                                                                  //__SILP__
            }                                                                                         //__SILP__
            return req.GetLong(key, defaultValue);                                                    //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Float, float)
        public static float GetReqFloat(ILogger logger, Data response, string key) {                      //__SILP__
            Data req = GetReq(logger, response);                                                         //__SILP__
            if (req == null) {                                                                           //__SILP__
                return default(float);                                                                   //__SILP__
            }                                                                                            //__SILP__
            return req.GetFloat(key);                                                                    //__SILP__
        }                                                                                                //__SILP__
                                                                                                         //__SILP__
        public static float GetReqFloat(ILogger logger, Data response, string key, float defaultValue) {  //__SILP__
            Data req = GetReq(logger, response);                                                         //__SILP__
            if (req == null) {                                                                           //__SILP__
                return defaultValue;                                                                     //__SILP__
            }                                                                                            //__SILP__
            return req.GetFloat(key, defaultValue);                                                      //__SILP__
        }                                                                                                //__SILP__
                                                                                                         //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Double, double)
        public static double GetReqDouble(ILogger logger, Data response, string key) {                       //__SILP__
            Data req = GetReq(logger, response);                                                            //__SILP__
            if (req == null) {                                                                              //__SILP__
                return default(double);                                                                     //__SILP__
            }                                                                                               //__SILP__
            return req.GetDouble(key);                                                                      //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public static double GetReqDouble(ILogger logger, Data response, string key, double defaultValue) {  //__SILP__
            Data req = GetReq(logger, response);                                                            //__SILP__
            if (req == null) {                                                                              //__SILP__
                return defaultValue;                                                                        //__SILP__
            }                                                                                               //__SILP__
            return req.GetDouble(key, defaultValue);                                                        //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(String, string)
        public static string GetReqString(ILogger logger, Data response, string key) {                       //__SILP__
            Data req = GetReq(logger, response);                                                            //__SILP__
            if (req == null) {                                                                              //__SILP__
                return default(string);                                                                     //__SILP__
            }                                                                                               //__SILP__
            return req.GetString(key);                                                                      //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public static string GetReqString(ILogger logger, Data response, string key, string defaultValue) {  //__SILP__
            Data req = GetReq(logger, response);                                                            //__SILP__
            if (req == null) {                                                                              //__SILP__
                return defaultValue;                                                                        //__SILP__
            }                                                                                               //__SILP__
            return req.GetString(key, defaultValue);                                                        //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Data, Data)
        public static Data GetReqData(ILogger logger, Data response, string key) {                     //__SILP__
            Data req = GetReq(logger, response);                                                      //__SILP__
            if (req == null) {                                                                        //__SILP__
                return default(Data);                                                                 //__SILP__
            }                                                                                         //__SILP__
            return req.GetData(key);                                                                  //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
        public static Data GetReqData(ILogger logger, Data response, string key, Data defaultValue) {  //__SILP__
            Data req = GetReq(logger, response);                                                      //__SILP__
            if (req == null) {                                                                        //__SILP__
                return defaultValue;                                                                  //__SILP__
            }                                                                                         //__SILP__
            return req.GetData(key, defaultValue);                                                    //__SILP__
        }                                                                                             //__SILP__
                                                                                                      //__SILP__
    }
}
