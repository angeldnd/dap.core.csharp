using System;
using System.Collections;
using System.Collections.Generic;

using angeldnd.dap;

namespace angeldnd.dap {
    public static class ResponseConsts {
        [DapParam(typeof(string))]
        public const string KeyUri = "uri";
        [DapParam(typeof(Data))]
        public const string KeyReq = "req";
        [DapParam(typeof(int))]
        public const string KeyStatus = "status";
        [DapParam(typeof(Data))]
        public const string KeyResult = "result";

        [DapParam(typeof(string))]
        public const string KeyMsg = "msg";

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
        private static Data Result(Handler handler, Data req, int status, Data result) {
            Data res = new Data();
            res.SetInt(ResponseConsts.KeyStatus, status);
            res.SetString(ResponseConsts.KeyUri, handler.Uri);
            if (result != null) {
                res.SetData(ResponseConsts.KeyResult, result);
            }
            if (req != null) {
                res.SetData(ResponseConsts.KeyReq, req);
            }
            return res;
        }

        public static Data Ok(Handler handler, Data req, Data result = null) {
            return Result(handler, req, ResponseConsts.OK, result);
        }

        public static Data Ok(Handler handler, Data req, string format, params object[] values) {
            return Ok(handler, req, new Data().S(ResponseConsts.KeyMsg, Log.GetMsg(format, values)));
        }

        public static Data Accepted(Handler handler, Data req, Data result = null) {
            return Result(handler, req, ResponseConsts.ACCEPTED, result);
        }

        public static Data Accepted(Handler handler, Data req, string format, params object[] values) {
            return Accepted(handler, req, new Data().S(ResponseConsts.KeyMsg, Log.GetMsg(format, values)));
        }

        public static Data BadRequest(Handler handler, Data req, Data result = null) {
            return Result(handler, req, ResponseConsts.BAD_REQ, result);
        }

        public static Data BadRequest(Handler handler, Data req, string format, params object[] values) {
            return BadRequest(handler, req, new Data().S(ResponseConsts.KeyMsg, Log.GetMsg(format, values)));
        }

        public static Data InvalidParams(Handler handler, Data req, Data paramsHint) {
            if (paramsHint == null) paramsHint = new Data();
            paramsHint.S(ResponseConsts.KeyMsg, "Invalid Params");
            return BadRequest(handler, req, paramsHint);
        }

        public static Data NotFound(Handler handler, Data req, Data data = null) {
            return Result(handler, req, ResponseConsts.NOT_FOUND, data);
        }

        public static Data NotFound(Handler handler, Data req, string format, params object[] values) {
            return NotFound(handler, req, new Data().S(ResponseConsts.KeyMsg, Log.GetMsg(format, values)));
        }

        public static Data InternalError(Handler handler, Data req, Data data = null) {
            return Result(handler, req, ResponseConsts.INTERNAL_ERROR, data);
        }

        public static Data InternalError(Handler handler, Data req, string format, params object[] values) {
            return InternalError(handler, req, new Data().S(ResponseConsts.KeyMsg, Log.GetMsg(format, values)));
        }

        public static Data NotImplemented(Handler handler, Data req, Data data = null) {
            return Result(handler, req, ResponseConsts.NOT_IMPLEMENTED, data);
        }

        public static Data NotImplemented(Handler handler, Data req, string format, params object[] values) {
            return NotImplemented(handler, req, new Data().S(ResponseConsts.KeyMsg, Log.GetMsg(format, values)));
        }

