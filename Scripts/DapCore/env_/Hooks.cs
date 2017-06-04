using System;
using System.Collections.Generic;

namespace angeldnd.dap {
    public sealed class Hooks : TableAspect<IContext, Hook> {
        private DebugHook _DebugHook = null;
        public DebugHook DebugHook {
            get { return _DebugHook; }
        }

        public Hooks(IContext owner, string key) : base(owner, key) {
        }

        public void Setup() {
            if (_DebugHook != null) {
                Error("Already Setup");
                return;
            }
            _DebugHook = Add<DebugHook>();
        }

        // Should only be called from IContext.OnAdded();
        public void _OnContextAdded(IContext context) {
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("Hooks._OnContextAdded");
            #endif
            string[] contextPathSegments = context.Path.Split(PathConsts.SegmentSeparator);
            ForEach((Hook hook) => {
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample(hook.Description);
                #endif
                hook._OnContextAdded(context, contextPathSegments);
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif
            });
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
        }

        // Should only be called from IContext.OnRemoved();
        public void _OnContextRemoved(IContext context) {
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.BeginSample("Hooks._OnContextRemoved");
            #endif
            string[] contextPathSegments = context.Path.Split(PathConsts.SegmentSeparator);
            ForEach((Hook hook) => {
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample(hook.Description);
                #endif
                hook._OnContextRemoved(context, contextPathSegments);
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif
            });
            #if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
            #endif
        }

        // Should only be called from IAspect.OnAdded();
        public void _OnAspectAdded(IAspect aspect) {
            //Only support DebugHook for performance.
            if (_DebugHook != null && _DebugHook.AspectMatchersCount > 0) {
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.BeginSample("DebugHook._OnAspectAdded");
                #endif
                string[] contextPathSegments = aspect.Context.Path.Split(PathConsts.SegmentSeparator);
                string[] aspectPathSegments = aspect.Path.Split(PathConsts.SegmentSeparator);
                _DebugHook._OnAspectAdded(aspect, contextPathSegments, aspectPathSegments);
                #if UNITY_EDITOR
                UnityEngine.Profiling.Profiler.EndSample();
                #endif
            }
        }
    }
}
