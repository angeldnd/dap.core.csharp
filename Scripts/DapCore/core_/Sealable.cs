using System;

namespace angeldnd.dap {
    public interface Sealable {
        bool Sealed { get; }
        void Seal();
    }
}
