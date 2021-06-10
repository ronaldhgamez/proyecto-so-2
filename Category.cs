using System;
using System.Collections.Generic;
using ScrapySharp.Html.Parsing;

namespace console_proyecto_so_2
{
    /// <summary>
    /// Representa una categoría dentro del clasificador, contiene una lista de palabras con sus ocurrencias
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Nombre de la categoría</param>
        public Category(string name)
        {
            Name = name;
            Words = new Dictionary<string, int>();
        }
        
        /// <summary>
        /// Nombre de la categoria
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Diccionario de palabras y la cantidad de apariciones de la palabraí dentro de la categoría
        /// </summary>
        public Dictionary<string, int> Words { get; set; }

        public int GetWordCount(string word)
        {
            return Words.ContainsKey(word) 
                ? Words[word] 
                : 0;
        }

        /// <summary>
        /// Obtiene la cantidad total de palabras de la categoría
        /// </summary>
        /// <returns></returns>
        public double GetTotalWords()
        {
            return Words.Count;
        }
        
    }
}