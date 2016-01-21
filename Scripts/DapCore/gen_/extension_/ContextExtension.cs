using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class ContextExtension {
        public static IProperty AddProperty(this IContext context, string path,
                                            Pass pass, bool open, Data data) {
            return context.Properties.AddProperty(path, pass, open, data);
        }

        public static IProperty GetProperty(this IContext context, string path) {
            return context.Properties.Get(path);
        }

        public static bool HasProperty(this IContext context, string path) {
            return context.Properties.Has(path);
        }

        public static bool FireEvent(this IContext context, string channelPath, Pass pass, Data evt) {
            return context.Channels.FireEvent(channelPath, pass, evt);
        }

        public static bool FireEvent(this IContext context, string channelPath, Pass pass) {
            return context.Channels.FireEvent(channelPath, pass, null);
        }

        public static bool FireEvent(this IContext context, string channelPath, Data evt) {
            return context.Channels.FireEvent(channelPath, evt);
        }

        public static bool FireEvent(this IContext context, string channelPath) {
            return context.Channels.FireEvent(channelPath);
        }

        public static Channel GetChannel(this IContext context, string channelPath) {
            return context.Channels.GetChannel(channelPath);
        }

        public static Channel AddChannel(this IContext context, string channelPath) {
            return context.Channels.AddChannel(channelPath);
        }

        public static Channel AddChannel(this IContext context, string channelPath, Pass pass) {
            return context.Channels.AddChannel(channelPath, pass);
        }

        public static Data HandleRequest(this IContext context, string handlerPath, Pass pass, Data req) {
            return context.Handlers.HandleRequest(handlerPath, pass, req);
        }

        public static Data HandleRequest(this IContext context, string handlerPath, Data req) {
            return context.Handlers.HandleRequest(handlerPath, req);
        }

        public static Data HandleRequest(this IContext context, string handlerPath) {
            return context.Handlers.HandleRequest(handlerPath);
        }

        public static Handler GetHandler(this IContext context, string handlerPath) {
            return context.Handlers.GetHandler(handlerPath);
        }

        public static Handler AddHandler(this IContext context, string handlerPath) {
            return context.Handlers.AddHandler(handlerPath);
        }

        public static Handler AddHandler(this IContext context, string handlerPath, Pass pass) {
            return context.Handlers.AddHandler(handlerPath, pass);
        }

        public static Handler AddHandler(this IContext context, string handlerPath, Pass pass, bool open, IRequestHandler requestHandler) {
            Handler handler = AddHandler(context, handlerPath, open ? pass.Open : pass);
            if (handler != null) {
                handler.Setup(pass, requestHandler);
            }
            return handler;
        }

        public static bool HasVar(this IContext context, string varPath) {
            return context.Vars.Has(varPath);
        }

        public static IVar GetVar(this IContext context, string path) {
            return context.Vars.Get(path);
        }

        public static bool SetVarValue<T>(this IContext context, string varPath, Pass pass, T val) {
            return context.Vars.SetValue<T>(varPath, pass, val);
        }

        public static bool SetVarValue<T>(this IContext context, string varPath, T val) {
            return context.Vars.SetValue<T>(varPath, null, val);
        }

        public static T GetVarValue<T>(this IContext context, string varPath, T defaultValue) {
            return context.Vars.GetValue<T>(varPath, defaultValue);
        }

        public static T GetVarValue<T>(this IContext context, string varPath) {
            return context.Vars.GetValue<T>(varPath, default(T));
        }

        public static T DepositVarValue<T>(this IContext context, string varPath, Pass pass, T val) {
            return context.Vars.DepositValue<T>(varPath, pass, val);
        }

        public static T DepositVarValue<T>(this IContext context, string varPath, T val) {
            return context.Vars.DepositValue<T>(varPath, val);
        }

        public static T WithdrawVarValue<T>(this IContext context, string varPath, Pass pass, T defaultValue) {
            return context.Vars.WithdrawValue<T>(varPath, pass, defaultValue);
        }

        public static T WithdrawVarValue<T>(this IContext context, string varPath, T defaultValue) {
            return context.Vars.WithdrawValue<T>(varPath, defaultValue);
        }

        public static T WithdrawVarValue<T>(this IContext context, string varPath, Pass pass) {
            return context.Vars.WithdrawValue<T>(varPath, pass);
        }

        public static T WithdrawVarValue<T>(this IContext context, string varPath) {
            return context.Vars.WithdrawValue<T>(varPath);
        }

        //SILP:CONTEXT_DEPOSIT_WITHDRAW(PropertyPass, Pass, ContextConsts.VarsPropertyPasses, passValue)
        public static Pass DepositPropertyPass(this IContext context, string key, Pass passValue) {  //__SILP__
            string varPath = ContextConsts.GetVarPath(ContextConsts.VarsPropertyPasses, key);        //__SILP__
            return context.Vars.DepositValue<Pass>(varPath, null, passValue);                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        public static Pass WithdrawPropertyPass(this IContext context, string key) {                 //__SILP__
            string varPath = ContextConsts.GetVarPath(ContextConsts.VarsPropertyPasses, key);        //__SILP__
            return context.Vars.WithdrawValue<Pass>(varPath);                                        //__SILP__
        }                                                                                            //__SILP__
                                                                                                     //__SILP__
        //SILP:CONTEXT_DEPOSIT_WITHDRAW(ChannelPass, Pass, ContextConsts.VarsChannelPasses, passValue)
        public static Pass DepositChannelPass(this IContext context, string key, Pass passValue) {  //__SILP__
            string varPath = ContextConsts.GetVarPath(ContextConsts.VarsChannelPasses, key);        //__SILP__
            return context.Vars.DepositValue<Pass>(varPath, null, passValue);                       //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__
        public static Pass WithdrawChannelPass(this IContext context, string key) {                 //__SILP__
            string varPath = ContextConsts.GetVarPath(ContextConsts.VarsChannelPasses, key);        //__SILP__
            return context.Vars.WithdrawValue<Pass>(varPath);                                       //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__
        //SILP:CONTEXT_DEPOSIT_WITHDRAW(HandlerPass, Pass, ContextConsts.VarsHandlerPasses, passValue)
        public static Pass DepositHandlerPass(this IContext context, string key, Pass passValue) {  //__SILP__
            string varPath = ContextConsts.GetVarPath(ContextConsts.VarsHandlerPasses, key);        //__SILP__
            return context.Vars.DepositValue<Pass>(varPath, null, passValue);                       //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__
        public static Pass WithdrawHandlerPass(this IContext context, string key) {                 //__SILP__
            string varPath = ContextConsts.GetVarPath(ContextConsts.VarsHandlerPasses, key);        //__SILP__
            return context.Vars.WithdrawValue<Pass>(varPath);                                       //__SILP__
        }                                                                                           //__SILP__
                                                                                                    //__SILP__

        //SILP: CONTEXT_PROPERTIES_HELPER(Bool, bool)
        public static BoolProperty AddBool(this IContext context, string path, Pass propertyPass, bool val) {  //__SILP__
            return context.Properties.AddBool(path, propertyPass, val);                                        //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static BoolProperty AddBool(this IContext context, string path, bool val) {                     //__SILP__
            return context.Properties.AddBool(path, val);                                                      //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static BoolProperty RemoveBool(this IContext context, string path, Pass pass) {                 //__SILP__
            return context.Properties.RemoveBool(path, pass);                                                  //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static BoolProperty RemoveBool(this IContext context, string path) {                            //__SILP__
            return context.Properties.RemoveBool(path);                                                        //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static bool IsBool(this IContext context, string path) {                                        //__SILP__
            return context.Properties.IsBool(path);                                                            //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static bool GetBool(this IContext context, string path) {                                       //__SILP__
            return context.Properties.GetBool(path);                                                           //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static bool GetBool(this IContext context, string path, bool defaultValue) {                    //__SILP__
            return context.Properties.GetBool(path, defaultValue);                                             //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static bool SetBool(this IContext context, string path, Pass propertyPass, bool value) {        //__SILP__
            return context.Properties.SetBool(path, propertyPass, value);                                      //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static bool SetBool(this IContext context, string path, bool value) {                           //__SILP__
            return context.Properties.SetBool(path, value);                                                    //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__

        //SILP: CONTEXT_PROPERTIES_HELPER(Int, int)
        public static IntProperty AddInt(this IContext context, string path, Pass propertyPass, int val) {  //__SILP__
            return context.Properties.AddInt(path, propertyPass, val);                                      //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public static IntProperty AddInt(this IContext context, string path, int val) {                     //__SILP__
            return context.Properties.AddInt(path, val);                                                    //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public static IntProperty RemoveInt(this IContext context, string path, Pass pass) {                //__SILP__
            return context.Properties.RemoveInt(path, pass);                                                //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public static IntProperty RemoveInt(this IContext context, string path) {                           //__SILP__
            return context.Properties.RemoveInt(path);                                                      //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public static bool IsInt(this IContext context, string path) {                                      //__SILP__
            return context.Properties.IsInt(path);                                                          //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public static int GetInt(this IContext context, string path) {                                      //__SILP__
            return context.Properties.GetInt(path);                                                         //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public static int GetInt(this IContext context, string path, int defaultValue) {                    //__SILP__
            return context.Properties.GetInt(path, defaultValue);                                           //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public static bool SetInt(this IContext context, string path, Pass propertyPass, int value) {       //__SILP__
            return context.Properties.SetInt(path, propertyPass, value);                                    //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        public static bool SetInt(this IContext context, string path, int value) {                          //__SILP__
            return context.Properties.SetInt(path, value);                                                  //__SILP__
        }                                                                                                   //__SILP__
                                                                                                            //__SILP__
        //SILP: CONTEXT_PROPERTIES_HELPER(Long, long)
        public static LongProperty AddLong(this IContext context, string path, Pass propertyPass, long val) {  //__SILP__
            return context.Properties.AddLong(path, propertyPass, val);                                        //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static LongProperty AddLong(this IContext context, string path, long val) {                     //__SILP__
            return context.Properties.AddLong(path, val);                                                      //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static LongProperty RemoveLong(this IContext context, string path, Pass pass) {                 //__SILP__
            return context.Properties.RemoveLong(path, pass);                                                  //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static LongProperty RemoveLong(this IContext context, string path) {                            //__SILP__
            return context.Properties.RemoveLong(path);                                                        //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static bool IsLong(this IContext context, string path) {                                        //__SILP__
            return context.Properties.IsLong(path);                                                            //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static long GetLong(this IContext context, string path) {                                       //__SILP__
            return context.Properties.GetLong(path);                                                           //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static long GetLong(this IContext context, string path, long defaultValue) {                    //__SILP__
            return context.Properties.GetLong(path, defaultValue);                                             //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static bool SetLong(this IContext context, string path, Pass propertyPass, long value) {        //__SILP__
            return context.Properties.SetLong(path, propertyPass, value);                                      //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static bool SetLong(this IContext context, string path, long value) {                           //__SILP__
            return context.Properties.SetLong(path, value);                                                    //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        //SILP: CONTEXT_PROPERTIES_HELPER(Float, float)
        public static FloatProperty AddFloat(this IContext context, string path, Pass propertyPass, float val) {  //__SILP__
            return context.Properties.AddFloat(path, propertyPass, val);                                          //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        public static FloatProperty AddFloat(this IContext context, string path, float val) {                     //__SILP__
            return context.Properties.AddFloat(path, val);                                                        //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        public static FloatProperty RemoveFloat(this IContext context, string path, Pass pass) {                  //__SILP__
            return context.Properties.RemoveFloat(path, pass);                                                    //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        public static FloatProperty RemoveFloat(this IContext context, string path) {                             //__SILP__
            return context.Properties.RemoveFloat(path);                                                          //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        public static bool IsFloat(this IContext context, string path) {                                          //__SILP__
            return context.Properties.IsFloat(path);                                                              //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        public static float GetFloat(this IContext context, string path) {                                        //__SILP__
            return context.Properties.GetFloat(path);                                                             //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        public static float GetFloat(this IContext context, string path, float defaultValue) {                    //__SILP__
            return context.Properties.GetFloat(path, defaultValue);                                               //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        public static bool SetFloat(this IContext context, string path, Pass propertyPass, float value) {         //__SILP__
            return context.Properties.SetFloat(path, propertyPass, value);                                        //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        public static bool SetFloat(this IContext context, string path, float value) {                            //__SILP__
            return context.Properties.SetFloat(path, value);                                                      //__SILP__
        }                                                                                                         //__SILP__
                                                                                                                  //__SILP__
        //SILP: CONTEXT_PROPERTIES_HELPER(Double, double)
        public static DoubleProperty AddDouble(this IContext context, string path, Pass propertyPass, double val) {  //__SILP__
            return context.Properties.AddDouble(path, propertyPass, val);                                            //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static DoubleProperty AddDouble(this IContext context, string path, double val) {                     //__SILP__
            return context.Properties.AddDouble(path, val);                                                          //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static DoubleProperty RemoveDouble(this IContext context, string path, Pass pass) {                   //__SILP__
            return context.Properties.RemoveDouble(path, pass);                                                      //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static DoubleProperty RemoveDouble(this IContext context, string path) {                              //__SILP__
            return context.Properties.RemoveDouble(path);                                                            //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static bool IsDouble(this IContext context, string path) {                                            //__SILP__
            return context.Properties.IsDouble(path);                                                                //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static double GetDouble(this IContext context, string path) {                                         //__SILP__
            return context.Properties.GetDouble(path);                                                               //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static double GetDouble(this IContext context, string path, double defaultValue) {                    //__SILP__
            return context.Properties.GetDouble(path, defaultValue);                                                 //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static bool SetDouble(this IContext context, string path, Pass propertyPass, double value) {          //__SILP__
            return context.Properties.SetDouble(path, propertyPass, value);                                          //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static bool SetDouble(this IContext context, string path, double value) {                             //__SILP__
            return context.Properties.SetDouble(path, value);                                                        //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        //SILP: CONTEXT_PROPERTIES_HELPER(String, string)
        public static StringProperty AddString(this IContext context, string path, Pass propertyPass, string val) {  //__SILP__
            return context.Properties.AddString(path, propertyPass, val);                                            //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static StringProperty AddString(this IContext context, string path, string val) {                     //__SILP__
            return context.Properties.AddString(path, val);                                                          //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static StringProperty RemoveString(this IContext context, string path, Pass pass) {                   //__SILP__
            return context.Properties.RemoveString(path, pass);                                                      //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static StringProperty RemoveString(this IContext context, string path) {                              //__SILP__
            return context.Properties.RemoveString(path);                                                            //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static bool IsString(this IContext context, string path) {                                            //__SILP__
            return context.Properties.IsString(path);                                                                //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static string GetString(this IContext context, string path) {                                         //__SILP__
            return context.Properties.GetString(path);                                                               //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static string GetString(this IContext context, string path, string defaultValue) {                    //__SILP__
            return context.Properties.GetString(path, defaultValue);                                                 //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static bool SetString(this IContext context, string path, Pass propertyPass, string value) {          //__SILP__
            return context.Properties.SetString(path, propertyPass, value);                                          //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        public static bool SetString(this IContext context, string path, string value) {                             //__SILP__
            return context.Properties.SetString(path, value);                                                        //__SILP__
        }                                                                                                            //__SILP__
                                                                                                                     //__SILP__
        //SILP: CONTEXT_PROPERTIES_HELPER(Data, Data)
        public static DataProperty AddData(this IContext context, string path, Pass propertyPass, Data val) {  //__SILP__
            return context.Properties.AddData(path, propertyPass, val);                                        //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static DataProperty AddData(this IContext context, string path, Data val) {                     //__SILP__
            return context.Properties.AddData(path, val);                                                      //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static DataProperty RemoveData(this IContext context, string path, Pass pass) {                 //__SILP__
            return context.Properties.RemoveData(path, pass);                                                  //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static DataProperty RemoveData(this IContext context, string path) {                            //__SILP__
            return context.Properties.RemoveData(path);                                                        //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static bool IsData(this IContext context, string path) {                                        //__SILP__
            return context.Properties.IsData(path);                                                            //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static Data GetData(this IContext context, string path) {                                       //__SILP__
            return context.Properties.GetData(path);                                                           //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static Data GetData(this IContext context, string path, Data defaultValue) {                    //__SILP__
            return context.Properties.GetData(path, defaultValue);                                             //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static bool SetData(this IContext context, string path, Pass propertyPass, Data value) {        //__SILP__
            return context.Properties.SetData(path, propertyPass, value);                                      //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
        public static bool SetData(this IContext context, string path, Data value) {                           //__SILP__
            return context.Properties.SetData(path, value);                                                    //__SILP__
        }                                                                                                      //__SILP__
                                                                                                               //__SILP__
    }
}
