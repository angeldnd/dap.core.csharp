using System;

namespace angeldnd.dap {
    public static class PassExtension {
        /*
         * This is created so for null Pass instance, can
         * still call pass.ToOpen();
         */
        public static Pass ToOpen(this Pass pass) {
            if (pass == null) return null;
            return pass.Open;
        }
    }
}
