using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public struct ContextConsts {
        public const string TypeContext = "Context";

        public const string AspectVars = "_vars";
        public const string AspectProperties = "_properties";
        public const string AspectChannels = "_channels";
        public const string AspectHandlers = "_handlers";

        public const string VarsPropertyPasses = "_property_passes";
        public const string VarsChannelPasses = "_channel_passes";
        public const string VarsHandlerPasses = "_handler_passes";

        public const string SuffixHandlerAsync = "~";
        public const string SuffixChannelResponse = ">";

        public static string GetAsyncHandlerPath(string handlerPath) {
            return handlerPath + SuffixHandlerAsync;
        }

        public static string GetResponseChannelPath(string handlerPath) {
            return handlerPath + SuffixChannelResponse;
        }
    }

    public class Context : Entity {
        public override string Type {
            get { return ContextConsts.TypeContext; }
        }

        private Pass _Pass = new Pass();
        protected Pass Pass {
            get { return _Pass; }
        }

        public readonly Properties Properties;
        public readonly Channels Channels;
        public readonly Handlers Handlers;
        public readonly Vars Vars;

        public Context() {
            Properties = Add<Properties>(ContextConsts.AspectProperties, _Pass);
            Channels = Add<Channels>(ContextConsts.AspectChannels, _Pass);
            Handlers = Add<Handlers>(ContextConsts.AspectHandlers, _Pass);
            Vars = Add<Vars>(ContextConsts.AspectVars, _Pass);
        }

        public void OtherAspects<T>(OnAspect<T> callback) where T : class, Aspect {
            Filter<T>(PatternMatcherConsts.WildcastSegments, (T aspect) => {
                if (aspect != Vars && aspect != Properties && aspect != Channels && aspect != Handlers) {
                    callback(aspect);
                }
            });
        }

        public List<T> OtherAspects<T>() where T : class, Aspect {
            List<T> result = null;
            OtherAspects<T>((T aspect) => {
                if (result == null) result = new List<T>();
                result.Add(aspect);
            });
            return result;
        }

        public bool FireEvent(string channelPath, Pass pass, Data evt) {
            return Channels.FireEvent(channelPath, pass, evt);
        }

        public bool FireEvent(string channelPath, Data evt) {
            return Channels.FireEvent(channelPath, evt);
        }

        public bool FireEvent(string channelPath) {
            return Channels.FireEvent(channelPath);
        }

        public Channel GetChannel(string channelPath) {
            return Channels.GetChannel(channelPath);
        }

        public Channel AddChannel(string channelPath) {
            return Channels.AddChannel(channelPath);
        }

        public Channel AddChannel(string channelPath, Pass pass) {
            return Channels.AddChannel(channelPath, pass);
        }

        public Data HandleRequest(string handlerPath, Pass pass, Data req) {
            return Handlers.HandleRequest(handlerPath, pass, req);
        }

        public Data HandleRequest(string handlerPath, Data req) {
            return Handlers.HandleRequest(handlerPath, req);
        }

        public Data HandleRequest(string handlerPath) {
            return Handlers.HandleRequest(handlerPath);
        }

        public Handler GetHandler(string handlerPath) {
            return Handlers.GetHandler(handlerPath);
        }

        public Handler AddHandler(string handlerPath) {
            return Handlers.AddHandler(handlerPath);
        }

        public Handler AddHandler(string handlerPath, Pass pass) {
            return Handlers.AddHandler(handlerPath, pass);
        }

        public bool HasVar(string varPath) {
            return Vars.Has(varPath);
        }

        public bool SetVarValue<T>(string varPath, T v) {
            return SetVarValue<T>(varPath, null, v);
        }

        public bool SetVarValue<T>(string varPath, Pass pass, T v) {
            if (Vars.HasVar<T>(varPath)) {
                return Vars.SetValue<T>(varPath, pass, v);
            } else {
                return Vars.AddVar<T>(varPath, pass, v) != null;
            }
        }

        public T GetVarValue<T>(string varPath, T defaultValue) {
            return Vars.GetValue<T>(varPath, defaultValue);
        }

        public T GetVarValue<T>(string varPath) {
            return GetVarValue<T>(varPath, default(T));
        }

        public T TakeVarValue<T>(string varPath, Pass pass, T defaultValue) {
            if (Vars.HasVar<T>(varPath)) {
                T result = Vars.GetValue<T>(varPath, defaultValue);
                Vars.RemoveVar<T>(varPath, pass);
                return result;
            } else {
                return defaultValue;
            }
        }

        public T TakeVarValue<T>(string varPath, T defaultValue) {
            return TakeVarValue<T>(varPath, null, defaultValue);
        }

        public T TakeVarValue<T>(string varPath) {
            return TakeVarValue<T>(varPath, null, default(T));
        }

        public bool HasVarsVar(string varsPath, string varPath) {
            Vars vars = Get<Vars>(varsPath);
            if (vars == null) {
                return vars.Has(varPath);
            }
            return false;
        }

        public bool SetVarsVarValue<T>(string varsPath, string varPath, T v) {
            return SetVarsVarValue<T>(varsPath, varPath, null, v);
        }

        public bool SetVarsVarValue<T>(string varsPath, string varPath, Pass pass, T v) {
            Vars vars = Get<Vars>(varsPath);
            if (vars == null) {
                vars = Add<Vars>(varsPath, _Pass);
            }
            if (vars != null) {
                if (vars.HasVar<T>(varPath)) {
                    return vars.SetValue<T>(varPath, pass, v);
                } else {
                    return vars.AddVar<T>(varPath, pass, v) != null;
                }
            }
            return false;
        }

        public T GetVarsVarValue<T>(string varsPath, string varPath, T defaultValue) {
            Vars vars = Get<Vars>(varsPath);
            if (vars != null) {
                return vars.GetValue<T>(varPath, defaultValue);
            }
            return defaultValue;
        }

        public T GetVarsVarValue<T>(string varsPath, string varPath) {
            return GetVarsVarValue<T>(varsPath, varPath, default(T));
        }

        public T TakeVarsVarValue<T>(string varsPath, string varPath, Pass pass, T defaultValue) {
            Vars vars = Get<Vars>(varsPath);
            if (vars != null) {
                if (vars.HasVar<T>(varPath)) {
                    T result = vars.GetValue<T>(varPath, defaultValue);
                    vars.RemoveVar<T>(varPath, pass);
                    if (vars.Count == 0) {
                        Remove<Vars>(varsPath, _Pass);
                    }
                    return result;
                } else {
                    Error("Var Not Exist: {0}, {1}", varsPath, varPath);
                }
            } else {
                Error("Vars Not Exist: {0}, {1}", varsPath, varPath);
            }
            return defaultValue;
        }

        public T TakeVarsVarValue<T>(string varsPath, string varPath, T defaultValue) {
            return TakeVarsVarValue<T>(varsPath, varPath, null, defaultValue);
        }

        public T TakeVarsVarValue<T>(string varsPath, string varPath) {
            return TakeVarsVarValue<T>(varsPath, varPath, null, default(T));
        }

        //SILP:CONTEXT_DEPOSIT_WITHDRAW(PropertyPass, Pass, ContextConsts.VarsPropertyPasses, passValue)
        public Pass DepositPropertyPass(string key, Pass passValue) {                        //__SILP__
            if (!SetVarsVarValue<Pass>(ContextConsts.VarsPropertyPasses, key, passValue)) {  //__SILP__
                Error("DepositPropertyPass Failed {0}", key);                                //__SILP__
            }                                                                                //__SILP__
            return passValue;                                                                //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        public Pass WithdrawPropertyPass(string key) {                                       //__SILP__
            return TakeVarsVarValue<Pass>(ContextConsts.VarsPropertyPasses, key);            //__SILP__
        }                                                                                    //__SILP__
                                                                                             //__SILP__
        //SILP:CONTEXT_DEPOSIT_WITHDRAW(ChannelPass, Pass, ContextConsts.VarsChannelPasses, passValue)
        public Pass DepositChannelPass(string key, Pass passValue) {                        //__SILP__
            if (!SetVarsVarValue<Pass>(ContextConsts.VarsChannelPasses, key, passValue)) {  //__SILP__
                Error("DepositChannelPass Failed {0}", key);                                //__SILP__
            }                                                                               //__SILP__
            return passValue;                                                               //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        public Pass WithdrawChannelPass(string key) {                                       //__SILP__
            return TakeVarsVarValue<Pass>(ContextConsts.VarsChannelPasses, key);            //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        //SILP:CONTEXT_DEPOSIT_WITHDRAW(HandlerPass, Pass, ContextConsts.VarsHandlerPasses, passValue)
        public Pass DepositHandlerPass(string key, Pass passValue) {                        //__SILP__
            if (!SetVarsVarValue<Pass>(ContextConsts.VarsHandlerPasses, key, passValue)) {  //__SILP__
                Error("DepositHandlerPass Failed {0}", key);                                //__SILP__
            }                                                                               //__SILP__
            return passValue;                                                               //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__
        public Pass WithdrawHandlerPass(string key) {                                       //__SILP__
            return TakeVarsVarValue<Pass>(ContextConsts.VarsHandlerPasses, key);            //__SILP__
        }                                                                                   //__SILP__
                                                                                            //__SILP__

        //SILP: CONTEXT_PROPERTIES_HELPER(Bool, bool)
        public BoolProperty AddBool(string path, Pass pass, bool val) {  //__SILP__
            return Properties.AddBool(path, pass, val);                  //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public BoolProperty AddBool(string path, bool val) {             //__SILP__
            return Properties.AddBool(path, val);                        //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public BoolProperty RemoveBool(string path, Pass pass) {         //__SILP__
            return Properties.RemoveBool(path, pass);                    //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public BoolProperty RemoveBool(string path) {                    //__SILP__
            return Properties.RemoveBool(path);                          //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool IsBool(string path) {                                //__SILP__
            return Properties.IsBool(path);                              //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool GetBool(string path) {                               //__SILP__
            return Properties.GetBool(path);                             //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool GetBool(string path, bool defaultValue) {            //__SILP__
            return Properties.GetBool(path, defaultValue);               //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool SetBool(string path, Pass pass, bool value) {        //__SILP__
            return Properties.SetBool(path, pass, value);                //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool SetBool(string path, bool value) {                   //__SILP__
            return Properties.SetBool(path, value);                      //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__

        //SILP: CONTEXT_PROPERTIES_HELPER(Int, int)
        public IntProperty AddInt(string path, Pass pass, int val) {  //__SILP__
            return Properties.AddInt(path, pass, val);                //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public IntProperty AddInt(string path, int val) {             //__SILP__
            return Properties.AddInt(path, val);                      //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public IntProperty RemoveInt(string path, Pass pass) {        //__SILP__
            return Properties.RemoveInt(path, pass);                  //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public IntProperty RemoveInt(string path) {                   //__SILP__
            return Properties.RemoveInt(path);                        //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool IsInt(string path) {                              //__SILP__
            return Properties.IsInt(path);                            //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public int GetInt(string path) {                              //__SILP__
            return Properties.GetInt(path);                           //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public int GetInt(string path, int defaultValue) {            //__SILP__
            return Properties.GetInt(path, defaultValue);             //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetInt(string path, Pass pass, int value) {       //__SILP__
            return Properties.SetInt(path, pass, value);              //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        public bool SetInt(string path, int value) {                  //__SILP__
            return Properties.SetInt(path, value);                    //__SILP__
        }                                                             //__SILP__
                                                                      //__SILP__
        //SILP: CONTEXT_PROPERTIES_HELPER(Long, long)
        public LongProperty AddLong(string path, Pass pass, long val) {  //__SILP__
            return Properties.AddLong(path, pass, val);                  //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public LongProperty AddLong(string path, long val) {             //__SILP__
            return Properties.AddLong(path, val);                        //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public LongProperty RemoveLong(string path, Pass pass) {         //__SILP__
            return Properties.RemoveLong(path, pass);                    //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public LongProperty RemoveLong(string path) {                    //__SILP__
            return Properties.RemoveLong(path);                          //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool IsLong(string path) {                                //__SILP__
            return Properties.IsLong(path);                              //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public long GetLong(string path) {                               //__SILP__
            return Properties.GetLong(path);                             //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public long GetLong(string path, long defaultValue) {            //__SILP__
            return Properties.GetLong(path, defaultValue);               //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool SetLong(string path, Pass pass, long value) {        //__SILP__
            return Properties.SetLong(path, pass, value);                //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool SetLong(string path, long value) {                   //__SILP__
            return Properties.SetLong(path, value);                      //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        //SILP: CONTEXT_PROPERTIES_HELPER(Float, float)
        public FloatProperty AddFloat(string path, Pass pass, float val) {  //__SILP__
            return Properties.AddFloat(path, pass, val);                    //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public FloatProperty AddFloat(string path, float val) {             //__SILP__
            return Properties.AddFloat(path, val);                          //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public FloatProperty RemoveFloat(string path, Pass pass) {          //__SILP__
            return Properties.RemoveFloat(path, pass);                      //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public FloatProperty RemoveFloat(string path) {                     //__SILP__
            return Properties.RemoveFloat(path);                            //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public bool IsFloat(string path) {                                  //__SILP__
            return Properties.IsFloat(path);                                //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public float GetFloat(string path) {                                //__SILP__
            return Properties.GetFloat(path);                               //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public float GetFloat(string path, float defaultValue) {            //__SILP__
            return Properties.GetFloat(path, defaultValue);                 //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public bool SetFloat(string path, Pass pass, float value) {         //__SILP__
            return Properties.SetFloat(path, pass, value);                  //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        public bool SetFloat(string path, float value) {                    //__SILP__
            return Properties.SetFloat(path, value);                        //__SILP__
        }                                                                   //__SILP__
                                                                            //__SILP__
        //SILP: CONTEXT_PROPERTIES_HELPER(Double, double)
        public DoubleProperty AddDouble(string path, Pass pass, double val) {  //__SILP__
            return Properties.AddDouble(path, pass, val);                      //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public DoubleProperty AddDouble(string path, double val) {             //__SILP__
            return Properties.AddDouble(path, val);                            //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public DoubleProperty RemoveDouble(string path, Pass pass) {           //__SILP__
            return Properties.RemoveDouble(path, pass);                        //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public DoubleProperty RemoveDouble(string path) {                      //__SILP__
            return Properties.RemoveDouble(path);                              //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool IsDouble(string path) {                                    //__SILP__
            return Properties.IsDouble(path);                                  //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public double GetDouble(string path) {                                 //__SILP__
            return Properties.GetDouble(path);                                 //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public double GetDouble(string path, double defaultValue) {            //__SILP__
            return Properties.GetDouble(path, defaultValue);                   //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool SetDouble(string path, Pass pass, double value) {          //__SILP__
            return Properties.SetDouble(path, pass, value);                    //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool SetDouble(string path, double value) {                     //__SILP__
            return Properties.SetDouble(path, value);                          //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        //SILP: CONTEXT_PROPERTIES_HELPER(String, string)
        public StringProperty AddString(string path, Pass pass, string val) {  //__SILP__
            return Properties.AddString(path, pass, val);                      //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public StringProperty AddString(string path, string val) {             //__SILP__
            return Properties.AddString(path, val);                            //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public StringProperty RemoveString(string path, Pass pass) {           //__SILP__
            return Properties.RemoveString(path, pass);                        //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public StringProperty RemoveString(string path) {                      //__SILP__
            return Properties.RemoveString(path);                              //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool IsString(string path) {                                    //__SILP__
            return Properties.IsString(path);                                  //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public string GetString(string path) {                                 //__SILP__
            return Properties.GetString(path);                                 //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public string GetString(string path, string defaultValue) {            //__SILP__
            return Properties.GetString(path, defaultValue);                   //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool SetString(string path, Pass pass, string value) {          //__SILP__
            return Properties.SetString(path, pass, value);                    //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        public bool SetString(string path, string value) {                     //__SILP__
            return Properties.SetString(path, value);                          //__SILP__
        }                                                                      //__SILP__
                                                                               //__SILP__
        //SILP: CONTEXT_PROPERTIES_HELPER(Data, Data)
        public DataProperty AddData(string path, Pass pass, Data val) {  //__SILP__
            return Properties.AddData(path, pass, val);                  //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public DataProperty AddData(string path, Data val) {             //__SILP__
            return Properties.AddData(path, val);                        //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public DataProperty RemoveData(string path, Pass pass) {         //__SILP__
            return Properties.RemoveData(path, pass);                    //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public DataProperty RemoveData(string path) {                    //__SILP__
            return Properties.RemoveData(path);                          //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool IsData(string path) {                                //__SILP__
            return Properties.IsData(path);                              //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public Data GetData(string path) {                               //__SILP__
            return Properties.GetData(path);                             //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public Data GetData(string path, Data defaultValue) {            //__SILP__
            return Properties.GetData(path, defaultValue);               //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool SetData(string path, Pass pass, Data value) {        //__SILP__
            return Properties.SetData(path, pass, value);                //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
        public bool SetData(string path, Data value) {                   //__SILP__
            return Properties.SetData(path, value);                      //__SILP__
        }                                                                //__SILP__
                                                                         //__SILP__
    }
}
