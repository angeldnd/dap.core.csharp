using System;

namespace angeldnd.dap {
    public struct Caret {
        public static Caret InvalidCaret = new Caret(string.Empty, -1, -1);

        public readonly string Source;
        public readonly int Line;
        public readonly int Column;

        public Caret(string source, int line, int column) {
            Source = source;
            Line = line;
            Column = column;
        }

        public override string ToString() {
            return string.Format("{0}<{1}: {2}>", Source, Line, Column);
        }
    }
}
