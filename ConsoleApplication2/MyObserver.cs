using System;

namespace ConsoleApplication2
{
    public class MyObserver : IObserver<string>
    {
        public void OnNext(string value)
        {
            Console.WriteLine(value);
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnCompleted()
        {
            Console.WriteLine("OnCompleted");
        }
    }
}