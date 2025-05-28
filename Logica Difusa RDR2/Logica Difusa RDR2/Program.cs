using System;
using System.Collections.Generic;
using System.Linq;

namespace Logica_Difusa_RDR2
{
    class Program
    {
        static void Main(string[] args)
        {
            var distancia = new FuzzyVariable("Distancia");
            distancia.AddMembershipFunction("Cercana", x => Trapezoidal(x, 0, 0, 10, 15));
            distancia.AddMembershipFunction("Media", x => Triangular(x, 10, 20, 30));
            distancia.AddMembershipFunction("Lejana", x => Trapezoidal(x, 25, 30, 100, 100));

            var amenaza = new FuzzyVariable("Amenaza");
            amenaza.AddMembershipFunction("Leve", x => Trapezoidal(x, 0, 0, 3, 5));
            amenaza.AddMembershipFunction("Media", x => Triangular(x, 3, 5, 7));
            amenaza.AddMembershipFunction("Grave", x => Trapezoidal(x, 5, 7, 10, 10));

            var confianza = new FuzzyVariable("Confianza");
            confianza.AddMembershipFunction("Baja", x => Trapezoidal(x, 0, 0, 30, 50));
            confianza.AddMembershipFunction("Media", x => Triangular(x, 30, 50, 70));
            confianza.AddMembershipFunction("Alta", x => Trapezoidal(x, 50, 70, 100, 100));

            var miedo = new FuzzyVariable("Miedo");
            miedo.AddMembershipFunction("Bajo", x => Trapezoidal(x, 0, 0, 3, 5));
            miedo.AddMembershipFunction("Medio", x => Triangular(x, 3, 5, 7));
            miedo.AddMembershipFunction("Alto", x => Trapezoidal(x, 5, 7, 10, 10));

            var reglas = new List<FuzzyRule>
            {
                // Reglas cuando la confianza es Baja
                new FuzzyRule(new[] {"Cercana", "Leve", "Baja"}, "Alto"),
                new FuzzyRule(new[] {"Cercana", "Media", "Baja"}, "Alto"),
                new FuzzyRule(new[] {"Cercana", "Grave", "Baja"}, "Alto"),
                new FuzzyRule(new[] {"Media", "Leve", "Baja"}, "Medio"),
                new FuzzyRule(new[] {"Media", "Media", "Baja"}, "Alto"),
                new FuzzyRule(new[] {"Media", "Grave", "Baja"}, "Alto"),
                new FuzzyRule(new[] {"Lejana", "Leve", "Baja"}, "Bajo"),
                new FuzzyRule(new[] {"Lejana", "Media", "Baja"}, "Medio"),
                new FuzzyRule(new[] {"Lejana", "Grave", "Baja"}, "Medio"),
                
                // Reglas cuando la confianza es Media
                new FuzzyRule(new[] {"Cercana", "Leve", "Media"}, "Medio"),
                new FuzzyRule(new[] {"Cercana", "Media", "Media"}, "Alto"),
                new FuzzyRule(new[] {"Cercana", "Grave", "Media"}, "Alto"),
                new FuzzyRule(new[] {"Media", "Leve", "Media"}, "Bajo"),
                new FuzzyRule(new[] {"Media", "Media", "Media"}, "Medio"),
                new FuzzyRule(new[] {"Media", "Grave", "Media"}, "Alto"),
                new FuzzyRule(new[] {"Lejana", "Leve", "Media"}, "Bajo"),
                new FuzzyRule(new[] {"Lejana", "Media", "Media"}, "Bajo"),
                new FuzzyRule(new[] {"Lejana", "Grave", "Media"}, "Medio"),
                
                // Reglas cuando la confianza es Alta
                new FuzzyRule(new[] {"Cercana", "Leve", "Alta"}, "Bajo"),
                new FuzzyRule(new[] {"Cercana", "Media", "Alta"}, "Medio"),
                new FuzzyRule(new[] {"Cercana", "Grave", "Alta"}, "Alto"),
                new FuzzyRule(new[] {"Media", "Leve", "Alta"}, "Bajo"),
                new FuzzyRule(new[] {"Media", "Media", "Alta"}, "Bajo"),
                new FuzzyRule(new[] {"Media", "Grave", "Alta"}, "Medio"),
                new FuzzyRule(new[] {"Lejana", "Leve", "Alta"}, "Bajo"),
                new FuzzyRule(new[] {"Lejana", "Media", "Alta"}, "Bajo"),
                new FuzzyRule(new[] {"Lejana", "Grave", "Alta"}, "Bajo")
            };

            Console.WriteLine("=== SISTEMA DE COMPORTAMIENTO DEL CABALLO - RED DEAD REDEMPTION 2 ===");
            Console.WriteLine("===============================================================");

            ProbarCaso(distancia, amenaza, confianza, miedo, reglas, 8, 10, 20, "Caso Crítico: Oso cercano, amenaza grave, confianza baja");
            Console.WriteLine("-----------------------------------------------");
            ProbarCaso(distancia, amenaza, confianza, miedo, reglas, 30, 2, 80, "Caso Seguro: Disparo lejano, amenaza leve, confianza alta");
            Console.WriteLine("-----------------------------------------------");
            ProbarCaso(distancia, amenaza, confianza, miedo, reglas, 15, 6, 60, "Caso Intermedio: Disparo a media distancia, amenaza media, confianza media");
            Console.WriteLine("-----------------------------------------------");

            Console.WriteLine("\n=== MODO INTERACTIVO ===");
            Console.WriteLine("Ingrese valores para probar el comportamiento del caballo");

            while (true)
            {
                try
                {
                    Console.Write("Distancia a la amenaza (0-100 metros): ");
                    if (!double.TryParse(Console.ReadLine(), out double distanciaInput))
                    {
                        Console.WriteLine("Error: Valor de distancia inválido. Intente nuevamente.");
                        continue;
                    }

                    Console.Write("Nivel de amenaza (0-10): ");
                    if (!double.TryParse(Console.ReadLine(), out double amenazaInput))
                    {
                        Console.WriteLine("Error: Valor de amenaza inválido. Intente nuevamente.");
                        continue;
                    }

                    Console.Write("Nivel de confianza (0-100%): ");
                    if (!double.TryParse(Console.ReadLine(), out double confianzaInput))
                    {
                        Console.WriteLine("Error: Valor de confianza inválido. Intente nuevamente.");
                        continue;
                    }

                    ProbarCaso(distancia, amenaza, confianza, miedo, reglas,
                             distanciaInput, amenazaInput, confianzaInput, "Evaluación personalizada");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            }
        }

        static void ProbarCaso(FuzzyVariable distancia, FuzzyVariable amenaza, FuzzyVariable confianza,
                             FuzzyVariable miedo, List<FuzzyRule> reglas,
                             double distanciaValor, double amenazaValor, double confianzaValor,
                             string descripcion)
        {
            Console.WriteLine($"\n- {descripcion}");
            Console.WriteLine($"- Distancia: {distanciaValor} metros");
            Console.WriteLine($"- Nivel de amenaza: {amenazaValor}/10");
            Console.WriteLine($"- Confianza: {confianzaValor}%");

            double resultado = EvaluarMiedo(distancia, amenaza, confianza, miedo, reglas,
                                         distanciaValor, amenazaValor, confianzaValor);

            InterpretarResultado(resultado);
        }

        static double EvaluarMiedo(FuzzyVariable distancia, FuzzyVariable amenaza, FuzzyVariable confianza,
                                 FuzzyVariable miedo, List<FuzzyRule> reglas,
                                 double distanciaValor, double amenazaValor, double confianzaValor)
        {
            // Validación y normalización de entradas
            distanciaValor = Math.Max(0, distanciaValor);
            amenazaValor = Math.Clamp(amenazaValor, 0, 10);
            confianzaValor = Math.Clamp(confianzaValor, 0, 100);

            // Fuzzificación
            var distanciaFuzz = distancia.Fuzzify(distanciaValor);
            var amenazaFuzz = amenaza.Fuzzify(amenazaValor);
            var confianzaFuzz = confianza.Fuzzify(confianzaValor);

            // Inferencia difusa
            var outputAggregated = new Dictionary<string, double>();

            foreach (var regla in reglas)
            {
                double gradoActivacion = Math.Min(
                    distanciaFuzz[regla.InputLabels[0]],
                    Math.Min(
                        amenazaFuzz[regla.InputLabels[1]],
                        confianzaFuzz[regla.InputLabels[2]]
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

            // Defuzzificación (método del centroide)
            double sumWeighted = 0;
            double sumDegrees = 0;
            int steps = 1000;

            for (int i = 0; i <= steps; i++)
            {
                double x = (double)i / steps * 10;
                double degree = 0;

                foreach (var mf in miedo.MembershipFunctions)
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
                return 5.0; // Valor por defecto si no hay activación
            }

            double resultado = sumWeighted / sumDegrees;
            return Math.Round(Math.Clamp(resultado, 0, 10), 2);
        }

        static void InterpretarResultado(double miedo)
        {
            if (double.IsNaN(miedo))
            {
                Console.WriteLine("Error crítico: No se pudo calcular el nivel de miedo (NaN)");
                return;
            }

            Console.WriteLine($"\nNivel de miedo del caballo: {miedo}/10 ({(miedo * 10):F0}%)");

            if (miedo <= 3.5)
                Console.WriteLine("Comportamiento: El caballo se inquieta un poco, tal vez relincha");
            else if (miedo <= 6.5)
                Console.WriteLine("Comportamiento: El caballo se mueve nervioso o se detiene");
            else
                Console.WriteLine("Comportamiento: El caballo entra en pánico, se echa hacia atrás o te lanza");
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