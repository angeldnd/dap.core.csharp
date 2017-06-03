using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Hooks : TableAspect<IContext, Hook> {
        private Hook _DebugHook = null;
        public Hook DebugHook {
            get { return _DebugHook; }
        }

        public Hooks(IContext owner, string key) : base(owner, key) {
        }

        public void Setup() {
            if (_DebugHook != null) {
                Error("Already Setup");
                return;
            }
            _DebugHook = Add();
            _DebugHook.Setup(
                "DebugHook",
                (IContext context) => {
                    context.Debugging = true;
                }
            );
        }

        // Should only be called from IContext.OnAdded();
        public void _OnContextAdded(IContext context) {
            string[] contextPathSegments = context.Path.Split(PathConsts.SegmentSeparator);
            ForEach((Hook hook) => {
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample("Hook._OnContextAdded: " + hook.Description);
                #endif
                hook._OnContextAdded(context, contextPathSegments);
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif
            });
        }

        // Should only be called from IContext.OnRemoved();
        public void _OnContextRemoved(IContext context) {
            string[] contextPathSegments = context.Path.Split(PathConsts.SegmentSeparator);
            ForEach((Hook hook) => {
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample("Hook._OnContextRemoved: " + hook.Description);
                #endif
                hook._OnContextRemoved(context, contextPathSegments);
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif
            });
        }
    }
}
