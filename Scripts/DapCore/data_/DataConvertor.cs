using System;
using System.Text;
using System.Collections.Generic;

namespace angeldnd.dap {
    public static class DataConvertorConsts {
        public const string NullStr = "null";

        public const string DataBegin = "{";
        public const string DataEnd = "}";
        public const string KeyBegin = ":";
        public const string ValueBegin = "=";

        public const string Space = " ";

        public readonly static char[] WordChars = new char[]{':', '=', '{', '}'};

        public static bool IsWordChar(char ch) {
            foreach (char word in WordChars) {
                if (word == ch) return true;
            }
            return false;
        }
    }

    public class DataConvertor : Convertor<Data> {
        public string Convert(Data val, string indent) {
            if (val == null) return DataConvertorConsts.NullStr;
            if (indent == "") indent = null;

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            AppendData(builder, indent, 0, val);
            return builder.ToString();
        }

        public override string Convert(Data val) {
            return Convert(val, "\t");
        }

        public override Data Parse(string str) {
            return null;
        }

        private void AppendIndents(StringBuilder builder, string indent, int indentLevel) {
            if (indentLevel <= 0 || indent == null) return;

            for (int i = 0; i < indentLevel; i++) {
                builder.Append(indent);
            }
        }

        private void AppendValueType(StringBuilder builder, DataType valueType) {
            switch (valueType) {
                case DataType.Bool:
                    builder.Append("B");
                    break;
                case DataType.Int:
                    builder.Append("I");
                    break;
                case DataType.Long:
                    builder.Append("L");
                    break;
                case DataType.Float:
                    builder.Append("F");
                    break;
                case DataType.Double:
                    builder.Append("D");
                    break;
                case DataType.String:
                    builder.Append("S");
                    break;
                case DataType.Data:
                    builder.Append("A");
                    break;
            }
        }

        private void AppendKeyBegin(StringBuilder builder, string indent) {
            builder.Append(DataConvertorConsts.KeyBegin);
            if (indent != null) {
                builder.Append(DataConvertorConsts.Space);
            }
        }

        private void AppendValueBegin(StringBuilder builder, string indent) {
            if (indent != null) {
                builder.Append(DataConvertorConsts.Space);
            }
            builder.Append(DataConvertorConsts.ValueBegin);
            if (indent != null) {
                builder.Append(DataConvertorConsts.Space);
            }
        }

        private void AppendData(StringBuilder builder, string indent, int indentLevel, Data data) {
            if (data == null) {
                builder.Append(DataConvertorConsts.NullStr);
                return;
            }

            builder.Append(DataConvertorConsts.DataBegin);
            if (indent != null && data.Count > 0) {
                builder.AppendLine();
            }

            foreach (string key in data.Keys) {
                AppendValue(builder, indent, indentLevel + 1, data, key);
            }

            AppendIndents(builder, indent, indentLevel);
            builder.Append(DataConvertorConsts.DataEnd);
        }

        public void AppendValue(StringBuilder builder, string indent, int indentLevel, Data data, string key) {
            AppendIndents(builder, indent, indentLevel);

            DataType valueType = data.GetValueType(key);
            AppendValueType(builder, valueType);

            AppendKeyBegin(builder, indent);
            AppendString(builder, key);

            AppendValueBegin(builder, indent);

            switch (valueType) {
                case DataType.Bool:
                    builder.Append(Convertor.BoolConvertor.Convert(data.GetBool(key)));
                    break;
                case DataType.Int:
                    builder.Append(Convertor.IntConvertor.Convert(data.GetInt(key)));
                    break;
                case DataType.Long:
                    builder.Append(Convertor.LongConvertor.Convert(data.GetLong(key)));
                    break;
                case DataType.Float:
                    builder.Append(Convertor.FloatConvertor.Convert(data.GetFloat(key)));
                    break;
                case DataType.Double:
                    builder.Append(Convertor.DoubleConvertor.Convert(data.GetDouble(key)));
                    break;
                case DataType.String:
                    AppendString(builder, data.GetString(key));
                    break;
                case DataType.Data:
                    AppendData(builder, indent, indentLevel, data.GetData(key));
                    break;
            }
            if (indent != null) {
                builder.AppendLine();
            } else {
                builder.Append(DataConvertorConsts.Space);
            }
        }

        public void AppendString(StringBuilder builder, string str) {
            if (string.IsNullOrEmpty(str)) return;

            for (int i = 0; i < str.Length; i++) {
                char ch = str[i];
                if (WordSplitterConsts.IsEmptyChar(ch) || DataConvertorConsts.IsWordChar(ch)) {
                    builder.Append(WordSplitterConsts.EscapeChar);
                }
                builder.Append(ch);
            }
        }
    }
}

