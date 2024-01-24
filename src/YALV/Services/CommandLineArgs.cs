namespace YALV
{
    class CommandLineArgs : ICommandLineArgs
    {
        public string[] Args { get; }

        public CommandLineArgs(string[] args)
        {
            Args = args;
        }
    }
}