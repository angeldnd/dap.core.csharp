using System;
using System.Text;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class DataJsonConvertorConsts {
        public const string StringEncloser = "\"";
        public const string Comma = ",";
        public const string ValueBegin = ":";

        public const char StringEncloserChar = '"';
        public const char CommaChar = ',';

        public static bool ShouldEscape(char ch) {
            if (ch == StringEncloserChar) {
                return true;
            } else if (ch == CommaChar) {
                return true;
            }
            return false;
        }
    }

    public class DataJsonConvertor : DataConvertor {
        public Data Parse(string source, string content) {
            throw new DapException("Not Implemented");
        }

        protected override void AppendDataValues(StringBuilder builder, string indent, int indentLevel, Data data) {
            int i = 0;
            foreach (string key in data.Keys) {
                i++;
                AppendValue(builder, indent, indentLevel + 1, data, key,
                            i == data.Count ? "" : DataJsonConvertorConsts.Comma);
            }
        }

        protected override void AppendTypeAndKey(StringBuilder builder, DataType valueType, string key, bool withSpace) {
            builder.Append(DataJsonConvertorConsts.StringEncloser);
            builder.Append(Convertor.DataTypeConvertor.Convert(valueType));
            builder.Append(DataConvertorConsts.KeyBegin);
            builder.Append(key);
            builder.Append(DataJsonConvertorConsts.StringEncloser);
            builder.Append(DataJsonConvertorConsts.ValueBegin);
            if (withSpace != null) {
                builder.Append(DataConvertorConsts.Space);
            }
        }

        protected override void AppendString(StringBuilder builder, string str) {
            builder.Append(DataJsonConvertorConsts.StringEncloser);
            if (!string.IsNullOrEmpty(str)) {
                for (int i = 0; i < str.Length; i++) {
                    char ch = str[i];
                    if (ch == '\n') {
                        builder.Append("\\n");
                    } else if (ch == '\r') {
                        builder.Append("\\r");
                    } else if (ch == '\t') {
                        builder.Append("\\t");
                    } else {
                        if (DataJsonConvertorConsts.ShouldEscape(ch)) {
                            builder.Append(WordSplitterConsts.EscapeChar);
                        }
                        builder.Append(ch);
                    }
                }
            };
            builder.Append(DataJsonConvertorConsts.StringEncloser);
        }
    }
}

