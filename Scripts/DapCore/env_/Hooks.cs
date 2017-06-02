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

        // Should only be called from IAspect.OnAdded();
        public void _OnAspectAdded(IAspect aspect) {
            string[] contextPathSegments = aspect.Context.Path.Split(PathConsts.SegmentSeparator);
            string[] aspectPathSegments = aspect.Path.Split(PathConsts.SegmentSeparator);
            ForEach((Hook hook) => {
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample("Hook._OnAspectAdded: " + hook.Description);
                #endif
                hook._OnAspectAdded(aspect, contextPathSegments, aspectPathSegments);
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif
            });
        }

        // Should only be called from IAspect.OnRemoved();
        public void _OnAspectRemoved(IAspect aspect) {
            string[] contextPathSegments = aspect.Context.Path.Split(PathConsts.SegmentSeparator);
            string[] aspectPathSegments = aspect.Path.Split(PathConsts.SegmentSeparator);
            ForEach((Hook hook) => {
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample("Hook._OnAspectRemoved: " + hook.Description);
                #endif
                hook._OnAspectRemoved(aspect, contextPathSegments, aspectPathSegments);
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif
            });
        }
    }
}
