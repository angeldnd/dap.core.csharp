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
                },
                (IAspect aspect) => {
                    aspect.Debugging = true;
                }
            );
        }

        // Should only be called from IContext.OnAdded();
        public void _OnContextAdded(IContext context) {
            ForEach((Hook hook) => {
                #if UNITY_5
                UnityEngine.Profiling.Profiler.BeginSample("Hook._OnContextAdded: " + hook.Description);
                #endif
                hook._OnContextAdded(context);
                #if UNITY_5
                UnityEngine.Profiling.Profiler.EndSample();
                #endif
            });
        }

        // Should only be called from IContext.OnRemoved();
        public void _OnContextRemoved(IContext context) {
            ForEach((Hook hook) => {
                #if UNITY_5
                UnityEngine.Profiling.Profiler.BeginSample("Hook._OnContextRemoved: " + hook.Description);
                #endif
                hook._OnContextRemoved(context);
                #if UNITY_5
                UnityEngine.Profiling.Profiler.EndSample();
                #endif
            });
        }

        // Should only be called from IAspect.OnAdded();
        public void _OnAspectAdded(IAspect aspect) {
            ForEach((Hook hook) => {
                #if UNITY_5
                UnityEngine.Profiling.Profiler.BeginSample("Hook._OnAspectAdded: " + hook.Description);
                #endif
                hook._OnAspectAdded(aspect);
                #if UNITY_5
                UnityEngine.Profiling.Profiler.EndSample();
                #endif
            });
        }

        // Should only be called from IAspect.OnRemoved();
        public void _OnAspectRemoved(IAspect aspect) {
            ForEach((Hook hook) => {
                #if UNITY_5
                UnityEngine.Profiling.Profiler.BeginSample("Hook._OnAspectRemoved: " + hook.Description);
                #endif
                hook._OnAspectRemoved(aspect);
                #if UNITY_5
                UnityEngine.Profiling.Profiler.EndSample();
                #endif
            });
        }
    }
}