        public static Data GetReq(Data response) {
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

        public static Data GetResult(Data response) {
            if (response == null) return null;

            return response.GetData(ResponseConsts.KeyResult, null);
        }

        public static string GetMsg(Data response) {
            return GetResultString(response, ResponseConsts.KeyMsg, string.Empty);
        }

        //SILP: RESPONSE_HELPER_GET_RESULT_TYPE(Bool, bool)
        public static bool GetResultBool(Data res, string key) {                     //__SILP__
            Data result = GetResult(res);                                            //__SILP__
            if (result == null) {                                                    //__SILP__
                return default(bool);                                                //__SILP__
            }                                                                        //__SILP__
            return result.GetBool(key);                                              //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public static bool GetResultBool(Data res, string key, bool defaultValue) {  //__SILP__
            Data result = GetResult(res);                                            //__SILP__
            if (result == null) {                                                    //__SILP__
                return defaultValue;                                                 //__SILP__
            }                                                                        //__SILP__
            return result.GetBool(key, defaultValue);                                //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        //SILP: RESPONSE_HELPER_GET_RESULT_TYPE(Int, int)
        public static int GetResultInt(Data res, string key) {                    //__SILP__
            Data result = GetResult(res);                                         //__SILP__
            if (result == null) {                                                 //__SILP__
                return default(int);                                              //__SILP__
            }                                                                     //__SILP__
            return result.GetInt(key);                                            //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public static int GetResultInt(Data res, string key, int defaultValue) {  //__SILP__
            Data result = GetResult(res);                                         //__SILP__
            if (result == null) {                                                 //__SILP__
                return defaultValue;                                              //__SILP__
            }                                                                     //__SILP__
            return result.GetInt(key, defaultValue);                              //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        //SILP: RESPONSE_HELPER_GET_RESULT_TYPE(Long, long)
        public static long GetResultLong(Data res, string key) {                     //__SILP__
            Data result = GetResult(res);                                            //__SILP__
            if (result == null) {                                                    //__SILP__
                return default(long);                                                //__SILP__
            }                                                                        //__SILP__
            return result.GetLong(key);                                              //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public static long GetResultLong(Data res, string key, long defaultValue) {  //__SILP__
            Data result = GetResult(res);                                            //__SILP__
            if (result == null) {                                                    //__SILP__
                return defaultValue;                                                 //__SILP__
            }                                                                        //__SILP__
            return result.GetLong(key, defaultValue);                                //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        //SILP: RESPONSE_HELPER_GET_RESULT_TYPE(Float, float)
        public static float GetResultFloat(Data res, string key) {                      //__SILP__
            Data result = GetResult(res);                                               //__SILP__
            if (result == null) {                                                       //__SILP__
                return default(float);                                                  //__SILP__
            }                                                                           //__SILP__
            return result.GetFloat(key);                                                //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        public static float GetResultFloat(Data res, string key, float defaultValue) {  //__SILP__
            Data result = GetResult(res);                                               //__SILP__
            if (result == null) {                                                       //__SILP__
                return defaultValue;                                                    //__SILP__
            }                                                                           //__SILP__
            return result.GetFloat(key, defaultValue);                                  //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        //SILP: RESPONSE_HELPER_GET_RESULT_TYPE(Double, double)
        public static double GetResultDouble(Data res, string key) {                       //__SILP__
            Data result = GetResult(res);                                                  //__SILP__
            if (result == null) {                                                          //__SILP__
                return default(double);                                                    //__SILP__
            }                                                                              //__SILP__
            return result.GetDouble(key);                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public static double GetResultDouble(Data res, string key, double defaultValue) {  //__SILP__
            Data result = GetResult(res);                                                  //__SILP__
            if (result == null) {                                                          //__SILP__
                return defaultValue;                                                       //__SILP__
            }                                                                              //__SILP__
            return result.GetDouble(key, defaultValue);                                    //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        //SILP: RESPONSE_HELPER_GET_RESULT_TYPE(String, string)
        public static string GetResultString(Data res, string key) {                       //__SILP__
            Data result = GetResult(res);                                                  //__SILP__
            if (result == null) {                                                          //__SILP__
                return default(string);                                                    //__SILP__
            }                                                                              //__SILP__
            return result.GetString(key);                                                  //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        public static string GetResultString(Data res, string key, string defaultValue) {  //__SILP__
            Data result = GetResult(res);                                                  //__SILP__
            if (result == null) {                                                          //__SILP__
                return defaultValue;                                                       //__SILP__
            }                                                                              //__SILP__
            return result.GetString(key, defaultValue);                                    //__SILP__
        }                                                                                  //__SILP__
                                                                                           //__SILP__
        //SILP: RESPONSE_HELPER_GET_RESULT_TYPE(Data, Data)
        public static Data GetResultData(Data res, string key) {                     //__SILP__
            Data result = GetResult(res);                                            //__SILP__
            if (result == null) {                                                    //__SILP__
                return default(Data);                                                //__SILP__
            }                                                                        //__SILP__
            return result.GetData(key);                                              //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public static Data GetResultData(Data res, string key, Data defaultValue) {  //__SILP__
            Data result = GetResult(res);                                            //__SILP__
            if (result == null) {                                                    //__SILP__
                return defaultValue;                                                 //__SILP__
            }                                                                        //__SILP__
            return result.GetData(key, defaultValue);                                //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__

        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Bool, bool)
        public static bool GetReqBool(Data res, string key) {                     //__SILP__
            Data req = GetReq(res);                                               //__SILP__
            if (req == null) {                                                    //__SILP__
                return default(bool);                                             //__SILP__
            }                                                                     //__SILP__
            return req.GetBool(key);                                              //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public static bool GetReqBool(Data res, string key, bool defaultValue) {  //__SILP__
            Data req = GetReq(res);                                               //__SILP__
            if (req == null) {                                                    //__SILP__
                return defaultValue;                                              //__SILP__
            }                                                                     //__SILP__
            return req.GetBool(key, defaultValue);                                //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Int, int)
        public static int GetReqInt(Data res, string key) {                    //__SILP__
            Data req = GetReq(res);                                            //__SILP__
            if (req == null) {                                                 //__SILP__
                return default(int);                                           //__SILP__
            }                                                                  //__SILP__
            return req.GetInt(key);                                            //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public static int GetReqInt(Data res, string key, int defaultValue) {  //__SILP__
            Data req = GetReq(res);                                            //__SILP__
            if (req == null) {                                                 //__SILP__
                return defaultValue;                                           //__SILP__
            }                                                                  //__SILP__
            return req.GetInt(key, defaultValue);                              //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Long, long)
        public static long GetReqLong(Data res, string key) {                     //__SILP__
            Data req = GetReq(res);                                               //__SILP__
            if (req == null) {                                                    //__SILP__
                return default(long);                                             //__SILP__
            }                                                                     //__SILP__
            return req.GetLong(key);                                              //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public static long GetReqLong(Data res, string key, long defaultValue) {  //__SILP__
            Data req = GetReq(res);                                               //__SILP__
            if (req == null) {                                                    //__SILP__
                return defaultValue;                                              //__SILP__
            }                                                                     //__SILP__
            return req.GetLong(key, defaultValue);                                //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Float, float)
        public static float GetReqFloat(Data res, string key) {                      //__SILP__
            Data req = GetReq(res);                                                  //__SILP__
            if (req == null) {                                                       //__SILP__
                return default(float);                                               //__SILP__
            }                                                                        //__SILP__
            return req.GetFloat(key);                                                //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        public static float GetReqFloat(Data res, string key, float defaultValue) {  //__SILP__
            Data req = GetReq(res);                                                  //__SILP__
            if (req == null) {                                                       //__SILP__
                return defaultValue;                                                 //__SILP__
            }                                                                        //__SILP__
            return req.GetFloat(key, defaultValue);                                  //__SILP__
        }                                                                            //__SILP__
                                                                                     //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Double, double)
        public static double GetReqDouble(Data res, string key) {                       //__SILP__
            Data req = GetReq(res);                                                     //__SILP__
            if (req == null) {                                                          //__SILP__
                return default(double);                                                 //__SILP__
            }                                                                           //__SILP__
            return req.GetDouble(key);                                                  //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        public static double GetReqDouble(Data res, string key, double defaultValue) {  //__SILP__
            Data req = GetReq(res);                                                     //__SILP__
            if (req == null) {                                                          //__SILP__
                return defaultValue;                                                    //__SILP__
            }                                                                           //__SILP__
            return req.GetDouble(key, defaultValue);                                    //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(String, string)
        public static string GetReqString(Data res, string key) {                       //__SILP__
            Data req = GetReq(res);                                                     //__SILP__
            if (req == null) {                                                          //__SILP__
                return default(string);                                                 //__SILP__
            }                                                                           //__SILP__
            return req.GetString(key);                                                  //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        public static string GetReqString(Data res, string key, string defaultValue) {  //__SILP__
            Data req = GetReq(res);                                                     //__SILP__
            if (req == null) {                                                          //__SILP__
                return defaultValue;                                                    //__SILP__
            }                                                                           //__SILP__
            return req.GetString(key, defaultValue);                                    //__SILP__
        }                                                                               //__SILP__
                                                                                        //__SILP__
        //SILP: RESPONSE_HELPER_GET_REQ_TYPE(Data, Data)
        public static Data GetReqData(Data res, string key) {                     //__SILP__
            Data req = GetReq(res);                                               //__SILP__
            if (req == null) {                                                    //__SILP__
                return default(Data);                                             //__SILP__
            }                                                                     //__SILP__
            return req.GetData(key);                                              //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
        public static Data GetReqData(Data res, string key, Data defaultValue) {  //__SILP__
            Data req = GetReq(res);                                               //__SILP__
            if (req == null) {                                                    //__SILP__
                return defaultValue;                                              //__SILP__
            }                                                                     //__SILP__
            return req.GetData(key, defaultValue);                                //__SILP__
        }                                                                         //__SILP__
                                                                                  //__SILP__
    }
}
