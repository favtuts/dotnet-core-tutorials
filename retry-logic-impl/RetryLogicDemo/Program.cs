using System;
class Program
{
    static void Main(string[] args)
    {
        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Hello, World!");

        //var result = ObjectApiTester.AddNewObject().Result;
        //ObjectApiTester.GetSingleOject();
        //ObjectApiTester.ListAllOjects();
        //ObjectApiTester.ListOjectsByIDS();
        //ObjectApiTester.UpdatePartialObject(result.Id);
        //ObjectApiTester.UpdateObject(result.Id);
        //ObjectApiTester.DeleteObject(result.Id);
        ObjectApiTester.DeleteObject("7");

        Console.ReadLine();
    }
}