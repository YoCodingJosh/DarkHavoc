using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DarkHavoc.Engine.DataReaders
{
    public class IniFile
    {
        public enum FileMode : uint
        {
            Write = 0,
            Read = 1,
        };

        private StreamReader readStream;
        private StreamWriter writeStream;
        private Dictionary<string, string> properties;
        private Dictionary<string, string> sections;

        public IniFile(string file, FileMode mode)
        {
            properties = new Dictionary<string, string>();
            sections = new Dictionary<string, string>();

            switch (mode)
            {
                case FileMode.Read:
                    readStream = new StreamReader(file);
                    ReadFile();
                    break;
                case FileMode.Write:
                    writeStream = new StreamWriter(file);
                    break;
            }
        }

        private void ReadFile()
        {
            while (!readStream.EndOfStream)
            {
                ReadLine();
            }
        }

        private void ReadLine()
        {
            string line = readStream.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
            {
                // Ignore the line because it's blank.
                return;
            }
            else if (line[0] == ';' || line[0] == '#' || line.StartsWith("//"))
            {
                // Ignore the line because it's a comment.
                return;
            }
            else if (line[0] == '[' && line[line.Length - 1] == ']')
            {
                // [section]
                string section = line.Replace("[", "");
                section = section.Remove(line.Length - 2);
            }
            else
            {
                // propertes (key=value)
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '=')
                    {
                        StringBuilder key = new StringBuilder();

                        for (int j = 0; j < i; j++)
                        {
                            key.Append(line[j]);
                        }

                        string value = line.Remove(0, i + 1); // Remove the key and the equal sign.

                        properties.Add(key.ToString(), value);

                        return; // Exit so we don't throw an exception.
                    }
                }

                // Did we made it out of the loop with nothing to show? Throw an exception!
                throw new Exception("Syntax error!");
            }
        }

        public bool GetValueFromKey(string key, out string value)
        {
            return properties.TryGetValue(key, out value);
        }

        public string GetValueFromKey(string key)
        {
            string result = null;

            properties.TryGetValue(key, out result);

            return result;
        }

        public List<string> GetListOfKeysFromSection(string section)
        {
            throw new NotImplementedException();
        }
    }
}
