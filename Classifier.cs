using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace console_proyecto_so_2
{
    public class Classifier
    {
        /// <summary>
        /// Categorías
        /// </summary>
        private Dictionary<string, Category> _categories;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="categories"></param>
        public Classifier(Dictionary<string, Category> categories)
        {
            _categories = categories;
        }

        /// <summary>
        /// Clasifica un texto
        /// </summary>
        /// <param name="text">Texto que se va a clasificar</param>
        /// <returns></returns>
        public double[] Classify(string text)
        {
            var scoreIndex = new Dictionary<string, int>();

            text = new string(text.Where(c => !char.IsPunctuation(c)).ToArray());

            var words = text.Split(' ');

            // Inicializa las categorías con una puntiación de 0
            var index = 0;
                
            foreach (var category in _categories)
            {
                scoreIndex.Add(category.Value.Name, index);

                index++;
            }

            var score = new double[index + 1];
            
            // Asigna una puntuación a cada categoría usando el algoritmo de Bayes Ingenuo
            // foreach (var word in words)
            // {
            //     foreach (var category in _categories)
            //     {
            //         var count = category.Value.GetWordCount(word);
            //
            //         if (count <= 0) continue;
            //         
            //         var wordScore = count / category.Value.GetTotalWords();
            //
            //         var currentScore = score[category.Value.Name];
            //         
            //         score[category.Value.Name] = currentScore == 0 
            //             ? wordScore 
            //             : currentScore * wordScore;
            //     }
            // }

            Parallel.ForEach(words, word =>
            {
                foreach (var category in _categories)
                {
                    var count = category.Value.GetWordCount(word);

                    if (count <= 0) continue;
                    
                    var wordScore = count / category.Value.GetTotalWords();
            
                    var currentScore = score[scoreIndex[category.Value.Name]];

                    Interlocked.Exchange(
                        ref score[scoreIndex[category.Value.Name]], 
                        currentScore == 0 
                            ? wordScore 
                            : currentScore * wordScore);
                }
            });
            
            return score;
        }
    }
}