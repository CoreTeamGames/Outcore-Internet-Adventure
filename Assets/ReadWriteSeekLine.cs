using System.Collections.Generic;
using System.IO;

namespace FileWorker
{
    public class ReadWriteSeekLine
    {
        public void ReadAllLines(string _path, List<string> lineList, int linesCount)
        {
            
            using (FileStream stream = new FileStream(_path, FileMode.OpenOrCreate))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    for (int i = 0; i < linesCount; i++)
                    {
                        lineList.Add(reader.ReadLine());
                    }
                }
            }
        }

        public string SeekAndReadLine(string _path, int _lineString)
        {
            string _line = "";
            using (FileStream stream = new FileStream(_path, FileMode.OpenOrCreate))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    for (int i = 0; i < _lineString; i++)
                    {
                        _line = reader.ReadLine();
                    }
                }
            }

            return _line;
        }
        #region Writer
        public void WriteLine(string line, string _path, string _filename)
        {
            using (FileStream stream = new FileStream(_path + "/" + _filename, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(line);
                }
            }
        }
        public void WriteLine(string[] lines, string _path, string _filename)
        {
            using (FileStream stream = new FileStream(_path + "/" + _filename, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        writer.WriteLine(lines[i]);
                    }
                }
            }
        }
        public void WriteLine(List<string> lineList, string _path, string _filename)
        {
            using (FileStream stream = new FileStream(_path + "/" + _filename, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    for (int i = 0; i < lineList.Count; i++)
                    {
                        writer.WriteLine(lineList[i]);
                    }
                }
            }
        }

        public void Write(string line, string _path, string _filename)
        {
            using (FileStream stream = new FileStream(_path + "/" + _filename, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(line);
                }
            }
        }
        #endregion

        public bool SeekAndReadLineToBool(string _path, int _lineString)
        {
            bool _bool = false;
            string _line = "";
            using (FileStream stream = new FileStream(_path, FileMode.OpenOrCreate))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    for (int i = 0; i < _lineString; i++)
                    {
                        _line = reader.ReadLine();
                    }
                }
            }
            if (_line.ToLower() == "true".ToLower() || _line == "1")
                _bool = true;
            else if (_line.ToLower() == "false".ToLower() || _line == "0")
                _bool = false;
            return _bool;
        }
    }
}