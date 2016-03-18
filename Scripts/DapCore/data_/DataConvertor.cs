using System;
using System.Text;
using System.Collections.Generic;

namespace angeldnd.dap {
    public class DataConvertor : Convertor<Data> {
        public string Convert(Data val, bool oneLine) {
            if (val == null) return "null";

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            AppendData(builder, oneLine, "", val);
            return builder.ToString();
        }

        public override string Convert(Data val) {
            return Convert(val, false);
        }

        public override Data Parse(string str) {
            return null;
        }

        private void AppendLine(StringBuilder builder, bool oneLine, string prefix, string line) {
            builder.Append(prefix);
            builder.Append(line);
            if (!oneLine) {
                builder.AppendLine();
            }
        }

        private void AppendData(StringBuilder builder, bool oneLine, string prefix, Data data) {
            AppendLine(builder, oneLine, prefix, "{");
            if (data != null) {
                string innerPrefix = prefix;
                if (!oneLine) {
                    innerPrefix = prefix + "\t";
                }
                int count = data.Count;
                int index = 0;
                foreach (string key in data.Keys) {
                    AppendLine(builder, oneLine, innerPrefix, string.Format("{0}:", key));
                    AppendValue(builder, oneLine, innerPrefix, data, key);
                    index++;
                    if (index < count) {
                        AppendLine(builder, oneLine, innerPrefix, ",");
                    }
                }
            }
            AppendLine(builder, oneLine, prefix, "}");
        }

        public void AppendValue(StringBuilder builder, bool oneLine, string prefix, Data data, string key) {
            DataType valueType = data.GetValueType(key);
            switch (valueType) {
                case DataType.Bool:
                    AppendLine(builder, oneLine, prefix, string.Format("{0}", data.GetBool(key)));
                    break;
                case DataType.Int:
                    AppendLine(builder, oneLine, prefix, string.Format("{0}i", data.GetInt(key)));
                    break;
                case DataType.Long:
                    AppendLine(builder, oneLine, prefix, string.Format("{0}l", data.GetLong(key)));
                    break;
                case DataType.Float:
                    AppendLine(builder, oneLine, prefix, string.Format("{0}f", data.GetFloat(key)));
                    break;
                case DataType.Double:
                    AppendLine(builder, oneLine, prefix, string.Format("{0}d", data.GetDouble(key)));
                    break;
                case DataType.String:
                    AppendLine(builder, oneLine, prefix, string.Format("\"{0}\"", QuoteString(data.GetString(key))));
                    break;
                case DataType.Data:
                    AppendData(builder, oneLine, prefix, data.GetData(key));
                    break;
            }
        }

        public string QuoteString(string str) {
            //TODO:
            return str;
        }
    }
}

