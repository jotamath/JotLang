using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;


namespace HelloWorld {


    public class Program { {

        public static void Main() {
            var names = new List<string> { "Alice", "Bob", "Charlie" }

            foreach (var name in names {
            Console.WriteLine($"Olá, {name}!");
        }

        var sum = Soma(10, 20);
        Console.WriteLine($"Soma: {sum}");
    }


    public static void Soma(a: int, b: int) => { return int = a + b; }
    }
