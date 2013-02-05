using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LuaInterface;

using System.Threading;

using SdlDotNet.Core;

namespace Engine.Components
{
    public class Script
    {
        public Lua mLua = new Lua();

        List<string> mCommandList;

        Thread luaThread;

        public Script()
        {
            Console.WriteLine("### INITIALIZING LUA ###");

            mCommandList = new List<string>();
        }

        public void InitScript()
        {
            Thread thr = new Thread(Update);
            thr.Start();
        }

        public void Update()
        {
            while (Engine.mIsAlive)
            {
                string a = Console.ReadLine();

                try { mLua.DoString(a); }
                catch (Exception ex) { Console.WriteLine("LUAERR: {0}, {1}", ex.Message, ex.InnerException); }
                finally { Console.WriteLine(); }
            }
        }

        public void PrintFunctions()
        {
            foreach (string s in mCommandList)
            {
                Console.WriteLine(s);
            }
        }

        public void RegisterFunction(string path, object obj, System.Reflection.MethodBase method)
        {
            mCommandList.Add(path);

            mLua.RegisterFunction(path, obj, method);
        }

        public void RunString(string command)
        {
            mLua.DoString(command);
        }

        public void RunFile(string path)
        {
            mLua.DoFile(path);
        }
    }
}
