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
        static List<Rule> megaDyck()
        {
            List<Rule> rules = new List<Rule>();
            rules.Add(new Rule("S", "(S)"));
            rules.Add(new Rule("S", "[S]"));
            rules.Add(new Rule("S", "{S}"));
            rules.Add(new Rule("S", "SS"));
            rules.Add(new Rule("S", ""));
            return rules;
        }

        static List<Rule> unaryMultiplication()
        {
            List<Rule> rules = new List<Rule>();
            rules.Add(new Rule("S", "LR"));
            rules.Add(new Rule("L", "aLX"));
            rules.Add(new Rule("R", "BR"));
            rules.Add(new Rule("L", "M"));
            rules.Add(new Rule("R", "E"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("XB", "BCX"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("XC", "CX"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("CB", "BC"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("XE", "E"));
            for (int i = 0; i < 10; i++) rules.Add(new Rule("MB", "bM"));
            rules.Add(new Rule("M", "K"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("KC", "cK"));
            rules.Add(new Rule("KE", ""));
            return rules;
        }

        static List<Rule> unaryMultiplication_bug1()
        {
            List<Rule> rules = new List<Rule>();
            rules.Add(new Rule("S", "LR"));
            rules.Add(new Rule("L", "aLX"));
            rules.Add(new Rule("R", "BR"));
            rules.Add(new Rule("R", "E"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("XB", "BCX"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("XC", "CX"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("CB", "BC"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("XE", "E"));
            for (int i = 0; i < 10; i++) rules.Add(new Rule("LB", "bL"));
            rules.Add(new Rule("L", "K"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("KC", "cK"));
            rules.Add(new Rule("KE", ""));
            return rules;
        }

        static List<Rule> unaryMultiplication_bug2()
        {
            List<Rule> rules = new List<Rule>();
            rules.Add(new Rule("S", "LR"));
            rules.Add(new Rule("L", "aLX"));
            rules.Add(new Rule("R", "BR"));
            rules.Add(new Rule("L", "M"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("XB", "BCX"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("XC", "CX"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("CB", "BC"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("XR", "R"));
            for (int i = 0; i < 10; i++) rules.Add(new Rule("MB", "bM"));
            rules.Add(new Rule("M", "K"));
            for (int i = 0; i < 100; i++) rules.Add(new Rule("KC", "cK"));
            rules.Add(new Rule("KR", ""));
            return rules;
        }

        static Random RNG = new Random();
        const int max_length = 80;
        const int max_steps = 1234567;

        static void Main(string[] args)
        {
            string initial = "S";
            List <Rule> rules = unaryMultiplication();

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
