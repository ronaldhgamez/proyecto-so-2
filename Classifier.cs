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
        /// Obtiene la cantidad total de palabras sumando todas las categorías
        /// </summary>
        /// <returns></returns>
        private double GetTotalWords()
        {
            return _categories.Values.Sum(cat => cat.GetTotalWords());
        }

        /// <summary>
        /// Clasifica una lista de noticias
        /// </summary>
        /// <param name="news">Lista de noticias</param>
        public void Classify(List<Website> news)
        {
            Parallel.ForEach(news, item =>
            {
                var (categoryName, score) = Classify(item);

                item.CategoryName = categoryName;

                item.Score = score;

            });
        }
        
        /// <summary>
        /// Clasifica un texto
        /// </summary>
        /// <param name="news">Noticia que se va a clasificar</param>
        /// <returns></returns>
        private (string, double) Classify(Website news)
        {
            var text = news.GetText();
            
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

            var scores = new double[index];
            
            // Asigna una puntuación a cada categoría usando el algoritmo de Bayes Ingenuo
            Parallel.ForEach(words, word =>
            {
                foreach (var category in _categories)
                {
                    var count = category.Value.GetWordCount(word);

                    var totalWords = category.Value.GetTotalWords();
                    
                    var currentScore = scores[scoreIndex[category.Value.Name]];
                    
                    double wordScore = 0 < count 
                        ? Math.Log(count / totalWords) 
                        : 0;

                    Interlocked.Exchange(
                        ref scores[scoreIndex[category.Value.Name]], 
                        currentScore == 0 
                            ? wordScore 
                            : currentScore + wordScore);
                }
            });
            
            // foreach (var (name, cat) in _categories)
            // {
            //     var i = scoreIndex.FirstOrDefault(x => x.Key == name);
            //
            //     var catTotalWords = cat.GetTotalWords();
            //     
            //     var totalWords = GetTotalWords();
            //     
            //     scores[i.Value] += Math.Log(catTotalWords / totalWords);
            // }

            var scoresMapped = scores.Where(x => x != 0).ToArray();

            var maxScore = 0.00;

            var categoryName = "Categoría Indefinida";
            
            if (scoresMapped.Length != 0)
            {
                maxScore = scoresMapped.Min();
                
                var arrayIndex = Array.IndexOf(scores, maxScore);
                            
                categoryName = scoreIndex.FirstOrDefault(x => x.Value == arrayIndex).Key;
            }
            
            return (categoryName, maxScore);
        }
    }
}