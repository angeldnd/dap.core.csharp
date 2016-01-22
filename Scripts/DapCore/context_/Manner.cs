using System;

namespace angeldnd.dap {
    public abstract class Manner : InTreeAspect<Manners> {
        public Manner(Manners owner, string path, Pass pass) : base(owner, path, pass) {
        }
    }
}
