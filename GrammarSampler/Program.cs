using System;
using System.Collections.Generic;
using System.Linq;

namespace GrammarSampler
{
    class Rule
    {
        public string Input { get; private set; }
        public string Output { get; private set; }

        public Rule(string input, string output)
        {
            Input = input;
            Output = output;
        }
    }

    class Program
    {
        static void add_(List<Rule> rules, string inputString, string outputString, int multiplicity = 1)
        {
            for (int i = 0; i < multiplicity; i++)
            {
                rules.Add(new Rule(inputString, outputString));
            }
        }

        static List<Rule> megaDyck()
        {
            List<Rule> rules = new List<Rule>();
            add_(rules, "S", "(S)", 3);
            add_(rules, "S", "[S]", 3);
            add_(rules, "S", "{S}", 3);
            add_(rules, "S", "SS", 4);
            add_(rules, "S", "", 5);
            return rules;
        }

        static List<Rule> unaryMultiplication()
        {
            List<Rule> rules = new List<Rule>();
            add_(rules, "S", "LR");
            add_(rules, "L", "aLX", 3);
            add_(rules, "R", "BR", 3);
            add_(rules, "L", "M");
            add_(rules, "R", "E");
            add_(rules, "XB", "BCX", 100);
            add_(rules, "XC", "CX", 100);
            add_(rules, "CB", "BC", 100);
            add_(rules, "XE", "E");
            add_(rules, "MB", "bM", 10);
            add_(rules, "M", "K");
            add_(rules, "KC", "cK", 100);
            add_(rules, "KE", "");
            return rules;
        }

        static Random RNG = new Random();
        const int max_length = 111;
        const int max_steps = 666000;

        static void Main(string[] args)
        {
            string initial = "S";
            List<Rule> rules = unaryMultiplication();

            HashSet<string> generated = new HashSet<string>();
            while (true)
            {
                string s = "INVALID";
                int n = max_steps + 1;
                while (s.Any(c => char.IsUpper(c)))
                {
                    if (s.Length > max_length || (n++) > max_steps)
                    {
                        s = initial;
                        n = 0;
                    }
                    Rule rule = rules[RNG.Next(rules.Count)];
                    if (s.Length < rule.Input.Length)
                    {
                        continue;
                    }
                    int pos = RNG.Next(s.Length - rule.Input.Length + 1);
                    if (s.Substring(pos, rule.Input.Length) == rule.Input)
                    {
                        s = s.Substring(0, pos) + rule.Output + s.Substring(pos + rule.Input.Length);
                    }
                }
                if (!generated.Contains(s))
                {
                    Console.WriteLine(s);
                    generated.Add(s);
                }
            }
        }
    }
}
