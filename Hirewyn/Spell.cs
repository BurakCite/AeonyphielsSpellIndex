using System;
using System.Collections.Generic;
using System.Text;

namespace Hirewyn
{
    public class Spell
    {
        private string name;
        private string school;
        private List<string> classes;
        private int level;
        private bool ritual;
        private Time castingTime;
        private Distance range;
        private string components;
        private Time duration;
        private bool concentration;
        private string effect;
        private string description;
        private string higherLevels;
        private string source;

        public Spell()
        {

        }

        public string Name { get => name; set => name = value; }
        public string School { get => school; set => school = value; }
        public int Level { get => level; set => level = value; }
        public bool Ritual { get => ritual; set => ritual = value; }
        public Time CastingTime { get => castingTime; set => castingTime = value; }
        public Distance Range { get => range; set => range = value; }
        public string Components { get => components; set => components = value; }
        public Time Duration { get => duration; set => duration = value; }
        public bool Concentration { get => concentration; set => concentration = value; }
        public string Effect { get => effect; set => effect = value; }
        public string Description { get => description; set => description = value; }
        public string HigherLevels { get => higherLevels; set => higherLevels = value; }
        public string Source { get => source; set => source = value; }
        public List<string> Classes { get => classes; set => classes = value; }
        public string GetClasses ()
        {
            string classesString = String.Empty;

            foreach (string className in classes)
            {
                classesString += className + ", ";
            }
            classesString = classesString.Remove(classesString.Length - 2);

            return classesString;
        }
    }
}
