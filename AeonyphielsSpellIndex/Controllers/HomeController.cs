using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hirewyn;

namespace AeonyphielsSpellIndex.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Spells = GetSpellsFromSession();
            return View();
        }
        private List<Spell> GetSpellsFromSession()
        {
            if (Session["Spells"] == null)
            {
                //List<string> spellsNames = FileOperations.GetSpellNames();
                //List<Spell> spellsForSession  = new List<Spell>();
                //foreach (string spellName in spellsNames)
                //{
                //    spellsForSession.Add(WebOperations.DownloadSpellData(spellName));
                //}
                //Session["Spells"] = spellsForSession;
                //FileOperations.SaveSpells(spellsForSession);
                Session["Spells"] = FileOperations.LoadSpells();
            }
            List<Spell> spells = (List<Spell>)Session["Spells"];
            return spells;
            //spells.Add(WebOperations.DownloadSpellData("Crusader's Mantle"));
            //return null;
        }
        public ActionResult Filter(string classNameDropdownList, string minLevelTextBox, string maxLevelTextBox, string ritualDropdownList, string diceDropdownList, string minimumCastingTimeTextBox, string minimumCastingTimeDropdownList, string maximumCastingTimeTextBox, string maximumCastingTimeDropdownList, string minimumRangeTextBox, string minimumRangeDropdownList, string maximumRangeTextBox, string maximumRangeDropdownList, string componentsDropdownList, string minimumDurationTextBox, string minimumDurationDropdownList, string maximumDurationTextBox, string maximumDurationDropdownList, string concentrationDropdownList, string higherLevelsDropdownList)
        {
            string className = "All";
            ViewBag.ClassName = className;
            if (!string.IsNullOrEmpty(classNameDropdownList))
            {
                className = classNameDropdownList;
                ViewBag.ClassName = className;
            }

            int minLevel = 0;
            ViewBag.MinLevel = minLevel;
            if (!string.IsNullOrEmpty(minLevelTextBox))
            {
                minLevel = Convert.ToInt32(minLevelTextBox);
                ViewBag.MinLevel = minLevel;
            }

            int maxLevel = 9;
            ViewBag.MaxLevel = maxLevel;
            if (!string.IsNullOrEmpty(maxLevelTextBox))
            {
                maxLevel = Convert.ToInt32(maxLevelTextBox);
                ViewBag.MaxLevel = maxLevel;
            }
            string ritual = "All";
            ViewBag.Ritual = ritual;
            if (!string.IsNullOrEmpty(ritualDropdownList))
            {
                ritual = ritualDropdownList;
                ViewBag.Ritual = ritual;
            }
            string dice = "All";
            ViewBag.Dice = dice;
            if (!string.IsNullOrEmpty(diceDropdownList))
            {
                dice = ritualDropdownList;
                ViewBag.Dice = dice;
            }

            int minimumCastingTimeValue = 0;
            ViewBag.MinimumCastingTimeValue = (int)minimumCastingTimeValue;
            if (!string.IsNullOrEmpty(minimumCastingTimeTextBox))
            {
                minimumCastingTimeValue = Convert.ToInt32(minimumCastingTimeTextBox);
                ViewBag.MinimumCastingTimeValue = (int)minimumCastingTimeValue;
            }
            Time.TimeEnum minimumCastingTimeType = Time.TimeEnum.Special;
            ViewBag.MinimumCastingTimeType = (int)minimumCastingTimeType;
            if (!string.IsNullOrEmpty(minimumCastingTimeDropdownList))
            {
                minimumCastingTimeType = (Time.TimeEnum)Enum.Parse(typeof(Time.TimeEnum), minimumCastingTimeDropdownList);
                ViewBag.MinimumCastingTimeType = (int)minimumCastingTimeType;
            }
            int maximumCastingTimeValue = 360;
            ViewBag.MaximumCastingTimeValue = (int)maximumCastingTimeValue;
            if (!string.IsNullOrEmpty(maximumCastingTimeTextBox))
            {
                maximumCastingTimeValue = Convert.ToInt32(maximumCastingTimeTextBox);
                ViewBag.MaximumCastingTimeValue = (int)maximumCastingTimeValue;
            }
            Time.TimeEnum maximumCastingTimeType = Time.TimeEnum.UntilDispelled;
            ViewBag.MaximumCastingTimeType = (int)maximumCastingTimeType;
            if (!string.IsNullOrEmpty(maximumCastingTimeDropdownList))
            {
                maximumCastingTimeType = (Time.TimeEnum)Enum.Parse(typeof(Time.TimeEnum), maximumCastingTimeDropdownList);
                ViewBag.MaximumCastingTimeType = (int)maximumCastingTimeType;
            }

            int minimumRangeValue = 0;
            ViewBag.MinimumRangeValue = minimumRangeValue;
            if (!string.IsNullOrEmpty(minimumRangeTextBox))
            {
                minimumRangeValue = Convert.ToInt32(minimumRangeTextBox);
                ViewBag.MinimumRangeValue = minimumRangeValue;
            }
            Distance.DistanceEnum minimumRangeType = Distance.DistanceEnum.Special;
            ViewBag.MinimumRangeType = (int)minimumRangeType;
            if (!string.IsNullOrEmpty(minimumRangeDropdownList))
            {
                minimumRangeType = (Distance.DistanceEnum)Enum.Parse(typeof(Distance.DistanceEnum), minimumRangeDropdownList);
                ViewBag.MinimumRangeType = (int)minimumRangeType;
            }
            int maximumRangeValue = 360;
            ViewBag.MaximumRangeValue = maximumRangeValue;
            if (!string.IsNullOrEmpty(maximumRangeTextBox))
            {
                maximumRangeValue = Convert.ToInt32(maximumRangeTextBox);
                ViewBag.MaximumRangeValue = maximumRangeValue;
            }
            Distance.DistanceEnum maximumRangeType = Distance.DistanceEnum.Unlimited;
            ViewBag.MaximumRangeType = (int)maximumRangeType;
            if (!string.IsNullOrEmpty(maximumRangeDropdownList))
            {
                maximumRangeType = (Distance.DistanceEnum)Enum.Parse(typeof(Distance.DistanceEnum), maximumRangeDropdownList);
                ViewBag.MaximumRangeType = (int)maximumRangeType;
            }

            string components = "All";
            ViewBag.Components = components;
            if (!string.IsNullOrEmpty(componentsDropdownList))
            {
                components = componentsDropdownList;
                ViewBag.Components = components;
            }

            int minimumDurationValue = 0;
            ViewBag.MinimumDurationValue = minimumDurationValue;
            if (!string.IsNullOrEmpty(minimumDurationTextBox))
            {
                minimumDurationValue = Convert.ToInt32(minimumDurationTextBox);
                ViewBag.MinimumDurationValue = minimumDurationValue;
            }
            Time.TimeEnum minimumDurationType = Time.TimeEnum.Special;
            ViewBag.MinimumDurationType = (int)minimumDurationType;
            if (!string.IsNullOrEmpty(minimumDurationDropdownList))
            {
                minimumDurationType = (Time.TimeEnum)Enum.Parse(typeof(Time.TimeEnum), minimumDurationDropdownList);
                ViewBag.MinimumDurationType = (int)minimumDurationType;
            }

            int maximumDurationValue = 360;
            ViewBag.MaximumDurationValue = maximumDurationValue;
            if (!string.IsNullOrEmpty(maximumDurationTextBox))
            {
                maximumDurationValue = Convert.ToInt32(maximumDurationTextBox);
                ViewBag.MaximumDurationValue = maximumDurationValue;
            }
            Time.TimeEnum maximumDurationType = Time.TimeEnum.UntilDispelled;
            ViewBag.MaximumDurationType = (int)maximumDurationType;
            if (!string.IsNullOrEmpty(maximumDurationDropdownList))
            {
                maximumDurationType = (Time.TimeEnum)Enum.Parse(typeof(Time.TimeEnum), maximumDurationDropdownList);
                ViewBag.MaximumDurationType = (int)maximumDurationType;
            }


            string concentration = "All";
            ViewBag.Concentration = concentration;
            if (!string.IsNullOrEmpty(concentrationDropdownList))
            {
                concentration = concentrationDropdownList;
                ViewBag.Concentration = concentration;
            }

            string higherLevels = "All";
            ViewBag.HigherLevels = higherLevels;
            if (!string.IsNullOrEmpty(higherLevelsDropdownList))
            {
                higherLevels = higherLevelsDropdownList;
                ViewBag.HigherLevels = higherLevels;
            }
            List<Spell> spells = GetSpellsFromSession();
            spells = spells.Where(e => (
            e.Classes.Contains(className) || className == "All") &&
            e.Level >= minLevel && e.Level <= maxLevel &&
            (ritual == "All" || e.Ritual == (ritual == "Yes")) &&
            //(dice == "All" || e.IsDiceRequired == (dice == "Yes")) &&
            (e.CastingTime.TimeType > minimumCastingTimeType || (e.CastingTime.TimeType == minimumCastingTimeType && e.CastingTime.Value >= minimumCastingTimeValue)) &&
            (e.CastingTime.TimeType < maximumCastingTimeType || (e.CastingTime.TimeType == maximumCastingTimeType && e.CastingTime.Value <= maximumCastingTimeValue)) &&
            (e.Range.DistanceType > minimumRangeType || (e.Range.DistanceType == minimumRangeType && e.Range.Value >= minimumRangeValue)) &&
            (e.Range.DistanceType < maximumRangeType || (e.Range.DistanceType == maximumRangeType && e.Range.Value <= maximumRangeValue)) &&
            (components == "All" || (components == "0" && !e.Components.Contains("M (")) || (components == "1" && e.Components.Contains("M (") && !e.Components.Contains("gp")) || (components == "2" && e.Components.Contains("gp") && !e.Components.Contains("consume")) || (components == "3" && e.Components.Contains("consume"))) &&
            (e.Duration.TimeType > minimumDurationType || (e.Duration.TimeType == minimumDurationType && e.Duration.Value >= minimumDurationValue)) &&
            (e.Duration.TimeType < maximumDurationType || (e.Duration.TimeType == maximumDurationType && e.Duration.Value <= maximumDurationValue)) &&
            (concentration == "All" || e.Concentration == (concentration == "Yes")) &&
            (higherLevels == "All" || (higherLevels == "Yes" && e.HigherLevels != ("No\r\n")) || (higherLevels == "No" && e.HigherLevels == "No\r\n") )).ToList();
            ViewBag.Spells = spells;
            return View();
        }

        public ActionResult Detail(string spellName)
        {
            if (Session["Spells"] == null)
            {
                Session["Spells"] = FileOperations.LoadSpells();
            }
            List<Spell> spells = (List<Spell>)Session["Spells"];
            Spell spell = (Spell)spells.FirstOrDefault(e => e.Name == spellName);
            ViewBag.Spell = spell;
            return View();
        }
    }
}