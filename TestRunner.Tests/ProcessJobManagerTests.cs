using Libraries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestRunner.Tests
{
    [TestClass]
    public class ProcessJobManagerTests
    {
        [TestMethod]
        public void OutPut_Command_Dir()
        {
            var output = new List<string>();
            void Do(object o)
            {
                output.Add(o.ToString());
            }            

            var command = new Command { Id = "1", Cmd = "dir" };
            var manager = new ProcessJobManager("cmd", null, null, null, Do);
            manager.Run(command);

            Assert.AreNotEqual(output.Count, 0);
        }

        

        [TestMethod]
        public void Before_Action_Is_Called()
        {
            var result = false;

            var command = new Command {Id = "1", Cmd = ""};
            var manager = new ProcessJobManager("cmd", (Command cmd) =>  result = true );
            manager.Run(command);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void After_Action_Is_Called()
        {
            var result = false;

            var command = new Command { Id = "1", Cmd = "" };
            var manager = new ProcessJobManager("cmd", null, (Command cmd) => result = true);
            manager.Run(command);

            Assert.IsTrue(result);
        }
    }
}
