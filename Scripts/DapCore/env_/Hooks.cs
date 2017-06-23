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
            IProfiler profiler = Log.BeginSample("Hooks._OnContextAdded");
            string[] contextPathSegments = context.Path.Split(PathConsts.SegmentSeparator);
            ForEach((Hook hook) => {
                if (profiler != null) profiler.BeginSample(hook.Description);
                hook._OnContextAdded(context, contextPathSegments);
                if (profiler != null) profiler.EndSample();
            });
            if (profiler != null) profiler.EndSample();
        }

        // Should only be called from IContext.OnRemoved();
        public void _OnContextRemoved(IContext context) {
            IProfiler profiler = Log.BeginSample("Hooks._OnContextRemoved");
            string[] contextPathSegments = context.Path.Split(PathConsts.SegmentSeparator);
            ForEach((Hook hook) => {
                if (profiler != null) profiler.BeginSample(hook.Description);
                hook._OnContextRemoved(context, contextPathSegments);
                if (profiler != null) profiler.EndSample();
            });
            if (profiler != null) profiler.EndSample();
        }

        // Should only be called from IAspect.OnAdded();
        public void _OnAspectAdded(IAspect aspect) {
            //Only support DebugHook for performance.
            if (_DebugHook != null && _DebugHook.AspectMatchersCount > 0) {
                IProfiler profiler = Log.BeginSample("DebugHook._OnAspectAdded");
                string[] contextPathSegments = aspect.Context.Path.Split(PathConsts.SegmentSeparator);
                string[] aspectPathSegments = aspect.Path.Split(PathConsts.SegmentSeparator);
                _DebugHook._OnAspectAdded(aspect, contextPathSegments, aspectPathSegments);
                if (profiler != null) profiler.EndSample();
            }
        }
    }
}
