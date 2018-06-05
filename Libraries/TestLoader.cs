using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries
{
    public static class TestLoader
    {
        public static Queue<Command> FromCSV(string fileName)
        {
            var result = new Queue<Command>();
            using (TextFieldParser parser = new TextFieldParser(fileName))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    result.Enqueue(new Command
                    {
                        Id = fields[0],
                        Cmd = fields[1]
                    });
                }
            }
            result.Dequeue();
            return result;
        }
    }
}
