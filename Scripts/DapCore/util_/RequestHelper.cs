using System;
using System.Collections;
using System.Collections.Generic;

using angeldnd.dap;

namespace angeldnd.dap {
    public static class RequestHelper {
        //SILP: REQUEST_HELPER_GET_TYPE(Bool, bool)
        public static bool GetBool(Handler handler, Data req, string key, Data hint) {                              //__SILP__
            if (req == null || !req.IsBool(key)) {                                                                  //__SILP__
                throw new BadRequestException(handler, req, hint);                                                  //__SILP__
            }                                                                                                       //__SILP__
            return req.GetBool(key);                                                                                //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
        public static bool GetBool(Handler handler, Data req, string key, string format, params object[] values) {  //__SILP__
            if (req == null || !req.IsBool(key)) {                                                                  //__SILP__
                throw new BadRequestException(handler, req, format, values);                                        //__SILP__
            }                                                                                                       //__SILP__
            return req.GetBool(key);                                                                                //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
        public static bool GetBool(Handler handler, Data req, string key) {                                         //__SILP__
            if (req == null || !req.IsBool(key)) {                                                                  //__SILP__
                throw new BadRequestException(handler, req, "Param Not Exist: [Bool] {0}", key);                    //__SILP__
            }                                                                                                       //__SILP__
            return req.GetBool(key);                                                                                //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
        public static bool GetBool(Data req, string key, bool defaultValue) {                                       //__SILP__
            if (req == null) {                                                                                      //__SILP__
                return defaultValue;                                                                                //__SILP__
            }                                                                                                       //__SILP__
            return req.GetBool(key, defaultValue);                                                                  //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
        //SILP: REQUEST_HELPER_GET_TYPE(Int, int)
        public static int GetInt(Handler handler, Data req, string key, Data hint) {                              //__SILP__
            if (req == null || !req.IsInt(key)) {                                                                 //__SILP__
                throw new BadRequestException(handler, req, hint);                                                //__SILP__
            }                                                                                                     //__SILP__
            return req.GetInt(key);                                                                               //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        public static int GetInt(Handler handler, Data req, string key, string format, params object[] values) {  //__SILP__
            if (req == null || !req.IsInt(key)) {                                                                 //__SILP__
                throw new BadRequestException(handler, req, format, values);                                      //__SILP__
            }                                                                                                     //__SILP__
            return req.GetInt(key);                                                                               //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        public static int GetInt(Handler handler, Data req, string key) {                                         //__SILP__
            if (req == null || !req.IsInt(key)) {                                                                 //__SILP__
                throw new BadRequestException(handler, req, "Param Not Exist: [Int] {0}", key);                   //__SILP__
            }                                                                                                     //__SILP__
            return req.GetInt(key);                                                                               //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        public static int GetInt(Data req, string key, int defaultValue) {                                        //__SILP__
            if (req == null) {                                                                                    //__SILP__
                return defaultValue;                                                                              //__SILP__
            }                                                                                                     //__SILP__
            return req.GetInt(key, defaultValue);                                                                 //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        //SILP: REQUEST_HELPER_GET_TYPE(Long, long)
        public static long GetLong(Handler handler, Data req, string key, Data hint) {                              //__SILP__
            if (req == null || !req.IsLong(key)) {                                                                  //__SILP__
                throw new BadRequestException(handler, req, hint);                                                  //__SILP__
            }                                                                                                       //__SILP__
            return req.GetLong(key);                                                                                //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
        public static long GetLong(Handler handler, Data req, string key, string format, params object[] values) {  //__SILP__
            if (req == null || !req.IsLong(key)) {                                                                  //__SILP__
                throw new BadRequestException(handler, req, format, values);                                        //__SILP__
            }                                                                                                       //__SILP__
            return req.GetLong(key);                                                                                //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
        public static long GetLong(Handler handler, Data req, string key) {                                         //__SILP__
            if (req == null || !req.IsLong(key)) {                                                                  //__SILP__
                throw new BadRequestException(handler, req, "Param Not Exist: [Long] {0}", key);                    //__SILP__
            }                                                                                                       //__SILP__
            return req.GetLong(key);                                                                                //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
        public static long GetLong(Data req, string key, long defaultValue) {                                       //__SILP__
            if (req == null) {                                                                                      //__SILP__
                return defaultValue;                                                                                //__SILP__
            }                                                                                                       //__SILP__
            return req.GetLong(key, defaultValue);                                                                  //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
        //SILP: REQUEST_HELPER_GET_TYPE(Float, float)
        public static float GetFloat(Handler handler, Data req, string key, Data hint) {                              //__SILP__
            if (req == null || !req.IsFloat(key)) {                                                                   //__SILP__
                throw new BadRequestException(handler, req, hint);                                                    //__SILP__
            }                                                                                                         //__SILP__
            return req.GetFloat(key);                                                                                 //__SILP__
        }                                                                                                             //__SILP__
                                                                                                                      //__SILP__
        public static float GetFloat(Handler handler, Data req, string key, string format, params object[] values) {  //__SILP__
            if (req == null || !req.IsFloat(key)) {                                                                   //__SILP__
                throw new BadRequestException(handler, req, format, values);                                          //__SILP__
            }                                                                                                         //__SILP__
            return req.GetFloat(key);                                                                                 //__SILP__
        }                                                                                                             //__SILP__
                                                                                                                      //__SILP__
        public static float GetFloat(Handler handler, Data req, string key) {                                         //__SILP__
            if (req == null || !req.IsFloat(key)) {                                                                   //__SILP__
                throw new BadRequestException(handler, req, "Param Not Exist: [Float] {0}", key);                     //__SILP__
            }                                                                                                         //__SILP__
            return req.GetFloat(key);                                                                                 //__SILP__
        }                                                                                                             //__SILP__
                                                                                                                      //__SILP__
        public static float GetFloat(Data req, string key, float defaultValue) {                                      //__SILP__
            if (req == null) {                                                                                        //__SILP__
                return defaultValue;                                                                                  //__SILP__
            }                                                                                                         //__SILP__
            return req.GetFloat(key, defaultValue);                                                                   //__SILP__
        }                                                                                                             //__SILP__
                                                                                                                      //__SILP__
        //SILP: REQUEST_HELPER_GET_TYPE(Double, double)
        public static double GetDouble(Handler handler, Data req, string key, Data hint) {                              //__SILP__
            if (req == null || !req.IsDouble(key)) {                                                                    //__SILP__
                throw new BadRequestException(handler, req, hint);                                                      //__SILP__
            }                                                                                                           //__SILP__
            return req.GetDouble(key);                                                                                  //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public static double GetDouble(Handler handler, Data req, string key, string format, params object[] values) {  //__SILP__
            if (req == null || !req.IsDouble(key)) {                                                                    //__SILP__
                throw new BadRequestException(handler, req, format, values);                                            //__SILP__
            }                                                                                                           //__SILP__
            return req.GetDouble(key);                                                                                  //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public static double GetDouble(Handler handler, Data req, string key) {                                         //__SILP__
            if (req == null || !req.IsDouble(key)) {                                                                    //__SILP__
                throw new BadRequestException(handler, req, "Param Not Exist: [Double] {0}", key);                      //__SILP__
            }                                                                                                           //__SILP__
            return req.GetDouble(key);                                                                                  //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public static double GetDouble(Data req, string key, double defaultValue) {                                     //__SILP__
            if (req == null) {                                                                                          //__SILP__
                return defaultValue;                                                                                    //__SILP__
            }                                                                                                           //__SILP__
            return req.GetDouble(key, defaultValue);                                                                    //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        //SILP: REQUEST_HELPER_GET_TYPE(String, string)
        public static string GetString(Handler handler, Data req, string key, Data hint) {                              //__SILP__
            if (req == null || !req.IsString(key)) {                                                                    //__SILP__
                throw new BadRequestException(handler, req, hint);                                                      //__SILP__
            }                                                                                                           //__SILP__
            return req.GetString(key);                                                                                  //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public static string GetString(Handler handler, Data req, string key, string format, params object[] values) {  //__SILP__
            if (req == null || !req.IsString(key)) {                                                                    //__SILP__
                throw new BadRequestException(handler, req, format, values);                                            //__SILP__
            }                                                                                                           //__SILP__
            return req.GetString(key);                                                                                  //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public static string GetString(Handler handler, Data req, string key) {                                         //__SILP__
            if (req == null || !req.IsString(key)) {                                                                    //__SILP__
                throw new BadRequestException(handler, req, "Param Not Exist: [String] {0}", key);                      //__SILP__
            }                                                                                                           //__SILP__
            return req.GetString(key);                                                                                  //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        public static string GetString(Data req, string key, string defaultValue) {                                     //__SILP__
            if (req == null) {                                                                                          //__SILP__
                return defaultValue;                                                                                    //__SILP__
            }                                                                                                           //__SILP__
            return req.GetString(key, defaultValue);                                                                    //__SILP__
        }                                                                                                               //__SILP__
                                                                                                                        //__SILP__
        //SILP: REQUEST_HELPER_GET_TYPE(Data, Data)
        public static Data GetData(Handler handler, Data req, string key, Data hint) {                              //__SILP__
            if (req == null || !req.IsData(key)) {                                                                  //__SILP__
                throw new BadRequestException(handler, req, hint);                                                  //__SILP__
            }                                                                                                       //__SILP__
            return req.GetData(key);                                                                                //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
        public static Data GetData(Handler handler, Data req, string key, string format, params object[] values) {  //__SILP__
            if (req == null || !req.IsData(key)) {                                                                  //__SILP__
                throw new BadRequestException(handler, req, format, values);                                        //__SILP__
            }                                                                                                       //__SILP__
            return req.GetData(key);                                                                                //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
        public static Data GetData(Handler handler, Data req, string key) {                                         //__SILP__
            if (req == null || !req.IsData(key)) {                                                                  //__SILP__
                throw new BadRequestException(handler, req, "Param Not Exist: [Data] {0}", key);                    //__SILP__
            }                                                                                                       //__SILP__
            return req.GetData(key);                                                                                //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
        public static Data GetData(Data req, string key, Data defaultValue) {                                       //__SILP__
            if (req == null) {                                                                                      //__SILP__
                return defaultValue;                                                                                //__SILP__
            }                                                                                                       //__SILP__
            return req.GetData(key, defaultValue);                                                                  //__SILP__
        }                                                                                                           //__SILP__
                                                                                                                    //__SILP__
    }
}
