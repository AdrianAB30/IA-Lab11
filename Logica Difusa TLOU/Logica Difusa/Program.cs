using System;
using System.Collections.Generic;
using System.Linq;

namespace Logica_Difusa_TLOU
{
    class Program
    {
        static void Main(string[] args)
        {
            var salud = new FuzzyVariable("Salud");
            salud.AddMembershipFunction("Baja", x => Trapezoidal(x, 0, 0, 30, 50));
            salud.AddMembershipFunction("Media", x => Triangular(x, 30, 50, 70));
            salud.AddMembershipFunction("Alta", x => Trapezoidal(x, 50, 70, 100, 100));

            var inventario = new FuzzyVariable("Inventario");
            inventario.AddMembershipFunction("Vacío", x => Trapezoidal(x, 0, 0, 1, 2));
            inventario.AddMembershipFunction("Escaso", x => Triangular(x, 1, 3, 5));
            inventario.AddMembershipFunction("Suficiente", x => Trapezoidal(x, 4, 5, 10, 10));

            var amenazas = new FuzzyVariable("Amenazas");
            amenazas.AddMembershipFunction("Bajas", x => Trapezoidal(x, 0, 0, 3, 5));
            amenazas.AddMembershipFunction("Medias", x => Triangular(x, 3, 5, 7));
            amenazas.AddMembershipFunction("Altas", x => Trapezoidal(x, 5, 7, 10, 10));

            var necesidad = new FuzzyVariable("Necesidad");
            necesidad.AddMembershipFunction("Bajo", x => Trapezoidal(x, 0, 0, 3, 5));
            necesidad.AddMembershipFunction("Medio", x => Triangular(x, 3, 5, 7));
            necesidad.AddMembershipFunction("Alto", x => Trapezoidal(x, 5, 7, 10, 10));

            var reglas = new List<FuzzyRule>
            {
                // Reglas cuando salud es Alta
                new FuzzyRule(new[] {"Baja", "Vacío", "Bajas"}, "Alto"),
                new FuzzyRule(new[] {"Baja", "Vacío", "Medias"}, "Alto"),
                new FuzzyRule(new[] {"Baja", "Vacío", "Altas"}, "Alto"),
                new FuzzyRule(new[] {"Baja", "Escaso", "Bajas"}, "Medio"),
                new FuzzyRule(new[] {"Baja", "Escaso", "Medias"}, "Alto"),
                new FuzzyRule(new[] {"Baja", "Escaso", "Altas"}, "Alto"),
                new FuzzyRule(new[] {"Baja", "Suficiente", "Bajas"}, "Medio"),
                new FuzzyRule(new[] {"Baja", "Suficiente", "Medias"}, "Alto"),
                new FuzzyRule(new[] {"Baja", "Suficiente", "Altas"}, "Alto"),
                
                // Reglas cuando salud es Media
                new FuzzyRule(new[] {"Media", "Vacío", "Bajas"}, "Medio"),
                new FuzzyRule(new[] {"Media", "Vacío", "Medias"}, "Alto"),
                new FuzzyRule(new[] {"Media", "Vacío", "Altas"}, "Alto"),
                new FuzzyRule(new[] {"Media", "Escaso", "Bajas"}, "Medio"),
                new FuzzyRule(new[] {"Media", "Escaso", "Medias"}, "Medio"),
                new FuzzyRule(new[] {"Media", "Escaso", "Altas"}, "Alto"),
                new FuzzyRule(new[] {"Media", "Suficiente", "Bajas"}, "Bajo"),
                new FuzzyRule(new[] {"Media", "Suficiente", "Medias"}, "Medio"),
                new FuzzyRule(new[] {"Media", "Suficiente", "Altas"}, "Medio"),
                
                // Reglas cuando salud es Alta
                new FuzzyRule(new[] {"Alta", "Vacío", "Bajas"}, "Bajo"),
                new FuzzyRule(new[] {"Alta", "Vacío", "Medias"}, "Medio"),
                new FuzzyRule(new[] {"Alta", "Vacío", "Altas"}, "Alto"),
                new FuzzyRule(new[] {"Alta", "Escaso", "Bajas"}, "Bajo"),
                new FuzzyRule(new[] {"Alta", "Escaso", "Medias"}, "Medio"),
                new FuzzyRule(new[] {"Alta", "Escaso", "Altas"}, "Medio"),
                new FuzzyRule(new[] {"Alta", "Suficiente", "Bajas"}, "Bajo"),
                new FuzzyRule(new[] {"Alta", "Suficiente", "Medias"}, "Bajo"),
                new FuzzyRule(new[] {"Alta", "Suficiente", "Altas"}, "Medio")
            };

            Console.WriteLine("=== SISTEMA DE CONTROL DE LOOT - THE LAST OF US ===");
            Console.WriteLine("=================================================");

            ProbarCaso(salud, inventario, amenazas, necesidad, reglas, 15, 0, 9, "Caso Crítico: Salud muy baja, sin recursos, amenazas altas");
            Console.WriteLine("-----------------------------------------------");
            ProbarCaso(salud, inventario, amenazas, necesidad, reglas, 90, 6, 2, "Caso Seguro: Salud alta, muchos recursos, amenazas bajas");
            Console.WriteLine("-----------------------------------------------");
            ProbarCaso(salud, inventario, amenazas, necesidad, reglas, 55, 3, 5, "Caso Intermedio: Salud media, algunos recursos, amenazas medias");
            Console.WriteLine("-----------------------------------------------");

            Console.WriteLine("\n=== MODO INTERACTIVO ===");
            Console.WriteLine("Ingrese valores para probar");

            while (true)
            {
                try
                {
                    Console.Write("Salud (0-100%): ");
                    string? input = Console.ReadLine(); 

                    if (double.TryParse(input, out double saludInput))
                    {
                        Console.Write("Objetos en inventario: ");
                        string? inventarioInputStr = Console.ReadLine();

                        if (!double.TryParse(inventarioInputStr, out double inventarioInput))
                        {
                            Console.WriteLine("Error: Valor de inventario inválido. Intente nuevamente.");
                            continue;
                        }

                        Console.Write("Nivel de amenazas (0-10): ");
                        string? amenazasInputStr = Console.ReadLine();

                        if (!double.TryParse(amenazasInputStr, out double amenazasInput))
                        {
                            Console.WriteLine("Error: Valor de amenazas inválido. Intente nuevamente.");
                            continue;
                        }

                        ProbarCaso(salud, inventario, amenazas, necesidad, reglas,
                                 saludInput, inventarioInput, amenazasInput, "Evaluación personalizada");
                    }
                    else
                    {
                        Console.WriteLine("Error: Valor de salud inválido. Intente nuevamente.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            }
        }

        static void ProbarCaso(FuzzyVariable salud, FuzzyVariable inventario, FuzzyVariable amenazas,
                             FuzzyVariable necesidad, List<FuzzyRule> reglas,
                             double saludValor, double inventarioValor, double amenazasValor,
                             string descripcion)
        {
            Console.WriteLine($"\n- {descripcion}");
            Console.WriteLine($"- Salud: {saludValor}%");
            Console.WriteLine($"- Inventario: {inventarioValor} objetos");
            Console.WriteLine($"- Amenazas: {amenazasValor}/10");

            double resultado = EvaluarNecesidad(salud, inventario, amenazas, necesidad, reglas,
                                             saludValor, inventarioValor, amenazasValor);

            InterpretarResultado(resultado);
        }

        static double EvaluarNecesidad(FuzzyVariable salud, FuzzyVariable inventario, FuzzyVariable amenazas,
                                     FuzzyVariable necesidad, List<FuzzyRule> reglas,
                                     double saludValor, double inventarioValor, double amenazasValor)
        {
            // Validación y normalización de entradas
            saludValor = Math.Clamp(saludValor, 0, 100);
            inventarioValor = Math.Max(0, inventarioValor);
            amenazasValor = Math.Clamp(amenazasValor, 0, 10);

            // Fuzzificación
            var saludFuzz = salud.Fuzzify(saludValor);
            var inventarioFuzz = inventario.Fuzzify(inventarioValor);
            var amenazasFuzz = amenazas.Fuzzify(amenazasValor);

            // Inferencia difusa
            var outputAggregated = new Dictionary<string, double>();

            foreach (var regla in reglas)
            {
                double gradoActivacion = Math.Min(
                    saludFuzz[regla.InputLabels[0]],
                    Math.Min(
                        inventarioFuzz[regla.InputLabels[1]],
                        amenazasFuzz[regla.InputLabels[2]]
                    )
                );

                if (gradoActivacion > 0.01) 
                {
                    if (outputAggregated.ContainsKey(regla.OutputLabel))
                        outputAggregated[regla.OutputLabel] = Math.Max(outputAggregated[regla.OutputLabel], gradoActivacion);
                    else
                        outputAggregated[regla.OutputLabel] = gradoActivacion;
                }
            }

            double sumWeighted = 0;
            double sumDegrees = 0;
            int steps = 1000; 

            for (int i = 0; i <= steps; i++)
            {
                double x = (double)i / steps * 10; 
                double degree = 0;

                foreach (var mf in necesidad.MembershipFunctions)
                {
                    if (outputAggregated.ContainsKey(mf.Key))
                    {
                        double membershipValue = mf.Value(x);
                        degree = Math.Max(degree, Math.Min(outputAggregated[mf.Key], membershipValue));
                    }
                }

                sumWeighted += x * degree;
                sumDegrees += degree;
            }

            if (sumDegrees <= 0.001)
            {
                return 5.0; 
            }

            double resultado = sumWeighted / sumDegrees;
            return Math.Round(Math.Clamp(resultado, 0, 10), 2);
        }

        static void InterpretarResultado(double necesidad)
        {
            if (double.IsNaN(necesidad))
            {
                Console.WriteLine("Error crítico: No se pudo calcular la necesidad (NaN)");
                return;
            }

            Console.WriteLine($"\nNivel de necesidad: {necesidad}/10 ({(necesidad * 10):F0}%)");

            if (necesidad <= 3.5)
                Console.WriteLine("Decisión: BAJA - Aparecerá poco o nada de loot");
            else if (necesidad <= 6.5)
                Console.WriteLine("Decisión: MEDIA - Aparecerá loot moderado");
            else
                Console.WriteLine("Decisión: ALTA - Aparecerá loot valioso o en mayor cantidad");
        }

        static double Triangular(double x, double a, double b, double c)
        {
            if (x <= a || x >= c) return 0;
            if (x == b) return 1;
            return x < b ? (x - a) / (b - a) : (c - x) / (c - b);
        }

        static double Trapezoidal(double x, double a, double b, double c, double d)
        {
            if (x <= a || x >= d) return 0;
            if (x >= b && x <= c) return 1;
            return x < b ? (x - a) / (b - a) : (d - x) / (d - c);
        }
    }

    class FuzzyVariable
    {
        public string Name { get; }
        public Dictionary<string, Func<double, double>> MembershipFunctions { get; }

        public FuzzyVariable(string name)
        {
            Name = name;
            MembershipFunctions = new Dictionary<string, Func<double, double>>();
        }

        public void AddMembershipFunction(string label, Func<double, double> function)
        {
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentException("La etiqueta no puede estar vacía");

            MembershipFunctions[label] = function ?? throw new ArgumentNullException(nameof(function));
        }

        public Dictionary<string, double> Fuzzify(double value)
        {
            var result = new Dictionary<string, double>();
            foreach (var mf in MembershipFunctions)
            {
                try
                {
                    result[mf.Key] = mf.Value(value);
                }
                catch
                {
                    result[mf.Key] = 0;
                }
            }
            return result;
        }
    }

    class FuzzyRule
    {
        public string[] InputLabels { get; }
        public string OutputLabel { get; }

        public FuzzyRule(string[] inputLabels, string outputLabel)
        {
            InputLabels = inputLabels ?? throw new ArgumentNullException(nameof(inputLabels));
            OutputLabel = outputLabel ?? throw new ArgumentNullException(nameof(outputLabel));

            if (InputLabels.Length != 3)
                throw new ArgumentException("Se requieren exactamente 3 etiquetas de entrada");
        }
    }
}