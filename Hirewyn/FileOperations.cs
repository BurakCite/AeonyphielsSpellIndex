using System;
using System.Collections.Generic;
using System.Linq;

namespace Hirewyn
{
    public class FileOperations
    {
        private static readonly int NumberOfLinesInSpell = 16;

        public static List<string> GetSpellNames()
        {
            string textFile = @"D:\SpellList.txt";
            List<string> lines = System.IO.File.ReadAllLines(textFile).ToList<string>();
            var linesToBeRemoved = lines.Where(e => e.Contains("//")).ToList<string>();
            foreach (string lineToBeRemoved in linesToBeRemoved)
            {
                lines.Remove(lineToBeRemoved);
            }

            return lines;
        }
        public static List<Spell> LoadSpells()
        {
            string textFile = @"D:\Spells.txt";
            string[] lines = System.IO.File.ReadAllLines(textFile);

            int counter = 0;
            string lastComponent = String.Empty;
            List<Spell> spells = new List<Spell>();
            Spell spell = new Spell();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("Name: "))
                {
                    spell.Name = lines[i].Substring(6);
                }
                else if (lines[i].Contains("School: "))
                {
                    spell.School = lines[i].Substring(8);
                }
                else if (lines[i].Contains("Classes: "))
                {
                    spell.Classes = lines[i].Substring(9).Replace(", ", ",").Split(',').ToList();
                }
                else if (lines[i].Contains("Level: "))
                {
                    spell.Level = Convert.ToInt32(lines[i].Substring(7));
                }
                else if (lines[i].Contains("Ritual: "))
                {
                    if (lines[i].Substring(8) == "Yes")
                    {
                        spell.Ritual = true;
                    }
                    else
                    {
                        spell.Ritual = false;
                    }
                }
                else if (lines[i].Contains("Casting Time: "))
                {
                    Time time = new Time();
                    string castingTimeString = lines[i].Substring(14);
                    string timeValue = castingTimeString.Split(' ').FirstOrDefault();
                    string timeEnum = castingTimeString.Split(' ').LastOrDefault();
                    if (timeValue != timeEnum && castingTimeString.Split(' ')[1] != null && castingTimeString.Split(' ')[1] == "bonus")
                    {
                        time.Value = Convert.ToInt32(timeValue);
                        time.TimeType = Time.TimeEnum.BonusAction;
                    }
                    else if (timeValue != timeEnum && castingTimeString.Split(' ')[1] != null && castingTimeString.Split(' ')[1] == "reaction,")
                    {
                        time.Value = Convert.ToInt32(timeValue);
                        time.TimeType = Time.TimeEnum.Reaction;
                    }
                    else if (castingTimeString == "Until dispelled")
                    {
                        time.Value = 0;
                        time.TimeType = Time.TimeEnum.UntilDispelled;
                    }
                    else if (castingTimeString == "Instantaneous")
                    {
                        time.Value = 0;
                        time.TimeType = Time.TimeEnum.Instantaneous;
                    }
                    else if (castingTimeString == "Special")
                    {
                        time.Value = 0;
                        time.TimeType = Time.TimeEnum.Special;
                    }
                    else
                    {
                        time.Value = Convert.ToInt32(timeValue);
                        if (timeEnum[timeEnum.Length - 1] == 's')
                        {
                            timeEnum = timeEnum.Remove(timeEnum.Length - 1);
                        }
                        timeEnum = ToUpperEveryWordStart(timeEnum);
                        time.TimeType = (Time.TimeEnum)Enum.Parse(typeof(Time.TimeEnum), timeEnum);
                    }
                    spell.CastingTime = time;
                }
                else if (lines[i].Contains("Range: "))
                {
                    Distance distance = new Distance();
                    string distanceString = lines[i].Substring(7);
                    string distanceValue = distanceString.Split(' ').FirstOrDefault();
                    string distanceEnum = distanceString.Split(' ').LastOrDefault();
                    distanceEnum = ToUpperEveryWordStart(distanceEnum);
                    if (distanceValue == distanceEnum && distanceEnum == "Self")
                    {
                        distance.Value = 0;
                        distance.DistanceType = Distance.DistanceEnum.Self;
                    }
                    else if (distanceValue == distanceEnum && distanceEnum == "Touch")
                    {
                        distance.Value = 0;
                        distance.DistanceType = Distance.DistanceEnum.Touch;
                    }
                    else if (distanceValue == distanceEnum && distanceEnum == "Sight")
                    {
                        distance.Value = 0;
                        distance.DistanceType = Distance.DistanceEnum.Sight;
                    }
                    else if (distanceValue == distanceEnum && distanceEnum == "Unlimited")
                    {
                        distance.Value = 0;
                        distance.DistanceType = Distance.DistanceEnum.Unlimited;
                    }
                    else if (distanceValue == distanceEnum && distanceEnum == "Special")
                    {
                        distance.Value = 0;
                        distance.DistanceType = Distance.DistanceEnum.Special;
                    }
                    else
                    {
                        if (distanceEnum[distanceEnum.Length - 1] == 's')
                        {
                            distanceEnum = distanceEnum.Remove(distanceEnum.Length - 1);
                        }
                        distance.Value = Convert.ToInt32(distanceValue);
                        distance.DistanceType = (Distance.DistanceEnum)Enum.Parse(typeof(Distance.DistanceEnum), distanceEnum);
                    }
                    spell.Range = distance;
                }
                else if (lines[i].Contains("Components: "))
                {
                    spell.Components = lines[i].Substring(12);
                }
                else if (lines[i].Contains("Duration: "))
                {
                    Time time = new Time();
                    string durationString = lines[i].Substring(10);
                    string timeValue = durationString.Split(' ').FirstOrDefault();
                    string timeEnum = durationString.Split(' ').LastOrDefault();
                    if (durationString.Contains("*"))
                    {
                        time.Value = 0;
                        time.TimeType = Time.TimeEnum.Special;
                    }
                    else if (durationString == "Until dispelled")
                    {
                        time.Value = 0;
                        time.TimeType = Time.TimeEnum.UntilDispelled;
                    }
                    else if (durationString == "Instantaneous")
                    {
                        time.Value = 0;
                        time.TimeType = Time.TimeEnum.Instantaneous;
                    }
                    else if (durationString == "Special")
                    {
                        time.Value = 0;
                        time.TimeType = Time.TimeEnum.Special;
                    }
                    else
                    {
                        time.Value = Convert.ToInt32(timeValue);
                        if (timeEnum[timeEnum.Length - 1] == 's')
                        {
                            timeEnum = timeEnum.Remove(timeEnum.Length - 1);
                        }
                        timeEnum = ToUpperEveryWordStart(timeEnum);
                        time.TimeType = (Time.TimeEnum)Enum.Parse(typeof(Time.TimeEnum), timeEnum);
                    }
                    spell.Duration = time;
                }
                else if (lines[i].Contains("Concentration: "))
                {
                    if (lines[i].Substring(15) == "Yes")
                    {
                        spell.Concentration = true;
                    }
                    else
                    {
                        spell.Concentration = false;
                    }
                }
                else if (lines[i].Contains("Effect: "))
                {
                    spell.Effect = lines[i].Substring(8);
                }
                else if (lines[i].Contains("Description: "))
                {
                    spell.Description = lines[i].Substring(13) + "\r\n";
                    lastComponent = "description";
                }
                else if (lines[i].Contains("Higher Levels: "))
                {
                    spell.HigherLevels = lines[i].Substring(15) + "\r\n";
                    lastComponent = "higherLevels";
                }
                else if (lines[i].Contains("Source: "))
                {
                    spell.Source = lines[i].Substring(8);
                    lastComponent = "source";
                }
                else if (lastComponent == "description")
                {
                    spell.Description += lines[i] + "\r\n";
                }
                else if (lastComponent == "higherLevels")
                {
                    spell.HigherLevels += lines[i] + "\r\n";
                }
                else if (lines[i] == String.Empty)
                {
                    spells.Add(spell);
                    spell = new Spell();
                }
                counter++;
            }
            return spells.OrderBy(e => e.Name).ToList();
        }
        public static string ToUpperEveryWordStart(string wordsString)
        {
            string[] words = wordsString.Split(' ');
            string upperedWordsString = String.Empty;
            foreach (string word in words)
            {
                upperedWordsString += word[0].ToString().ToUpper() + word.Substring(1);
            }
            return upperedWordsString;
        }
        public static void SaveSpells(List<Spell> spells)
        {
            List<string> lines = new List<string>();
            foreach (Spell spell in spells)
            {
                lines.Add("Name: " + spell.Name);

                lines.Add("School: " + spell.School);

                string classes = String.Empty;
                foreach (string className in spell.Classes)
                {
                    classes += className + ", ";
                }
                classes = classes.Remove(classes.Length - 2);
                lines.Add("Classes: " + classes);

                lines.Add("Level: " + spell.Level);

                lines.Add("Ritual: " + (spell.Ritual ? "Yes" : "No"));

                lines.Add("Casting Time: " + spell.CastingTime);

                lines.Add("Range: " + spell.Range);

                lines.Add("Components: " + spell.Components);

                lines.Add("Duration: " + spell.Duration);

                lines.Add("Concentration: " + (spell.Concentration ? "Yes" : "No"));

                lines.Add("Effect: " + spell.Effect);

                //lines.Add("Save: " + spell.Save);

                lines.Add("Description: " + spell.Description);

                lines.Add("Higher Levels: " + (spell.HigherLevels != null ? spell.HigherLevels : "No"));

                lines.Add("Source: " + spell.Source);

                lines.Add(string.Empty);
            }
            System.IO.File.WriteAllLines(@"D:\Spells.txt", lines);

        }
    }
}
