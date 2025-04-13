using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Jot {
    public static class Transpiler {
        public static string Transpile(string jtCode) {
            var result = new StringBuilder();
            
            // Adicionar usings padrão
            result.AppendLine("using System;");
            result.AppendLine("using System.Collections.Generic;");
            result.AppendLine("using System.Linq;");
            result.AppendLine("using Microsoft.AspNetCore.Mvc;");
            result.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            result.AppendLine("using Microsoft.Extensions.Logging;");
            result.AppendLine();
            
            var lines = jtCode.Split('\n');
            int indentLevel = 0;
            
            foreach (var line in lines) {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine)) {
                    result.AppendLine();
                    continue;
                }
                
                // Processar imports
                if (trimmedLine.StartsWith("import")) {
                    result.AppendLine(trimmedLine.Replace("import", "using") + ";");
                    continue;
                }
                
                // Processar module (namespace)
                if (trimmedLine.StartsWith("module")) {
                    result.AppendLine();
                    result.AppendLine(trimmedLine.Replace("module", "namespace") + " {");
                    indentLevel++;
                    continue;
                }
                
                // Processar class
                if (trimmedLine.StartsWith("class")) {
                    result.AppendLine();
                    result.AppendLine(GetIndent(indentLevel) + "public " + trimmedLine + " {");
                    indentLevel++;
                    continue;
                }
                
                // Processar fn (função)
                if (trimmedLine.StartsWith("fn")) {
                    result.AppendLine();
                    var methodLine = trimmedLine.Replace("fn", "public static void");
                    if (methodLine.Contains("=>")) {
                        methodLine = methodLine.Replace("=>", "=> { return");
                        methodLine += "; }";
                    } else if (!methodLine.EndsWith("{")) {
                        methodLine += " {";
                    }
                    result.AppendLine(GetIndent(indentLevel) + methodLine);
                    indentLevel++;
                    continue;
                }
                
                // Processar prop (propriedade)
                if (trimmedLine.StartsWith("prop")) {
                    var propertyLine = trimmedLine.Replace("prop", "public");
                    if (!propertyLine.EndsWith(";")) {
                        propertyLine += " { get; set; }";
                    }
                    result.AppendLine(GetIndent(indentLevel) + propertyLine);
                    continue;
                }
                
                // Processar print
                if (trimmedLine.Contains("print(")) {
                    var printLine = trimmedLine.Replace("print(", "Console.WriteLine(");
                    // Adicionar $ para strings com interpolação
                    if (printLine.Contains("{") && printLine.Contains("}")) {
                        var quoteIndex = printLine.IndexOf('"');
                        if (quoteIndex >= 0) {
                            printLine = printLine.Insert(quoteIndex, "$");
                        }
                    }
                    if (!printLine.EndsWith(";")) {
                        printLine += ";";
                    }
                    result.AppendLine(GetIndent(indentLevel) + printLine);
                    continue;
                }
                
                // Processar foreach
                if (trimmedLine.StartsWith("foreach")) {
                    var foreachLine = trimmedLine.Replace("foreach", "foreach (var");
                    if (!foreachLine.EndsWith("{")) {
                        foreachLine += " {";
                        indentLevel++;
                    }
                    result.AppendLine(GetIndent(indentLevel) + foreachLine);
                    continue;
                }
                
                // Processar list<T>
                if (trimmedLine.Contains("list<")) {
                    trimmedLine = trimmedLine.Replace("list<", "List<");
                }
                
                // Processar dict<K,V>
                if (trimmedLine.Contains("dict<")) {
                    trimmedLine = trimmedLine.Replace("dict<", "Dictionary<");
                }
                
                // Processar atributos HTTP
                if (trimmedLine.StartsWith("@")) {
                    var attrLine = trimmedLine
                        .Replace("@httpget", "[HttpGet]")
                        .Replace("@httppost", "[HttpPost]")
                        .Replace("@httpput", "[HttpPut]")
                        .Replace("@httpdelete", "[HttpDelete]")
                        .Replace("@route", "[Route]")
                        .Replace("@api", "[ApiController]");
                    result.AppendLine(GetIndent(indentLevel) + attrLine);
                    continue;
                }
                
                // Processar chaves de fechamento
                if (trimmedLine.Equals("}")) {
                    indentLevel--;
                    result.AppendLine(GetIndent(indentLevel) + "}");
                    continue;
                }
                
                // Processar outras linhas
                if (!string.IsNullOrEmpty(trimmedLine)) {
                    if (!trimmedLine.EndsWith(";") && !trimmedLine.EndsWith("{") && !trimmedLine.EndsWith("}")) {
                        trimmedLine += ";";
                    }
                    result.AppendLine(GetIndent(indentLevel) + trimmedLine);
                }
            }
            
            return result.ToString();
        }
        
        private static string GetIndent(int level) {
            return new string(' ', level * 4);
        }
    }
} 